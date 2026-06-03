# Guia de Apresentacao - MiniKanban

Este guia serve como roteiro para a apresentacao final do projeto MiniKanban, alinhado aos requisitos do trabalho do Modulo 3.

## 1. Abertura

Apresente o projeto em poucas frases:

> O MiniKanban e uma API REST para gerenciamento de quadros Kanban. O sistema permite criar usuarios, autenticar com JWT, criar boards, adicionar membros, organizar colunas, criar cards, associar tags e registrar comentarios.

Pontos principais:

*   Tema: sistema Kanban.
*   Objetivo: gerenciar boards, colunas, cards, membros, tags e comentarios.
*   Stack: .NET 8, ASP.NET Core, PostgreSQL, Entity Framework Core, JWT, Swagger/Scalar e Docker.

## 2. Arquitetura

Mostre a solution no editor:

```text
src/
├── MiniKanban.API
├── MiniKanban.Application
├── MiniKanban.Domain
├── MiniKanban.Infrastructure
├── MiniKanban.Exceptions
└── MiniKanban.sln

tests/
└── MiniKanban.Tests
```

Explique a responsabilidade de cada camada:

*   `MiniKanban.API`: entrada HTTP, Minimal APIs, autenticação, autorização, Swagger/Scalar e tratamento global de exceções.
*   `MiniKanban.Application`: DTOs, interfaces e services/casos de uso.
*   `MiniKanban.Domain`: entidades, enum e contratos de repositórios.
*   `MiniKanban.Infrastructure`: DbContext, configurações EF Core, repositories, Unit of Work e injeção de dependência.
*   `MiniKanban.Exceptions`: exceções customizadas.
*   `MiniKanban.Tests`: testes unitários dos services.

Frase importante:

> O projeto usa Minimal APIs em vez de Controllers, mas a responsabilidade da camada API continua a mesma: receber requisições HTTP e delegar regras de negocio para a Application.

## 3. Modelagem do Dominio

Mostre o diagrama em:

```text
docs/modelagem-entidades.jpg
```

Entidades principais:

*   `User`: usuario do sistema.
*   `Board`: quadro Kanban.
*   `BoardMember`: relacao entre usuario e board, com papel no quadro.
*   `KanbanColumn`: coluna do board.
*   `Card`: tarefa dentro de uma coluna.
*   `Tag`: etiqueta pertencente a um board.
*   `CardTag`: associacao muitos-para-muitos entre card e tag.
*   `Comment`: comentario em um card.

Relacionamentos para destacar:

*   Um usuario pode criar varios boards.
*   Um board possui varios membros.
*   Um board possui varias colunas.
*   Uma coluna possui varios cards.
*   Um card pode ter varios comentarios.
*   Um card pode ter varias tags, e uma tag pode estar em varios cards.

## 4. Banco de Dados e EF Core

Mostre:

```text
src/MiniKanban.Infrastructure/Data/Context/MiniKanbanDbContext.cs
src/MiniKanban.Infrastructure/Data/Configurations/
src/MiniKanban.Infrastructure/Data/Repositories/
db/create-tables.sql
```

Explique:

*   O banco usado e PostgreSQL.
*   O acesso aos dados usa Entity Framework Core com Npgsql.
*   O `MiniKanbanDbContext` declara os `DbSet`.
*   As configuracoes das entidades ficam em `Infrastructure/Data/Configurations`.
*   Os repositories ficam na camada Infrastructure.
*   O script `db/create-tables.sql` cria as tabelas no container PostgreSQL.
*   A API tambem possui uma rotina de inicializacao via EF Core para criar tabelas quando necessario.

Comando para subir o ambiente:

```bash
docker compose up --build
```

Portas:

```text
Frontend:   http://localhost:3000
API:        http://localhost:5093
Scalar:     http://localhost:5093/api-docs
PostgreSQL: localhost:5433
```

## 5. Autenticacao e Autorizacao

Mostre:

```text
src/MiniKanban.API/Program.cs
src/MiniKanban.API/Endpoints/AuthEndpoints.cs
src/MiniKanban.Application/Services/Auth/
```

Explique:

*   A API usa JWT Bearer Token.
*   `POST /api/auth/register` cria usuario.
*   `POST /api/auth/login` autentica e retorna token.
*   `GET /api/me` retorna o usuario autenticado.
*   Senhas sao armazenadas com hash, nao em texto puro.
*   Endpoints do dominio usam `.RequireAuthorization()`.

Usuario inicial para demonstracao:

```json
{
  "username": "admin",
  "password": "Password123"
}
```

Fluxo no Scalar:

1. Acesse `http://localhost:5093/api-docs`.
2. Execute `POST /api/auth/login`.
3. Copie o token retornado.
4. Configure o Bearer Token no Scalar.
5. Execute um endpoint protegido, como `GET /api/me`.

## 6. Endpoints Para Demonstrar

Use o Scalar ou o arquivo:

```text
src/MiniKanban.API/MiniKanban.API.http
```

Fluxo recomendado de demonstracao:

