# AggregateRoot

## هدف این سند

این سند primitive مربوط به `AggregateRoot` را در خانواده
`00.BuildingBlocks/03.DomainAndApplicationPrimitives/01.Domain`
تعریف می‌کند.

نقش این سند:

* تعریف دقیق `AggregateRoot`
* روشن کردن دلیل نیاز به آن در زمین X
* مشخص کردن مرز آن با `Entity`
* مشخص کردن نسبت آن با `DomainEvent`
* ثبت تصمیم‌های طراحی نسخه اول
* مشخص کردن scope و non-goalها
* فراهم کردن مرجع ثابت برای پیاده‌سازی aggregate rootها در زمین X

---

## تعریف

`AggregateRoot` یک primitive دامنه برای نمایش ریشه یک aggregate است.

در زمین X، `AggregateRoot` برای این استفاده می‌شود که:

* مرز aggregate را به‌صورت صریح مدل کند
* نقطه ورود تغییرات aggregate باشد
* محل طبیعی نگهداری domain eventها را فراهم کند
* پایه‌ای روشن برای repository targeting و consistency در سطح aggregate ایجاد کند

`AggregateRoot` یک `Entity` است، اما هر `Entity` یک `AggregateRoot` نیست.

---

## جایگاه در taxonomy

* دسته: `BuildingBlocks`
* خانواده: `03.DomainAndApplicationPrimitives`
* زیرخانواده: `01.Domain`
* primitive: `AggregateRoot`

---

## چرا در زمین X به AggregateRoot نیاز داریم

در زمین X، وجود `AggregateRoot` به این دلایل justified است:

* مشخص کردن مرز aggregate در مدل دامنه
* جلوگیری از blur شدن تفاوت بین entityهای معمولی و rootها
* فراهم کردن محل طبیعی برای نگهداری domain eventها
* ایجاد پایه روشن برای primitiveها و patternهای بعدی خانواده دامنه و اپلیکیشن

هدف `AggregateRoot` این نیست که یک framework کامل برای event sourcing یا lifecycle management بسازد.
هدف فقط استانداردسازی حداقل رفتار مشترک aggregate rootها است.

---

## مدل طراحی نسخه اول

نسخه اول `AggregateRoot` در زمین X با این مدل طراحی می‌شود:

* `AggregateRoot` به‌صورت `abstract class` پیاده‌سازی می‌شود
* از `Entity<TId>` ارث می‌برد
* `IAggregateRoot` را پیاده‌سازی می‌کند
* collection داخلی domain eventها را نگه می‌دارد
* امکان افزودن، خواندن و پاک کردن domain eventها را فراهم می‌کند
* concernهای aggregate-level را از `Entity` جدا نگه می‌دارد

---

## قرارداد فنی

### شکل پایه

```csharp
public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot
```

### قرارداد marker

```csharp
public interface IAggregateRoot
{
}
```

### اعضای اصلی نسخه اول

* collection داخلی برای نگهداری `IDomainEvent`
* `IReadOnlyCollection<IDomainEvent> DomainEvents`
* `AddDomainEvent(IDomainEvent domainEvent)`
* `ClearDomainEvents()`

---

## چرا IAggregateRoot متدی ندارد

در نسخه اول، `IAggregateRoot` یک marker interface است.

هدف آن این است که semantic مشخصی را بیان کند:

> این type، ریشه یک aggregate است.

این interface برای behavior طراحی نشده است.

### دلیل این تصمیم

* نقش اصلی interface، typing و constraint است
* behavior واقعی داخل `AggregateRoot<TId>` باقی می‌ماند
* encapsulation aggregate حفظ می‌شود
* concernهای داخلی aggregate به abstraction سطح بالاتر نشت نمی‌کنند
* از abstraction غیرضروری جلوگیری می‌شود

### نتیجه

در این مرحله، متدی مثل `AddDomainEvent` داخل `IAggregateRoot` تعریف نمی‌شود.

---

## مرز با Entity

`Entity` و `AggregateRoot` در زمین X دو primitive متفاوت هستند.

### Entity

* identity
* equality
* primitive پایه برای اشیای دامنه دارای هویت

### AggregateRoot

* خود یک entity است
* مرز aggregate را مشخص می‌کند
* محل طبیعی concernهای aggregate-level است
* domain eventها را نگه می‌دارد

### نتیجه

`AggregateRoot` از `Entity` ارث می‌برد، اما concernهای اضافه‌ای دارد که نباید وارد `Entity` شوند.

---

## نسبت با DomainEvent

`AggregateRoot` و `DomainEvent` دو primitive مکمل‌اند.

