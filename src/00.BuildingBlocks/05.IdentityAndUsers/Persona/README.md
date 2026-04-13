# Persona

Persona یک capability در خانواده `06.IdentityAndUsers` در زمین X است که مسئله **دسترسی استاندارد، قابل تزریق و قابل پیش‌بینی به اطلاعات کاربر جاری** را حل می‌کند.

این capability برای سناریوهایی طراحی شده است که application یا BuildingBlockهای دیگر نیاز دارند بدون وابستگی مستقیم به `HttpContext`، `ClaimsPrincipal` یا جزئیات ASP.NET Core، به اطلاعات کاربر جاری دسترسی پیدا کنند.

Persona در نسخه فعلی یک concern مشخص را پوشش می‌دهد:

* خواندن اطلاعات هویتی کاربر جاری
* تشخیص وضعیت احراز هویت
* خواندن claimها
* دسترسی به metadata وبی پرکاربرد مانند `IpAddress` و `UserAgent`
* ارائه implementation مبتنی بر ASP.NET Core

---

## مسئله‌ای که Persona حل می‌کند

در بسیاری از applicationها، کدهای مختلف در لایه‌های متفاوت نیاز دارند بدانند:

* کاربر جاری چه کسی است
* آیا کاربر احراز هویت شده است یا نه
* شناسه و نام کاربر چیست
* claim خاصی وجود دارد یا نه
* IP و UserAgent درخواست فعلی چیست

اگر این نیازها مستقیماً با `HttpContext` یا `ClaimsPrincipal` حل شوند، چند مشکل ایجاد می‌شود:

* وابستگی مستقیم لایه‌های مختلف به ASP.NET Core
* سخت‌تر شدن تست‌پذیری
* تکرار منطق خواندن claimها
* نشت concern وب به بخش‌هایی که فقط به اطلاعات کاربر نیاز دارند
* کاهش یکدستی API در سطح پروژه

Persona این مسئله را با یک قرارداد ساده و یک implementation وبی حل می‌کند.

---

## مرز capability

Persona عمداً یک capability محدود و focused است.

### در scope

Persona در نسخه فعلی این مسئولیت‌ها را دارد:

* ارائه contract برای current user
* ارائه contract وبی برای current user
* خواندن اطلاعات پایه کاربر جاری
* خواندن claimها
* تشخیص `IsAuthenticated`
* خواندن `IpAddress`
* خواندن `UserAgent`
* registration ساده در ASP.NET Core

### خارج از scope

Persona در نسخه فعلی این موارد را حل نمی‌کند:

* authentication flow
* login / logout orchestration
* token issuing
* authorization policy evaluation
* permission engine
* role-based access decision
* user CRUD
* profile management
* account settings
* user provisioning
* external identity provider integration

به‌عبارت دیگر، Persona **کاربر جاری را مصرف می‌کند**، نه این‌که احراز هویت یا مدیریت کامل کاربران را پیاده‌سازی کند.

---

## مدل طراحی

Persona در نسخه فعلی با این مدل طراحی شده است:

* یک contract پایه: `ICurrentUser`
* یک contract وبی: `IWebCurrentUser`
* یک implementation مبتنی بر `HttpContext`
* یک entry point روشن برای registration
* options سبک برای mapping و fallbackها
* بدون provider model
* بدون abstractionهای غیرضروری اضافه
* بدون exception hierarchy اختصاصی

این طراحی باعث می‌شود capability هم reusable بماند و هم نیاز رایج وب را به‌خوبی پوشش دهد.

---

## ساختار capability

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

## پروژه‌ها

### 1. Persona.Abstractions

این پروژه قراردادهای مصرفی Persona را نگه می‌دارد.

مسئولیت‌ها:

* تعریف `ICurrentUser`
* تعریف `IWebCurrentUser`
* نگه‌داری API پایه و مستقل از host

این پروژه نباید به ASP.NET Core وابسته باشد.

---

### 2. Persona.AspNetCore

این پروژه implementation وبی Persona را ارائه می‌دهد.

مسئولیت‌ها:

* خواندن اطلاعات کاربر از `HttpContext`
* نگاشت claimها
* خواندن `IpAddress`
* خواندن `UserAgent`
* ثبت سرویس‌ها در DI
* ارائه options برای claim typeها و fallbackها

---

### 3. Sample

این sample برای validate کردن رفتار capability در سناریوی واقعی وب استفاده می‌شود.

هدف sample:

* نمایش registration
* نمایش current user در request
* نمایش claim reading
* نمایش رفتار در حالت authenticated و unauthenticated

---

## قراردادها

### ICurrentUser

قرارداد پایه برای دسترسی به اطلاعات کاربر جاری.

اعضای اصلی:

* `UserId`
* `UserName`
* `FirstName`
* `LastName`
* `IsAuthenticated`
* `GetClaim(string claimType)`
* `GetClaims(string claimType)`

این contract باید برای مصرف‌کننده‌ای مناسب باشد که فقط به اطلاعات کاربر نیاز دارد و نباید به ASP.NET Core وابسته شود.

---

### IWebCurrentUser

