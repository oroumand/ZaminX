# ADR 009: مرز options، diagnostics و exception در capabilityهای provider-based

## وضعیت

پذیرفته‌شده

---

## زمینه

در زمین X بعضی capabilityهای عمومی و بین‌برشی با مدل «انتزاع + provider» طراحی می‌شوند.

نمونه‌های فعلی و نزدیک به این الگو:

* Object Mapper
* Serializer
* بعضی capabilityهای مشابه در خانواده قابلیت‌های عمومی و بین‌برشی

در این نوع capabilityها معمولاً providerهای مختلف بر پایه ابزارها یا کتابخانه‌های متفاوت وجود دارند.

برای مثال:

* در Serializer ممکن است از `System.Text.Json` یا `Newtonsoft.Json` استفاده شود
* در Object Mapper ممکن است providerهای مختلف بر پایه ابزارهای متفاوت تعریف شوند

در این capabilityها سه خطر تکرارشونده وجود دارد:

* options کتابخانه بیرونی وارد قرارداد مصرفی capability شود
* diagnostics و logging به بخشی از API مصرفی capability تبدیل شود
* exceptionهای ابزار بیرونی مستقیماً به مصرف‌کننده نشت کنند

اگر این مرزها رعایت نشوند، مشکلات زیر ایجاد می‌شود:

* مصرف‌کننده عملاً به ابزار بیرونی وابسته می‌شود
* تغییر provider یا تغییر API کتابخانه بیرونی در سراسر سیستم منتشر می‌شود
* قرارداد capability از مسئله اصلی فاصله می‌گیرد و آلوده می‌شود
* concernهای اجرایی به هسته طراحی capability وارد می‌شوند
* exception handling بین providerهای مختلف ناهماهنگ می‌شود

---

## تصمیم

در capabilityهای provider-based زمین X، قواعد زیر اعمال می‌شود.

### 1. مرز options

* options و تنظیمات provider فقط در registration مجازند
* options provider نباید وارد قرارداد مصرفی capability شوند
* API مصرفی capability باید provider-agnostic باقی بماند

### 2. مرز diagnostics

* logging و diagnostics بخشی از قرارداد مصرفی capability نیستند
* providerهای اصلی نباید برای هر فراخوانی logging روتین تولید کنند
* payload خام یا داده حساس نباید در logging عمومی ثبت شود
* در صورت نیاز به diagnostics عمیق، این کار باید با decorator، wrapper یا سازوکار observability بیرونی انجام شود

### 3. مرز exception

* exceptionهای ابزار بیرونی نباید بدون کنترل به مصرف‌کننده نشت کنند
* هر provider باید خطاهای بیرونی را به exception capability-level مناسب تبدیل کند
* exception capability باید context حداقلی لازم برای دیباگ را حفظ کند، مانند:

  * نوع عملیات
  * نوع هدف
  * نام provider
  * inner exception

---

## دلایل تصمیم

این تصمیم برای رسیدن به این اهداف گرفته می‌شود:

* حفظ مرز capability و جلوگیری از نشت API ابزار بیرونی
* ساده و مینیمال نگه داشتن قرارداد مصرفی
* امکان جایگزینی provider بدون شکستن مصرف‌کننده
* جلوگیری از آلودگی capability با concernهای اجرایی
* ایجاد الگوی یکدست برای exception handling در capabilityهای مشابه
* کاهش نویز logging در capabilityهای پرتکرار

---

## پیامدها

### پیامدهای مثبت

* قرارداد capability تمیز و قابل پیش‌بینی باقی می‌ماند
* providerها بهتر محصور می‌شوند
* تغییر provider یا library impact کمتری روی مصرف‌کننده دارد
* observability در لایه درست طراحی می‌شود
* exception handling برای مصرف‌کننده روشن‌تر و یکدست‌تر می‌شود

### هزینه‌ها و محدودیت‌ها

* provider باید بخشی از مسئولیت mapping options و exception را بر عهده بگیرد
* در بعضی سناریوهای خاص، مصرف‌کننده برای استفاده از قابلیت‌های بسیار اختصاصی ابزار بیرونی باید آگاهانه از مرز capability عبور کند
* برای diagnostics عمیق ممکن است نیاز به decorator یا زیرساخت observability جداگانه باشد

---

## دامنه تصمیم

این تصمیم برای همه capabilityها به‌صورت پیش‌فرض الزامی نیست.

این ADR مخصوص capabilityهایی است که:

* با مدل provider-based طراحی شده‌اند
* مرز capability مشخص دارند
* در معرض نشت API ابزار بیرونی هستند

---

## نمونه‌های مشمول

نمونه‌های فعلی:

* Object Mapper
* Serializer

نمونه‌های احتمالی آینده:

* هر capability عمومی و بین‌برشی دیگری که چند provider فنی داشته باشد و با قرارداد مستقل مصرف شود

---

## ارتباط با سایر اسناد

* `docs/04.decision-records/adr/adr-001-cross-cutting-capabilities-provider-model.md`
* `docs/04.decision-records/adr/adr-006-product-naming-vs-technical-naming.md`
* `docs/03.modules/00.BuildingBlocks/01.CrossCutting/serializer.md`
