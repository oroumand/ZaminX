# زمین X

زمین X نسل جدیدی از فریم‌ورک زمین است که با تمرکز بر سادگی، ماژولار بودن، بازاستفاده و توسعه‌پذیری بازطراحی شده است.

این پروژه با هدف ارائه مجموعه‌ای از BuildingBlockها، الگوهای کاربردی، یکپارچه‌سازی‌ها، ساختارهای آماده و اپلیکیشن‌های کوچک قابل استفاده برای ساخت سیستم‌های مدرن بر پایه .NET طراحی شده است.

---

## هدف پروژه

زمین X تلاش می‌کند:

* پیچیدگی‌های تکراری در توسعه نرم‌افزار را کاهش دهد
* یک معماری یکدست و قابل توسعه ارائه دهد
* امکان انتخاب و جایگزینی پیاده‌سازی‌ها را فراهم کند
* توسعه سیستم‌های ماژولار را ساده‌تر کند
* شروع پروژه‌های جدید را سریع‌تر و کم‌هزینه‌تر کند

---

## ساختار محصول

زمین X از پنج دسته اصلی تشکیل شده است:

### BuildingBlocks

اجزای مستقل، خودبسنده و قابل بازاستفاده که می‌توانند به‌تنهایی استفاده و بسته‌بندی شوند.

نکته مهم:

* وابستگی به تکنولوژی مانع قرارگیری در این دسته نیست
* وجود انتزاع الزامی نیست
* معیار اصلی: استقلال مصرف

---

### ApplicationPatterns

الگوهای پرکاربرد سطح اپلیکیشن که معمولاً از چند BuildingBlock استفاده می‌کنند و یک رفتار تکرارشونده را استاندارد می‌کنند.

---

### Integrations

اجزایی که مسئول اتصال به سرویس‌ها، سیستم‌ها و boundaryهای بیرونی هستند.

---

### Foundations

ساختارهای آماده و opinionated برای شروع پروژه‌ها.

نمونه‌های هدف:

* MonolithStructure
* ModularMonolith
* MicroserviceModule (در حال بررسی)

---

### Applications

اپلیکیشن‌های کوچک، قابل اجرا و ارزشمند که با استفاده از زمین X ساخته می‌شوند و برای استفاده مستقیم یا شروع سریع توسعه کاربرد دارند.

---

## فلسفه طراحی

### 1. استقلال اجزا

هر capability تا حد امکان باید مستقل، خودبسنده و قابل بازاستفاده باشد.

---

### 2. انتزاع فقط در صورت نیاز

در زمین X ایجاد انتزاع یک الزام پیش‌فرض نیست.

اگر برای یک capability نیاز واقعی وجود داشته باشد، قرارداد مستقل تعریف می‌شود.
اگر بستر .NET انتزاع مناسبی داشته باشد، از همان استفاده می‌شود.

---

### 3. سادگی در استفاده

تمامی APIها و setup باید ساده، شفاف و قابل فهم باشند.

---

### 4. قابلیت توسعه و جایگزینی

در صورت نیاز، پیاده‌سازی‌ها باید قابل جایگزینی باشند.

---

### 5. مستندسازی به‌عنوان بخشی از محصول

مستندسازی بخشی از خود محصول است، نه خروجی جانبی.

---

## نمونه capabilityها

نمونه‌هایی از capabilityها در زمین X:

### CrossCutting

* Object Mapper
* Serializer (Prism)
* Translator (Parrot)
* Logger
* ApplicationPartDetector

> نکته:
> DependencyInjection دیگر در این خانواده قرار نمی‌گیرد و به خانواده RuntimeAndRegistration منتقل شده است.

---

### RuntimeAndRegistration

این خانواده شامل capabilityهایی است که مربوط به:

* runtime composition
* service registration
* startup setup

هستند.

#### DependencyInjection (Axon)

Axon نام محصولی capability مربوط به DependencyInjection است.

این capability برای:

* ثبت خودکار سرویس‌ها
* حذف wiring دستی
* assembly scanning
* registration standardization
* startup simplification

طراحی شده است.

Axon به‌جای ایجاد abstraction جدید، روی ساده‌سازی registration در `IServiceCollection` تمرکز دارد و با استفاده از marker interfaceها و policyهای مشخص، رفتار سیستم را قابل پیش‌بینی می‌کند.

---

### Domain و Application Primitives

* Entity
* AggregateRoot
* Mediator

---

### Data و Persistence

* Auditing

---

در میان capabilityهای عمومی و بین‌برشی، `Object Mapper` به‌عنوان capability مرجع این خانواده در نظر گرفته می‌شود.
همچنین `Serializer` با نام محصولی `Prism` و `Translator` با نام محصولی `Parrot` به‌عنوان capabilityهای مرجع بعدی این خانواده طراحی و پیاده‌سازی اولیه شده‌اند.

---

## ساختار پروژه

ساختار کلی پروژه:

src/
00.BuildingBlocks
01.ApplicationPatterns
02.Integrations
03.Foundations
04.Applications

---

## مستندات

مستندات در مسیر زیر قرار دارند:

docs/

برای شروع:

* README.md
* docs/index.md
* docs/01.vision/index.md
* docs/02.architecture/project-state.md
* docs/02.architecture/index.md

---

## وضعیت پروژه

پروژه در فاز طراحی محصول، معماری و پیاده‌سازی تدریجی capabilityهای اصلی قرار دارد.

تمرکز فعلی:

* تثبیت taxonomy پروژه
* تعریف معماری کلان
* طراحی نقشه ماژول‌ها
* ثبت تصمیم‌های کلیدی
* ایجاد backlog اولیه
* پیاده‌سازی capabilityهای مرجع در خانواده CrossCutting
* آغاز توسعه capabilityهای خانواده RuntimeAndRegistration (Axon)

---

## نقشه راه کوتاه‌مدت

* تثبیت معماری
* تعریف BuildingBlockهای اصلی
* طراحی ApplicationPatterns
* تعریف Foundations (MonolithStructure و ModularMonolith)
* توسعه capabilityهای مرجع CrossCutting
* توسعه و تکمیل Axon (DependencyInjection)
* طراحی Applications اولیه:

  * مدیریت ترجمه
  * مشاهده تاریخچه تغییرات (Auditing)
* ایجاد backlog و roadmap

---

## لایسنس

MIT
