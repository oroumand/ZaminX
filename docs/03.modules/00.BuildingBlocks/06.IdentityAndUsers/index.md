# IdentityAndUsers

## معرفی

خانواده `IdentityAndUsers` در زمین X مسئول capabilityهایی است که به **هویت کاربر جاری، context کاربر و concernهای identity-adjacent** مربوط می‌شوند.

این خانواده در مرز بین:

* application concerns
* security concerns
* user context

قرار دارد و تلاش می‌کند بدون تبدیل شدن به یک identity subsystem سنگین، مجموعه‌ای از BuildingBlockهای reusable و قابل‌استفاده در پروژه‌های مختلف فراهم کند.

---

## هدف این خانواده

هدف اصلی این خانواده این است که:

* دسترسی به اطلاعات کاربر جاری را استاندارد کند
* وابستگی مستقیم به frameworkها (مثل ASP.NET Core) را در مصرف‌کننده کاهش دهد
* مرز بین **مصرف identity** و **مدیریت identity** را شفاف نگه دارد
* capabilityهای identity-adjacent را به‌صورت modular و قابل reuse ارائه دهد
* از تبدیل شدن به یک سیستم monolithic برای identity جلوگیری کند

---

## جایگاه در taxonomy

* دسته: `BuildingBlocks`
* خانواده: `06.IdentityAndUsers`

این خانواده:

* CrossCutting خالص نیست (چون به semantics کاربر نزدیک است)
* Runtime صرف نیست (چون فقط registration یا setup نیست)
* Domain primitive هم نیست (چون behavior کاربردی دارد)

بنابراین در یک خانواده مستقل نگه داشته شده است.

---

## مرزبندی مفهومی

برای جلوگیری از drift و over-engineering، این خانواده مرزهای مشخصی دارد.

### در scope این خانواده

* current user access
* claim access
* authentication state inspection
* identity-adjacent context
* user metadata در سطح request
* abstractionهای سبک برای مصرف identity

---

### خارج از scope این خانواده (در نسخه فعلی)

* authentication flow (login / logout orchestration)
* token issuing
* identity provider integration (OAuth, OIDC, …)
* authorization policy engine
* permission system
* role management
* user CRUD
* profile management
* account lifecycle

این موارد یا در capabilityهای جداگانه طراحی می‌شوند یا در سطح application و integration قرار می‌گیرند.

---

## capabilityهای فعلی

در وضعیت فعلی، این خانواده شامل capability زیر است:

* `Persona`

این فهرست در آینده گسترش پیدا خواهد کرد.

---

## Persona

### تعریف

Persona capability مربوط به **دسترسی استاندارد به اطلاعات کاربر جاری** است.

این capability یک abstraction ساده و قابل تزریق برای current user فراهم می‌کند و implementation وبی آن برای ASP.NET Core ارائه شده است.

---

### مسئله‌ای که Persona حل می‌کند

در applicationهای واقعی، بخش‌های مختلف سیستم نیاز دارند:

* بدانند کاربر جاری چه کسی است
* وضعیت احراز هویت را بررسی کنند
* claimها را بخوانند
* metadata وبی مانند IP و UserAgent را دریافت کنند

اگر این نیازها مستقیماً با `HttpContext` یا `ClaimsPrincipal` حل شوند:

* coupling به framework افزایش می‌یابد
* تست‌پذیری کاهش می‌یابد
* logic تکراری می‌شود
* API یکدست از بین می‌رود

Persona این مشکل را با یک contract ساده و یک implementation استاندارد حل می‌کند.

---

### قراردادها

Persona دو contract اصلی ارائه می‌دهد:

#### ICurrentUser

برای دسترسی به اطلاعات پایه کاربر جاری:

* `UserId`
* `UserName`
* `FirstName`
* `LastName`
* `IsAuthenticated`
* `GetClaim(...)`
* `GetClaims(...)`

---

#### IWebCurrentUser

برای سناریوهای وب، با افزودن:

* `IpAddress`
* `UserAgent`

این تفکیک باعث می‌شود contract پایه reusable بماند و concernهای وبی فقط در جایی expose شوند که لازم هستند.

