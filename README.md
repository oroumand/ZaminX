# زمین X

زمین X نسل جدیدی از فریم‌ورک زمین است که با تمرکز بر سادگی، ماژولار بودن، بازاستفاده و توسعه‌پذیری بازطراحی شده است.

این پروژه با هدف ارائه مجموعه‌ای از BuildingBlockها، الگوهای کاربردی، یکپارچه‌سازی‌ها، ساختارهای آماده و اپلیکیشن‌های کوچک قابل استفاده برای ساخت سیستم‌های مدرن بر پایه .NET طراحی شده است.

زمین X تلاش می‌کند بین **سادگی در استفاده** و **قدرت در توسعه‌پذیری** تعادل ایجاد کند و به توسعه‌دهندگان اجازه دهد بدون درگیر شدن با پیچیدگی‌های تکراری، روی حل مسئله تمرکز کنند.

---

## هدف پروژه

زمین X با اهداف زیر طراحی شده است:

* کاهش پیچیدگی‌های تکراری در توسعه نرم‌افزار
* ایجاد یک معماری یکدست و قابل توسعه
* فراهم کردن امکان انتخاب و جایگزینی پیاده‌سازی‌ها
* تسهیل توسعه سیستم‌های ماژولار
* کاهش هزینه شروع پروژه‌های جدید
* استانداردسازی الگوهای رایج در پروژه‌های .NET

---

## ساختار محصول

زمین X از پنج دسته اصلی تشکیل شده است:

### 1. BuildingBlocks

اجزای مستقل، self-contained و قابل reuse که می‌توانند بدون وابستگی به سایر بخش‌ها استفاده شوند.

ویژگی‌ها:

* مستقل از context اپلیکیشن
* قابل publish به‌صورت package
* قابل استفاده در پروژه‌های مختلف
* تمرکز روی یک concern مشخص

نکته مهم:

* وابستگی به تکنولوژی مانع BuildingBlock بودن نیست
* abstraction پیش‌فرض نیست
* معیار اصلی: **استقلال مصرف**

---

### 2. ApplicationPatterns

الگوهای سطح اپلیکیشن که معمولاً از چند BuildingBlock تشکیل شده‌اند و یک رفتار تکرارشونده را استاندارد می‌کنند.

---

### 3. Integrations

اجزایی که مسئول ارتباط با سیستم‌های خارجی هستند:

* APIها
* سرویس‌های third-party
* message brokers
* storage providers

---

### 4. Foundations

ساختارهای آماده و opinionated برای شروع سریع پروژه‌ها.

نمونه‌ها:

* MonolithStructure
* ModularMonolith
* (در آینده) MicroserviceModule

---

### 5. Applications

اپلیکیشن‌های آماده، قابل اجرا و ارزشمند که با استفاده از زمین X ساخته می‌شوند.

---

## فلسفه طراحی

### استقلال اجزا

هر capability باید تا حد امکان self-contained باشد.

---

### انتزاع فقط در صورت نیاز

abstraction زمانی ایجاد می‌شود که:

* چند پیاده‌سازی واقعی وجود داشته باشد
* یا نیاز به decoupling وجود داشته باشد

---

### سادگی در استفاده

APIها باید:

* قابل فهم باشند
* predictable باشند
* کم‌پیچیدگی باشند

---

### قابلیت توسعه

در صورت نیاز، امکان replace یا extend کردن behavior وجود داشته باشد.

---

### مستندسازی به‌عنوان بخشی از محصول

docs بخشی از خود capability است، نه چیز جداگانه.

---

## نمونه capabilityها

### CrossCutting

* Object Mapper (مرجع طراحی)
* Serializer (Prism)
* Translator (Parrot)
* Logger
* ApplicationPartDetector

این capabilityها:

* reusable هستند
* runtime dependency ندارند
* abstraction در آن‌ها در صورت نیاز استفاده شده

---

## RuntimeAndRegistration

این خانواده شامل capabilityهایی است که مربوط به:

* runtime composition
* service registration
* startup setup

هستند.

---

### DependencyInjection (Axon)

Axon capability مربوط به DI است.

مسئولیت‌ها:

* حذف wiring دستی
* استانداردسازی registration
* assembly scanning
* ساده‌سازی startup

ویژگی مهم:

Axon abstraction جدید ایجاد نمی‌کند و روی `IServiceCollection` کار می‌کند.

---

### OpenApi (Lumen)

**Lumen** capability مربوط به OpenAPI در زمین X است.

---

## تعریف Lumen

Lumen مسئول این موارد است:

* ثبت OpenAPI در ASP.NET Core
* expose کردن document endpoint
* مدیریت مسیر و naming document
* ترکیب UIهای مختلف برای نمایش API
* ساده‌سازی setup برای API documentation

---

## ویژگی‌های Lumen

* استفاده از OpenAPI built-in در ASP.NET Core (.NET 10)
* استفاده کامل از Options pattern استاندارد
* عدم استفاده از hackهایی مثل:

  * OptionsWrapper
  * Options.Create
  * Replace
* طراحی minimal و قابل توسعه
* امکان فعال‌سازی چند UI به‌صورت همزمان
* separation کامل بین registration و runtime

---

## UIهای پشتیبانی‌شده

Lumen از UIهای زیر پشتیبانی می‌کند:

* Scalar
* Swagger UI
* ReDoc

---

## مدل طراحی Lumen

### Core + UI Integration

Lumen شامل:

* یک Core capability
* چند UI integration

---

### نکته مهم

UIها capability مستقل نیستند.

بلکه:

* بخشی از Lumen هستند
* فقط presentation layer محسوب می‌شوند
* concern اصلی Lumen نیستند

---

## ساختار پروژه

```
src/
  00.BuildingBlocks
    02.RuntimeAndRegistration
      OpenApi
        Lumen
        Scalar
        Swagger
        Redoc
```

---

## مستندات

تمام مستندات در مسیر زیر قرار دارند:

```
docs/
```

ساختار docs:

* vision
* architecture
* modules
* decisions
* project-state

---

## وضعیت پروژه

زمین X در حال حاضر در فاز:

* تثبیت معماری
* توسعه capabilityهای اصلی
* ایجاد consistency در design

قرار دارد.

---

## تمرکز فعلی

* تثبیت CrossCutting
* تکمیل Axon
* طراحی و تثبیت Lumen
* تعریف ApplicationPatterns

---

## نقشه راه کوتاه‌مدت

* تثبیت کامل taxonomy
* تکمیل BuildingBlockها
* توسعه Foundations
* توسعه Applications اولیه
* publish اولیه capabilityها

---

## لایسنس

MIT
