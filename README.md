# زمین X

زمین X نسل جدیدی از فریم‌ورک زمین است که با تمرکز بر **سادگی، ماژولار بودن، بازاستفاده، توسعه‌پذیری و یکدستی معماری** بازطراحی شده است.

این پروژه با هدف ارائه مجموعه‌ای از **BuildingBlockها، الگوهای کاربردی، یکپارچه‌سازی‌ها، ساختارهای آماده و اپلیکیشن‌های قابل اجرا** برای ساخت سیستم‌های مدرن بر پایه .NET طراحی شده است.

زمین X تلاش می‌کند بین **سادگی در استفاده** و **قدرت در توسعه‌پذیری** تعادل ایجاد کند تا توسعه‌دهنده بتواند بدون درگیر شدن با پیچیدگی‌های تکراری، روی حل مسئله و ساخت محصول تمرکز کند.

---

## هدف پروژه

زمین X با اهداف زیر طراحی شده است:

* کاهش پیچیدگی‌های تکراری در توسعه نرم‌افزار
* ایجاد یک معماری یکدست، قابل فهم و قابل توسعه
* فراهم کردن امکان انتخاب و جایگزینی پیاده‌سازی‌ها در مرزهای درست
* تسهیل توسعه سیستم‌های ماژولار
* کاهش هزینه شروع پروژه‌های جدید
* استانداردسازی الگوهای رایج در پروژه‌های .NET
* ارائه capabilityهای reusable و production-ready

---

## ساختار محصول

زمین X از پنج دسته اصلی تشکیل شده است:

### 1. BuildingBlocks

اجزای مستقل، self-contained و قابل reuse که می‌توانند بدون وابستگی به سایر بخش‌ها استفاده شوند.

ویژگی‌های BuildingBlockها:

* مستقل از context اپلیکیشن
* قابل publish به‌صورت package
* قابل استفاده در پروژه‌های مختلف
* متمرکز روی یک concern مشخص
* قابل توسعه و جایگزینی در صورت نیاز

نکته مهم:

* وابستگی به تکنولوژی مانع BuildingBlock بودن نیست
* abstraction پیش‌فرض نیست
* معیار اصلی، **استقلال مصرف** و **مرزبندی درست concern** است

---

### 2. ApplicationPatterns

الگوهای سطح اپلیکیشن که معمولاً از چند BuildingBlock تشکیل شده‌اند و یک رفتار تکرارشونده را استاندارد می‌کنند.

این بخش برای حل مسائل تکراری در سطح application طراحی می‌شود، نه برای تعریف primitiveهای فنی خام.

---

### 3. Integrations

اجزایی که مسئول ارتباط با سیستم‌های خارجی هستند، مانند:

* APIها
* سرویس‌های third-party
* message brokerها
* storage providerها
* providerهای بیرونی که application برای کارکرد خود به آن‌ها متکی است

---

### 4. Foundations

ساختارهای آماده و opinionated برای شروع سریع پروژه‌ها.

نمونه‌های این بخش:

* MonolithStructure
* ModularMonolith
* MicroserviceModule (در آینده)

Foundationها معمولاً از چند capability مختلف استفاده می‌کنند و یک نقطه شروع آماده برای ساخت application فراهم می‌کنند.

---

### 5. Applications

اپلیکیشن‌های آماده، قابل اجرا و ارزشمند که با استفاده از زمین X ساخته می‌شوند.

این بخش جایی است که BuildingBlockها، Patternها، Foundationها و Integrationها در کنار هم به یک محصول واقعی تبدیل می‌شوند.

---

## فلسفه طراحی

### استقلال اجزا

هر capability باید تا حد امکان self-contained باشد و در مرز concern خودش بماند.

### انتزاع فقط در صورت نیاز

abstraction زمانی ایجاد می‌شود که:

* چند پیاده‌سازی واقعی وجود داشته باشد
* یا نیاز واقعی به decoupling وجود داشته باشد
* یا application نباید به implementation خاصی گره بخورد

در غیر این صورت wrapper و abstraction اضافه تولید نمی‌شود.

### سادگی در استفاده

