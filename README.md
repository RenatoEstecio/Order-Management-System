# 📦 OrderManagementSystem

Sistema de gerenciamento de pedidos construído em .NET 10 com arquitetura em camadas, separando regras de negócio, persistência e API.

---

## 🚀 Tecnologias

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- xUnit
- Moq
- FluentAssertions

---

## 📁 Estrutura da solução

```text
OrderManagementSystem/
│
├── EFCore
│   ├── Entities (Cliente, Pedido, Produto, PedidoItem, PedidoHistorico)
│   ├── ContextEFCore
│
├── Library
│   ├── BLL (Services / regras de negócio)
│   ├── Repository (acesso a dados)
│   ├── DTO / ResponseDTO
│   ├── Enums
│   ├── UTIL
│
├── OrderManagementSystem (API)
│   ├── Controllers
│   ├── Program.cs
│   ├── appsettings.json
│   ├── Dockerfile
│
└── OrderManagementSystem.Tests
    ├── Services (testes unitários)
```

## ▶️ Executar o projeto

🗄️ Banco de Dados e abordagem com EF Core

O sistema utiliza SQL Server como banco de dados relacional e a persistência é gerenciada via Entity Framework Core, adotando uma abordagem Database First com Scaffold.

⚙️ Geração do modelo (Scaffold)

O modelo de dados foi gerado automaticamente a partir do banco existente utilizando o comando: Scaffold-DbContext 
Localizado em EFCore/DatabaseDesign/SQLoms.sql 

Esse processo gerou:

O DbContext → ContextEFCore
As entidades do banco dentro do projeto EFCore
O mapeamento automático entre tabelas e classes

🧱 Estrutura do banco

O script EFCore/DatabaseDesign/SQLoms.sql define toda a base do sistema.

📌 Principais tabelas

- Cliente
- Produto
- Pedido
- PedidoItem
- PedidoHistorico
- PedidoStatus

dotnet restore  
dotnet build  
dotnet run --project src/OrderManagementSystem/OrderManagementSystem.csproj  

---

## 🧪 Executar testes

dotnet test  

---

## 🧪 Estratégia de testes

- Testes unitários
- xUnit como framework
- Moq para isolamento de dependências
- FluentAssertions para validações

### Foco dos testes:

- PedidoService (principal)
- Validações de regras de negócio
- Fluxos de erro e exceção
- Criação e consulta de pedidos

---

## 🧱 Arquitetura

- EFCore → Entidades e DbContext
- Repository → acesso a dados
- BLL (Service) → regras de negócio
- API → exposição HTTP

---

🧾 Estratégia de Persistência

A aplicação adota Entity Framework Core como camada de persistência, utilizando abordagem Database First (Scaffold).

🏗️ Por que Database First?

A escolha foi baseada nos seguintes fatores:

- O schema do banco já estava definido previamente em SQL Server
- Necessidade de acelerar o desenvolvimento inicial do domínio
- Garantia de aderência total ao modelo relacional existente
- Facilidade de regeneração do modelo via Scaffold-DbContext

🔄 Uso do Entity Framework Core

O EF Core é utilizado para:

- Mapeamento objeto-relacional (ORM)
- Consultas com LINQ
- Tracking de entidades
- Execução de updates em lote (ExecuteUpdateAsync)
- Controle de transações no fluxo de pedidos

⚙️ Padrão de acesso a dados

O sistema utiliza uma separação clara:

- Repository Layer → acesso direto ao ContextEFCore
- Service Layer (BLL) → regras de negócio
- EF Core DbContext → persistência real

🔁 Transações

Fluxos críticos (como criação de pedido) utilizam transações explícitas:

- Garante consistência entre Pedido
- Garante consistência entre Itens
- Garante consistência entre Estoque
- Garante consistência entre Histórico

Se qualquer etapa falhar, tudo é revertido (Rollback).

⚖️ Trade-offs da abordagem

Vantagens:

- Alta produtividade
- Menor boilerplate de SQL
- Integração nativa com LINQ
- Facilidade de manutenção

Desvantagens:

- Menor controle fino de SQL gerado
- Dependência do EF Core
- Possível custo de performance em queries complexas
- Scaffold pode sobrescrever alterações manuais

📌 Decisão arquitetural

A estratégia foi desenhada para priorizar:

- consistência
- velocidade de desenvolvimento
- manutenção simples

em vez de:

- controle absoluto de SQL
- performance extrema

## 💡 Decisões técnicas

### Separação em camadas
Facilita manutenção e testes.

### GUID + INT
- GUID: identificação pública
- INT: chave interna do banco

### Repository Pattern
Abstração do EF Core.

### Transações
Garantem consistência no pedido + estoque.

---

## 📦 Estoque

- Reduzido no momento da criação do pedido
- Atualização feita dentro de transação

---

## 💰 Valores monetários

Tipo: decimal

⚠️ Decisão técnica

Não foi utilizado double nem float devido à imprecisão binária desses tipos, que pode gerar erros como:

0.1 + 0.2 ≠ 0.3 (em double)
Diferenças acumuladas em pedidos com múltiplos itens

O decimal garante precisão adequada para sistemas comerciais e financeiros.

Arredondamento

Para padronizar cálculos, é utilizado arredondamento com 2 casas decimais e regra AwayFromZero, evitando distorções em valores intermediários:

```csharp
public static decimal Round(decimal value)
{
    return Math.Round(value, 2, MidpointRounding.AwayFromZero);
}


🚀 Pontos fora do escopo e melhorias futuras

Algumas melhorias foram deixadas de fora para manter o foco na regra de negócio e no fluxo principal do sistema.

🐳 Docker Compose

- Não foi implementado totalmente o ambiente containerizado
- Futuramente seria usado para rodar API e SQL Server de forma padronizada
- Facilita setup local e evita dependências instaladas manualmente

☁️ AWS EC2 (Deploy)

- A aplicação não foi publicada em ambiente cloud
- Evolução seria deploy em AWS EC2 com configuração de produção
- Possível integração com CI/CD para automação de deploy

🗄️ AWS S3 (Imagens)

- Não há suporte a upload de imagens no sistema
- Futuro: uso do S3 para armazenar imagens de produtos
- Permite escalabilidade e evita armazenamento local

🧪 Ambiente de desenvolvimento

- Setup atual depende de configuração manual (banco + API)
- Futuro: uso de Docker Compose para subir tudo com um comando
- Melhoraria onboarding e padronização entre ambientes
