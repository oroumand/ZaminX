# Persona.AspNetCore

این پروژه implementation وبی Persona را برای ASP.NET Core ارائه می‌دهد.

هدف آن این است که اطلاعات کاربر جاری از `HttpContext` و `ClaimsPrincipal` خوانده شوند و از طریق قراردادهای Persona به‌شکل استاندارد در اختیار application قرار بگیرند.

---

## مسئولیت‌ها

این پروژه مسئول این موارد است:

* پیاده‌سازی `ICurrentUser`
* پیاده‌سازی `IWebCurrentUser`
* خواندن claimها از `ClaimsPrincipal`
* خواندن `IpAddress`
* خواندن `UserAgent`
* ثبت سرویس‌ها در DI
* پشتیبانی از تنظیم options

---

## اجزای اصلی

### Configurations

در این بخش کلاس `PersonaAspNetCoreOptions` قرار می‌گیرد.

وظیفه آن:

* تعیین claim typeها
* تعیین fallback valueها
* فراهم کردن configuration ساده و روشن

---

### Services

در این بخش implementation اصلی یعنی `HttpContextCurrentUser` قرار می‌گیرد.

این سرویس:

* به `IHttpContextAccessor` متکی است
* اطلاعات کاربر را از `HttpContext.User` استخراج می‌کند
* metadata وبی را از request و connection می‌خواند
* رفتار fallback را اعمال می‌کند

---

### Extensions

در این بخش entry point مربوط به registration قرار می‌گیرد.

متد اصلی:

```csharp
AddPersonaAspNetCore(...)
```

این متد مسئول ثبت این اجزا است:

* `IHttpContextAccessor`
* `HttpContextCurrentUser`
* `ICurrentUser`
* `IWebCurrentUser`

---

### Internals

در این بخش helperهای داخلی implementation قرار می‌گیرند.

نمونه:

* extension متدهای مربوط به `ClaimsPrincipal`

این helperها بخشی از API عمومی capability نیستند و فقط برای ساده‌تر شدن implementation استفاده می‌شوند.

---

## Options

`PersonaAspNetCoreOptions` این موارد را قابل تنظیم می‌کند:

* claim type مربوط به `UserId`
* claim type مربوط به `UserName`
* claim type مربوط به `FirstName`
* claim type مربوط به `LastName`

و fallbackهای زیر:

* `DefaultUserId`
* `DefaultUserName`
* `DefaultFirstName`
* `DefaultLastName`
* `DefaultIpAddress`
* `DefaultUserAgent`

---

## رفتار implementation

این implementation با این اصول کار می‌کند:

* `IsAuthenticated` مستقیماً از `HttpContext.User.Identity.IsAuthenticated` خوانده می‌شود
* propertyهای هویتی ابتدا از claimها resolve می‌شوند
* اگر claimها موجود نباشند، fallbackها استفاده می‌شوند
* `IpAddress` از connection خوانده می‌شود
* `UserAgent` از request header خوانده می‌شود
* نبودن claim یا نبودن user، رفتار exception-based ایجاد نمی‌کند

---

## شیوه registration

### بدون configuration

```csharp
builder.Services.AddPersonaAspNetCore();
```

### با configure action

```csharp
builder.Services.AddPersonaAspNetCore(options =>
{
    options.DefaultUserId = "system";
    options.DefaultUserName = "anonymous";
});
```

### با IConfiguration

```csharp
builder.Services.AddPersonaAspNetCore(builder.Configuration, "Persona");
```

---

## مرز این پروژه

این پروژه implementation host-specific Persona است.

بنابراین:

* وابستگی به ASP.NET Core در اینجا مجاز است
* اما business logic نباید وارد آن شود
* authorization logic نباید وارد آن شود
* login/logout flow نباید وارد آن شود
* token management نباید وارد آن شود

این پروژه فقط bridge بین ASP.NET Core runtime و contractهای Persona است.

---

## چرا این پروژه جدا از Abstractions است

جدا کردن implementation از قرارداد باعث می‌شود:

* مصرف‌کننده به host وابسته نشود
* application code بتواند فقط به contract ارجاع دهد
* قابلیت reuse بهتر شود
* ساختار capability شفاف‌تر بماند

---

## جمع‌بندی

`Persona.AspNetCore` پیاده‌سازی عملی Persona در سناریوی وب است و نقش آن این است که current user access را از سطح framework به یک API ساده، تزریق‌پذیر و استاندارد تبدیل کند.