قرارداد وبی که از `ICurrentUser` ارث می‌برد و metadata وبی را اضافه می‌کند.

اعضای اضافه:

* `IpAddress`
* `UserAgent`

این تفکیک باعث می‌شود contract پایه ساده و reusable بماند و concernهای وبی فقط در جایی expose شوند که واقعاً لازم هستند.

---

## Registration

Persona برای ثبت سرویس‌ها در ASP.NET Core یک entry point روشن ارائه می‌دهد:

```csharp
builder.Services.AddPersonaAspNetCore();
```

همچنین می‌توان options را از code یا configuration تنظیم کرد.

نمونه:

```csharp
builder.Services.AddPersonaAspNetCore(options =>
{
    options.DefaultUserId = "system";
    options.DefaultUserName = "anonymous";
});
```

یا:

```csharp
builder.Services.AddPersonaAspNetCore(builder.Configuration, "Persona");
```

---

## Options

`PersonaAspNetCoreOptions` برای این اهداف استفاده می‌شود:

* تعیین claim typeهای مربوط به:

  * `UserId`
  * `UserName`
  * `FirstName`
  * `LastName`
* تعیین fallbackهای پیش‌فرض برای:

  * `DefaultUserId`
  * `DefaultUserName`
  * `DefaultFirstName`
  * `DefaultLastName`
  * `DefaultIpAddress`
  * `DefaultUserAgent`

Options در Persona فقط برای configuration behavior استفاده می‌شود و نباید به یک abstraction پیچیده یا model سنگین تبدیل شود.

---

## رفتار runtime

Persona در runtime این قواعد را دنبال می‌کند:

* اگر کاربر احراز هویت نشده باشد، `IsAuthenticated` برابر `false` خواهد بود
* اگر claim مورد انتظار وجود نداشته باشد، propertyها از fallback استفاده می‌کنند
* اگر fallback هم تنظیم نشده باشد، propertyهای متنی `null` می‌شوند
* `GetClaim` در صورت نبودن claim، `null` برمی‌گرداند
* `GetClaims` در صورت نبودن claim، collection خالی برمی‌گرداند
* نبودن claim عادی، exception تولید نمی‌کند

این رفتار باعث می‌شود API ساده، predictable و بدون surprise باشد.

---

## نمونه استفاده

```csharp
app.MapGet("/me", (ICurrentUser currentUser, IWebCurrentUser webCurrentUser) =>
{
    return Results.Ok(new
    {
        currentUser.IsAuthenticated,
        currentUser.UserId,
        currentUser.UserName,
        currentUser.FirstName,
        currentUser.LastName,
        webCurrentUser.IpAddress,
        webCurrentUser.UserAgent
    });
});
```

---

## چرا Persona BuildingBlock است

Persona با وجود وابستگی به ASP.NET Core در implementation، همچنان یک BuildingBlock معتبر است چون:

* concern مشخص و محدودی دارد
* مستقل از business context است
* در پروژه‌های مختلف قابل reuse است
* setup روشن و self-contained دارد
* مصرف‌کننده را از implementation جزئیات host جدا می‌کند

---

## چرا Persona UsersManagement کامل نیست

نام backlog اولیه از جنس `UsersManagement` بود، اما تحلیل capability نشان داد که مسئله واقعی نسخه اول این بخش **دسترسی به current user** است، نه مدیریت کامل کاربران.

بنابراین Persona در نسخه فعلی:

* current user access را حل می‌کند
* profile یا account management را حل نمی‌کند
* user lifecycle را حل نمی‌کند
* identity provider integration را حل نمی‌کند

اگر در آینده نیاز واقعی به user lifecycle یا account management به‌وجود بیاید، آن concern باید در capability یا asset جداگانه‌ای طراحی شود.

---

## Non-goals

Persona در نسخه اول عمداً این موارد را دنبال نمی‌کند:

* provider model
* multi-host abstraction پیچیده
* authorization service
* role evaluator
* user profile model
* session orchestration
* token inspection API
* audit pipeline اختصاصی
* diagnostic pipeline اختصاصی

---

## وضعیت فعلی

وضعیت فعلی Persona:

* naming نهایی شده
* مرز capability مشخص شده
* structure پروژه‌ها مشخص شده
* contracts و implementation اولیه طراحی شده‌اند
* sample اولیه تعریف شده
* مستندات capability آماده شده‌اند

---

## مسیر توسعه بعدی

گام‌های بعدی معمولاً می‌توانند شامل این موارد باشند:

* افزودن tests
* تکمیل sample
* نهایی‌سازی مستندات سطح repo
* بررسی integration با capabilityهای دیگر در سناریوهای واقعی

اما هر توسعه بعدی باید همچنان مرز محدود Persona را حفظ کند.

---

## جمع‌بندی

Persona یک capability سبک، روشن و reusable برای current user access در زمین X است.

این capability تلاش می‌کند:

* دسترسی به اطلاعات کاربر جاری را استاندارد کند
* وابستگی به host را در implementation نگه دارد
* API مصرفی ساده و قابل‌فهم ارائه دهد
* و بدون ورود به scopeهای بزرگ‌تر identity، یک building block کاربردی و production-friendly فراهم کند.
