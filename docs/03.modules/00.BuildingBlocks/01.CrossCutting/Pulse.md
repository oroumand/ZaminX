# Pulse

## معرفی

Pulse یک CrossCutting capability در ZaminX است که مجموعه‌ای از utilityهای عمومی، سبک و reusable را ارائه می‌دهد.

این capability با هدف حذف helperهای پراکنده و استانداردسازی رفتارهای تکرارشونده طراحی شده است.

---

## Problem

در پروژه‌های قبلی:

* utilityها پراکنده بودند
* naming یکدست نبود
* abstractionهای غیرضروری ایجاد شده بود
* dependencyهای پنهان وجود داشت

---

## Solution

Pulse این مشکلات را با:

* یک capability متمرکز
* بدون abstraction اضافه
* با APIهای ساده و مستقیم

حل می‌کند.

---

## Scope

### شامل

* Persian DateTime utilities
* Guard clauses
* Extension methods منتخب

### خارج از scope

* DI helpers
* Service Locator
* Resource system
* Translator

---

## Design Decisions

### 1. عدم استفاده از abstraction

هیچ interface یا provider model تعریف نشده است.

دلیل:

* فقط یک implementation داریم
* نیاز واقعی به decoupling وجود ندارد

---

### 2. static-first design

تمام APIها به‌صورت static ارائه شده‌اند.

دلیل:

* سادگی در استفاده
* عدم نیاز به DI
* performance بهتر

---

### 3. Persian-first

تمام DateTime utilities با تمرکز روی تقویم شمسی طراحی شده‌اند.

---

### 4. حذف Utility Monolith

نسخه قبلی Zamin.Utilities:

* بسیار بزرگ و پراکنده بود
* شامل concerns نامرتبط بود

در Pulse:

* scope محدود شده
* structure اصلاح شده
* بخش‌های غیرضروری حذف شده

---

## Migration Notes

موارد منتقل‌شده:

* Persian DateTime utilities (کامل)
* Extensions منتخب
* Guards

موارد حذف‌شده:

* ZaminServices
* Resources
* Translator

---

## Structure

```
DateTime/
Globalization/
Guards/
Extensions/
```

---

## Usage Guidelines

* فقط برای helperهای عمومی استفاده شود
* business logic وارد این capability نشود
* از اضافه کردن abstraction خودداری شود

---

## Future

در صورت نیاز:

* capability جدا برای advanced date/time
* یا globalization گسترده‌تر

ایجاد خواهد شد، نه توسعه بی‌رویه Pulse

---

## Summary

Pulse یک utility layer ساده، سریع و قابل اعتماد است که:

* پیچیدگی را کاهش می‌دهد
* reuse را افزایش می‌دهد
* consistency را بهبود می‌دهد
