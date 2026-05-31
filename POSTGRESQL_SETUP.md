## 🔧 Configuração PostgreSQL e Auto-Migrations

### ✅ Mudanças Realizadas

#### 1. **Banco de Dados: SQL Server → PostgreSQL**
   - **Antes:** `Microsoft.EntityFrameworkCore.SqlServer`
   - **Depois:** `Npgsql.EntityFrameworkCore.PostgreSQL`
   
   **Arquivo:** `MiniKanban.Infrastructure.csproj`

#### 2. **Connection String Configurada**
   ```json
   "DefaultConnection": "Host=localhost;Port=5432;Database=MiniKanbanDb;Username=postgres;Password=postgres;"
   ```
   **Arquivo:** `appsettings.json`

#### 3. **Código Limpo**
   - Removidos todos os comentários desnecessários
   - Removido código de exemplo (WeatherForecast)
   - Código mais limpo e profissional

#### 4. **Auto-Migrations Implementadas**
   ```csharp
   using (var scope = app.Services.CreateScope())
   {
       var dbContext = scope.ServiceProvider.GetRequiredService<MiniKanbanDbContext>();
       await dbContext.Database.MigrateAsync();
   }
   ```
   - Executa `MigrateAsync()` na inicialização
   - **Cria** o banco automaticamente se não existir
   - **Atualiza** o banco se houver migrations novas

#### 5. **Serviços Registrados em Program.cs**
   - `AddInfrastructureServices()` registra DbContext e UnitOfWork
   - Injeta services do Infrastructure no container

#### 6. **Referências de Projetos Atualizadas**
   - `MiniKanban.API` referencia `MiniKanban.Infrastructure`
   - `MiniKanban.Application` referencia `MiniKanban.Domain`

---

### 📋 Checklist de Arquivos Modificados

- ✅ `MiniKanban.Infrastructure/MiniKanban.Infrastructure.csproj` - PostgreSQL
- ✅ `MiniKanban.Infrastructure/Data/Context/MiniKanbanDbContext.cs` - Limpeza comentários
- ✅ `MiniKanban.Infrastructure/IoC/ServiceCollectionExtensions.cs` - PostgreSQL
- ✅ `MiniKanban.API/Program.cs` - Auto-migrations e registro de serviços
- ✅ `MiniKanban.API/appsettings.json` - Connection string PostgreSQL
- ✅ `MiniKanban.API/MiniKanban.API.csproj` - Referência Infrastructure
- ✅ `MiniKanban.Application/MiniKanban.Application.csproj` - Referência Domain

---

### 🚀 Como Usar

#### Pré-requisitos
Este projeto agora usa **PostgreSQL**. Você precisa ter instalado:

1. **PostgreSQL** instalado
2. **PgAdmin** (gerenciador) ou **DBeaver** (opcional)
3. PostgreSQL rodando na porta `5432`

#### Configurar Banco de Dados (Primeira Execução)

A aplicação fará automaticamente na inicialização:
1. Se o banco não existir, cria
2. Se houver migrations, aplica
3. Pronto para usar!

#### Alterar Connection String (Opcional)

Se seu PostgreSQL está em outro local, edite `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=SEU_HOST;Port=5432;Database=MiniKanbanDb;Username=SUA_USER;Password=SUA_SENHA;"
  }
}
```

---

### 📊 Flow da Inicialização

```
1. Program.cs inicia
   ↓
2. Registra serviços (AddInfrastructureServices)
   ↓
3. Cria scope e pega DbContext
   ↓
4. Executa Database.MigrateAsync()
   ↓
5. Se banco não existe → CRIA
   Se houver migrations → APLICA
   Se tudo OK → CONTINUA
   ↓
6. App inicia normalmente
```

---

### 🔌 Próximas Tarefas

1. **Criar Migrations Iniciais**
   ```bash
   dotnet ef migrations add Initial -p MiniKanban.Infrastructure
   ```

2. **Criar Entidades de Negócio**
   - Task.cs
   - Board.cs
   - Column.cs
   - User.cs

3. **Registrar DbSets no DbContext**
   ```csharp
   public DbSet<Task> Tasks { get; set; }
   public DbSet<Board> Boards { get; set; }
   ```

4. **Criar nova Migration**
   ```bash
   dotnet ef migrations add AddEntities -p MiniKanban.Infrastructure
   ```

5. **A partir daí, a app atualiza automaticamente na inicialização!**

---

### 🛡️ Segurança - Connection String

⚠️ **NÃO deixe credenciais em `appsettings.json` em produção!**

Use **User Secrets** em desenvolvimento:
```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "host=...;user=...;password=..."
```

Ou use **variáveis de ambiente** em produção.

---

### ✅ Build Status

```
✅ MiniKanban.Exceptions
✅ MiniKanban.Domain
✅ MiniKanban.Infrastructure (com PostgreSQL)
✅ MiniKanban.Application
✅ MiniKanban.API (com auto-migrations)

Compilação com sucesso!
0 Avisos | 0 Erros
```

---

### 📚 Recursos

- [Entity Framework Core + PostgreSQL](https://www.npgsql.org/efcore/)
- [Database Migrations](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
- [ApplicationBuilder.BeginScope](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.servicecollectionserviceextensions.createscope)