### DomainEvent

* قرارداد پایه برای رخدادهای دامنه را تعریف می‌کند

### AggregateRoot

* رخدادهای دامنه را نگهداری می‌کند
* امکان جمع‌آوری و پاک‌سازی آن‌ها را فراهم می‌کند

### نتیجه

`AggregateRoot` به `IDomainEvent` وابسته است، اما مسئول dispatch یا publish کردن eventها نیست.

---

## چه چیزهایی عمداً داخل AggregateRoot نیستند

در نسخه اول، این موارد **جزو scope `AggregateRoot` نیستند**:

* event dispatch
* mediator integration
* event sourcing behavior
* reflection-based apply
* replay infrastructure
* repository implementation
* persistence concernها
* outbox concernها
* concurrency concernها
* auditing concernها

این حذف‌ها intentional هستند و به معنی فراموش شدن این concernها نیستند؛
فقط به این معنی‌اند که این موارد در این مرحله نباید وارد primitive پایه شوند.

---

## Non-goals

این primitive در نسخه اول این هدف‌ها را ندارد:

* تبدیل شدن به زیرساخت event sourcing
* پیاده‌سازی pipeline برای publish domain event
* تعریف repository abstraction
* حل concernهای infrastructure
* تحمیل lifecycle پیچیده به aggregateها

---

## legacy notes

در پروژه قدیمی Onion، `AggregateRoot` علاوه بر نگهداری domain eventها، behaviorهای بیشتری مثل `Apply`، `Mutate` و الگوهای reflection-based برای event application داشت.

در زمین X این تصمیم‌ها در نسخه اول حفظ نشده‌اند.

### چه چیزهایی از نسخه قدیمی حفظ شده‌اند

* اصل جدا بودن `AggregateRoot` از `Entity`
* اصل نگهداری domain eventها در aggregate root
* جایگاه طبیعی aggregate root به‌عنوان مرز aggregate

### چه چیزهایی بازطراحی شده‌اند

* حذف behaviorهای reflection-based
* حذف event application convention
* minimal شدن primitive پایه
* محدود شدن scope به نگهداری eventها و مرزبندی aggregate

---

## naming و structure

### Solution

Kernel.slnx

### Project

ZaminX.BuildingBlocks.Domain

### Namespace

ZaminX.BuildingBlocks.Domain.Entities

### مسیر فایل‌های کد

* `src/00.BuildingBlocks/03.DomainAndApplicationPrimitives/01.Domain/src/ZaminX.BuildingBlocks.Domain/Entities/IAggregateRoot.cs`
* `src/00.BuildingBlocks/03.DomainAndApplicationPrimitives/01.Domain/src/ZaminX.BuildingBlocks.Domain/Entities/AggregateRoot.cs`

### مسیر تست

* `src/00.BuildingBlocks/03.DomainAndApplicationPrimitives/01.Domain/tests/ZaminX.BuildingBlocks.Domain.Tests/Entities/AggregateRootTests.cs`

---

## استراتژی تست

حداقل تست‌های لازم برای نسخه اول `AggregateRoot`:

1. aggregate root باید `IAggregateRoot` را پیاده‌سازی کند
2. در شروع باید domain event نداشته باشد
3. افزودن event باید collection را به‌درستی به‌روزرسانی کند
4. پاک کردن eventها باید collection را خالی کند
5. افزودن `null` باید خطا بدهد

---

## observationهای باز

موارد زیر در این مرحله هنوز guideline نهایی نیستند و فقط observation محسوب می‌شوند:

* این‌که در آینده access modifier متد افزودن event تغییر کند یا نه
* این‌که متدهایی مثل `RemoveDomainEvent` نیاز شوند یا نه
* این‌که abstraction دیگری برای event access لازم شود یا نه
* این‌که integration با mediator یا outbox چگونه طراحی شود
* این‌که versioning یا metadata روی event collection لازم شود یا نه

تا وقتی نیاز واقعی اثبات نشده، این موارد وارد primitive پایه نمی‌شوند.

---

## جمع‌بندی

در زمین X، `AggregateRoot` یک primitive ساده، صریح و کم‌مسئولیت است که:

* از `Entity` ارث می‌برد
* مرز aggregate را مشخص می‌کند
* domain eventها را نگه می‌دارد
* concernهای aggregate-level را از `Entity` جدا می‌کند

این primitive intentionally minimal نگه داشته می‌شود تا:

* reusable بماند
* از over-engineering جلوگیری شود
* پایه‌ای روشن برای primitiveها و patternهای بعدی فراهم شود
* concernهای application و infrastructure زودتر از موعد وارد دامنه نشوند
