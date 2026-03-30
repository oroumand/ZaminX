# وضعیت پروژه زمین X

## وضعیت کلی
زمین X در فاز طراحی محصول و معماری قرار دارد.

در این فاز، تمرکز بر تعریف محصول، taxonomy، دسته‌های اصلی، مرز ماژول‌ها، ADRها و backlog است.  
فعلاً توسعه کد به‌جز بخش Mapper مبنا نیست.

---

## وضعیت ریپو
- ریپو تمیز شده است
- ساختار اصلی ریشه حفظ شده است
- ماژول `Mapper` در `00.BuildingBlocks` وجود دارد و مرجع سبک ساخت capabilityها است

---

## ساختار فعلی src
- `00.BuildingBlocks`
- `01.ApplicationPatterns`
- `02.Integrations`
- `03.Foundations`
- `04.Applications`

---

## ماژول‌های موجود

### BuildingBlocks
- `Mapper` ✅ تکمیل شده

---

## ماژول‌های در دست طراحی
- `Translator`
- `ApplicationPatterns`
- `Integrations`
- `Foundations`
- `Applications`
- نقشه اولیه ماژول‌ها

---

## تصمیم‌های تثبیت‌شده

- taxonomy پروژه بر اساس پنج دسته اصلی `BuildingBlocks`, `ApplicationPatterns`, `Integrations`, `Foundations`, `Applications` سازمان‌دهی می‌شود
- هر capability تا حد امکان مستقل، ماژولار و قابل بازاستفاده طراحی می‌شود
- مستندسازی بخشی از فرایند اصلی توسعه است
- فعلاً طراحی محصول و معماری مقدم بر توسعه ماژول‌های جدید است
- docs به‌عنوان source of truth پروژه در نظر گرفته می‌شود

### تعریف BuildingBlocks
- معیار اصلی قرارگیری در `BuildingBlocks` استقلال مصرف، خودبسندگی و قابل بازاستفاده بودن است
- یک جزء می‌تواند در `BuildingBlocks` قرار بگیرد حتی اگر به تکنولوژی خاص وابسته باشد، مادامی که به‌صورت مستقل قابل استفاده باشد
- وجود انتزاع الزامی نیست؛ فقط در صورت نیاز ایجاد می‌شود
- اگر انتزاع مناسبی از قبل در بستر .NET وجود داشته باشد، از همان استفاده می‌شود
- setup، registration و providerها نیز در صورت مستقل بودن می‌توانند در همین دسته قرار بگیرند

### تعریف اجزای pattern-like در BuildingBlocks
- الگوهای پایه مانند `Entity`، `AggregateRoot`، `ValueObject`، `DomainEvent` و `Mediator` در صورت ارائه به‌عنوان جزء مستقل و قابل بازاستفاده، در `BuildingBlocks` قرار می‌گیرند
- وجود ماهیت الگویی، آن‌ها را الزاماً در `ApplicationPatterns` قرار نمی‌دهد

### تعریف ApplicationPatterns
- `ApplicationPatterns` الگوهای پرکاربرد سطح اپلیکیشن هستند که معمولاً از چند BuildingBlock استفاده می‌کنند
- این دسته برای ارائه جریان‌ها و رفتارهای تکرارشونده در سطح اپلیکیشن استفاده می‌شود

### تعریف Integrations
- `Integrations` مسئول اتصال به سیستم‌ها و سرویس‌های بیرونی هستند
- معیار اصلی این دسته، اتصال به boundary بیرونی است، نه صرفاً وابستگی به تکنولوژی خاص

### تعریف Foundations
- `Foundations` شامل ساختارهای آماده و scaffoldها برای شروع پروژه‌ها هستند
- این دسته شامل artifact قابل اجرا نیست
- جهت‌گیری فعلی این دسته بر پایه `MonolithStructure` و `ModularMonolith` است
- `MicroserviceModule` فعلاً به‌عنوان گزینه باز برای آینده ثبت می‌شود

### تعریف Applications
- `Applications` شامل اپلیکیشن‌های کوچک، قابل اجرا و قابل استفاده هستند که بر پایه زمین X ساخته می‌شوند و به شروع سریع توسعه و adoption کمک می‌کنند
- این دسته بخشی از ارزش محصول است، نه صرفاً demo

### تعریف Auditing
- در حوزه داده و ماندگاری، `Auditing` یکی از capabilityهای اصلی در نظر گرفته می‌شود
- `Auditing` شامل ثبت خودکار اطلاعات ایجاد و تغییر، و در صورت نیاز ثبت تاریخچه کامل تغییرات است
- برای مشاهده و تحلیل تاریخچه تغییرات، یک اپلیکیشن در دسته `Applications` در نظر گرفته می‌شود

### قواعد تصمیم‌گیری برای جایگذاری
- اگر یک جزء مستقل و قابل استفاده است → `BuildingBlocks`
- اگر یک الگوی تکرارشونده در سطح اپلیکیشن است → `ApplicationPatterns`
- اگر اتصال به بیرون است → `Integrations`
- اگر ساختار پروژه، قالب یا scaffold است → `Foundations`
- اگر اپلیکیشن قابل اجرا و ارزشمند برای استفاده مستقیم است → `Applications`

### ساختار خانواده‌ای BuildingBlocks
- BuildingBlocks به خانواده‌های مشخص تقسیم می‌شوند
- هر خانواده دارای یک پوشه مستقل در مستندات است
- هر خانواده یک فایل index برای تعریف کلی دارد
- هر ماژول دارای مستند مستقل خود در همان خانواده است
- این ساختار به‌عنوان ساختار استاندارد مستندسازی BuildingBlocks تثبیت شده است

