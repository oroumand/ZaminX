# Runtime And Registration

## معرفی

این بخش شامل capabilityهایی است که مسئول:

* setup سیستم
* service registration
* runtime composition

هستند.

این capabilityها در زمان startup اجرا می‌شوند و رفتار سیستم را در سطح زیرساختی تنظیم می‌کنند.

---

## تعریف

RuntimeAndRegistration شامل capabilityهایی است که:

* قبل از اجرای اصلی application فعال می‌شوند
* dependencyها را register می‌کنند
* و wiring سیستم را انجام می‌دهند

---

## ویژگی‌های کلیدی

* execution در startup
* تمرکز روی configuration و wiring
* وابستگی به runtime (مثل ASP.NET Core)
* APIهای ساده و مستقیم

---

## تفاوت با CrossCutting

CrossCutting:

* behavior ارائه می‌دهد
* در طول اجرای برنامه استفاده می‌شود

RuntimeAndRegistration:

* setup انجام می‌دهد
* در startup اجرا می‌شود

---

## capabilityها

### DependencyInjection (Axon)

Axon capability مربوط به DI است.

مسئول:

* حذف wiring دستی
* assembly scanning
* استانداردسازی registration

---

### OpenApi (Lumen)

Lumen capability مربوط به API documentation است.

---

## تعریف Lumen

Lumen مسئول:

* ثبت OpenAPI
* expose کردن document endpoint
* مدیریت configuration document
* ترکیب UIهای نمایش API

است.

---

## مدل طراحی Lumen

Lumen شامل:

* Core (OpenAPI registration)
* UI integrations:

  * Scalar
  * Swagger UI
  * ReDoc

---

## اصول طراحی این خانواده

### 1. سادگی

APIها باید:

* کوتاه باشند
* واضح باشند
* قابل فهم باشند

---

### 2. عدم پیچیدگی غیرضروری

از موارد زیر اجتناب می‌شود:

* abstraction غیرضروری
* provider model پیچیده

---

### 3. separation بین registration و runtime

* `Add...` برای setup
* `Use...` برای runtime

---

### 4. Options استاندارد

از:

* AddOptions
* Bind
* Configure

استفاده می‌شود

و از:

* OptionsWrapper
* Replace

اجتناب می‌شود

---

## ساختار پروژه

```
OpenApi/
  Lumen/
  Scalar/
  Swagger/
  Redoc/
```

---

## مسیر مطالعه پیشنهادی

1. Axon (DependencyInjection)
2. Lumen (OpenApi)

---

## وضعیت فعلی

* Axon تثبیت شده
* Lumen در حال تکمیل و refinement است

---

## جمع‌بندی

این خانواده:

* پایه setup سیستم است
* wiring را استاندارد می‌کند
* و نقش مهمی در سادگی startup دارد

Lumen به‌عنوان capability اصلی API documentation، بخش مهمی از این خانواده محسوب می‌شود.
