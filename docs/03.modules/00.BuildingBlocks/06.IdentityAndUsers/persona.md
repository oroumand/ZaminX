# Persona

## معرفی

Persona یک capability در خانواده `06.IdentityAndUsers` در زمین X است که دسترسی استاندارد به اطلاعات کاربر جاری را فراهم می‌کند.

این capability برای حل concern مربوط به **Current User Access** طراحی شده است؛ یعنی application بتواند بدون وابستگی مستقیم به `HttpContext` و `ClaimsPrincipal`، اطلاعات کاربر جاری را به‌شکل یکدست و قابل تزریق دریافت کند.

Persona در نسخه فعلی یک building block محدود، opinionated و host-aware است که implementation اصلی آن برای ASP.NET Core ارائه می‌شود.

---

## مسئله

در applicationهای واقعی، بخش‌های مختلف سیستم معمولاً به اطلاعات کاربر جاری نیاز دارند، از جمله:

* شناسه کاربر
* نام کاربر
* نام و نام خانوادگی
* وضعیت احراز هویت
* claimهای سفارشی
* IP
* UserAgent

اگر این اطلاعات در هر نقطه مستقیماً از `HttpContext` یا `ClaimsPrincipal` خوانده شوند، مشکلات زیر ایجاد می‌شود:

* وابستگی مستقیم به ASP.NET Core
* تکرار logic خواندن claimها
* نشت concern وب به لایه‌های دیگر
* کاهش تست‌پذیری
* چندپارگی API در سطح پروژه

Persona این مسئله را با یک مرز مصرفی روشن و یک implementation وبی حل می‌کند.

---

## جایگاه در taxonomy

* دسته: `BuildingBlocks`
* خانواده: `06.IdentityAndUsers`
* نوع concern: current user access
* مدل: contract + host-specific implementation

Persona یک capability از جنس CrossCutting عمومی نیست، چون مستقیماً به semantics کاربر و هویت جاری نزدیک است.

همچنین یک subsystem کامل identity هم نیست، چون authentication، authorization و user lifecycle را حل نمی‌کند.

به همین دلیل جایگاه آن در خانواده `06.IdentityAndUsers` است.

---

## مرز capability

### در scope

Persona در نسخه فعلی این مسئولیت‌ها را دارد:

* ارائه contract برای current user
* ارائه contract وبی برای current user
* خواندن claimها
* تشخیص `IsAuthenticated`
* دسترسی به `IpAddress`
* دسترسی به `UserAgent`
* registration ساده در ASP.NET Core
* configuration سبک برای mapping claim typeها و fallbackها

### خارج از scope

Persona در نسخه فعلی این موارد را حل نمی‌کند:

* authentication flow
* login orchestration
* token issuing
* authorization
* policy evaluation
* permission system
* user CRUD
* profile management
* account lifecycle
* external identity provider integration

---

## مدل طراحی

مدل طراحی Persona در نسخه اول به‌صورت زیر است:

* یک پروژه قراردادها
* یک پروژه implementation برای ASP.NET Core
* یک sample
* بدون provider model
* بدون abstractionهای اضافی غیرضروری
* بدون builder پیچیده
* با Options pattern ساده و استاندارد

این مدل باعث می‌شود capability از یک طرف minimal و قابل‌فهم بماند و از طرف دیگر نیاز وب را به‌صورت واقعی پوشش دهد.

---

## قراردادها

### ICurrentUser

قرارداد پایه Persona برای دسترسی به اطلاعات کاربر جاری.

اعضا:

* `UserId`
* `UserName`
* `FirstName`
* `LastName`
* `IsAuthenticated`
* `GetClaim(string claimType)`
* `GetClaims(string claimType)`

این قرارداد باید حتی‌الامکان مستقل از host باقی بماند.

---

### IWebCurrentUser

قرارداد وبی Persona که از `ICurrentUser` ارث می‌برد و این دو عضو را اضافه می‌کند:

* `IpAddress`
* `UserAgent`

این تفکیک باعث می‌شود concernهای وبی فقط در contract مربوط به وب expose شوند.

---

## implementation

Implementation اصلی Persona در نسخه فعلی `HttpContextCurrentUser` است.

این سرویس:

* به `IHttpContextAccessor` متکی است
* داده‌های کاربر را از `ClaimsPrincipal` می‌خواند
* claimها را resolve می‌کند
* `IpAddress` را از connection استخراج می‌کند
* `UserAgent` را از request header می‌خواند
* fallbackهای تنظیم‌شده را اعمال می‌کند

---

## registration

Persona برای ASP.NET Core یک entry point روشن دارد:

