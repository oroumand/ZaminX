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

## 🆕 Logging

### وضعیت فعلی

* طراحی معماری کامل
* scope مشخص
* implementation انجام شده
* sample آماده
* در حال تثبیت docs

---

### تعریف Logging

Logging capability مربوط به:

* Serilog registration
* logging pipeline setup
* enrichment
* contextual logging
* request logging
* startup logging

است.

---

### تصمیم‌های معماری Logging

#### 1. جایگاه

* خانواده: RuntimeAndRegistration
* دلیل: تمرکز روی setup و runtime

---

#### 2. مدل طراحی

* بدون abstraction
* بدون provider model
* builder سبک
* integration مستقیم با Serilog

---

#### 3. Options

* استفاده از Options pattern استاندارد
* Bind از IConfiguration
* Configure از code
* بدون:

  * OptionsWrapper
  * Options.Create
  * Replace

---

#### 4. Enrichment

پشتیبانی از:

* MachineName
* EnvironmentName
* ThreadId
* CorrelationId
* TraceId / SpanId
* Application metadata

---

#### 5. Contextual Logging

پشتیبانی از:

* UserId / UserName
* claim-based helpers
* properties سفارشی
* resolver-based per-request model

---

#### 6. Sinks

نسخه اول:

* Console
* File
* Seq

---

#### 7. Startup Logging

* bootstrap logger
* run wrapper
* fatal exception handling

---

### اهداف نسخه اول

* setup ساده logging
* یکپارچه‌سازی Serilog
* کاهش boilerplate
* ارائه API قابل فهم

---

### Non-goals

* abstraction logging
* multi-provider
* plugin system پیچیده
* sinkهای متعدد خارج از scope

---

## تصمیم‌های اخیر

* انتقال DI به RuntimeAndRegistration
* تثبیت Axon
* حذف Scalar به‌عنوان capability مستقل
* تعریف Lumen
* تعریف Logging
* استانداردسازی Options usage
* حذف over-engineering در runtime capabilityها

---

## ریسک‌ها

* over-engineering
* پیچیده شدن runtime capabilityها
* misuse از abstraction

---

## Domain Primitives (Kernel)

وضعیت: ✅ پیاده‌سازی شده (نسخه اولیه)

این خانواده شامل primitiveهای پایه دامنه است:

* Entity
* AggregateRoot
* ValueObject
* DomainEvent
* DomainException

ویژگی‌ها:

* طراحی minimal و بدون over-engineering
* بدون وابستگی به application یا infrastructure
* آماده برای استفاده در application layer

نکات مهم:

* DomainEvent به‌صورت marker interface طراحی شده
* AggregateRoot مسئول نگهداری eventها است (نه dispatch)
* ValueObject از الگوی self-referencing generic استفاده می‌کند
* DomainException دارای Code متنی است (بدون key یا numeric id)

وضعیت بعدی:

➡️ ورود به Application Primitives (Relay)

## مسیر آینده

* تثبیت Lumen
* تثبیت Logging
* بهبود docs
* ورود به Application Primitives (Relay)

---

## جمع‌بندی

زمین X در حال حرکت به سمت:

* سادگی
* modularity
* consistency

است.

Logging نقش مهمی در استانداردسازی logging در این مسیر دارد.
