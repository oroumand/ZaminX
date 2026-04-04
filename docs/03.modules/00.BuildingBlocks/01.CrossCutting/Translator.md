# Translator

Translator یکی از capabilityهای خانواده CrossCutting در زمین X است.

نام فنی این capability `Translator` و نام محصولی آن `Parrot` است.

Parrot برای هم‌راستایی با الگوی naming محصولی در capabilityهای مرجع CrossCutting انتخاب شده است و نمایانگر تکرار و بازتولید متن در contextهای مختلف است.

---

## هدف

هدف Translator:

* مدیریت ترجمه‌ها در سطح application
* جلوگیری از پراکندگی منطق ترجمه در کد
* فراهم کردن abstraction برای منابع مختلف ترجمه
* پشتیبانی از چند provider به‌صورت هم‌زمان
* امکان refresh و به‌روزرسانی runtime ترجمه‌ها

---

## جایگاه در CrossCutting

Translator یک capability عمومی و بین‌برشی است که:

* به دامنه خاصی وابسته نیست
* به لایه خاصی محدود نیست
* در بخش‌های مختلف سیستم قابل استفاده است

---

## مدل طراحی

Translator از الگوی **core-orchestrated provider capability** استفاده می‌کند.

در این مدل:

* یک core capability وجود دارد
* providerها به core داده می‌دهند
* consumer فقط با core کار می‌کند

---

## ساختار capability

این capability شامل اجزای زیر است:

### قرارداد مصرفی

```csharp
ITranslator
```

API اصلی برای دریافت ترجمه‌ها.

---

### Core (Parrot)

Core capability مسئول:

* نگهداری ترجمه‌ها در حافظه
* merge داده‌ها از providerها
* ارائه API مصرفی
* مدیریت lifecycle

---

### Providerها

providerها مسئول:

* تأمین داده ترجمه
* اتصال به منابع مختلف (SQL، فایل، API و ...)

نمونه:

* SqlServerTranslationProvider

---

### قراردادهای درونی

برای ارتباط بین core و providerها:

* ITranslationDataProvider
* ITranslationRefreshService
* ITranslationMissingKeyRegistrar

این قراردادها بخشی از API مصرفی نیستند.

---

## الگوی اجرا

### در startup

* providerها register می‌شوند
* Parrot ساخته می‌شود
* ترجمه‌ها load می‌شوند

---

### در runtime

* ترجمه‌ها از memory خوانده می‌شوند
* در صورت نیاز refresh انجام می‌شود

---

## مدیریت refresh

در Parrot:

* منطق refresh در `ParrotRefreshService` متمرکز است
* این سرویس hosted service نیست
* hosted serviceها فقط trigger هستند

### triggerها

* ParrotStartupHostedService → برای startup
* SqlServerTranslationReloadHostedService → برای reload

---

## registration

الگوی registration:

```
services.AddParrot(...)
        .UseSqlServer(...)
```

---

## ویژگی‌های کلیدی

* پشتیبانی از چند provider
* جداسازی کامل consumer از provider
* in-memory caching
* refresh runtime
* extensibility بالا

---

## محدودیت‌ها

* نیاز به provider برای کارکرد کامل
* پیچیدگی بیشتر نسبت به capabilityهای ساده
* نیاز به مدیریت صحیح lifecycle

---

## ارتباط با guideline

این capability نمونه مرجع برای:

* core-orchestrated design
* builder pattern در CrossCutting
* separation بین consumer API و internal contracts

---

## وضعیت فعلی

این capability به‌عنوان یکی از capabilityهای مرجع CrossCutting در زمین X در نظر گرفته می‌شود.