# وضعیت پروژه زمین X

## وضعیت کلی
زمین X در فاز طراحی محصول و معماری قرار دارد.
در این فاز، تمرکز بر تعریف محصول، taxonomy، لایه‌ها، مرز ماژول‌ها، ADRها و backlog است.
فعلاً توسعه کد به‌جز بخش Mapper مبنا نیست.

## وضعیت ریپو
- ریپو تمیز شده است
- ساختار اصلی ریشه حفظ شده است
- ماژول `Mapper` در `00.BuildingBlocks` وجود دارد و مرجع سبک ساخت capabilityها است

## ساختار فعلی src
- `00.BuildingBlocks`
- `01.ApplicationPatterns`
- `02.Integrations`
- `03.Foundations`

## ماژول‌های موجود
### BuildingBlocks
- Mapper ✅ تکمیل شده

## ماژول‌های در دست طراحی
- Translator
- Application Patterns
- Foundations اولیه
- نقشه Integrations

## تصمیم‌های تثبیت‌شده
- پروژه بر اساس چهار لایه اصلی `00.BuildingBlocks`, `01.ApplicationPatterns`, `02.Integrations`, `03.Foundations` سازمان‌دهی می‌شود
- هر capability تا حد امکان مستقل و ماژولار طراحی می‌شود
- مستندسازی بخشی از فرایند اصلی توسعه است
- فعلاً طراحی محصول و معماری مقدم بر توسعه ماژول‌های جدید است

## تصمیم‌های باز
- تعریف دقیق مسئولیت هر لایه
- فهرست اولیه ماژول‌های هر لایه
- مرزبندی بین BuildingBlocks و ApplicationPatterns
- تعریف دقیق Foundations
- استراتژِی مستندسازی نهایی
- ساختار backlog و roadmap

## فاز فعلی
فاز ۱: Product Definition & Architecture Design

## خروجی‌های مورد انتظار این فاز
- Product Vision
- Consumer Entry Points
- Repository Taxonomy
- Layer Definitions
- Module Map
- ADRهای کلیدی
- Backlog اولیه
- Roadmap اولیه

## آخرین به‌روزرسانی مهم
- ریپو به وضعیت تمیز بعد از Mapper بازگردانده شد
- تصمیم گرفته شد ادامه کار از طراحی محصول و معماری شروع شود، نه از توسعه ماژول‌های جدید

## تصمیم‌های جدید

- ساختار نهایی docs تعریف شد
- مستندسازی به 7 بخش اصلی تقسیم شد:
  vision, architecture, modules, adr, backlog, guidelines, reference
- docs به عنوان source of truth پروژه در نظر گرفته شد
- ترتیب تولید مستندات به صورت مرحله‌ای تعریف شد

## تصمیم‌های جدید

### تعریف taxonomy پروژه

پروژه Zamin X بر اساس پنج دسته اصلی سازمان‌دهی می‌شود:

- BuildingBlocks
- ApplicationPatterns
- Integrations
- Foundations
- Applications

---

### تعریف BuildingBlocks

BuildingBlocks اجزای مستقل، reusable و self-contained هستند که می‌توانند به‌تنهایی استفاده و پکیج شوند.

ویژگی‌ها:
- ممکن است abstraction داشته باشند یا نداشته باشند
- ممکن است به تکنولوژی خاص وابسته باشند
- می‌توانند provider-based باشند
- می‌توانند شامل setup و registration باشند

معیار اصلی: استقلال مصرف

---

### تعریف ApplicationPatterns

ApplicationPatterns الگوهای پرکاربرد سطح اپلیکیشن هستند که معمولاً از چند BuildingBlock استفاده می‌کنند.

---

### تعریف Integrations

Integrations مسئول اتصال به سیستم‌ها و سرویس‌های بیرونی هستند.

---

### تعریف Foundations

Foundations شامل ساختارهای آماده و scaffoldها برای شروع پروژه‌ها هستند.

---

### تعریف Applications

Applications شامل اپلیکیشن‌های کوچک، runnable و قابل استفاده هستند که بر پایه Zamin X ساخته می‌شوند و به adoption سریع کمک می‌کنند.

---

### قواعد تصمیم‌گیری برای جایگذاری

- اگر یک جزء مستقل و قابل استفاده است → BuildingBlocks
- اگر یک الگوی تکرارشونده است → ApplicationPatterns
- اگر اتصال به بیرون است → Integrations
- اگر ساختار پروژه است → Foundations
- اگر اپ runnable است → Applications