1. `POST /api/auth/login`
2. `GET /api/me`
3. `POST /api/boards`
4. `GET /api/boards`
5. `POST /api/kanban-columns`
6. `GET /api/boards/{boardId}/kanban-columns`
7. `POST /api/cards`
8. `GET /api/columns/{columnId}/cards`
9. `POST /api/tags`
10. `POST /api/card-tags`
11. `POST /api/comments`
12. `GET /api/cards/{cardId}/comments`

Endpoints publicos:

*   `POST /api/auth/register`
*   `POST /api/auth/login`
*   `GET /health`

Endpoints protegidos:

*   Usuarios: `/api/me`, `/api/users`
*   Boards: `/api/boards`
*   Membros: `/api/board-members`
*   Colunas: `/api/kanban-columns`
*   Cards: `/api/cards`
*   Tags: `/api/tags`
*   CardTags: `/api/card-tags`
*   Comments: `/api/comments`

## 7. Regras de Negocio

Regras implementadas para comentar:

*   Ao criar um board, o usuario autenticado vira automaticamente membro com role `Owner`.
*   Nao e permitido adicionar o mesmo usuario duas vezes ao mesmo board.
*   Nao e permitido adicionar `Owner` manualmente por `/api/board-members`.
*   Nao e permitido remover o membro `Owner` do board.
*   Nao e permitido alterar a role `Owner` pelo endpoint de membros.
*   Ao criar coluna, a API valida se o board existe.
*   A ordem da coluna nao pode ser negativa.
*   O WIP limit nao pode ser negativo.
*   A ordem da coluna deve ser unica dentro do board no momento da criacao.
*   Cards registram o usuario autenticado como criador.
*   Comentarios registram o usuario autenticado como autor.

## 8. Testes

Mostre:

```text
tests/MiniKanban.Tests/
```

Comando:

```bash
dotnet test tests/MiniKanban.Tests/MiniKanban.Tests.csproj --no-restore -p:UseSharedCompilation=false -m:1
```

Cobertura atual:

*   Registro de usuario.
*   Validacao de username/email duplicado.
*   Criacao, atualizacao, busca e remocao de board.
*   Adicao, atualizacao e remocao de membros.
*   Criacao e atualizacao de colunas Kanban.

Resultado esperado:

```text
Passed: 17
Failed: 0
```

## 9. Tratamento de Erros

Mostre:

```text
src/MiniKanban.API/Handlers/GlobalExceptionHandler.cs
src/MiniKanban.API/Filters/ExceptionResponseOperationFilter.cs
src/MiniKanban.Exceptions/
```

Explique:

*   A API possui tratamento global de excecoes.
*   Erros de negocio retornam resposta padronizada.
*   A documentacao OpenAPI inclui respostas `400` e `500` via filtro customizado.

## 10. Decisoes Tecnicas

Pontos que podem ser defendidos:

*   Uso de arquitetura em camadas para separar responsabilidades.
*   Uso de DTOs para nao expor entidades diretamente.
*   Uso de services para concentrar regras de negocio.
*   Uso de repositories e Unit of Work para isolar persistencia.
*   Uso de JWT para proteger endpoints.
*   Uso de Scalar para demonstracao dos endpoints.
*   Uso de Docker Compose para facilitar execucao do projeto.
*   Uso de Minimal APIs por serem mais diretas, mantendo separacao em arquivos de endpoints.

## 11. Dificuldades e Melhorias Futuras

Dificuldades que podem ser citadas:

*   Organizar a divisao de responsabilidades entre camadas.
*   Garantir que os endpoints protegidos usem corretamente o usuario autenticado.
*   Ajustar Docker, banco e documentacao para a demonstracao.

Melhorias futuras:

*   Criar migrations versionadas do EF Core.
*   Ampliar testes para cards, tags, comentarios e endpoints HTTP.
*   Implementar controle de permissao por role dentro de cada board.
*   Melhorar o frontend para consumir todos os fluxos da API.
*   Criar filtros mais avancados de cards por prioridade, prazo e responsavel.

## 12. Checklist Antes da Apresentacao

Execute:

```bash
docker compose up --build
```

Verifique:

*   `http://localhost:3000` abre o frontend.
*   `http://localhost:5093/health` retorna `{"status":"Healthy"}`.
*   `http://localhost:5093/api-docs` abre o Scalar.
*   `POST /api/auth/login` retorna token JWT.
*   `GET /api/me` funciona com Bearer Token.
*   `dotnet test tests/MiniKanban.Tests/MiniKanban.Tests.csproj --no-restore -p:UseSharedCompilation=false -m:1` passa com 17 testes.

## 13. Divisao Sugerida da Apresentacao

Pessoa 1:

*   Problema, objetivo e modelagem.

Pessoa 2:

*   Arquitetura em camadas e organizacao da solution.

Pessoa 3:

*   Banco, EF Core, repositories e Docker.

Pessoa 4:

*   Autenticacao JWT, endpoints publicos/protegidos e demonstracao no Scalar.

Pessoa 5:

*   Testes, tratamento de erros, dificuldades e melhorias futuras.

