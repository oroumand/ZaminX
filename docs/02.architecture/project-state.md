# Project State - Zamin X

## وضعیت کلی پروژه

زمین X در حال حاضر در مرحله:

* طراحی معماری
* تثبیت ساختار
* پیاده‌سازی تدریجی capabilityها

قرار دارد.

تمرکز روی ایجاد یک foundation پایدار برای توسعه آینده است.

---

## وضعیت BuildingBlocks

### CrossCutting

Capabilityهای اصلی:

* Mapper
* Serializer (Prism)
* Translator (Parrot)

وضعیت:

* طراحی تثبیت شده
* پیاده‌سازی اولیه انجام شده
* در حال consolidation و بهبود consistency

---

## RuntimeAndRegistration

این خانواده شامل capabilityهای مربوط به:

* registration
* runtime setup
* wiring

است.

---

### DependencyInjection (Axon)

وضعیت:

* طراحی کامل
* پیاده‌سازی انجام شده
* در حال استفاده به‌عنوان reference capability

---

### OpenApi (Lumen)

وضعیت فعلی:

* طراحی معماری کامل
* scope مشخص
* implementation اولیه انجام شده
* در حال refinement

---

## تعریف Lumen

Lumen capability مربوط به:

* OpenAPI registration
* API documentation exposure
* UI composition

است.

---

## تصمیم‌های معماری Lumen

### 1. جایگاه

* خانواده: RuntimeAndRegistration
* دلیل: تمرکز روی setup و runtime

---

### 2. مدل طراحی

* بدون abstraction مصرفی
* بدون provider model
* builder سبک
* separation بین registration و runtime

---

### 3. Options

Lumen از Options pattern استاندارد استفاده می‌کند:

* Bind از IConfiguration
* Configure از code
* Validation

و از موارد زیر استفاده نمی‌کند:

* OptionsWrapper
* Options.Create
* Replace

---

### 4. UIها

UIها:

* capability مستقل نیستند
* integration داخلی هستند

---

### 5. ساختار

* Core project (Lumen)
* UI projects:

  * Scalar
  * Swagger
  * Redoc
* Sample

---

### 6. اهداف نسخه اول

* ثبت OpenAPI
* expose document
* فعال‌سازی UIها
* setup ساده

---

### 7. Non-goals

* abstraction پیچیده
* provider model
* multi-document پیچیده
* customization عمیق

---

## تصمیم‌های اخیر

* انتقال DI به RuntimeAndRegistration
* تثبیت Axon
* حذف Scalar به‌عنوان capability مستقل
* تعریف Lumen
* استانداردسازی Options usage

---

## ریسک‌ها

* over-engineering
* پیچیده شدن runtime capabilityها
* misuse از abstraction

---

## مسیر آینده

* تثبیت Lumen
* تست UI integration
* بهبود docs
* publish

---

## جمع‌بندی

زمین X در حال حرکت به سمت:

* سادگی
* modularity
* consistency

است.

Lumen نقش مهمی در استانداردسازی API documentation در این مسیر دارد.