APIها باید:

* قابل فهم باشند
* predictable باشند
* کم‌پیچیدگی باشند
* رفتارهای پنهان و surprising نداشته باشند

### قابلیت توسعه

در صورت نیاز، امکان replace یا extend کردن behavior باید وجود داشته باشد، اما این توسعه‌پذیری نباید از ابتدا باعث over-engineering شود.

### مستندسازی به‌عنوان بخشی از محصول

در زمین X مستندات بخشی از خود capability هستند، نه چیزی جدا از آن.

هر capability باید علاوه بر کد، توضیح معماری، مرز concern، مدل استفاده و تصمیم‌های طراحی خود را نیز داشته باشد.

---

## خانواده‌های فعلی در BuildingBlocks

### 01. CrossCutting

Capabilityهایی که concernهای عمومی و reusable را پوشش می‌دهند.

نمونه‌ها:

* Object Mapper
* Serializer
* Translator
* Caching

این دسته معمولاً concernهایی را شامل می‌شود که در لایه‌های مختلف application قابل استفاده هستند و به context خاصی وابسته نیستند.

---

### 02. RuntimeAndRegistration

Capabilityهایی که مربوط به runtime composition، service registration و startup behavior هستند.

نمونه‌های فعلی:

#### DependencyInjection (Axon)

Capability مربوط به DI در زمین X با تمرکز بر:

* حذف wiring دستی
* استانداردسازی registration
* assembly scanning
* ساده‌سازی startup

نکته مهم:

Axon abstraction جدید غیرضروری ایجاد نمی‌کند و روی `IServiceCollection` کار می‌کند.

#### OpenApi (Lumen)

Capability مربوط به OpenAPI و API documentation در زمین X.

مسئولیت‌های Lumen:

* ثبت OpenAPI در ASP.NET Core
* expose کردن document endpoint
* مدیریت مسیر و naming document
* ترکیب UIهای مختلف برای نمایش API
* ساده‌سازی setup برای API documentation

ویژگی‌ها:

* استفاده از OpenAPI built-in در ASP.NET Core (.NET 10)
* استفاده از Options pattern استاندارد
* عدم استفاده از hackهایی مانند:

  * `OptionsWrapper`
  * `Options.Create`
  * `Replace`
* طراحی minimal و قابل توسعه
* امکان فعال‌سازی چند UI به‌صورت هم‌زمان
* جداسازی روشن بین registration و runtime

UIهای پشتیبانی‌شده:

* Scalar
* Swagger UI
* ReDoc

نکته مهم:

UIها capability مستقل نیستند؛ آن‌ها بخشی از Lumen و presentation layer آن هستند.

#### Logging

Capability مربوط به Logging در زمین X با تمرکز بر:

* setup و registration ساده Serilog
* استانداردسازی logging در سطح application
* فراهم کردن تجربه یکپارچه برای sinks، enrichers و contextual logging

ویژگی‌ها:

* استفاده از Serilog به‌عنوان implementation اصلی
* بدون abstraction اضافی
* builder سبک برای configuration
* پشتیبانی از:

  * Console
  * File
  * Seq
* پشتیبانی از:

  * CorrelationId
  * TraceId / SpanId
  * Application metadata
* پشتیبانی از contextual logging:

  * UserId / UserName
  * properties سفارشی
* startup logging ساده‌شده

نکته مهم:

Logging در این فاز provider-based نیست و به‌صورت setup-oriented و minimal طراحی شده است.

---

### 03. DomainAndApplicationPrimitives

این خانواده primitiveهای پایه دامنه و اپلیکیشن را نگه می‌دارد.

نمونه‌ها:

* Entity
* AggregateRoot
* ValueObject
* DomainEvent
* DomainException
* Result Pattern
* Application primitiveهای پایه

این بخش سنگ‌بنای طراحی applicationها و capabilityهای دامنه‌محور در زمین X است.

---

### 04. DataAndPersistence

این خانواده capabilityهای مربوط به داده، persistence و رفتارهای تکرارشونده لایه data را نگه می‌دارد.

#### Axiom

