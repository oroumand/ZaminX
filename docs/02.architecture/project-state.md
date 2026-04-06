# وضعیت فعلی پروژه زمین X

## نمای کلی

زمین X در حال توسعه به‌عنوان یک پلتفرم ماژولار برای ساخت سیستم‌های enterprise است.

تمرکز فعلی پروژه:

* تثبیت معماری
* طراحی capabilityها در سطح BuildingBlocks
* مستندسازی دقیق تصمیم‌ها
* پیاده‌سازی تدریجی capabilityهای کلیدی

---

## ساختار کلان پروژه

taxonomy پروژه شامل 5 دسته اصلی است:

* BuildingBlocks
* ApplicationPatterns
* Integrations
* Foundations
* Applications

در حال حاضر تمرکز روی BuildingBlocks است.

---

## وضعیت BuildingBlocks

### 01.CrossCutting

خانواده CrossCutting شامل capabilityهای عمومی و بین‌برشی است.

capabilityهای این خانواده:

---

### Object Mapper (Morpher)

وضعیت: تثبیت‌شده

* طراحی کامل شده
* پیاده‌سازی انجام شده
* نام محصولی `Morpher` برای این capability تثبیت شده است
* نام فنی این capability همچنان `Object Mapper` باقی می‌ماند
* این capability به‌عنوان مرجع طراحی سایر capabilityها استفاده می‌شود

---

### Serializer (Prism)

وضعیت: تثبیت‌شده (نسخه اولیه)

* طراحی کامل شده
* پیاده‌سازی اولیه انجام شده
* الگوی provider-based تثبیت شده
* ساختار داخلی پروژه‌ها مشخص شده

---

### Translator (Parrot)

وضعیت: پیاده‌سازی شده (نسخه اولیه)

* طراحی capability کامل شده
* تصمیم‌های معماری از طریق ADR ثبت شده‌اند
* مدل provider-based پیاده‌سازی شده
* پشتیبانی از چند فراهم‌کننده داده (multi-provider) وجود دارد
* رفتار override بر اساس ترتیب registration پیاده‌سازی شده
* کش درون‌حافظه‌ای برای عملکرد سریع پیاده‌سازی شده
* امکان refresh داده‌ها بدون restart فراهم شده است
* SQL Server provider به‌عنوان provider اولیه پیاده‌سازی شده
* ثبت کلیدهای جاافتاده به‌صورت اختیاری پشتیبانی می‌شود
* sample Web API برای نمایش نحوه استفاده اضافه شده است

---

### Caching (StashX)

وضعیت: پیاده‌سازی شده (نسخه اولیه)

* طراحی capability کامل شده
* جایگاه آن در `01.CrossCutting` تثبیت شده است
* مدل طراحی آن به‌صورت provider-based و replacement-based تثبیت شده است
* نام فنی capability برابر `Caching` و نام محصولی آن برابر `StashX` است
* contract مصرفی `IStashX` تعریف شده است
* providerهای اولیه InMemory، Redis و SqlServer پیاده‌سازی شده‌اند
* setup مربوط به SqlServer به‌صورت explicit و opt-in طراحی شده است
* sample واحد برای نمایش switching بین providerها اضافه شده است
* تست اولیه هر سه provider انجام شده است

---

### نکته مهم درباره CrossCutting

* capability مربوط به DependencyInjection از این خانواده خارج شده است
* این capability به خانواده جدید RuntimeAndRegistration منتقل شده است
* دلیل این تغییر، تفاوت ماهیت setup-time در مقابل usage-time است

---

### 02.RuntimeAndRegistration

این خانواده برای capabilityهایی تعریف شده است که مربوط به:

* runtime composition
* service registration
* startup setup

هستند.

این دسته از capabilityها با CrossCutting متفاوت هستند زیرا:

* در زمان setup عمل می‌کنند
* behavior مصرفی مستقیم ارائه نمی‌دهند
* و نقش آن‌ها آماده‌سازی runtime است

---

#### DependencyInjection (Axon)

وضعیت: طراحی و آغاز پیاده‌سازی

