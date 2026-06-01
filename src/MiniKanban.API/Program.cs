using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MiniKanban.Infrastructure.Api;
using MiniKanban.Domain.Entities;
using MiniKanban.Application.Helpers;
using MiniKanban.Infrastructure.Data.Context;
using MiniKanban.Infrastructure.IoC;
using MiniKanban.API.Handlers;
using MiniKanban.API.Filters;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<ExceptionResponseOperationFilter>();

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MiniKanban API",
        Version = "v1",
        Description = "API de gerenciamento de quadros Kanban com suporte a colunas e tarefas. Inclui autenticação via JWT e persistência em PostgreSQL."
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Informe apenas o token JWT retornado pelo endpoint de login. A UI enviara o cabecalho Authorization: Bearer <token>."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddInfrastructureServices(connectionString);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt Key is missing.")))
    };
});

builder.Services.AddAuthorization();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

try
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<MiniKanbanDbContext>();
        
        var migrations = dbContext.Database.GetMigrations();
        if (migrations.Any())
        {
            await dbContext.Database.MigrateAsync();
        }
        else
        {
            var databaseCreator = dbContext.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
            if (databaseCreator != null)
            {
                if (!databaseCreator.Exists()) 
                    await databaseCreator.CreateAsync();
                
                if (!databaseCreator.HasTables()) 
                    await databaseCreator.CreateTablesAsync();
            }
        }

        if (!await dbContext.Users.AnyAsync())
        {
            var adminUser = new User
            {
                Username = "admin",
                Email = "admin@kanban.com",
                PasswordHash = PasswordHasher.Hash("Password123"),
                Role = "Admin"
            };
            await dbContext.Users.AddAsync(adminUser);
            await dbContext.SaveChangesAsync();
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Database migration/initialization failed: {ex.Message}");
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.MapScalarApiReference("/api-docs", options =>
    {
        options.WithOpenApiRoutePattern("/swagger/{documentName}/swagger.json");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapEndpoints();

app.Run();