Axiom capability مربوط به لایه Data در زمین X است و یک foundation استاندارد برای abstraction و implementation دسترسی به داده ارائه می‌دهد.

Axiom با این اهداف طراحی شده است:

* جدا نگه داشتن application از implementationهای persistence
* پشتیبانی از read side و write side از ابتدا
* آماده بودن برای CQRS در صورت نیاز
* استانداردسازی paging و sorting
* ارائه implementation پایه برای EF Core
* پشتیبانی از providerهای مختلف
* ارائه audit پایه به‌صورت reusable

##### اجزای Axiom

**1. Data.Abstractions**

مدل‌ها و قراردادهای مشترک:

* `IDataAuditContext`
* `DefaultDataAuditContext`
* `PagedQuery`
* `PagedQuery<TSearch>`
* `PagedResult<TData>`

**2. Data.Write.Abstractions**

قراردادهای write side:

* `IWriteRepository<TAggregate, TId>`
* `IUnitOfWork`

**3. Data.Read.Abstractions**

قراردادهای read side:

* `IReadRepository<TEntity, TId>`

**4. Data.EntityFrameworkCore**

هسته مشترک EF Core:

* registration entry point
* builderهای پایه
* provider validation
* interceptor registration
* auditing infrastructure
* model configuration helperهای مشترک

**5. Data.EntityFrameworkCore.Write**

پیاده‌سازی write side روی EF Core:

* `EfUnitOfWork<TDbContext>`
* `EfWriteRepository<TEntity, TId, TDbContext>`
* graph loading hook با `CreateAggregateQuery()`
* write repository scanning

**6. Data.EntityFrameworkCore.Read**

پیاده‌سازی read side روی EF Core:

* `EfReadRepository<TEntity, TId, TDbContext>`
* paging helper
* sorting helper
* default sorting fallback
* read repository scanning

**7. Providerها**

در نسخه فعلی providerهای زیر ارائه شده‌اند:

* SQL Server
* PostgreSQL

##### مدل طراحی Axiom

Axiom از ابتدا read و write را جدا می‌بیند تا:

* CQRS در صورت نیاز به‌راحتی قابل استفاده باشد
* behaviorهای read و write روشن و مستقل بمانند
* registration و implementation هر سمت ساده و قابل فهم باشد

Axiom فقط در مرزهای لازم abstraction ارائه می‌دهد؛ یعنی جایی که application نباید به EF Core یا provider خاص وابسته شود.

##### Paging و Sorting در Axiom

Axiom برای query side یک مدل یکدست برای paging ارائه می‌دهد:

* `PagedQuery`
* `PagedQuery<TSearch>`
* `PagedResult<TData>`

ویژگی‌های این مدل:

* شماره صفحه و اندازه صفحه
* امکان کنترل `IncludeTotalCount`
* sorting با `SortBy` و `SortDescending`
* fallback sorting در implementation
* helper مرکزی برای ساخت خروجی page شده

##### Audit در Axiom

مدل فعلی audit:

* `CreatedAt`
* `CreatedBy`
* `ModifiedAt`
* `ModifiedBy`

ویژگی‌ها:

* propertyها به‌صورت shadow property تعریف می‌شوند
* مقداردهی از طریق `SaveChangesInterceptor` انجام می‌شود
* user/time از `IDataAuditContext` گرفته می‌شود
* وابستگی مستقیم به user management یا business concern وجود ندارد

##### Sampleهای Axiom

نسخه فعلی Axiom شامل دو sample end-to-end است:

* SQL Server sample
* PostgreSQL sample

این sampleها برای validate کردن این flowها استفاده می‌شوند:

* registration کامل
* read/write DbContext
* repository scanning
* paging و sorting
* auditing
* provider behavior

---

## ساختار پروژه

ساختار فعلی ریپو به‌صورت کلی به شکل زیر است:

