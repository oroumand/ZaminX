# ZaminX.BuildingBlocks.Domain

این پروژه شامل primitiveهای پایه دامنه در زمین X است.

هدف این پروژه ارائه مجموعه‌ای از building blockهای سبک، قابل استفاده مجدد و مستقل برای طراحی مدل دامنه است.

---

## 🎯 هدف

این پروژه برای حل مسائل زیر طراحی شده است:

* استانداردسازی مدل دامنه
* جلوگیری از تکرار پیاده‌سازی‌های common
* فراهم کردن پایه‌ای تمیز برای DDD
* کاهش پیچیدگی در طراحی domain model

---

## 📦 Primitiveها

### Entity

نمایش شیئی با هویت:

* دارای `Id`
* equality مبتنی بر identity

---

### AggregateRoot

ریشه aggregate:

* ارث‌بری از `Entity`
* نگهداری `DomainEvent`
* مرز aggregate

---

### ValueObject

شیء بدون هویت:

* equality مبتنی بر مقدار
* استفاده از self-referencing generic

---

### DomainEvent

رخداد دامنه:

* contract پایه برای eventهای دامنه
* بدون behavior اضافی

---

### DomainException

خطای دامنه:

* دارای `Code` (string)
* مناسب برای mapping در لایه‌های بالاتر

---

## 🧠 اصول طراحی

* minimal
* بدون abstraction غیرضروری
* بدون dependency به application یا infrastructure
* تمرکز بر domain semantics

---

## 🚫 خارج از scope

این پروژه شامل موارد زیر نیست:

* repository
* mediator
* result pattern
* validation framework
* persistence concern

---

## 🧭 جایگاه در زمین X

این پروژه بخشی از:

```
00.BuildingBlocks/03.DomainAndApplicationPrimitives
```

و زیرمجموعه:

```
01.Domain (Kernel)
```

است.

---

## 🚀 وضعیت

این پروژه در فاز:

* تثبیت primitiveهای دامنه
* آماده برای استفاده در application layer

قرار دارد.
