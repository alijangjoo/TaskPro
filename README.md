# TaskPro

بک‌اند مدیریت کار Supect.TaskPro روی ‎.NET 9. کارها به‌صورت Work Item با زمان‌بندی تکرارشونده، تخمین، چک‌لیست، هزینه، لاگ کار، منبع و هشدار ثبت می‌شوند. ورود با شماره موبایل و OTP است و دسترسی API با JWT و Refresh Token کنترل می‌شود.

راهکار `Supect.TaskPro.sln` سه میزبان دارد: API اصلی تسک، سرویس کاربران، و دروازه YARP. منطق دامنه روی فریم‌ورک داخلی OysterFx (پوشه `Fx`) و الگوی Command/Query سوار است.

## پیش‌نیاز

- ‎.NET SDK 9
- PostgreSQL
- دو دیتابیس: `TaskProDb` برای تسک‌ها و `TaskProUsersDb` برای Identity

## اجرا

```powershell
dotnet restore Supect.TaskPro.sln
dotnet build Supect.TaskPro.sln
dotnet run --project src/Endpoints/Suspect.TaskPro.Endpoints.Api --launch-profile https
dotnet run --project src/Endpoints/Supect.TaskPro.Endpoints.Auth.Api --launch-profile https
dotnet run --project ApiGatewayService/Supect.TaskPro.GW.YARP.Api --launch-profile https
```

| میزبان | پروژه | HTTP | HTTPS | Swagger |
| --- | --- | --- | --- | --- |
| Task API | `src/Endpoints/Suspect.TaskPro.Endpoints.Api` | `http://localhost:5274` | `https://localhost:7108` | `/swagger` |
| Auth API | `src/Endpoints/Supect.TaskPro.Endpoints.Auth.Api` | `http://localhost:5025` | `https://localhost:7020` | `/swagger` |
| Gateway | `ApiGatewayService/Supect.TaskPro.GW.YARP.Api` | `http://localhost:5118` | `https://localhost:7016` | — |

Task API هنگام بالا آمدن `AppCommandDbContext.Database.EnsureCreated()` را صدا می‌زند. مهاجرت‌های EF در `src/Infra/Persistence/Postgres.Commands/Migrations` و `src/Endpoints/Supect.TaskPro.Endpoints.Auth.Api/Migrations` هستند. برای اعمال مهاجرت Auth:

```powershell
dotnet ef database update --project src/Endpoints/Supect.TaskPro.Endpoints.Auth.Api
```

## پیکربندی

مقادیر واقعی را در User Secrets یا محیط بگذارید. کلیدهای مورد استفاده:

**Task API** (`appsettings.json`)

- `ConnectionStrings:CommandDb` و `ConnectionStrings:QueryDb` — هر دو PostgreSQL و در توسعه روی `TaskProDb`
- `Jwt` و `JwtSettings` — Issuer و Audience پیش‌فرض `Supect.TaskPro.API`، Access Token حدود ۱۲۰ دقیقه، Refresh Token حدود ۳۰ روز
- `ApiKeys:Main`
- `Swagger` — عنوان سند `Supect TaskPro Api`
- `Serilog` — کنسول، فایل `%TEMP%\Logs\TaskPro.Log.txt`، و Elasticsearch روی `http://localhost:9200` با ایندکس `taskpro-index-{yyyy.MM}`
- `PanelPayamak:Providers:KavehNegar:ServicePath` — قالب lookup کاوه‌نگار
- `UserService:BaseURL` — پیش‌فرض `https://localhost:7020/`

**Auth API**

- `ConnectionStrings:DefaultConnection` — PostgreSQL روی `TaskProUsersDb`
- کاربر Identity با `MobileNumber` ساخته می‌شود و ایمیل الزامی نیست

**Gateway**

- مسیر `/taskService/api/tasks/{**catch-all}` پیشوند `/taskService` را برمی‌دارد و به `https://localhost:7108/` می‌فرستد
- مسیرهای Auth از این دروازه عبور نمی‌کنند

## ورود

1. `POST /api/Auth/register` روی Auth API با شماره موبایل. اگر شماره موجود باشد `400` برمی‌گردد و شناسه کاربر در پاسخ است.
2. `GET /api/Auth/by-mobile/{mobileNumber}` شناسه را برمی‌گرداند.
3. `POST /api/auth/login/otp` روی Task API کد را می‌فرستد.
4. `POST /api/auth/login/otp/{mobileNumber}/verify` کد را تأیید و توکن می‌دهد.
5. `POST /api/auth/token/refresh` توکن را تازه می‌کند.

Work Itemها روی کنترلر با `[Authorize]` هستند و Bearer token می‌خواهند.

## API پیاده‌سازی‌شده

پایه Task API، یا از پشت دروازه با پیشوند `/taskService`.

