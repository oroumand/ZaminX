# وضعیت فعلی پروژه زمین X

## نمای کلی

زمین X در حال توسعه به‌عنوان یک پلتفرم ماژولار برای ساخت سیستم‌های enterprise است.

تمرکز فعلی پروژه:

- تثبیت معماری
- طراحی capabilityها در سطح BuildingBlocks
- مستندسازی دقیق تصمیم‌ها
- پیاده‌سازی تدریجی capabilityهای کلیدی

---

## ساختار کلان پروژه

taxonomy پروژه شامل 5 دسته اصلی است:

- BuildingBlocks
- ApplicationPatterns
- Integrations
- Foundations
- Applications

در حال حاضر تمرکز روی BuildingBlocks است.

---

## وضعیت BuildingBlocks

### 01.CrossCutting

خانواده CrossCutting شامل capabilityهای عمومی و بین‌برشی است.

capabilityهای این خانواده:

---

### Object Mapper

وضعیت: تثبیت‌شده

- طراحی کامل شده
- پیاده‌سازی انجام شده
- به‌عنوان مرجع طراحی سایر capabilityها استفاده می‌شود

---

### Serializer (Prism)

وضعیت: تثبیت‌شده (نسخه اولیه)

- طراحی کامل شده
- پیاده‌سازی اولیه انجام شده
- الگوی provider-based تثبیت شده
- ساختار داخلی پروژه‌ها مشخص شده

---

### Translator (Parrot)

وضعیت: پیاده‌سازی شده (نسخه اولیه)

- طراحی capability کامل شده
- تصمیم‌های معماری از طریق ADR ثبت شده‌اند
- مدل provider-based پیاده‌سازی شده
- پشتیبانی از چند فراهم‌کننده داده (multi-provider) وجود دارد
- رفتار override بر اساس ترتیب registration پیاده‌سازی شده
- کش درون‌حافظه‌ای برای عملکرد سریع پیاده‌سازی شده
- امکان refresh داده‌ها بدون restart فراهم شده است
- SQL Server provider به‌عنوان provider اولیه پیاده‌سازی شده
- ثبت کلیدهای جاافتاده به‌صورت اختیاری پشتیبانی می‌شود
- sample Web API برای نمایش نحوه استفاده اضافه شده است

---

## وضعیت مستندات

- ساختار docs تثبیت شده است
- ADRها بر اساس استاندارد MADR نوشته می‌شوند
- تصمیم‌های مهم معماری ثبت شده‌اند
- اسناد capabilityها در حال تکمیل هستند

---

## وضعیت ADRها

ADRهای زیر ثبت شده‌اند:

- ADR 001 → مدل انتزاع و provider
- ADR 002 → ...
- ADR 003 → ...
- ADR 004 → ...
- ADR 005 → ...
- ADR 006 → ...
- ADR 007 → ...
- ADR 008 → ...
- ADR 009 → ...
- ADR 010 → تفکیک قرارداد مصرفی و تأمین داده در Translation
- ADR 011 → پشتیبانی از چند فراهم‌کننده داده و override
- ADR 012 → الزام ثبت حداقل یک provider
- ADR 013 → کش درون‌حافظه‌ای و refresh بدون restart
- ADR 014 → تفکیک مسیرهای formatting

(لیست کامل ADRها در پوشه مربوطه نگهداری می‌شود)

---

## تصمیم‌های تثبیت‌شده

در حال حاضر این تصمیم‌ها در سطح معماری تثبیت شده‌اند:

- استفاده از مدل provider-based برای capabilityهای مناسب
- جداسازی کامل API مصرفی از ابزارهای بیرونی
- نگه داشتن registration در provider
- استفاده از naming محصولی برای capabilityها
- تفکیک docs از READMEها
- استفاده از MADR برای ADRها

---

## گام‌های بعدی

- تکمیل مستندات capabilityها
- افزودن providerهای بیشتر برای Translator
- ادامه توسعه capabilityهای CrossCutting
- ورود تدریجی به ApplicationPatterns

---

## جمع‌بندی

پروژه زمین X در مرحله تثبیت معماری و توسعه BuildingBlocks قرار دارد.

سه capability کلیدی در خانواده CrossCutting اکنون در وضعیت پایدار اولیه قرار دارند:

- Object Mapper
- Serializer (Prism)
- Translator (Parrot)

این capabilityها پایه توسعه بخش‌های بالاتر سیستم را تشکیل می‌دهند.