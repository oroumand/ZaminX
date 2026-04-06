# OpenApi (Lumen)

## معرفی

**Lumen** نام محصولی capability مربوط به OpenAPI در زمین X است.

این capability در خانواده `RuntimeAndRegistration` قرار دارد و مسئول مدیریت کامل فرآیند:

* ثبت OpenAPI
* expose کردن document
* ترکیب و فعال‌سازی UIهای نمایش API

می‌باشد.

Lumen به‌عنوان نقطه مرکزی API documentation در سیستم عمل می‌کند و تلاش می‌کند setup این بخش را ساده، استاندارد و قابل توسعه کند.

---

## مسئله‌ای که Lumen حل می‌کند

در ASP.NET Core، برای فعال‌سازی API documentation معمولاً نیاز به:

* ثبت OpenAPI
* تنظیم document endpoint
* اضافه کردن Swagger یا ابزارهای مشابه
* مدیریت مسیرها و configuration

داریم.

این فرآیند معمولاً:

* پراکنده است
* در چند نقطه از startup انجام می‌شود
* و بین پروژه‌ها inconsistent است

---

### هدف Lumen

Lumen این مشکلات را با ارائه:

* یک entry point واحد
* یک مدل ساده برای configuration
* و یک ساختار استاندارد برای UIها

حل می‌کند.

---

## جایگاه در معماری

Lumen در مسیر زیر قرار دارد:

```
00.BuildingBlocks/
  02.RuntimeAndRegistration/
    OpenApi/
      Lumen
```

---

### دلیل این جایگاه

چون concern اصلی Lumen:

* runtime wiring
* service registration
* startup setup

است، نه behavior قابل reuse در business logic.

---

## تعریف capability

Lumen یک capability است که:

* OpenAPI را register می‌کند
* document endpoint را expose می‌کند
* UIها را روی آن سوار می‌کند

---

## مدل طراحی

### Core + UI Integration

Lumen شامل دو بخش است:

#### 1. Core

مسئول:

* `AddOpenApi`
* options
* document configuration
* runtime mapping

---

#### 2. UI Integrations

شامل:

* Scalar
* Swagger UI
* ReDoc

---

### نکته مهم

UIها:

* capability مستقل نیستند
* provider محسوب نمی‌شوند
* فقط integration هستند

---

## اصول طراحی

### 1. عدم استفاده از abstraction غیرضروری

Lumen:

* interface مصرفی ندارد
* service business ارائه نمی‌دهد

---

### 2. استفاده از Options استاندارد

Lumen فقط از:

* `AddOptions`
* `Bind`
* `Configure`
* `Validate`

استفاده می‌کند.

و از موارد زیر اجتناب می‌کند:

* OptionsWrapper
* Options.Create
* Replace

---

### 3. separation of concerns

دو فاز کاملاً جدا هستند:

#### Registration

در `AddZaminXOpenApi(...)`

#### Runtime

در `UseZaminXOpenApi(...)`

---

### 4. سادگی در API

API باید:

* minimal باشد
* قابل فهم باشد
* predictable باشد

---

## API سطح بالا

### Registration

```csharp
builder.Services.AddZaminXOpenApi(
    builder.Configuration,
    lumen =>
    {
        lumen.UseScalar();
        lumen.UseSwagger();
        lumen.UseRedoc();
    });
```

---

### Runtime

```csharp
app.UseZaminXOpenApi();
```

---

## Options

### LumenOptions

```csharp
public class LumenOptions
{
    public bool Enabled { get; set; }
    public string DocumentPath { get; set; }
}
```

---

### UI Options

هر UI options مخصوص خود را دارد:

* ScalarUiOptions
* SwaggerUiOptions
* RedocUiOptions

---

## رفتار runtime

در runtime:

1. بررسی Enabled
2. map کردن OpenAPI document
3. فعال‌سازی UIها بر اساس registry

---

## ساختار پروژه

```
OpenApi/
  Lumen/
  Scalar/
  Swagger/
  Redoc/
  samples/
```

---

## تصمیم‌های مهم

### Scalar دیگر capability نیست

Scalar:

* بخشی از Lumen است
* فقط UI محسوب می‌شود

---

### abstraction حذف شده

چون:

* نیاز واقعی وجود نداشت
* complexity اضافه ایجاد می‌کرد

---

### builder سبک انتخاب شده

برای:

* ترکیب UIها
* حفظ سادگی

---

## Non-Goals

Lumen در نسخه فعلی این موارد را هدف قرار نمی‌دهد:

* provider model
* multi-document پیچیده
* customization عمیق UI
* plugin system

---

## مزایا

* کاهش boilerplate
* یکپارچگی در setup
* افزایش خوانایی startup
* consistency بین پروژه‌ها
* سادگی استفاده

---

## محدودیت‌ها

* customization محدود
* تمرکز روی use-caseهای رایج
* عدم پشتیبانی از سناریوهای پیچیده multi-doc

---

## مسیر توسعه آینده

* بهبود customization UIها
* پشتیبانی بهتر از multi-document
* integration با API versioning
* افزودن diagnostics بهتر

---

## جمع‌بندی

Lumen یک capability سبک، متمرکز و استاندارد برای مدیریت OpenAPI در زمین X است.

این capability:

* پیچیدگی setup را کاهش می‌دهد
* ساختار را استاندارد می‌کند
* و foundation مناسبی برای توسعه API documentation فراهم می‌کند
