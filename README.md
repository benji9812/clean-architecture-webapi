# Clean Architecture Web API

ASP.NET Core Web API built with Clean Architecture, CQRS, MediatR, and Entity Framework Core.

## Domain

**Project** (1) → (many) **Task**

A project is a container for tasks. Each task has a status (`Todo`, `InProgress`, `Done`, `Cancelled`) and a priority (`Low`, `Medium`, `High`, `Critical`).

## Architecture

```
CleanArchitecture.API           → HTTP layer (Controllers, Middleware, Program.cs)
CleanArchitecture.Application   → CQRS (Commands, Queries, Handlers, Validators, Behaviors)
CleanArchitecture.Domain        → Entities, Enums, Repository Interfaces
CleanArchitecture.Infrastructure → EF Core DbContext, Repository implementations, Migrations
```

Dependency rule: every arrow points **inward** toward Domain. Domain knows nothing about any other layer.

```
API ──► Application ──► Domain ◄── Infrastructure
 └──────────────────────────────► Infrastructure
```

### CQRS Flow

```
HTTP Request
  └─► Controller
        └─► _mediator.Send(new CreateProjectCommand(...))
              └─► LoggingBehavior (logs request name)
                    └─► ValidationBehavior (FluentValidation)
                          └─► CreateProjectCommandHandler : IRequestHandler<>
                                └─► IProjectRepository.AddAsync(entity)
                                      └─► AppDbContext.SaveChangesAsync()
                                            └─► HTTP 201 Created + ProjectDto
```

## Tech Stack

| Layer | Technology |
|---|---|
| Web API | ASP.NET Core 10 |
| CQRS | MediatR 14 |
| Validation | FluentValidation 12 |
| ORM | Entity Framework Core 10 |
| Database | SQL Server / LocalDB |
| API Docs | Scalar (at `/scalar/v1`) |

## Getting Started

### Prerequisites

- .NET 10 SDK
- SQL Server or LocalDB

### Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/benji9812/clean-architecture-webapi.git
   cd clean-architecture-webapi
   ```

2. Update the connection string in `CleanArchitecture.API/appsettings.json` if needed:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CleanArchitectureDb;Trusted_Connection=True;"
   }
   ```

3. Configure local secrets (required for login/JWT in development):
   ```bash
   dotnet user-secrets --project CleanArchitecture.API set "JwtSettings:SecretKey" "<min-32-char-random-string>"
   dotnet user-secrets --project CleanArchitecture.API set "TestUsers:0:Username" "<admin-username>"
   dotnet user-secrets --project CleanArchitecture.API set "TestUsers:0:Password" "<admin-password>"
   dotnet user-secrets --project CleanArchitecture.API set "TestUsers:0:Role" "Admin"
   dotnet user-secrets --project CleanArchitecture.API set "TestUsers:1:Username" "<user-username>"
   dotnet user-secrets --project CleanArchitecture.API set "TestUsers:1:Password" "<user-password>"
   dotnet user-secrets --project CleanArchitecture.API set "TestUsers:1:Role" "User"
   ```
   Example template: `CleanArchitecture.API/secrets.example.json`.

4. Run the API (migrations apply automatically on startup):
   ```bash
   dotnet run --project CleanArchitecture.API
   ```

5. Open Scalar at: **https://localhost:{port}/scalar/v1**

### Migrations (manual)

```bash
# Create a new migration
dotnet ef migrations add <MigrationName> \
  --project CleanArchitecture.Infrastructure \
  --startup-project CleanArchitecture.API

# Apply to database
dotnet ef database update \
  --project CleanArchitecture.Infrastructure \
  --startup-project CleanArchitecture.API
```

## API Endpoints

### Projects

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/projects` | List all projects |
| GET | `/api/projects/{id}` | Get project by ID |
| POST | `/api/projects` | Create project |
| PUT | `/api/projects/{id}` | Update project |
| DELETE | `/api/projects/{id}` | Delete project (cascades tasks) |

**POST /api/projects** body:
```json
{
  "name": "My Project",
  "description": "Optional description"
}
```

### Tasks

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/tasks/project/{projectId}` | List tasks for a project |
| GET | `/api/tasks/{id}` | Get task by ID |
| POST | `/api/tasks` | Create task |
| PUT | `/api/tasks/{id}` | Update task |
| DELETE | `/api/tasks/{id}` | Delete task |

**POST /api/tasks** body:
```json
{
  "title": "Implement login",
  "description": "Optional",
  "priority": "High",
  "dueDate": "2026-12-31T00:00:00Z",
  "projectId": "00000000-0000-0000-0000-000000000000"
}
```

Valid values: `priority` → `Low`, `Medium`, `High`, `Critical` | `status` → `Todo`, `InProgress`, `Done`, `Cancelled`

## Error Responses

All errors return RFC 7807 `ProblemDetails`:

```json
{
  "title": "Validation Error",
  "status": 400,
  "detail": "...",
  "errors": [
    { "propertyName": "Name", "errorMessage": "Name is required." }
  ]
}
```

## CI / Automated Smoke Tests

The workflow is named **Build, Migrate & Smoke Test** and lives at `.github/workflows/build-and-smoketest.yml`.

It triggers automatically on every push and pull request targeting `main` (and can also be run manually via `workflow_dispatch`).

It spins up a disposable SQL Server container, builds the solution, runs the API, and executes an end-to-end smoke test suite covering: authentication (Admin + User login), rejection of invalid credentials (401), JWT + role-based access control (Admin vs. User, 403 for unauthorized role), full CRUD on Project and Task entities, the Project → Task one-to-many relationship, input validation (400 instead of a 500 crash on empty name), and cleanup (cascade delete).

Required GitHub repository secrets by name only (Settings → Secrets and variables → Actions):
- `CI_SQL_SA_PASSWORD`
- `CI_JWT_SECRET`
- `CI_ADMIN_PASSWORD`
- `CI_USER_PASSWORD`

Anyone can see the live pass/fail status under the repo's Actions tab.
