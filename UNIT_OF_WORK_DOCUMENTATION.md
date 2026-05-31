# Arquitetura Unit of Work com Padrão Repository

## 📋 Resumo da Implementação

Implementação completa do padrão **Unit of Work** com **Generic Repository** para a camada de dados do MiniKanban, seguindo princípios SOLID e clean architecture.

---

## 🏗️ Estrutura de Pastas

```
MiniKanban.Domain/
├── Entities/
│   └── BaseEntity.cs          # Classe base para todas as entidades
└── Interfaces/
    ├── IRepository.cs          # Interface genérica de repositório
    └── IUnitOfWork.cs          # Interface do padrão Unit of Work

MiniKanban.Infrastructure/
├── Data/
│   ├── Abstractions/
│   │   └── UnitOfWork.cs       # Implementação do Unit of Work
│   ├── Context/
│   │   └── MiniKanbanDbContext.cs  # DbContext do Entity Framework
│   └── Repositories/
│       └── Repository.cs       # Implementação genérica de repositório
└── IoC/
    └── ServiceCollectionExtensions.cs  # Injeção de dependências
```

---

## 🔑 Componentes Principais

### 1️⃣ **BaseEntity** (Domain)
Classe base abstrata que todas as entidades herdam:
- `Id`: Identificador único
- `CreatedAt`: Data de criação (UTC)
- `UpdatedAt`: Data da última atualização (UTC)
- `IsActive`: Flag de atividade lógica

### 2️⃣ **IRepository<TEntity>** (Domain)
Interface genérica que define operações CRUD:
- `GetByIdAsync(id)`: Buscar entidade por ID
- `GetAllAsync()`: Listar todas as entidades
- `FindAsync(predicate)`: Buscar por predicado
- `AddAsync(entity)`: Adicionar entidade
- `UpdateAsync(entity)`: Atualizar entidade
- `DeleteAsync(entity)`: Deletar entidade
- `ExistsAsync(id)`: Verificar existência
- `CountAsync()`: Contar entidades

### 3️⃣ **IUnitOfWork** (Domain)
Interface do padrão Unit of Work:
- `Repository<TEntity>()`: Obter repositório tipado
- `CommitAsync()`: Salvar alterações
- `RollbackAsync()`: Reverter alterações
- `BeginTransaction()`: Iniciar transação
- `CommitTransactionAsync()`: Confirmar transação
- `RollbackTransactionAsync()`: Reverter transação

### 4️⃣ **MiniKanbanDbContext** (Infrastructure)
Contexto do Entity Framework Core:
- Gerencia DbSets
- Atualiza timestamps automaticamente
- Configurações de modelo

### 5️⃣ **Repository<TEntity>** (Infrastructure)
Implementação genérica que:
- Usa padrão Repository
- Trabalha com qualquer entidade `TEntity`
- Oferece operações assíncronas
- Integrado com DbContext

### 6️⃣ **UnitOfWork** (Infrastructure)
Implementação do padrão:
- Gerencia repositórios
- Controla transações
- Lazy loading de repositórios (criados sob demanda)
- Suporte a savepoint em transações

---

## 🚀 Como Usar

### Passo 1: Registrar no Program.cs

```csharp
using MiniKanban.Infrastructure.IoC;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços
builder.Services.AddInfrastructureServices(
    builder.Configuration.GetConnectionString("DefaultConnection")!);

var app = builder.Build();
```

### Passo 2: Adicionar Connection String

