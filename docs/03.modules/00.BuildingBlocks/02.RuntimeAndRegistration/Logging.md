# Logging (RuntimeAndRegistration)

## تعریف

Logging capability مسئول:

* setup logging
* registration
* runtime wiring

است.

---

## جایگاه

* BuildingBlocks
* RuntimeAndRegistration

---

## مدل طراحی

### Runtime-oriented

تمرکز روی:

* startup
* wiring
* middleware

---

### Minimal

* بدون abstraction
* بدون provider model
* بدون complexity اضافی

---

## اجزا

### 1. Registration

* AddZaminXLogging
* UseSerilog

---

### 2. Contextual Logging

* ZaminXLoggingContextBuilder
* inline middleware

---

### 3. Enrichment

* Application metadata
* Trace / Span
* Correlation

---

### 4. Sinks

* Console
* File
* Seq

---

## تصمیم‌های کلیدی

* استفاده از Serilog
* حذف abstraction
* استفاده از Options استاندارد (در صورت نیاز)
* عدم استفاده از patternهای پیچیده

---

## اهداف

* ساده‌سازی logging
* کاهش boilerplate
* ایجاد consistency

---

## Non-goals

* multi-provider logging
* abstraction
* plugin system

---

## ارتباط با سایر capabilityها

* Axon → DI
* Lumen → API docs

---

## جمع‌بندی

Logging یک capability سبک، عملی و runtime-oriented است که هدف آن ساده‌سازی logging در زمین X است.