```text
src/
  00.BuildingBlocks/
    01.CrossCutting/
      Caching/
        Abstractions/
        Inmemory/
        Redis/
        SqlServer/
      ObjectMapper/
        Abstractions/
        AutoMapper/
      Serializer/
        Abstractions/
        Microsoft/
        Newtonsoft/
      Translations/
        Abstractions/
        Parrot/
        SqlServer/

    02.RuntimeAndRegistration/
      OpenApi/
        Lumen/
        Scalar/
        Swagger/
        Redoc/
      Logging/

    03.DomainAndApplicationPrimitives/
      01.Domain/
        Kernel.slnx
        src/
          ZaminX.BuildingBlocks.Domain/
        tests/
          ZaminX.BuildingBlocks.Domain.Tests/

    04.DataAndPersistence/
      Axiom/
        Axiom.slnx
        src/
          ZaminX.BuildingBlocks.Data.Abstractions/
          ZaminX.BuildingBlocks.Data.Write.Abstractions/
          ZaminX.BuildingBlocks.Data.Read.Abstractions/
          ZaminX.BuildingBlocks.Data.EntityFrameworkCore/
          ZaminX.BuildingBlocks.Data.EntityFrameworkCore.Write/
          ZaminX.BuildingBlocks.Data.EntityFrameworkCore.Read/
          ZaminX.BuildingBlocks.Data.EntityFrameworkCore.SqlServer/
          ZaminX.BuildingBlocks.Data.EntityFrameworkCore.PostgreSql/
        tests/
          ZaminX.BuildingBlocks.Data.Abstractions.Tests/
          ZaminX.BuildingBlocks.Data.EntityFrameworkCore.Tests/
        samples/
          ZaminX.Samples.Data.SqlServer/
          ZaminX.Samples.Data.PostgreSql/
```

---

## مستندات

تمام مستندات در مسیر زیر قرار دارند:

```text
docs/
```

ساختار docs شامل این بخش‌ها است:

* vision
* architecture
* modules
* decisions
* project-state

در زمین X، docs مرجع اصلی توضیح capabilityها، تصمیم‌های طراحی و وضعیت پروژه هستند.
README نقش نمای کلی و entry point را دارد، نه جایگزین کامل docs.

---

## وضعیت پروژه

زمین X در حال حاضر در فاز:

* تثبیت معماری
* توسعه capabilityهای اصلی
* ایجاد consistency در design
* ساخت foundationهای reusable برای استفاده در پروژه‌های واقعی

---

## تمرکز فعلی

تمرکز فعلی پروژه روی این موارد است:

* تثبیت CrossCutting capabilityها
* تکمیل و تثبیت Axon
* طراحی و تثبیت Lumen
* طراحی و تثبیت Logging
* تثبیت Domain primitives
* تثبیت capability داده (Axiom) شامل abstraction، implementation و providerها
* تعریف و توسعه ApplicationPatterns
* تکمیل مستندات و هم‌راستاسازی آن‌ها با کد

---

## نقشه راه کوتاه‌مدت

* تثبیت کامل taxonomy
* تکمیل BuildingBlockهای اصلی
* تثبیت و گسترش Data & Persistence (Axiom)
* توسعه Foundationهای اولیه
* توسعه Applicationهای اولیه
* publish اولیه capabilityها
* افزایش پوشش تست و بهبود developer experience

---

## موارد خارج از scope فعلی

در فاز فعلی، تمرکز پروژه روی ساخت capabilityهای پایه است. بنابراین برخی concerns عمداً خارج از scope نگه داشته شده‌اند یا هنوز در مرحله بعدی قرار دارند، مانند:

* outbox و integration persistence patternها
* soft delete عمومی
* multi-tenancy عمومی
* query DSL عمومی
* providerهای بیشتر برای Axiom
* advanced transaction orchestration
* applicationهای نهایی گسترده

---

## جهت کلی پروژه

زمین X قرار نیست صرفاً یک مجموعه helper و utility باشد.
هدف آن ساخت یک ecosystem منسجم از capabilityهای reusable است که:

* معماری تمیز را به‌صورت عملی enforce کند
* هزینه شروع پروژه را کاهش دهد
* تکرارهای رایج را حذف کند
* توسعه را سریع‌تر و قابل پیش‌بینی‌تر کند
* و در عین حال flexibility لازم برای توسعه سیستم‌های واقعی را حفظ کند

---

## لایسنس

MIT
