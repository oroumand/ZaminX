# Project State - Zamin X

## وضعیت کلی پروژه

زمین X در حال حاضر در مرحله:

* طراحی معماری
* تثبیت ساختار
* پیاده‌سازی تدریجی capabilityها

قرار دارد.

تمرکز روی ایجاد یک foundation پایدار برای توسعه آینده است.

---

## وضعیت BuildingBlocks

### CrossCutting

Capabilityهای اصلی:

* Mapper
* Serializer (Prism)
* Translator (Parrot)

وضعیت:

* طراحی تثبیت شده
* پیاده‌سازی اولیه انجام شده
* در حال consolidation و بهبود consistency

---

## RuntimeAndRegistration

این خانواده شامل capabilityهای مربوط به:

* registration
* runtime setup
* wiring

است.

---

### DependencyInjection (Axon)

وضعیت:

* طراحی کامل
* پیاده‌سازی انجام شده
* در حال استفاده به‌عنوان reference capability

---

### OpenApi (Lumen)

وضعیت فعلی:

* طراحی معماری کامل
* scope مشخص
* implementation اولیه انجام شده
* در حال refinement

---

## تعریف Lumen

Lumen capability مربوط به:

* OpenAPI registration
* API documentation exposure
* UI composition

است.

---

## تصمیم‌های معماری Lumen

### 1. جایگاه

* خانواده: RuntimeAndRegistration
* دلیل: تمرکز روی setup و runtime

---

### 2. مدل طراحی

* بدون abstraction مصرفی
* بدون provider model
* builder سبک
* separation بین registration و runtime

---

### 3. Options

Lumen از Options pattern استاندارد استفاده می‌کند:

* Bind از IConfiguration
* Configure از code
* Validation

و از موارد زیر استفاده نمی‌کند:

* OptionsWrapper
* Options.Create
* Replace

---

### 4. UIها

UIها:

* capability مستقل نیستند
* integration داخلی هستند

---

### 5. ساختار

* Core project (Lumen)
* UI projects:

  * Scalar
  * Swagger
  * Redoc
* Sample

---

### 6. اهداف نسخه اول

* ثبت OpenAPI
* expose document
* فعال‌سازی UIها
* setup ساده

---

### 7. Non-goals

* abstraction پیچیده
* provider model
* multi-document پیچیده
* customization عمیق

### Pulse

وضعیت: ✅ تکمیل شده (Initial Version)

شرح:

Capability مربوط به utilityهای عمومی شامل:

* Persian DateTime
* Guard Clauses
* Extension Methods منتخب

ویژگی‌ها:

* بدون abstraction
* static-first
* بدون dependency خارجی
* تمرکز بر سادگی و usability

جایگزین:

* Zamin.Utilities (deprecated)

---

## 🆕 Logging

### وضعیت فعلی

* طراحی معماری کامل
* scope مشخص
* implementation انجام شده
* sample آماده
* در حال تثبیت docs

---

### تعریف Logging

Logging capability مربوط به:

* Serilog registration
* logging pipeline setup
* enrichment
* contextual logging
* request logging
* startup logging

است.

---

### تصمیم‌های معماری Logging

#### 1. جایگاه

* خانواده: RuntimeAndRegistration
* دلیل: تمرکز روی setup و runtime

---

#### 2. مدل طراحی

* بدون abstraction
* بدون provider model
* builder سبک
* integration مستقیم با Serilog

---

#### 3. Options

* استفاده از Options pattern استاندارد
* Bind از IConfiguration
* Configure از code
* بدون:

  * OptionsWrapper
  * Options.Create
  * Replace

---

#### 4. Enrichment

پشتیبانی از:

* MachineName
* EnvironmentName
* ThreadId
* CorrelationId
* TraceId / SpanId
* Application metadata

---

#### 5. Contextual Logging

پشتیبانی از:

* UserId / UserName
* claim-based helpers
* properties سفارشی
* resolver-based per-request model

---

#### 6. Sinks

نسخه اول:

* Console
* File
* Seq

---

#### 7. Startup Logging

* bootstrap logger
* run wrapper
* fatal exception handling

---

### اهداف نسخه اول

* setup ساده logging
* یکپارچه‌سازی Serilog
* کاهش boilerplate
* ارائه API قابل فهم

---

### Non-goals

* abstraction logging
* multi-provider
* plugin system پیچیده
* sinkهای متعدد خارج از scope

---
## Axiom (Data & Persistence)

**وضعیت:** 🟢 طراحی و پیاده‌سازی اولیه کامل شده
**دسته‌بندی:** BuildingBlocks / DataAndPersistence

Axiom capability مربوط به لایه Data در زمین X است و یک foundation استاندارد برای abstraction و implementation دسترسی به داده ارائه می‌دهد.

### اجزای اصلی

* Data.Abstractions (Paging + Audit Context)
* Data.Write.Abstractions (Repository + UnitOfWork)
* Data.Read.Abstractions (Read Repository)
* EF Core Shared Infrastructure
* EF Core Read / Write Implementation
* SQL Server Provider
* PostgreSQL Provider

### ویژگی‌ها