```csharp
builder.Services.AddPersonaAspNetCore();
```

همچنین overloadهای configure و configuration binding نیز پشتیبانی می‌شوند.

این registration:

* `IHttpContextAccessor` را ثبت می‌کند
* implementation اصلی را ثبت می‌کند
* `ICurrentUser` را به implementation متصل می‌کند
* `IWebCurrentUser` را به implementation متصل می‌کند

---

## options

`PersonaAspNetCoreOptions` این بخش‌ها را قابل تنظیم می‌کند:

### Claim Type Mapping

* `UserIdClaimType`
* `UserNameClaimType`
* `FirstNameClaimType`
* `LastNameClaimType`

### Fallback Values

* `DefaultUserId`
* `DefaultUserName`
* `DefaultFirstName`
* `DefaultLastName`
* `DefaultIpAddress`
* `DefaultUserAgent`

هدف options در Persona این است که behavior consumption را قابل تنظیم کند، نه این‌که یک configuration model سنگین یا provider system ایجاد شود.

---

## رفتار runtime

Persona در runtime با این قواعد کار می‌کند:

* نبودن user باعث exception نمی‌شود
* نبودن claim باعث exception نمی‌شود
* `GetClaim` در صورت نبودن claim مقدار `null` برمی‌گرداند
* `GetClaims` در صورت نبودن claim collection خالی برمی‌گرداند
* propertyهای هویتی ابتدا claimها را بررسی می‌کنند
* در نبود claim از fallback استفاده می‌شود
* `IsAuthenticated` مستقیماً از runtime identity state خوانده می‌شود

---

## ساختار پروژه

```text
Persona/
  Persona.slnx
  README.md
  src/
    Abstractions/
      ZaminX.BuildingBlocks.IdentityAndUsers.Persona.Abstractions.csproj
      Contracts/
        ICurrentUser.cs
        IWebCurrentUser.cs
    AspNetCore/
      ZaminX.BuildingBlocks.IdentityAndUsers.Persona.AspNetCore.csproj
      Configurations/
        PersonaAspNetCoreOptions.cs
      Extensions/
        PersonaServiceCollectionExtensions.cs
      Internals/
        ClaimsPrincipalExtensions.cs
      Services/
        HttpContextCurrentUser.cs
  samples/
    ZaminX.Samples.IdentityAndUsers.Persona.AspNetCore/
```

---

## sample

Sample مربوط به Persona برای validate کردن این flowها استفاده می‌شود:

* registration
* anonymous access
* authenticated access
* claim resolution
* web metadata access

این sample minimal نگه داشته شده تا focus روی خود capability باقی بماند.

---

## تصمیم‌های کلیدی

### 1. Persona به‌جای UsersManagement

تحلیل capability نشان داد که مسئله واقعی نسخه اول این بخش، مدیریت کامل کاربران نیست؛ بلکه دسترسی به current user است.

بنابراین نام محصولی capability `Persona` و concern فنی آن `CurrentUser` در نظر گرفته شد.

---

### 2. دو contract به‌جای یک contract سنگین

به‌جای قرار دادن `IpAddress` و `UserAgent` در contract پایه، دو contract تعریف شد:

* `ICurrentUser`
* `IWebCurrentUser`

این تصمیم باعث شد contract پایه reusableتر بماند و در عین حال نیازهای وبی هم پوشش داده شوند.

---

### 3. بدون provider model

Persona در نسخه اول provider-based طراحی نشده است، چون در این مرحله چند implementation هم‌ارز واقعی وجود ندارد و نیاز اصلی فقط یک implementation روشن برای ASP.NET Core است.

---

### 4. بدون authorization concern

Persona claim access را فراهم می‌کند، اما permission evaluation یا policy decision را انجام نمی‌دهد.

این تصمیم برای جلوگیری از drift capability گرفته شده است.

---

## وضعیت فعلی

وضعیت فعلی Persona:

* scope مشخص شده
* naming نهایی شده
* ساختار پروژه‌ها تعیین شده
* قراردادها و implementation پایه تعریف شده‌اند
* sample اولیه آماده شده
* مستندات capability آماده شده‌اند

---

## جمع‌بندی

Persona یک BuildingBlock محدود، شفاف و reusable در خانواده `06.IdentityAndUsers` است که مسئله current user access را برای applicationهای مبتنی بر ASP.NET Core استاندارد می‌کند.

این capability تلاش می‌کند بدون ورود به scopeهای بزرگ‌تر identity، یک مرز روشن، API ساده و implementation قابل‌فهم برای دسترسی به اطلاعات کاربر جاری ارائه دهد.
