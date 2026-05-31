## 🗄️ Criando Primeira Migration

### Passo 1: Instalar Entity Framework CLI (se não tiver)

```bash
dotnet tool install --global dotnet-ef
```

Ou atualizar se já tem:
```bash
dotnet tool update --global dotnet-ef
```

---

### Passo 2: Criar Entidade de Exemplo

Crie o arquivo `C:\...\MiniKanban.Domain\Entities\Task.cs`:

```csharp
namespace MiniKanban.Domain.Entities;

public class Task : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = "Todo";
    public int BoardId { get; set; }
}
```

---

### Passo 3: Registrar DbSet no DbContext

Edite `MiniKanban.Infrastructure/Data/Context/MiniKanbanDbContext.cs`:

```csharp
public class MiniKanbanDbContext : DbContext
{
    public MiniKanbanDbContext(DbContextOptions<MiniKanbanDbContext> options) : base(options)
    {
    }

    public DbSet<Task> Tasks { get; set; }

    // ... resto do código ...
}
```

---

### Passo 4: Criar a Migration

No terminal, na raiz da solução:

```bash
dotnet ef migrations add Initial -p MiniKanban.Infrastructure
```

**Output esperado:**
```
Build started...
Build succeeded.
Done. To undo this action, use 'ef migrations remove'
```

Isso criará um arquivo em:
```
MiniKanban.Infrastructure/Migrations/20260531120000_Initial.cs
```

---

### Passo 5: Aplicar Migration (Opcional)

Se quiser aplicar manualmente (sem usar auto-migration):

```bash
dotnet ef database update -p MiniKanban.Infrastructure
```

**Mas NÃO PRECISA** pois o `Program.cs` já faz isso automaticamente!

---

### Passo 6: Verificar Banco de Dados

Abra **pgAdmin** ou **DBeaver** e conecte:
- **Host:** localhost
- **Port:** 5432
- **Database:** MiniKanbanDb
- **User:** postgres
- **Password:** postgres

Você verá as tabelas criadas automaticamente!

---

## 📝 Outros Exemplos de Migrations

### Adicionar Nova Entidade

1. Crie a classe em `Domain/Entities/`
2. Adicione `public DbSet<SuaEntidade> SuasEntidades { get; set; }` no DbContext
3. Execute:
   ```bash
   dotnet ef migrations add AddSuaEntidade -p MiniKanban.Infrastructure
   ```

### Modificar Entidade Existente

1. Altere a entidade
2. Execute:
   ```bash
   dotnet ef migrations add ModifyTask -p MiniKanban.Infrastructure
   ```

### Remover Última Migration

Se não aplicou ainda:
```bash
dotnet ef migrations remove -p MiniKanban.Infrastructure
```

### Ver Migrations Aplicadas

```bash
dotnet ef migrations list -p MiniKanban.Infrastructure
```

---

## 🔄 Auto-Migrations Funcionam Assim

Quando inicia a aplicação, no `Program.cs`:

```csharp
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MiniKanbanDbContext>();
    await dbContext.Database.MigrateAsync();
}
```

1. Verifica migrations não aplicadas
2. Aplica uma por uma na ordem correta
3. Se todas estão aplicadas, não faz nada
4. Se banco não existe, cria
5. **Nada de manual!**

---

## ✅ Checklist

- ✅ PostgreSQL instalado
- ✅ dotnet-ef instalado
- ✅ Entidades criadas em `Domain/Entities/`
- ✅ DbSets registrados em DbContext
- ✅ Migrations criadas
- ✅ Auto-migration no Program.cs
- ✅ Build sem erros
- ✅ App ready!

---

## 🚨 Troubleshooting

### Erro: "A compatible .NET SDK was not found"
Solução: Já foi resolvido (arquivo global.json atualizado)

### Erro: "Cannot connect to PostgreSQL"
```bash
# Verificar se PostgreSQL está rodando
# Windows: Services > postgres > Start
# Linux: sudo systemctl start postgresql
# Mac: brew services start postgresql
```

### Erro: "Database 'MiniKanbanDb' does not exist"
Isso é NORMAL na primeira execução. A app vai criar automaticamente!

### Erro: "Password authentication failed"
Verifique credenciais em `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=MiniKanbanDb;Username=postgres;Password=postgres;"
}
```

### Erro: "Migration already exists"
Mude o nome:
```bash
dotnet ef migrations add InitialData -p MiniKanban.Infrastructure
```

---

## 📚 Referências

- [EF Core Migrations](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
- [Npgsql Documentation](https://www.npgsql.org/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)