* جداسازی کامل Read و Write (CQRS-ready)
* abstraction حداقلی و هدفمند
* paging و sorting استاندارد
* default sorting fallback
* repository scanning بدون dependency خارجی
* provider-agnostic design
* auditing پایه با shadow property + interceptor

### وضعیت فعلی

* implementation نسخه اول کامل شده
* sampleهای SQL Server و PostgreSQL آماده هستند
* registration flow تثبیت شده
* مستندسازی انجام شده

## تصمیم‌های اخیر

* انتقال DI به RuntimeAndRegistration
* تثبیت Axon
* حذف Scalar به‌عنوان capability مستقل
* تعریف Lumen
* تعریف Logging
* استانداردسازی Options usage
* حذف over-engineering در runtime capabilityها
* تعریف و تثبیت Axiom به‌عنوان capability لایه Data در BuildingBlocks

---

## ریسک‌ها

* over-engineering
* پیچیده شدن runtime capabilityها
* misuse از abstraction

---

## Domain Primitives (Kernel)

وضعیت: ✅ پیاده‌سازی شده (نسخه اولیه)

این خانواده شامل primitiveهای پایه دامنه است:

* Entity
* AggregateRoot
* ValueObject
* DomainEvent
* DomainException

ویژگی‌ها:

* طراحی minimal و بدون over-engineering
* بدون وابستگی به application یا infrastructure
* آماده برای استفاده در application layer

نکات مهم:

* DomainEvent به‌صورت marker interface طراحی شده
* AggregateRoot مسئول نگهداری eventها است (نه dispatch)
* ValueObject از الگوی self-referencing generic استفاده می‌کند
* DomainException دارای Code متنی است (بدون key یا numeric id)

وضعیت بعدی:

➡️ ورود به Application Primitives (Relay)

---

## 🆕 Application Primitives (Relay)

وضعیت: ✅ پیاده‌سازی شده (نسخه اول کامل)

این خانواده شامل primitiveهای لایه Application است که برای orchestration و اجرای use caseها استفاده می‌شوند.

---

### تعریف Relay

Relay مجموعه‌ای از primitiveهای Application برای:

* Message-based architecture
* Command / Query / Event handling
* Mediator pattern
* Pipeline processing

است.

---

### اجزای اصلی

#### Messaging

* ICommand
* IQuery
* IEvent
* IMessage (base contract)

---

#### Handlers

* ICommandHandler
* IQueryHandler
* IEventHandler

---

#### Result Pattern

* Result
* Result<T>
* پشتیبانی از چندین error
* حذف exception از flow اصلی application

---

#### Mediator

* IMediator
* Send برای requestها
* Publish برای eventها
* orchestration کامل execution

---

#### Pipeline Behaviors

پشتیبانی از:

* pre-processing
* post-processing
* short-circuit execution

---

### Built-in Behaviors

نسخه اول شامل:

* RequestTelemetryBehavior
* ValidationBehavior
* ExceptionToResultBehavior

ویژگی‌ها:

* فعال به‌صورت پیش‌فرض
* قابل فعال/غیرفعال شدن
* ordering قابل کنترل

---

### Custom Behavior

پشتیبانی از:

```csharp
options.AddOpenBehavior(typeof(MyBehavior<,>));
```

ویژگی‌ها:

* بدون نیاز به registration دستی
* ثبت خودکار در DI
* اضافه شدن خودکار به pipeline

---

### Dependency Injection

Relay شامل registration استاندارد است:

#### AddZaminXApplication

مسئول:

* ثبت IMediator
* ثبت built-in behaviorها
* ثبت custom behaviorها

---

#### AddZaminXApplicationHandlers

مسئول:

* scan و register handlerها از assembly

---

### Modular Monolith Support

طراحی به‌صورت کامل سازگار با:

* modular monolith

الگو:

```csharp
services.AddZaminXApplication();

services.AddZaminXApplicationHandlers(typeof(ModuleMarker).Assembly);
```

هر ماژول:

* handlerهای خودش را register می‌کند
* dependencyهای خودش را مدیریت می‌کند

---

### Validation

Relay از abstraction داخلی استفاده می‌کند:

```csharp
IMessageValidator<TMessage>
```

ویژگی‌ها:

* بدون وابستگی مستقیم به FluentValidation
* امکان استفاده از adapter

---

### Exception Handling

* exceptionها به Result تبدیل می‌شوند
* exception خام از Application خارج نمی‌شود

---

### ASP.NET Core Integration

در پروژه جدا:

* ZaminX.BuildingBlocks.Application.AspNetCore

ویژگی:

* تبدیل Result به HTTP Response

---

### ویژگی‌های کلیدی

* طراحی minimal و بدون over-engineering
* pipeline قابل توسعه
* ordering deterministic
* separation بین infrastructure و module registration
* سازگار با Clean Architecture

---

### Non-goals

* message bus
* distributed messaging
* retry policy
* transaction management
* caching
* event pipeline (در این نسخه)

---

## مسیر آینده

* تثبیت Lumen
* تثبیت Logging
* بهبود docs
* شروع Application Services (UseCase layer)

---

