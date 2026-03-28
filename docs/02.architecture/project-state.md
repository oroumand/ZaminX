# وضعیت پروژه Zamin X

## وضعیت کلی
Zamin X در فاز طراحی محصول و معماری قرار دارد.
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
- strategy مستندسازی نهایی
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