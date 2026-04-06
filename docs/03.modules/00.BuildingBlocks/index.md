# BuildingBlocks

## معرفی

BuildingBlockها اجزای پایه‌ای و اصلی زمین X هستند.

این اجزا:

* مستقل هستند
* self-contained هستند
* قابل reuse هستند
* و می‌توانند بدون وابستگی به سایر بخش‌ها استفاده شوند

BuildingBlockها foundation کل سیستم را تشکیل می‌دهند و سایر لایه‌ها بر روی آن‌ها ساخته می‌شوند.

---

## تعریف BuildingBlock

یک BuildingBlock در زمین X:

* یک capability مشخص را ارائه می‌دهد
* scope محدود و واضح دارد
* به application خاص وابسته نیست
* و قابل استفاده در contextهای مختلف است

---

## ویژگی‌های کلیدی

### استقلال مصرف

BuildingBlock باید بتواند:

* بدون نیاز به سایر BuildingBlockها استفاده شود (در حد ممکن)
* در پروژه‌های مختلف reuse شود

---

### self-contained بودن

هر BuildingBlock باید:

* dependencyهای خود را مدیریت کند
* setup خود را ارائه دهد
* و نیاز به wiring پیچیده نداشته باشد

---

### تمرکز روی یک concern

هر BuildingBlock باید:

* یک مسئله مشخص را حل کند
* از scope خود خارج نشود

---

### عدم وابستگی به application

BuildingBlock نباید:

* به business logic خاص وابسته باشد
* یا نیاز به context خاصی داشته باشد

---

## دسته‌بندی BuildingBlocks

BuildingBlockها در زمین X به چند خانواده تقسیم می‌شوند:

---

### CrossCutting

این دسته شامل capabilityهایی است که:

* در بخش‌های مختلف سیستم استفاده می‌شوند
* behavior مشترک ارائه می‌دهند
* معمولاً reusable و abstraction-friendly هستند

نمونه‌ها:

* Object Mapper
* Serializer (Prism)
* Translator (Parrot)

---

### RuntimeAndRegistration

این دسته شامل capabilityهایی است که:

* مسئول setup سیستم هستند
* در startup اجرا می‌شوند
* wiring و registration را مدیریت می‌کنند

نمونه‌ها:

* DependencyInjection (Axon)
* OpenApi (Lumen)

---

## تفاوت CrossCutting و RuntimeAndRegistration

| ویژگی          | CrossCutting          | RuntimeAndRegistration |
| -------------- | --------------------- | ---------------------- |
| نوع مسئله      | behavior              | setup                  |
| زمان استفاده   | runtime (در طول اجرا) | startup                |
| abstraction    | رایج                  | معمولاً unnecessary    |
| provider model | رایج                  | معمولاً ندارد          |

---

## اصول طراحی BuildingBlock

### 1. abstraction پیش‌فرض نیست

فقط زمانی abstraction ایجاد می‌شود که:

* چند implementation وجود داشته باشد
* یا نیاز واقعی به decoupling باشد

---

### 2. setup باید ساده باشد

* registration باید واضح باشد
* API باید minimal باشد
* رفتار باید قابل پیش‌بینی باشد

---

### 3. dependency به تکنولوژی مجاز است

BuildingBlock می‌تواند:

* به ASP.NET Core وابسته باشد
* یا به library خاصی متکی باشد

تا زمانی که:

* مسئله را درست حل کند
* و قابل reuse باشد

---

### 4. naming باید مبتنی بر مسئله باشد

نه implementation.

مثال:

* `OpenApi` بهتر از `ScalarRegistration`
* چون مسئله را درست بیان می‌کند

---

## ساختار پیشنهادی

```
00.BuildingBlocks/
  CrossCutting/
  RuntimeAndRegistration/
```

---

## مسیر مطالعه پیشنهادی

1. CrossCutting (برای درک reusable behaviorها)
2. RuntimeAndRegistration (برای درک setup سیستم)

---

## وضعیت فعلی

* CrossCutting تثبیت شده
* RuntimeAndRegistration در حال تکمیل است
* Lumen به‌عنوان capability جدید اضافه شده است

---

## جمع‌بندی

BuildingBlockها:

* پایه سیستم هستند
* باید ساده و مستقل باشند
* و نقش کلیدی در consistency پروژه دارند