## جمع‌بندی

زمین X در حال حرکت به سمت:

* سادگی
* modularity
* consistency

است.

Application Primitives (Relay) به‌عنوان foundation لایه Application به‌طور کامل پیاده‌سازی شده و پروژه آماده ورود به فاز Application Services است.

---

## 🆕 IdentityAndUsers

### وضعیت فعلی

* family تعریف شده
* Persona به‌عنوان capability اولیه این خانواده طراحی شده
* scope و naming نهایی شده
* structure اولیه solution و projectها مشخص شده
* implementation اولیه در حال تثبیت است
* sample اولیه تعریف شده
* docs capability آماده شده‌اند

---

### تعریف Persona

Persona capability مربوط به:

* current user access
* claim reading
* authentication state inspection
* web request user metadata access

است.

Persona در نسخه فعلی یک BuildingBlock برای **دسترسی به اطلاعات کاربر جاری** است، نه یک سیستم کامل برای UsersManagement، Authentication یا Authorization.

---

### تصمیم‌های معماری Persona

#### 1. جایگاه

* خانواده: `06.IdentityAndUsers`
* دلیل: concern این capability به semantics کاربر جاری و identity-adjacent behavior مربوط است و CrossCutting خالص یا Runtime setup صرف نیست

---

#### 2. مدل طراحی

* contract + implementation
* یک contract پایه
* یک contract وبی
* یک implementation روشن برای ASP.NET Core
* بدون provider model
* بدون abstraction اضافه خارج از مرز لازم

---

#### 3. قراردادها

قراردادهای نسخه اول:

* `ICurrentUser`
* `IWebCurrentUser`

`ICurrentUser` اطلاعات پایه current user را ارائه می‌دهد و `IWebCurrentUser` metadata وبی مانند `IpAddress` و `UserAgent` را اضافه می‌کند.

---

#### 4. ساختار

ساختار نسخه اول Persona:

* `Persona.Abstractions`
* `Persona.AspNetCore`
* `ZaminX.Samples.IdentityAndUsers.Persona.AspNetCore`

---

#### 5. Options

Persona از Options pattern استاندارد استفاده می‌کند:

* Bind از IConfiguration
* Configure از code
* بدون الگوهای hacky
* بدون configuration model سنگین

Options در این capability برای mapping claim typeها و fallback valueها استفاده می‌شود.

---

#### 6. اهداف نسخه اول

* استانداردسازی current user access
* حذف وابستگی مستقیم مصرف‌کننده به `HttpContext`
* ارائه API ساده و قابل‌فهم
* پشتیبانی از metadata وبی پرکاربرد
* ارائه registration روشن و minimal

---

#### 7. Non-goals

* authentication flow
* authorization policy engine
* user CRUD
* profile management
* account lifecycle
* external identity provider integration
* provider model پیچیده

---

### جمع‌بندی

Persona اولین capability خانواده `IdentityAndUsers` در زمین X است و مرز این خانواده را از همان ابتدا روی concernهای روشن و reusable نگه می‌دارد.

نسخه فعلی Persona بر حل مسئله current user access متمرکز است و عمداً وارد concernهای بزرگ‌تر identity subsystem نمی‌شود.


### ApplicationPatterns
وضعیت ApplicationPatterns

این دسته شامل الگوهای reusable سطح application است که روی BuildingBlockهای موجود سوار می‌شوند و رفتارهای تکرارشونده را استاندارد می‌کنند.

HandlerExecution

وضعیت:

طراحی نهایی انجام شده
ساختار سلوشن و پروژه‌ها تعریف شده
implementation اولیه انجام شده
تست‌های پایه نوشته شده
مستندات capability آماده شده
تعریف HandlerExecution

HandlerExecution یک Application Pattern reusable برای استانداردسازی تجربه پیاده‌سازی handlerها در زمین X است.

این capability:

primitive جدید برای handler تعریف نمی‌کند
abstraction جدید غیرضروری اضافه نمی‌کند
mediator جدید یا pipeline جدید ارائه نمی‌دهد
روی capabilityهای موجود مانند Application، Axiom، ObjectMapper، Serializer، Translator و Persona سوار می‌شود

هدف آن:

کاهش boilerplate در handlerها
یکدست‌سازی ساخت Result
ارائه dependency bundle تایپ‌شده برای dependencyهای پرتکرار
ارائه base classهای ساده برای command handlerها و query handlerها
نگه‌داری context محدود و کنترل‌شده برای execution هر handler
اجزای نسخه فعلی
HandlerServices
ResultContext
ApplicationHandlerBase
CommandHandlerBase
QueryHandlerBase
نکات مهم طراحی
پروژه Abstractions جدا برای این capability نداریم
service locator استفاده نشده است
dependencyها explicit و constructor-based هستند
current user به‌صورت optional و از Persona استفاده می‌شود
logger از طریق ILoggerFactory در base class و به‌صورت lazy ساخته می‌شود
ResultContext eager نیست و فقط در صورت نیاز materialize می‌شود
command handlerها به write repository و IUnitOfWork وابسته‌اند
query handlerها به read repository وابسته‌اند