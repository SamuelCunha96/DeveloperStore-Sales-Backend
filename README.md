# DeveloperStore — Sales API

REST API for managing sales records, built with .NET 8, DDD, CQRS, and Clean Architecture.

## Tech Stack

| Layer | Technology |
|---|---|
| Runtime | .NET 8 / C# |
| API | ASP.NET Core Web API |
| CQRS | MediatR 12 |
| ORM | Entity Framework Core 8 |
| Relational DB | PostgreSQL |
| Document DB | MongoDB |
| Mapping | AutoMapper 14 |
| Validation | FluentValidation 11 |
| Unit Tests | xUnit + NSubstitute + Bogus |
| API Docs | Swagger / Swashbuckle |

## Project Structure

```
src/
├── DeveloperStore.Sales.API/           # ASP.NET Core Web API, controllers, Swagger
├── DeveloperStore.Sales.Application/   # CQRS handlers (MediatR), DTOs, AutoMapper profiles
├── DeveloperStore.Sales.Domain/        # Entities, value objects, domain events, repository interfaces
└── DeveloperStore.Sales.Infrastructure/ # EF Core (PostgreSQL), MongoDB, repository implementations

tests/
├── DeveloperStore.Sales.Unit.Tests/    # Domain and application unit tests
└── DeveloperStore.Sales.Functional.Tests/ # API integration/functional tests
```

## Business Rules

- **4–9 items** of the same product → **10% discount**
- **10–20 items** of the same product → **20% discount**
- **Above 20 items** → not allowed (validation error)
- **Below 4 items** → no discount

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/) (for PostgreSQL and MongoDB)

## Configuration

Copy the example environment file and fill in your values:

```bash
cp src/DeveloperStore.Sales.API/appsettings.Development.json.example src/DeveloperStore.Sales.API/appsettings.Development.json
```

Or set the following environment variables:

| Variable | Description | Example |
|---|---|---|
| `ConnectionStrings__PostgreSQL` | PostgreSQL connection string | `Host=localhost;Port=5432;Database=developerstore_sales;Username=postgres;Password=postgres` |
| `ConnectionStrings__MongoDB` | MongoDB connection string | `mongodb://localhost:27017` |
| `MongoDB__DatabaseName` | MongoDB database name | `developerstore_events` |

## Running with Docker Compose

```bash
docker-compose up -d
```

This starts PostgreSQL, MongoDB, and the API on `http://localhost:8080`.

## Running Locally

### 1. Start the databases

```bash
docker-compose up -d postgres mongo
```

### 2. Apply EF Core migrations

```bash
dotnet ef database update --project src/DeveloperStore.Sales.Infrastructure --startup-project src/DeveloperStore.Sales.API
```

### 3. Run the API

```bash
dotnet run --project src/DeveloperStore.Sales.API
```

The API will be available at `http://localhost:5000`.  
Swagger UI: `http://localhost:5000/swagger`

## Running Tests

```bash
# Unit tests
dotnet test tests/DeveloperStore.Sales.Unit.Tests

# Functional tests
dotnet test tests/DeveloperStore.Sales.Functional.Tests

# All tests
dotnet test
```

## API Endpoints

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/v1/sales` | List sales (paginated, filterable) |
| `GET` | `/api/v1/sales/{id}` | Get sale by ID |
| `POST` | `/api/v1/sales` | Create a new sale |
| `PUT` | `/api/v1/sales/{id}` | Update a sale |
| `DELETE` | `/api/v1/sales/{id}` | Cancel a sale |
| `DELETE` | `/api/v1/sales/{id}/items/{itemId}` | Cancel a sale item |

## Domain Events (logged)

- `SaleCreated` — fired when a sale is created
- `SaleModified` — fired when a sale is updated
- `SaleCancelled` — fired when a sale is cancelled
- `ItemCancelled` — fired when a sale item is cancelled

Events are published via MediatR notifications and logged to the application log.