| روش | مسیر | کار |
| --- | --- | --- |
| POST | `/api/auth/login/otp` | ارسال OTP |
| POST | `/api/auth/login/otp/{mobileNumber}/verify` | تأیید OTP |
| POST | `/api/auth/token/refresh` | تازه‌سازی توکن |
| POST | `/api/tasks/work-items` | ساخت Work Item |
| PUT | `/api/tasks/work-items` | ویرایش |
| GET | `/api/tasks/work-items/{businessKey}` | دریافت یکی |
| GET | `/api/tasks/work-items/GetAll` | فهرست |
| POST | `/api/tasks/work-items/{businessKey}/done` | انجام یک Schedule؛ بدنه `Guid` همان schedule است |
| DELETE | `/api/tasks/work-items/{businessKey}/delete` | حذف |
| POST | `/api/tasks/work-items/{businessKey}/alarm/set` | تنظیم هشدار؛ بدنه `DateTime` |
| POST | `/api/tasks/work-items/{businessKey}/alarm/clear` | پاک کردن هشدار |
| POST | `/api/Tasks/EstimateCommand/InitialEstimate` | تخمین اولیه |
| POST | `/api/Tasks/EstimateCommand/RevisionEstimate` | بازنگری تخمین |
| POST | `/api/Tasks/CheckListCommand/AddCheckList` | افزودن چک‌لیست |
| POST | `/api/Tasks/CheckListCommand/RemoveCheckList` | حذف چک‌لیست |
| POST | `/api/Tasks/CostCommand/AddWorkItemCost` | افزودن هزینه |
| POST | `/api/Tasks/CostCommand/RemoveCost` | حذف هزینه |
| POST | `/api/Tasks/LogWorkCommand/AddWorkLog` | ثبت لاگ کار |
| POST | `/api/Tasks/LogWorkCommand/RemoveWorkLog` | حذف لاگ کار |
| POST | `/api/Wrappers/WrapperCommand/CreateWrapper` | ساخت Wrapper |

کنترلرهای `CheckListQuery`، `EstimateQuery`، `CostQuery` و `LogWorkQuery` مسیر دارند و اکشن عمومی ندارند.

Auth API:

| روش | مسیر | کار |
| --- | --- | --- |
| POST | `/api/Auth/register` | ثبت کاربر با موبایل |
| GET | `/api/Auth/by-mobile/{mobileNumber}` | شناسه کاربر |

## دامنه

Aggregate اصلی `WorkItem` است: عنوان، توضیح، وزن، والد، وضعیت، تکرار، هشدار، تخمین و تاریخچه تخمین، تخصیص منبع، هزینه، مصرف مصالح، چک‌لیست، لاگ کار و Schedule.

- وضعیت: `Unknown`، `Draft`، `Active`، `Done`. وقتی همه Scheduleها انجام شوند وضعیت `Done` می‌شود.
- تکرار: `None`، `Daily`، `Weekly`. تاریخ گذشته در زمان ساخت Schedule رد می‌شود و تاریخ تکراری خطا می‌دهد.
- `Wrapper` ظرف پروژه است با نوع `Project`، `Phase`، `Routine`، `Etc`، درصد پیشرفت، وزن و KPI.
- `Resource` و `ResourceRate` منبع و نرخ را نگه می‌دارند.
- `OtpCode` و `RefreshToken` برای ورود هستند.
- در `TaskOrchestration` کلاس‌های `WorkFlow`، `KpiEngine`، `HealthEngine` و `SimulationEngine` وجود دارند.

## ساختار

```text
Supect.TaskPro.sln
src/Endpoints/Suspect.TaskPro.Endpoints.Api          API تسک، JWT، Swagger، Serilog
src/Endpoints/Supect.TaskPro.Endpoints.Auth.Api      Identity و ثبت موبایل
ApiGatewayService/Supect.TaskPro.GW.YARP.Api         YARP
src/AppCore/Domain                                   موجودیت و قانون دامنه
src/AppCore/DomainServices
src/AppCore/AppServices                              هندلر Command و Query
src/AppCore/Shared                                   قراردادها
src/Infra/Persistence/Postgres.Commands              EF نوشتن و مهاجرت
src/Infra/Persistence/Postgres.Queries               EF خواندن
src/Infra/PanelPayamak                               انتزاع پیامک و کاوه‌نگار
src/Infra/InternalServices/...UserApiCaller          فراخوانی Auth API
Fx/                                                  OysterFx: Aggregate، CommandBus، JWT، REST caller
src/tests/AppCore                                    xUnit و Moq
```

نام پوشه راهکار `Supect` است و فضای نام دامنه `Suspect.TaskPro` است. هر دو در کد همین‌طور مانده‌اند.

## تست و کانتینر

```powershell
dotnet test Supect.TaskPro.sln
```

پروژه‌های تست: `Suspect.TaskPro.AppCore.AppServices.Tests` و `Suspect.TaskPro.AppCore.AppServices.IntegrationTests`.

Dockerfile هر دو API روی `mcr.microsoft.com/dotnet/aspnet:9.0` است و پورت‌های `8080` و `8081` را باز می‌کند. زمینه بیلد، ریشه مخزن است.

## لاگ

Serilog روی Task API به کنسول، فایل موقت ویندوز، و در صورت بالا بودن Elasticsearch به ایندکس ماهانه می‌نویسد. Health check پکیج SQL Server در پروژه API ارجاع شده است؛ دیتابیس اجرا PostgreSQL است.