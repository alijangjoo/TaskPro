# TaskPro

Supect.TaskPro is a .NET 9 backend for work management. A work item can carry a recurring schedule, an estimate, checklists, costs, work logs, resource assignments, and an alarm. Sign-in is by mobile number and a one-time password. API access uses a JWT and a refresh token.

The solution `Supect.TaskPro.sln` hosts three processes: the task API, the user service, and a YARP gateway. Domain logic sits on the in-repo OysterFx framework (`Fx`) and follows a command/query split.

## Prerequisites

- .NET SDK 9
- PostgreSQL
- Two databases: `TaskProDb` for tasks and `TaskProUsersDb` for ASP.NET Identity

## Run

```powershell
dotnet restore Supect.TaskPro.sln
dotnet build Supect.TaskPro.sln
dotnet run --project src/Endpoints/Suspect.TaskPro.Endpoints.Api --launch-profile https
dotnet run --project src/Endpoints/Supect.TaskPro.Endpoints.Auth.Api --launch-profile https
dotnet run --project ApiGatewayService/Supect.TaskPro.GW.YARP.Api --launch-profile https
```

| Host | Project | HTTP | HTTPS | Swagger |
| --- | --- | --- | --- | --- |
| Task API | `src/Endpoints/Suspect.TaskPro.Endpoints.Api` | `http://localhost:5274` | `https://localhost:7108` | `/swagger` |
| Auth API | `src/Endpoints/Supect.TaskPro.Endpoints.Auth.Api` | `http://localhost:5025` | `https://localhost:7020` | `/swagger` |
| Gateway | `ApiGatewayService/Supect.TaskPro.GW.YARP.Api` | `http://localhost:5118` | `https://localhost:7016` | — |

On startup the task API calls `AppCommandDbContext.Database.EnsureCreated()`. Entity Framework migrations live in `src/Infra/Persistence/Postgres.Commands/Migrations` and `src/Endpoints/Supect.TaskPro.Endpoints.Auth.Api/Migrations`. Apply the auth migrations with:

```powershell
dotnet ef database update --project src/Endpoints/Supect.TaskPro.Endpoints.Auth.Api
```

## Configuration

Put real values in user secrets or the environment. Do not commit passwords, JWT signing keys, or SMS provider secrets.

**Task API** (`appsettings.json`)

- `ConnectionStrings:CommandDb` and `ConnectionStrings:QueryDb` — PostgreSQL, both aimed at `TaskProDb` in development
- `Jwt` and `JwtSettings` — issuer and audience default to `Supect.TaskPro.API`; access token about 120 minutes; refresh token about 30 days
- `ApiKeys:Main`
- `Swagger` — document title `Supect TaskPro Api`
- `Serilog` — console, file `%TEMP%\Logs\TaskPro.Log.txt`, and Elasticsearch at `http://localhost:9200` with index `taskpro-index-{yyyy.MM}`
- `PanelPayamak:Providers:KavehNegar:ServicePath` — Kavenegar verify/lookup template
- `UserService:BaseURL` — defaults to `https://localhost:7020/`

**Auth API**

- `ConnectionStrings:DefaultConnection` — PostgreSQL database `TaskProUsersDb`
- Identity users are created from `MobileNumber`. Email is not required.

**Gateway**

- `/taskService/api/tasks/{**catch-all}` drops the `/taskService` prefix and forwards to `https://localhost:7108/`
- Auth routes are not proxied by this gateway

## Sign-in

1. `POST /api/Auth/register` on the auth API with a mobile number. An existing number returns `400`. The response includes the user id.
2. `GET /api/Auth/by-mobile/{mobileNumber}` returns that id.
3. `POST /api/auth/login/otp` on the task API sends the code.
4. `POST /api/auth/login/otp/{mobileNumber}/verify` checks the code and returns tokens.
5. `POST /api/auth/token/refresh` issues a new access token.

Work-item actions sit on a controller marked `[Authorize]` and expect a bearer token.

## Implemented API

Paths below are on the task API. Through the gateway, prefix them with `/taskService`.

