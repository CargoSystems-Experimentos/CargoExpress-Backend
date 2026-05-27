# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build & Run Commands

```bash
# Build the solution
dotnet build ACME.CargoExpress.API.sln

# Run the API
dotnet run --project ACME.CargoExpress.API/ACME.CargoExpress.API.csproj

# Run unit tests
dotnet test CargoExpress.UnitTests/CargoExpress.UnitTests.csproj

# Run integration tests (SpecFlow/BDD)
dotnet test CargoExpress.IntegrationTests/CargoExpress.IntegrationTests.csproj

# Run a single test by name
dotnet test CargoExpress.UnitTests/CargoExpress.UnitTests.csproj --filter "FullyQualifiedName~TestClassName.TestMethodName"

# Docker
docker build -f Dockerfile -t cargoexpress-api .
docker run -p 8080:8080 cargoexpress-api
```

Swagger UI is available at `http://localhost:<port>/swagger` when running locally.

## Architecture Overview

This is an ASP.NET Core 8.0 Web API implementing **Domain-Driven Design (DDD)** with three bounded contexts:

- **IAM** — Identity & Access Management (authentication, JWT, user accounts)
- **User** — User profiles (Client, Entrepreneur, Configuration)
- **Registration** — Core business domain (Trip, Driver, Vehicle, Expense, OngoingTrip, Alert)

Each bounded context follows the same four-layer structure:

```
[Context]/
├── Domain/               # Entities, aggregates, value objects, repo interfaces, service interfaces
│   ├── Model/
│   │   ├── Aggregates/   # Aggregate roots (Trip, Client, Entrepreneur, User)
│   │   ├── Entities/     # Domain entities (Driver, Vehicle, Expense, Alert, OngoingTrip)
│   │   ├── Commands/     # Mutation command objects
│   │   ├── Queries/      # Read query objects
│   │   └── ValueObjects/
│   ├── Repositories/     # IRepository interfaces
│   └── Services/         # ICommandService / IQueryService interfaces
├── Application/
│   └── Internal/
│       ├── CommandServices/  # Business logic for mutations
│       └── QueryServices/    # Business logic for reads
├── Infrastructure/
│   └── Persistence/EFC/
│       ├── Configuration/    # EF Core entity mappings (Fluent API)
│       └── Repositories/     # Repository implementations
└── Interfaces/
    ├── REST/
    │   ├── Resources/        # Request/response DTOs
    │   └── Transform/        # Assemblers: entity ↔ DTO conversion
    └── ACL/                  # Anti-Corruption Layer (IAM context only)
```

## Key Conventions

**CQRS-inspired services**: Command services handle writes; Query services handle reads. Both implement domain service interfaces.

**Repository pattern**: All repos extend `IBaseRepository<T>` (standard CRUD + `IUnitOfWork`). Specialized queries are added as extra interface methods (e.g., `FindByClientIdAsync`).

**Route naming**: Kebab-case is enforced globally via a custom route convention registered in `Program.cs`. Controller routes use `[Route("api/v1/[controller]")]`.

**Assembler pattern**: Every controller uses a static `ResourceFromEntityAssembler` / `CommandFromResourceAssembler` to translate between HTTP resources and domain objects. Never pass DTOs into domain services directly.

**Authentication**: JWT Bearer tokens. `RequestAuthorizationMiddleware` validates tokens on every request. Endpoints default to `[Authorize]`; open endpoints use `[AllowAnonymous]`.

## Database

- **MySQL** via `MySql.EntityFrameworkCore`; connection strings in `appsettings.json` (`DefaultConnection` = Railway cloud, `LocalConnection` = local MySQL).
- **EF Core** with Fluent API mappings in `AppDbContext.OnModelCreating()`.
- Tables use snake_case plural naming convention (custom extension applied to model builder).
- Audit fields (`CreatedDate`, `UpdateDate`) are auto-populated via the `EntityFrameworkCore.CreatedUpdatedDate` interceptor.
- The database is created/migrated automatically on startup in `Program.cs`.

## Dependency Injection Registration

All repositories, command services, and query services are registered manually in `Program.cs`. When adding a new service or repository, register it there following the existing pattern.

## Integration Tests

Tests use SpecFlow (BDD) with SQLite in-memory database. Feature files are in `CargoExpress.IntegrationTests/Features/`. Step definitions bind to Gherkin scenarios for API-level testing.