### الگوی مستندسازی ماژول‌ها
- برای مستندسازی هر ماژول یک template استاندارد تعریف شده است
- این template شامل تعریف، مسئله، جایگاه، تصمیم‌های طراحی و نحوه استفاده است
- این الگو برای تمام دسته‌ها (BuildingBlocks، ApplicationPatterns، Integrations و ...) استفاده می‌شود
- هدف این الگو حفظ consistency و جلوگیری از تکرار است

### جایگاه Mapper
- Mapper به‌عنوان یک BuildingBlock در خانواده CrossCutting در نظر گرفته می‌شود
- Mapper اولین ماژول کامل پروژه است و نقش مرجع طراحی را دارد
- مستندسازی و تحلیل Mapper مبنای تصمیم‌های معماری برای سایر ماژول‌ها خواهد بود



### مدل provider-based برای capabilityهای عمومی و بین‌برشی
- در capabilityهای عمومی و بین‌برشی، اگر نیاز واقعی به مرز انتزاعی وجود داشته باشد، از مدل «انتزاع + provider» استفاده می‌شود
- در این مدل، مصرف‌کننده به قرارداد capability وابسته می‌شود، نه به ابزار بیرونی
- registration هر provider در خود همان provider نگه داشته می‌شود
- ابزار بیرونی نباید به API مصرفی capability نشت کند
- READMEهای محلی نقش فنی و ورود سریع دارند، اما مرجع اصلی تصمیم‌های معماری و محصولی بخش docs است

### دو نوع قرارداد در capabilityها
- در capabilityهای این خانواده، قراردادها می‌توانند در دو دسته قرار بگیرند:
  - قراردادهای مصرفی capability برای استفاده توسعه‌دهنده
  - قراردادهای درونی capability برای نیازهای داخلی خود capability
- این تفکیک برای capabilityهایی مانند Translation حیاتی است، چون خود capability ممکن است برای دسترسی به sourceها یا providerهای داده به قراردادهای درونی نیاز داشته باشد
- این تفکیک از مخلوط شدن API مصرفی capability با dependencyهای درونی آن جلوگیری می‌کند

### جایگاه و naming رسمی Object Mapper
- نام این capability در سطح محصول `Object Mapper` است
- سند این capability در مسیر `docs/03.modules/00.BuildingBlocks/01.CrossCutting/object-mappers.md` نگهداری می‌شود
- naming فنی پروژه‌ها، پوشه‌ها و فضای نام‌های این capability باید با taxonomy رسمی زمین X هم‌راستا شود
- الگوی مطلوب naming فنی این capability از جنس این ساختار است:
  - `ZaminX.BuildingBlocks.CrossCutting.ObjectMapper.Abstractions`
  - `ZaminX.BuildingBlocks.CrossCutting.ObjectMapper.AutoMapper`
---

## تصمیم‌های باز
- فهرست اولیه ماژول‌های هر دسته
- تعیین فهرست اولیه Applications
- نهایی‌سازی ساختار `MonolithStructure`
- نهایی‌سازی ساختار `ModularMonolith`
- بررسی و تصمیم‌گیری نهایی درباره `MicroserviceModule`
- تعیین مرز دقیق بعضی capabilityها در نقشه ماژول‌ها
- ساختار backlog و roadmap

---

## فاز فعلی
فاز ۱: Product Definition & Architecture Design

---

## خروجی‌های مورد انتظار این فاز
- Product Vision
- Consumer Entry Points
- Repository Taxonomy
- Layer Definitions
- Module Map
- Initial Applications Map
- ADRهای کلیدی
- Backlog اولیه
- Roadmap اولیه

---

## آخرین به‌روزرسانی مهم
- ریپو به وضعیت تمیز بعد از Mapper بازگردانده شد
- تصمیم گرفته شد ادامه کار از طراحی محصول و معماری شروع شود، نه از توسعه ماژول‌های جدید
- ساختار نهایی docs تعریف شد
- مستندسازی به 7 بخش اصلی تقسیم شد:
  - vision
  - architecture
  - modules
  - adr
  - backlog
  - guidelines
  - reference
- ترتیب تولید مستندات به‌صورت مرحله‌ای تعریف شد
- taxonomy پروژه از مدل چهار دسته‌ای به مدل پنج دسته‌ای توسعه یافت
- `Applications` به‌عنوان دسته مستقل به taxonomy پروژه اضافه شد
- ساختار مستندات `03.modules` به‌صورت خانواده‌محور برای BuildingBlocks نهایی شد
- برای هر خانواده یک index مستقل تعریف شد
- ساختار مستندات ماژول‌ها به‌صورت فایل مستقل برای هر ماژول تثبیت شد
- template استاندارد برای مستندات ماژول‌ها تعریف شد

---

## نکات اجرایی فعلی
- `Mapper` مرجع سبک طراحی capabilityها در پروژه است
- `Mapper` به‌عنوان reference implementation برای خانواده CrossCutting در نظر گرفته می‌شود
- الگوی طراحی Mapper مبنای طراحی ماژول‌های بعدی مانند Translator، Serializer و ... خواهد بود- `03.modules/index.md` مرجع نقشه اولیه ماژول‌ها است
- هر تصمیم مهم باید هم‌زمان در اسناد مرتبط و در این فایل ثبت شود
- تصمیم‌های عمیق و ماندگار باید به ADR منتقل شوند