Em `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=MiniKanbanDb;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

### Passo 3: Criar Entidades

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

### Passo 4: Registrar DbSet

No `MiniKanbanDbContext.cs`:
```csharp
public DbSet<Task> Tasks { get; set; }
public DbSet<Board> Boards { get; set; }
```

### Passo 5: Usar em Controladores

```csharp
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
        var taskRepository = _unitOfWork.Repository<Task>();
        var task = await taskRepository.GetByIdAsync(id);
        
        if (task == null)
            return NotFound();

        return Ok(task);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask(CreateTaskDto dto)
    {
        var taskRepository = _unitOfWork.Repository<Task>();
        
        var task = new Task 
        { 
            Title = dto.Title,
            Description = dto.Description,
            Status = "Todo"
        };

        await taskRepository.AddAsync(task);
        await _unitOfWork.CommitAsync();

        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
    }
}
```

---

## 📊 Diagrama de Fluxo

```
┌─────────────────────────────────────────────────────────────┐
│                    Controlador (API)                         │
└────────────┬────────────────────────────────────────────────┘
             │
             │ Injeta IUnitOfWork
             ▼
┌─────────────────────────────────────────────────────────────┐
│                  IUnitOfWork (Interface)                     │
│  - Repository<T>()  → Obtém repositório tipado              │
│  - CommitAsync()    → Salva alterações no BD                │
│  - Transações       → Controla transações BD                │
└────────────┬────────────────────────────────────────────────┘
             │
             ▼
┌─────────────────────────────────────────────────────────────┐
│            UnitOfWork (Implementação)                        │
│  - Gerencia dicionário de repositórios                      │
│  - Controla MiniKanbanDbContext                             │
│  - Transações com IDbContextTransaction                     │
└────────────┬────────────────────────────────────────────────┘
             │
             ├──────────────────────┬──────────────────────┐
             │                      │                      │
             ▼                      ▼                      ▼
    ┌─────────────────┐   ┌──────────────────┐   ┌──────────────────┐
    │ Repository<T>   │   │ Repository<T>    │   │ Repository<T>    │
    │  (Tasks)        │   │  (Boards)        │   │  (Outros)        │
    └────────┬────────┘   └────────┬─────────┘   └────────┬─────────┘
             │                     │                      │
             └─────────────────────┼──────────────────────┘
                                   │
                                   ▼
                    ┌─────────────────────────────┐
                    │  MiniKanbanDbContext        │
                    │  - DbSet<Task>              │
                    │  - DbSet<Board>             │
                    │  - SaveChangesAsync()       │
                    └────────────┬────────────────┘
                                 │
                                 ▼
                    ┌─────────────────────────────┐
                    │    SQL Server Database      │
                    │  - Tables                   │
                    │  - Relations                │
                    └─────────────────────────────┘
```

---

## ✅ Benefícios da Implementação

✨ **Separação de Responsabilidades**: Cada camada tem sua função clara

🔄 **Padrão Repository**: Abstrai acesso aos dados

🎯 **Unit of Work**: Garante consistência transacional

🔌 **Genérico**: `Repository<T>` funciona com qualquer entidade

📝 **Assíncrono**: Todas operações assíncronas

🔐 **Seguro**: Tipagem forte com genéricos

⚡ **Performance**: Lazy loading de repositórios

🛡️ **Transações**: Suporte nativo a transações

---

## 🔧 Próximos Passos

1. **Criar Entidades de Negócio**
   ```bash
   # Exemplos: Task, Board, User, Column
   ```

2. **Gerar Migrations**
   ```bash
   dotnet ef migrations add Initial
   ```

3. **Atualizar Banco de Dados**
   ```bash
   dotnet ef database update
   ```

4. **Implementar Serviços na Application**
   - Regras de negócio
   - Validações
   - DTOs

5. **Criar Endpoints na API**
   - Controllers
   - Mapeamento de DTOs

---

## 📚 Referências

- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [Unit of Work Pattern](https://martinfowler.com/eaaCatalog/unitOfWork.html)
- [Repository Pattern](https://martinfowler.com/eaaCatalog/repository.html)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

---

## ✨ Status

✅ **Implementação Completa**
- ✅ Domain Layer (Entidades e Interfaces)
- ✅ Infrastructure Layer (DbContext, Repository, UnitOfWork)
- ✅ Injeção de Dependências
- ✅ Build sem erros