* طراحی capability کامل شده
* جایگاه معماری آن در RuntimeAndRegistration تثبیت شده است
* نام محصولی `Axon` برای این capability انتخاب شده است
* نام فنی capability همچنان `DependencyInjection` باقی می‌ماند
* marker-based registration به‌عنوان مسیر اصلی طراحی تثبیت شده است
* convention-based registration به‌صورت opt-in تعریف شده است
* facade ساده روی `IServiceCollection` به‌عنوان entry point در نظر گرفته شده است
* policyهای مهم شامل duplicate handling، filtering و validation تعریف شده‌اند
* هدف اصلی این capability حذف wiring دستی و استانداردسازی registration است

---

## وضعیت مستندات

* ساختار docs تثبیت شده است
* ADRها به‌عنوان ابزار رسمی ثبت تصمیم‌ها پذیرفته شده‌اند
* docs به‌عنوان source of truth در نظر گرفته می‌شوند
* برای capabilityهای اصلی BuildingBlocks، مستندات مستقل در حال تکمیل و تثبیت هستند
* guidelineهای طراحی CrossCutting تثبیت شده‌اند و به‌عنوان مبنای capabilityهای بعدی استفاده می‌شوند
* مستندات مربوط به خانواده RuntimeAndRegistration و capability Axon در حال اضافه شدن و تثبیت هستند

---

## تصمیم‌های تثبیت‌شده مهم

در وضعیت فعلی، این تصمیم‌ها تثبیت شده‌اند:

* taxonomy پنج‌بخشی پروژه

* خانواده‌محور بودن BuildingBlocks

* قرارگیری capabilityهای عمومی در `01.CrossCutting`

* docs as source of truth

* جداسازی نام فنی و نام محصولی در capabilityهای اصلی CrossCutting

* وجود دو الگوی رسمی طراحی برای capabilityهای provider-based در CrossCutting:

  * replacement-based
  * core-orchestrated

* انتقال DependencyInjection از CrossCutting به RuntimeAndRegistration

* تعریف خانواده RuntimeAndRegistration

* انتخاب نام محصولی Axon برای DependencyInjection

* تثبیت marker-based registration به‌عنوان مسیر اصلی

* تعریف convention-based registration به‌صورت opt-in

---

## اولویت‌های نزدیک

تمرکز نزدیک پروژه:

* تکمیل و تثبیت capabilityهای اصلی خانواده CrossCutting
* آغاز و تکمیل implementation capability Axon
* ادامه توسعه BuildingBlockهای داده و ماندگاری
* تکمیل مستندات capabilityها
* تعریف backlog دقیق برای ApplicationPatterns
* تثبیت Foundations اولیه

---

## نکات باز

برخی موضوع‌ها هنوز نیازمند تصمیم‌گیری یا توسعه بیشتر هستند:

* مرز دقیق بعضی capabilityهای RuntimeAndRegistration
* نحوه تعامل RuntimeAndRegistration با سایر خانواده‌ها
* اولویت‌بندی capabilityهای بعدی در این خانواده
* تعیین scope دقیق بعضی ApplicationPatternها
* strategy انتشار و بسته‌بندی capabilityها
* تکمیل ADRهای مرتبط با capabilityهای جدید در صورت نیاز

---

## جمع‌بندی

پروژه از مرحله تعریف خام taxonomy عبور کرده و وارد مرحله تثبیت capabilityهای مرجع، اجرای نسخه‌های اولیه و ثبت رسمی تصمیم‌های معماری شده است.

در وضعیت فعلی:

* Object Mapper، Serializer، Translator و Caching مهم‌ترین capabilityهای پیاده‌سازی‌شده یا تثبیت‌شده در خانواده CrossCutting هستند
* Axon به‌عنوان اولین capability خانواده RuntimeAndRegistration در حال طراحی و پیاده‌سازی است
* docs و ADRها نقش محوری در حفظ یکپارچگی تصمیم‌ها دارند
* مسیر توسعه پروژه به‌سمت تکمیل BuildingBlocks، تثبیت Foundations و سپس توسعه ApplicationPatterns و Applications ادامه پیدا می‌کند