---

### implementation

Implementation اصلی Persona:

* مبتنی بر `HttpContext`
* استفاده از `IHttpContextAccessor`
* خواندن claimها از `ClaimsPrincipal`
* استخراج metadata از request

نام implementation:

* `HttpContextCurrentUser`

---

### registration

Persona یک entry point ساده برای ASP.NET Core دارد:

```csharp
builder.Services.AddPersonaAspNetCore();
```

همچنین از:

* configure action
* IConfiguration binding

پشتیبانی می‌کند.

---

### Options

`PersonaAspNetCoreOptions` برای این موارد استفاده می‌شود:

#### mapping

* `UserIdClaimType`
* `UserNameClaimType`
* `FirstNameClaimType`
* `LastNameClaimType`

#### fallback

* `DefaultUserId`
* `DefaultUserName`
* `DefaultFirstName`
* `DefaultLastName`
* `DefaultIpAddress`
* `DefaultUserAgent`

هدف Options در Persona:

* کنترل behavior
* نه ایجاد abstraction پیچیده

---

### رفتار runtime

Persona در runtime:

* برای نبودن user exception نمی‌دهد
* برای نبودن claim exception نمی‌دهد
* از fallback استفاده می‌کند
* API predictable ارائه می‌دهد

---

### چرا Persona به‌جای UsersManagement

نام اولیه backlog از جنس `UsersManagement` بود، اما تحلیل capability نشان داد که:

مسئله واقعی نسخه اول این بخش:

> دسترسی به current user

است، نه مدیریت کامل کاربران.

بنابراین:

* نام محصولی: `Persona`
* concern فنی: `CurrentUser`

در نظر گرفته شد.

---

### Non-goals Persona

Persona عمداً این موارد را پوشش نمی‌دهد:

* authentication flow
* authorization
* role/permission evaluation
* user CRUD
* profile management
* identity provider integration
* provider model

---

### ساختار پروژه

```text
Persona/
  Persona.slnx
  src/
    Abstractions/
      ZaminX.BuildingBlocks.IdentityAndUsers.Persona.Abstractions/
    AspNetCore/
      ZaminX.BuildingBlocks.IdentityAndUsers.Persona.AspNetCore/
  samples/
    ZaminX.Samples.IdentityAndUsers.Persona.AspNetCore/
```

---

## مسیر توسعه آینده

این خانواده در آینده می‌تواند شامل capabilityهای زیر شود:

* Auth (authentication integration)
* identity provider integration
* token handling abstraction (در صورت نیاز واقعی)
* identity bridging برای سناریوهای distributed

اما این توسعه‌ها باید:

* grounded باشند
* over-engineering نداشته باشند
* با مرز Persona تداخل نکنند

---

## تصمیم‌های کلیدی این خانواده

### 1. جداسازی مصرف از مدیریت identity

Persona فقط identity را **مصرف** می‌کند، نه این‌که آن را مدیریت کند.

---

### 2. جلوگیری از identity subsystem شدن

این خانواده عمداً به یک سیستم کامل identity تبدیل نمی‌شود.

---

### 3. abstraction فقط در مرز لازم

Persona فقط در جایی abstraction ایجاد می‌کند که:

* وابستگی به host باید قطع شود
* reuse واقعی وجود دارد

---

### 4. طراحی مرحله‌ای

این خانواده به‌صورت step-by-step توسعه پیدا می‌کند، نه به‌صورت big design upfront.

---

## وضعیت فعلی

* Persona طراحی و پیاده‌سازی اولیه شده
* docs آن کامل شده
* sample اولیه وجود دارد
* مرز capability مشخص شده
* integration با سایر capabilityها در حال تثبیت است

---

## جمع‌بندی

خانواده `IdentityAndUsers` در زمین X مسئول capabilityهای مرتبط با هویت کاربر جاری است.

Persona اولین گام این خانواده است و:

* current user access را استاندارد می‌کند
* API ساده و قابل‌فهم ارائه می‌دهد
* وابستگی به framework را محدود می‌کند
* و بدون ورود به scopeهای سنگین identity، یک building block کاربردی و production-friendly فراهم می‌کند.