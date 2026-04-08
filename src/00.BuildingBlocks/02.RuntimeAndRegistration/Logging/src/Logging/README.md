# Logging (Zamin X)

## معرفی

Logging در زمین X یک capability از خانواده **RuntimeAndRegistration** است که وظیفه آن:

* راه‌اندازی (setup) logging
* ثبت (registration) Serilog
* ساده‌سازی استفاده از logging در پروژه‌ها
* استانداردسازی enrichment و contextual logging

می‌باشد.

این capability به‌صورت **minimal، بدون abstraction و بدون over-engineering** طراحی شده است.

---

## 🎯 هدف

هدف این capability:

* حذف boilerplate مربوط به Serilog
* ارائه API ساده و یکپارچه
* فراهم کردن logging استاندارد برای همه پروژه‌ها
* پشتیبانی از logging در:

  * startup
  * runtime
  * request
  * context

---

## ⚙️ Scope

این capability شامل موارد زیر است:

* Serilog setup
* sink configuration
* enrichment
* contextual logging
* request logging
* startup logging

---

## ❌ Non-goals

این capability:

* abstraction برای logging ایجاد نمی‌کند
* provider model ندارد
* multi-logger پشتیبانی نمی‌کند
* plugin system ندارد

---

## 🧱 ساختار

```text
src/
  ZaminX.BuildingBlocks.RuntimeAndRegistration.Logging/
samples/
  ZaminX.BuildingBlocks.RuntimeAndRegistration.Logging.WebApiSample/
```

---

## 🚀 شروع سریع

### Program.cs

```csharp
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddZaminXLogging();

var app = builder.Build();

app.UseZaminXLoggingContext(context =>
{
    context.SetUserIdFromClaims("sub");
    context.SetUserNameFromClaims("name");

    context.Set("TenantId", ctx =>
        ctx.Request.Headers["X-Tenant-Id"].FirstOrDefault());
});

app.MapGet("/", () =>
{
    Log.Information("Hello from Logging 🚀");
    return "OK";
});

await app.RunAsync();
```

---

## 🔌 Sinks

در نسخه فعلی:

* Console
* File
* Seq

```csharp
.WriteTo.Console()
.WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
.WriteTo.Seq("http://localhost:5341")
```

---

## 🧩 Enrichment

### Built-in

* FromLogContext
* CorrelationId (در صورت اضافه شدن)
* TraceId / SpanId (در صورت وجود Activity)
* Application metadata

---

## 🔁 Contextual Logging

### تعریف

Contextual logging یعنی اضافه کردن property به همه logهای یک request.

---

### API

```csharp
app.UseZaminXLoggingContext(context =>
{
    context.Set("Key", ctx => value);
});
```

---

### User helpers

```csharp
context.SetUserIdFromClaims("sub");
context.SetUserNameFromClaims("name");
```

---

### Service-based

```csharp
context.SetUserId((ctx, sp) =>
{
    var svc = sp.GetRequiredService<IUserService>();
    return svc.UserId;
});
```

---

## 🌐 Request Logging

به‌صورت داخلی از:

```csharp
UseSerilogRequestLogging
```

استفاده می‌شود.

---

## 🔄 Startup Logging

```csharp
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();
```

و سپس:

```csharp
builder.Host.UseSerilog(...)
```

---

## 🧠 طراحی داخلی

### 1. AddZaminXLogging

مسئول:

* bootstrap logger
* ثبت Serilog
* تنظیم sinks
* اتصال به DI

---

### 2. ZaminXLoggingContextBuilder

مسئول:

* تعریف propertyهای contextual
* نگهداری registrationها

---

### 3. ContextPropertyRegistration

مدل داخلی برای:

* نام property
* تابع تولید مقدار (value factory)

---

### 4. UseZaminXLoggingContext

middleware سبک (inline) که:

* در هر request اجرا می‌شود
* propertyها را به LogContext اضافه می‌کند
* بعد از request آن‌ها را dispose می‌کند

---

## 📌 نکات مهم

* Serilog implementation detail است
* DI فقط برای enricherها و service-based context استفاده می‌شود
* هیچ abstraction اضافه‌ای وجود ندارد
* design کاملاً runtime-oriented است

---

## 🧪 Sample

در مسیر:

```text
samples/ZaminX.BuildingBlocks.RuntimeAndRegistration.Logging.WebApiSample
```

---

## 📚 docs

برای توضیحات معماری:

```text
docs/.../logging.md
```
