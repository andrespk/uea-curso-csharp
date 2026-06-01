# Guia de Contribuição Assistida por IA (CONTRIBUTE.md) 🤖💻

Bem-vindo! Este projeto foi estruturado para ser altamente compatível e amigável ao desenvolvimento assistido por Inteligência Artificial (ex: Gemini, Copilot, ChatGPT, Cursor, etc.). 

Se você é um desenvolvedor utilizando IA para programar neste repositório, ou um agente autônomo operando diretamente no workspace, siga as diretrizes abaixo para garantir que o código gerado mantenha a consistência, arquitetura e qualidade desejadas.

---

## 📂 1. Contexto Essencial do Projeto

Sempre forneça as seguintes informações de contexto para a IA no início da sessão de chat:

1.  **Arquitetura do Sistema:** O projeto segue a **Clean Architecture (Arquitetura Limpa)** estruturada sob as seguintes pastas em `src/`:
    *   `MiniKanban.Domain`: Entidades essenciais e abstrações (sem dependências de frameworks).
    *   `MiniKanban.Exceptions`: Exceções de regras de negócios e validação.
    *   `MiniKanban.Application`: DTOs, Serviços e Interfaces.
    *   `MiniKanban.Infrastructure`: Persistência com EF Core, Repositórios, Migrações e IoC.
    *   `MiniKanban.API`: Minimal APIs, Middleware de Exceção, Filtros OpenAPI e Segurança.
2.  **Ambiente Local:** O banco roda em Docker (PostgreSQL na porta `5433`). A API roda via .NET CLI na porta `5093`.
3.  **Documentação de Referência:** O arquivo principal é o `README.md` na raiz do projeto.

---

## 🎨 2. Padrões de Código e Diretrizes para Geração de Código

Oriente sua ferramenta de IA a seguir rigidamente estas boas práticas de C# e .NET 8:

### 2.1. C# Moderno (C# 12 / .NET 8)
*   **Namespaces com Escopo de Arquivo:** Use sempre `namespace MiniKanban.Namespace;` ao invés de chaves aninhadas.
*   **Construtores Primários:** Utilize Construtores Primários (Primary Constructors) para injeção de dependência em classes onde for apropriado.
*   **Expressões de Coleção:** Prefira expressões de coleção (`[]` ou `Array.Empty<T>()`) em vez de inicializações verbosas de arrays e listas.

### 2.2. Fluxo de Tratamento de Erros e Documentação OpenAPI
*   **Tratamento de Exceções de Domínio:** Em vez de retornar `Results.BadRequest("mensagem")` diretamente no endpoint, instancie ou lance uma exceção herdada de `BusinessException` ou `ValidationError` (localizadas em `MiniKanban.Exceptions`).
*   **Tratamento Global de Exceções:** A API possui o `GlobalExceptionHandler` que captura automaticamente essas exceções e formata a resposta no padrão RFC 7807 (`ProblemDetails`) com status `400` (ou `500` para erros inesperados).
*   **Documentação Automática:** Não gere métodos redundantes como `.ProducesProblem(400)` ou `.ProducesProblem(500)` nos mapeamentos de Minimal APIs, pois o `ExceptionResponseOperationFilter` injeta esses retornos automaticamente na especificação OpenAPI do Swagger/Scalar.

### 2.3. Integridade do Código Existente
*   **Preservação de Contexto:** Ao pedir para a IA modificar um arquivo, instrua-a a **não remover comentários, XML docs ou lógicas de logs** existentes que não estejam diretamente relacionados à alteração atual.

---

## 🚀 3. Guia de Comandos Rápidos para Agentes de IA

Se você estiver delegando tarefas a um Agente de IA capaz de executar comandos no terminal, compartilhe estes comandos para automação e validação:

### Banco de Dados (PostgreSQL)
*   **Subir container do banco:**
    ```powershell
    docker compose -f db/docker-compose.yml up -d
    ```
*   **Iniciar container existente:**
    ```powershell
    docker start minikanban-db
    ```

### Compilação e Execução
*   **Restaurar e Compilar a Solução:**
    ```powershell
    dotnet build src/MiniKanban.sln
    ```
*   **Executar a API localmente:**
    ```powershell
    dotnet run --project src/MiniKanban.API/MiniKanban.API.csproj --launch-profile http
    ```

### Testes Manuais rápidos no Powershell
*   **Efetuar chamada de Login de Teste:**
    ```powershell
    Invoke-RestMethod -Uri http://localhost:5093/api/auth/login -Method Post -ContentType 'application/json' -Body '{"Username":"admin", "Password":"Password123"}'
    ```

---

## 📝 4. Prompt de Inicialização Recomendado (System Prompt para IAs)

Você pode copiar e colar o trecho abaixo no início da sua conversa com qualquer assistente de IA para alinhar o comportamento dele:

> *"Atue como um desenvolvedor C# especialista em .NET 8 e Clean Architecture. Estamos trabalhando no projeto MiniKanban. A solução é composta por MiniKanban.Domain, MiniKanban.Application, MiniKanban.Infrastructure, MiniKanban.Exceptions e MiniKanban.API. Siga as convenções de código modernos do C# 12, use namespaces com escopo de arquivo, garanta que exceções de validação ou de negócios herdem das classes contidas em MiniKanban.Exceptions (pois são tratadas globalmente e documentadas via ExceptionResponseOperationFilter), e certifique-se de que qualquer código sugerido compile com sucesso contra a solução src/MiniKanban.sln."*