| Method | Path | Purpose |
| --- | --- | --- |
| POST | `/api/auth/login/otp` | Send an OTP |
| POST | `/api/auth/login/otp/{mobileNumber}/verify` | Verify the OTP |
| POST | `/api/auth/token/refresh` | Refresh the token |
| POST | `/api/tasks/work-items` | Create a work item |
| PUT | `/api/tasks/work-items` | Edit a work item |
| GET | `/api/tasks/work-items/{businessKey}` | Get one work item |
| GET | `/api/tasks/work-items/GetAll` | List work items |
| POST | `/api/tasks/work-items/{businessKey}/done` | Mark one schedule done. The body is that schedule's `Guid`. |
| DELETE | `/api/tasks/work-items/{businessKey}/delete` | Delete a work item |
| POST | `/api/tasks/work-items/{businessKey}/alarm/set` | Set an alarm. The body is a `DateTime`. |
| POST | `/api/tasks/work-items/{businessKey}/alarm/clear` | Clear the alarm |
| POST | `/api/Tasks/EstimateCommand/InitialEstimate` | First estimate |
| POST | `/api/Tasks/EstimateCommand/RevisionEstimate` | Revised estimate |
| POST | `/api/Tasks/CheckListCommand/AddCheckList` | Add a checklist |
| POST | `/api/Tasks/CheckListCommand/RemoveCheckList` | Remove a checklist |
| POST | `/api/Tasks/CostCommand/AddWorkItemCost` | Add a cost |
| POST | `/api/Tasks/CostCommand/RemoveCost` | Remove a cost |
| POST | `/api/Tasks/LogWorkCommand/AddWorkLog` | Add a work log |
| POST | `/api/Tasks/LogWorkCommand/RemoveWorkLog` | Remove a work log |
| POST | `/api/Wrappers/WrapperCommand/CreateWrapper` | Create a wrapper |

`CheckListQuery`, `EstimateQuery`, `CostQuery`, and `LogWorkQuery` are routed and currently expose no public actions.

Auth API:

| Method | Path | Purpose |
| --- | --- | --- |
| POST | `/api/Auth/register` | Register a user by mobile number |
| GET | `/api/Auth/by-mobile/{mobileNumber}` | Look up the user id |

## Domain

`WorkItem` is the main aggregate: title, description, weight, parent, status, recurrence, alarm, estimate and estimate history, resource assignments, costs, material usage, checklists, work logs, and schedules.

- Status: `Unknown`, `Draft`, `Active`, `Done`. The item becomes `Done` when every schedule is done.
- Recurrence: `None`, `Daily`, `Weekly`. A schedule date in the past is skipped, and a duplicate date is rejected.
- `Wrapper` is the project container. Its type is `Project`, `Phase`, `Routine`, or `Etc`, with progress percent, weight, and KPIs.
- `Resource` and `ResourceRate` store a resource and its rate.
- `OtpCode` and `RefreshToken` support sign-in.
- `TaskOrchestration` includes `WorkFlow`, `KpiEngine`, `HealthEngine`, and `SimulationEngine`.

## Layout

```text
Supect.TaskPro.sln
src/Endpoints/Suspect.TaskPro.Endpoints.Api          Task API, JWT, Swagger, Serilog
src/Endpoints/Supect.TaskPro.Endpoints.Auth.Api      Identity and mobile registration
ApiGatewayService/Supect.TaskPro.GW.YARP.Api         YARP gateway
src/AppCore/Domain                                   Entities and domain rules
src/AppCore/DomainServices
src/AppCore/AppServices                              Command and query handlers
src/AppCore/Shared                                   Contracts
src/Infra/Persistence/Postgres.Commands              EF writes and migrations
src/Infra/Persistence/Postgres.Queries               EF reads
src/Infra/PanelPayamak                               SMS abstraction and Kavenegar
src/Infra/InternalServices/...UserApiCaller          Calls the auth API
Fx/                                                  OysterFx: aggregates, command bus, JWT, REST caller
src/tests/AppCore                                    xUnit and Moq
```

The solution folder is spelled `Supect`. The domain namespace is `Suspect.TaskPro`. Both spellings are kept as they appear in the code.

## Tests and containers

```powershell
dotnet test Supect.TaskPro.sln
```

Test projects: `Suspect.TaskPro.AppCore.AppServices.Tests` and `Suspect.TaskPro.AppCore.AppServices.IntegrationTests`.

Each API Dockerfile uses `mcr.microsoft.com/dotnet/aspnet:9.0` and exposes ports `8080` and `8081`. The Docker build context is the repository root.

## Logging

Serilog on the task API writes to the console, a temp file on Windows, and, when Elasticsearch is up, a monthly index. The API project references a SQL Server health-check package. The database in use is PostgreSQL.