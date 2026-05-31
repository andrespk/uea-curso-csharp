# Unit of Work Pattern - MiniKanban

## Estrutura Implementada

### Domain Layer (MiniKanban.Domain)
- **BaseEntity**: Classe base para todas as entidades com Id, CreatedAt, UpdatedAt e IsActive
- **IRepository<TEntity>**: Interface genérica para repositório
- **IUnitOfWork**: Interface para padrão Unit of Work

### Infrastructure Layer (MiniKanban.Infrastructure)
- **MiniKanbanDbContext**: Contexto do Entity Framework Core
- **Repository<TEntity>**: Implementação genérica de repositório
- **UnitOfWork**: Implementação do padrão Unit of Work com suporte a transações
- **ServiceCollectionExtensions**: Extensão para injeção de dependências

## Como Usar

### 1. Configurar no Program.cs

```csharp
using MiniKanban.Infrastructure.IoC;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços
builder.Services.AddInfrastructureServices(
    builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new NullReferenceException("Connection string not found"));

var app = builder.Build();
```

### 2. Configurar Connection String

No arquivo `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=MiniKanbanDb;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

### 3. Criar Entidades

Exemplo - Criar uma entidade Task:

```csharp
using MiniKanban.Domain.Entities;

namespace MiniKanban.Domain.Entities;

public class Task : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int BoardId { get; set; }
    public string Status { get; set; } = "Todo";
}
```

### 4. Registrar DbSet no DbContext

No arquivo `MiniKanbanDbContext.cs`:

```csharp
public DbSet<Task> Tasks { get; set; }
public DbSet<Board> Boards { get; set; }
```

### 5. Usar no Controlador/Serviço

```csharp
using MiniKanban.Domain.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public TasksController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Task>> GetTask(int id)
    {
        var repository = _unitOfWork.Repository<Task>();
        var task = await repository.GetByIdAsync(id);
        
        if (task == null)
            return NotFound();

        return Ok(task);
    }

    [HttpPost]
    public async Task<ActionResult<Task>> CreateTask(CreateTaskDto dto)
    {
        var repository = _unitOfWork.Repository<Task>();
        
        var task = new Task 
        { 
            Title = dto.Title,
            Description = dto.Description,
            BoardId = dto.BoardId,
            Status = "Todo"
        };

        await repository.AddAsync(task);
        await _unitOfWork.CommitAsync();

        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, UpdateTaskDto dto)
    {
        var repository = _unitOfWork.Repository<Task>();
        var task = await repository.GetByIdAsync(id);
        
        if (task == null)
            return NotFound();

        task.Title = dto.Title;
        task.Description = dto.Description;
        
        await repository.UpdateAsync(task);
        await _unitOfWork.CommitAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var repository = _unitOfWork.Repository<Task>();
        var task = await repository.GetByIdAsync(id);
        
        if (task == null)
            return NotFound();

        await repository.DeleteAsync(task);
        await _unitOfWork.CommitAsync();

        return NoContent();
    }
}
```

### 6. Usar Transações

```csharp
// Iniciar transação
_unitOfWork.BeginTransaction();

try
{
    var taskRepository = _unitOfWork.Repository<Task>();
    var boardRepository = _unitOfWork.Repository<Board>();

    // Operações
    var newTask = new Task { /* ... */ };
    await taskRepository.AddAsync(newTask);

    var board = await boardRepository.GetByIdAsync(1);
    board.UpdatedAt = DateTime.UtcNow;
    await boardRepository.UpdateAsync(board);

    // Confirmar transação
    await _unitOfWork.CommitTransactionAsync();
}
catch
{
    // Reverter em caso de erro
    await _unitOfWork.RollbackTransactionAsync();
    throw;
}
```

## Próximos Passos

1. Created migrations: `dotnet ef migrations add Initial`
2. Update database: `dotnet ef database update`
3. Criar entidades específicas do negócio
4. Implementar repositórios específicos se necessário
5. Adicionar services na camada Application

