# Axiom

## معرفی

Axiom capability مربوط به لایه Data در زمین X است و یک foundation استاندارد برای abstraction و implementation دسترسی به داده ارائه می‌دهد.

این capability با هدف ایجاد یک مدل reusable، قابل توسعه و CQRS-friendly طراحی شده است تا application بتواند بدون وابستگی مستقیم به EF Core یا provider خاص، با قراردادهای write و read کار کند.

---

## جایگاه

* خانواده: `BuildingBlocks`
* زیرخانواده: `04.DataAndPersistence`

دلیل این جایگاه:

* concern اصلی آن یک concern پایه و reusable در لایه data است
* به application خاص وابسته نیست
* به‌صورت self-contained قابل استفاده است
* implementationهای مختلف می‌توانند روی abstractionهای آن سوار شوند

---

## مسئله‌ای که حل می‌کند

Axiom این مسائل تکرارشونده را حل می‌کند:

* تعریف contractهای استاندارد برای write side
* تعریف contractهای استاندارد برای read side
* استانداردسازی paging و sorting
* جداسازی application از EF Core
* ارائه پیاده‌سازی پایه برای EF Core
* ثبت ساده و قابل فهم DbContextهای write و read
* پشتیبانی از providerهای مختلف
* فراهم کردن audit پایه به‌صورت reusable

---

## اجزای اصلی

### 1. Data.Abstractions

این بخش مدل‌ها و قراردادهای مشترک را نگه می‌دارد:

* `IDataAuditContext`
* `DefaultDataAuditContext`
* `PagedQuery`
* `PagedQuery<TSearch>`
* `PagedResult<TData>`

### 2. Data.Write.Abstractions

این بخش قراردادهای write side را نگه می‌دارد:

* `IUnitOfWork`
* `IWriteRepository<TAggregate, TId>`

### 3. Data.Read.Abstractions

این بخش قراردادهای read side را نگه می‌دارد:

* `IReadRepository<TEntity, TId>`

### 4. Data.EntityFrameworkCore

این بخش implementation مشترک EF Core را ارائه می‌دهد:

* registration entry point
* builderهای پایه
* provider validation
* interceptor registration
* auditing infrastructure

### 5. Data.EntityFrameworkCore.Write

این بخش implementation write side روی EF Core را ارائه می‌دهد:

* `EfUnitOfWork<TDbContext>`
* `EfWriteRepository<TEntity, TId, TDbContext>`
* write repository scanning

### 6. Data.EntityFrameworkCore.Read

این بخش implementation read side روی EF Core را ارائه می‌دهد:

* `EfReadRepository<TEntity, TId, TDbContext>`
* paging helper
* sorting helper
* read repository scanning

### 7. Providerها

در نسخه فعلی providerهای زیر برای EF Core ارائه شده‌اند:

* SQL Server
* PostgreSQL

---

## مدل طراحی

### تفکیک Read و Write

Axiom از ابتدا read side و write side را جدا می‌بیند تا:

* CQRS در صورت نیاز به‌راحتی قابل پیاده‌سازی باشد
* behaviorهای متفاوت read و write در لایه data روشن بمانند
* registration و implementation هر سمت مستقل و قابل فهم بماند

### abstraction فقط در مرز لازم

Axiom فقط در مرزهایی abstraction ارائه می‌دهد که واقعاً لازم‌اند:

* application نباید به EF Core گره بخورد
* چند implementation واقعی برای persistence قابل تصور است
* providerها باید قابل تعویض باشند

### implementation پایه روی EF Core

EF Core در Axiom implementation اصلی نسخه فعلی است، اما API مصرفی Axiom application را به EF Core وابسته نمی‌کند.

### provider-specific registration

انتخاب provider روی builder مربوط به EF Core انجام می‌شود، نه روی builder اصلی DataAccess. این موضوع باعث می‌شود registration خواناتر و مرزبندی concernها روشن‌تر باشد.

---

## Audit

Axiom از audit پایه در سطح EF Core پشتیبانی می‌کند.

مدل فعلی audit:

* `CreatedAt`
* `CreatedBy`
* `ModifiedAt`
* `ModifiedBy`

ویژگی‌های این مدل:

* propertyها به‌صورت shadow property تعریف می‌شوند
* مقداردهی از طریق `SaveChangesInterceptor` انجام می‌شود
* user/time از `IDataAuditContext` گرفته می‌شود
* وابستگی مستقیم به user management یا concernهای business وجود ندارد

---

## Paging و Sorting

Axiom برای query side یک مدل یکدست برای paging ارائه می‌دهد:

* `PagedQuery`
* `PagedQuery<TSearch>`
* `PagedResult<TData>`

ویژگی‌ها:

* امکان دریافت شماره صفحه و اندازه صفحه
* امکان کنترل `IncludeTotalCount`
* امکان sorting با `SortBy` و `SortDescending`
* امکان fallback sorting در implementation
* helper مرکزی برای ساخت خروجی page شده

---

## Registration Model

ورودی اصلی registration در Axiom:

* `AddZaminXDataAccess(...)`

برای EF Core:

* `UseEntityFrameworkCore<TDbContext>(...)`
* `UseReadEntityFrameworkCore<TDbContext>(...)`

برای providerها:

* `WithSqlServer(...)`
* `WithPostgreSql(...)`

ویژگی‌های این مدل:

* builder سبک
* registration قابل فهم
* validation زودهنگام
* پنهان‌سازی جزئیات provider از لایه‌های بالاتر

---

## Sampleها

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

## موارد خارج از scope

در نسخه فعلی Axiom این موارد جزو scope نیستند:

* ChangeDataLog
* business auditing
* soft delete
* outbox
* multi-tenancy
* specification pattern عمومی
* query DSL عمومی
* domain event dispatch
* advanced transaction orchestration

---

## وضعیت فعلی

وضعیت فعلی Axiom:

* طراحی معماری تثبیت شده
* implementation نسخه اول انجام شده
* sampleهای SQL Server و PostgreSQL آماده شده‌اند
* مستندسازی capability در حال تثبیت است

---

## نسبت با سایر capabilityها

Axiom:

* از Domain primitives استفاده مستقیم اجباری نمی‌کند
* به EF Core implementation به‌عنوان implementation پایه تکیه دارد
* می‌تواند در Foundations و Applications به‌عنوان data foundation استفاده شود
* می‌تواند در آینده implementationهای دیگری غیر از EF Core هم دریافت کند

---

## اهداف نسخه اول

اهداف نسخه اول Axiom:

* ارائه abstractionهای پایه read/write
* ارائه implementation پایه برای EF Core
* پشتیبانی از SQL Server و PostgreSQL
* استانداردسازی paging و sorting
* ارائه audit پایه
* ارائه sampleهای قابل اجرا

---

## تصمیم‌های باز

مواردی که می‌توانند در آینده توسعه پیدا کنند:

* transaction abstraction جداگانه
* projection-oriented read contracts
* filtering model عمومی
* concurrency helpers
* row version conventions
* providerهای بیشتر
