# Axon (DependencyInjection)

## معرفی

**Axon** نام محصولی capability فنی `DependencyInjection` در زمین X است.

این capability برای حل یکی از مشکلات رایج در توسعه سیستم‌های مبتنی بر .NET طراحی شده است:

> پیچیدگی، تکرار و عدم یکپارچگی در registration سرویس‌ها در DI container

Axon تلاش می‌کند این مسئله را با یک مدل ساده، قابل پیش‌بینی و policy-driven حل کند.

---

## مسئله

در اکثر پروژه‌ها، registration سرویس‌ها به مرور زمان:

* پراکنده در چندین فایل
* وابسته به conventionهای غیررسمی
* تکراری
* وابسته به حافظه توسعه‌دهنده
* و مستعد خطا

می‌شود.

نمونه‌های رایج:

```csharp
services.AddScoped<IOrderService, OrderService>();
services.AddScoped<IUserService, UserService>();
services.AddTransient<INotificationService, NotificationService>();
```

با رشد پروژه، این pattern:

* scale نمی‌شود
* maintain نمی‌شود
* و به‌راحتی دچار drift می‌شود

---

## هدف Axon

Axon برای این طراحی شده است که:

* registration را استاندارد کند
* wiring دستی را حذف کند
* setup را ساده کند
* رفتار سیستم را قابل پیش‌بینی کند
* و از drift جلوگیری کند

---

## جایگاه معماری

* **Category:** BuildingBlocks
* **Family:** RuntimeAndRegistration
* **Technical Capability:** DependencyInjection
* **Product Name:** Axon

### چرا RuntimeAndRegistration؟

Axon با این concernها سروکار دارد:

* runtime composition
* service registration
* startup configuration

بنابراین در CrossCutting قرار نمی‌گیرد، چون:

* رفتار مصرفی مستقیم ارائه نمی‌دهد
* concern آن usage-time نیست
* concern آن setup-time است

---

## boundary

### داخل scope

Axon مسئول این موارد است:

* marker-based registration
* assembly scanning
* convention-based registration
* lifetime mapping
* duplicate handling
* filtering
* open generic registration
* startup validation

### خارج از scope

Axon مسئول این موارد نیست:

* service resolution abstraction
* business logic execution
* application orchestration
* interception / AOP
* provider-specific integration registration
* full dependency graph validation

---

## مدل طراحی

### 1. facade روی IServiceCollection

Axon intentionally ساده طراحی شده است.

```csharp
services.AddZaminXDependencyInjection(options =>
{
    options.AddAssemblyContaining<Program>();
});
```

هیچ abstraction جدیدی روی container ایجاد نمی‌شود.

---

### 2. marker-based registration (مسیر اصلی)

Axon از marker interfaceها برای تعیین lifetime استفاده می‌کند:

* ITransientDependency
* IScopedDependency
* ISingletonDependency

```csharp
public sealed class OrderService : IOrderService, IScopedDependency
{
}
```

این approach:

* explicit است
* ساده است
* قابل درک است
* و ambiguity کمی دارد

---

### 3. convention-based registration (opt-in)

در صورت نیاز:

```csharp
options.EnableConventionBasedRegistration();
```

Axon می‌تواند این mapping را تشخیص دهد:

* OrderService → IOrderService
* UserRepository → IUserRepository

این feature به‌صورت پیش‌فرض فعال نیست.

---

### 4. policy-driven behavior

Axon به‌جای behavior مبهم، policy شفاف ارائه می‌دهد:

#### duplicate handling

* Skip
* Replace
* Throw

پیشنهاد:

```text
Throw
```

---

### 5. filtering

Axon باید کنترل کامل روی scanning بدهد:

* exclude type
* exclude namespace
* predicate filtering

---

### 6. startup validation

Axon باید fail-fast باشد:

* بدون assembly
* بدون strategy فعال
* چند lifetime marker
* duplicate conflict

---

### 7. open generic support

Axon می‌تواند genericها را register کند، اما:

* باید قابل کنترل باشد
* behavior آن مشخص باشد

---

### 8. self registration

در صورت نیاز:

* implementation به‌عنوان خودش هم ثبت شود

---

## بازطراحی نسبت به نسخه قدیمی

### حفظ شده

* marker-based registration
* assembly scanning
* IServiceCollection extension

### اصلاح شده

* namingهای اشتباه
* نبود policy
* نبود validation
* placement اشتباه در CrossCutting
* dependency به assembly loading قدیمی

---

## اصول طراحی

Axon بر اساس این اصول ساخته می‌شود:

### سادگی

API باید قابل فهم باشد.

### شفافیت

behavior باید قابل پیش‌بینی باشد.

### حداقل abstraction

abstraction فقط در صورت نیاز.

### کنترل‌پذیری

registration باید قابل کنترل باشد.

### مستندسازی

docs بخشی از محصول است.

---

## نمونه کامل

```csharp
services.AddZaminXDependencyInjection(options =>
{
    options.AddAssemblyContaining<Program>();
    options.EnableConventionBasedRegistration();
    options.DuplicateRegistrationBehavior = DuplicateRegistrationBehavior.Throw;
});
```

---

## نتیجه

Axon یک capability برای:

> composition + registration + startup simplification

است.

موفقیت آن نه در featureهای زیاد، بلکه در:

* سادگی
* consistency
* و جلوگیری از chaos در registration

است.