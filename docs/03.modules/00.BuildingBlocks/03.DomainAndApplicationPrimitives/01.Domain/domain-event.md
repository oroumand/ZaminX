# DomainEvent

## هدف این سند

این سند primitive مربوط به `DomainEvent` را در خانواده
`00.BuildingBlocks/03.DomainAndApplicationPrimitives/01.Domain`
تعریف می‌کند.

نقش این سند:

* تعریف دقیق `DomainEvent`
* روشن کردن دلیل نیاز به آن در زمین X
* مشخص کردن مرز آن با `Entity` و `AggregateRoot`
* ثبت تصمیم‌های طراحی نسخه اول
* مشخص کردن scope و non-goalها
* فراهم کردن مرجع ثابت برای پیاده‌سازی `AggregateRoot`

---

## تعریف

`DomainEvent` یک primitive دامنه برای نمایش رخدادی است که درون مدل دامنه اتفاق افتاده و از نظر دامنه معنادار است.

در زمین X، `DomainEvent` برای این استفاده می‌شود که:

* رخدادهای مهم دامنه را به‌صورت صریح مدل کند
* پایه‌ای ساده برای نگهداری eventها در `AggregateRoot` فراهم کند
* بدون وارد کردن concernهای application یا infrastructure، یک قرارداد حداقلی برای eventهای دامنه ارائه دهد

در نسخه اول، `DomainEvent` intentionally minimal است و فقط به‌عنوان قرارداد پایه تعریف می‌شود.

---

## جایگاه در taxonomy

* دسته: `BuildingBlocks`
* خانواده: `03.DomainAndApplicationPrimitives`
* زیرخانواده: `01.Domain`
* primitive: `DomainEvent`

دلیل قرارگیری در این خانواده این است که `DomainEvent` یک primitive دامنه‌ای reusable و self-contained است که می‌تواند مستقل از یک اپلیکیشن خاص مصرف شود.

---

## چرا در زمین X به DomainEvent نیاز داریم

در زمین X، وجود `DomainEvent` به این دلایل justified است:

1. فراهم کردن یک قرارداد روشن برای رخدادهای دامنه
2. آماده‌سازی پایه لازم برای `AggregateRoot`
3. جلوگیری از drift در نحوه تعریف رخدادهای دامنه در ماژول‌های مختلف
4. حفظ separation بین domain concerns و application/infrastructure concerns

هدف `DomainEvent` این نیست که pipeline اجرای eventها یا مکانیزم dispatch را تعریف کند؛
هدف فقط استانداردسازی حداقل قرارداد event دامنه است.

---

## مدل طراحی نسخه اول

نسخه اول `DomainEvent` در زمین X با این مدل طراحی می‌شود:

* `DomainEvent` به‌صورت `interface` پیاده‌سازی می‌شود
* نام قرارداد پایه برابر `IDomainEvent` است
* نسخه اول این primitive هیچ property یا behavior اجباری ندارد
* این primitive فعلاً فقط نقش marker contract را ایفا می‌کند

---

## قرارداد فنی

### شکل پایه

```csharp
public interface IDomainEvent
{
}
```

---

## چرا interface انتخاب شد

در نسخه اول، زمین X فقط به یک قرارداد حداقلی برای eventهای دامنه نیاز دارد.

انتخاب `interface` به این دلایل انجام شده است:

* ساده‌ترین شکل قرارداد را فراهم می‌کند
* هیچ implementation policy اضافه‌ای تحمیل نمی‌کند
* از ایجاد base class سنگین و زودهنگام جلوگیری می‌کند
* برای نیاز فعلی `AggregateRoot` کافی است

این تصمیم intentional است و به این معنی نیست که در آینده هرگز metadata یا base type اضافه نخواهد شد؛
فقط به این معنی است که در این مرحله، نیاز واقعی برای آن اثبات نشده است.

---

## مرز با Entity

`Entity` و `DomainEvent` دو primitive متفاوت هستند.

### Entity

* نمایش شیء دامنه دارای هویت
* تعریف identity و equality

### DomainEvent

* نمایش رخداد معنادار دامنه
* بدون identity semantics مشابه entity
* بدون equality semantics مبتنی بر identity

نتیجه:
`DomainEvent` entity نیست و نباید concernهای entity را حمل کند.

---

## مرز با AggregateRoot

`DomainEvent` و `AggregateRoot` نیز primitiveهای جدا هستند.

### DomainEvent

* فقط قرارداد event دامنه را تعریف می‌کند

### AggregateRoot

* محل طبیعی نگهداری domain eventها است
* eventها را جمع‌آوری و مدیریت می‌کند
* مرز aggregate را نگه می‌دارد

نتیجه:
`DomainEvent` مسئول نگهداری، dispatch یا lifecycle management نیست؛
این concernها در صورت نیاز در `AggregateRoot` یا لایه‌های بالاتر مدیریت می‌شوند.

---

## چه چیزهایی عمداً داخل DomainEvent نیستند

در نسخه اول، این موارد **جزو scope `DomainEvent` نیستند**:

* `OccurredOn`
* `EventId`
* `Version`
* `CorrelationId`
* `CausationId`
* `AggregateId`
* `UserId`
* dispatch logic
* mediator integration
* persistence concernها
* serialization concernها
* outbox concernها

این حذف‌ها intentional هستند و به معنی فراموش شدن این concernها نیستند؛
فقط به این معنی‌اند که این موارد یا متعلق به primitiveهای دیگرند یا در این مرحله نباید وارد `DomainEvent` شوند.

---

## Non-goals

این primitive در نسخه اول این هدف‌ها را ندارد:

* تعریف event pipeline
* تعریف publisher / dispatcher
* تعریف integration event
* تعریف metadata استاندارد برای همه eventها
* تعریف base class اجباری برای همه رخدادها
* حل concernهای infrastructure مانند outbox یا serialization

---

## legacy notes از پروژه قدیمی زمین

در پروژه قدیمی Onion، domain events در کنار `AggregateRoot` و سایر primitiveهای دامنه وجود داشتند و aggregate root مسئول نگهداری آن‌ها بود.

در زمین X این اصل حفظ می‌شود، اما نسخه اول `DomainEvent` intentionally سبک‌تر تعریف می‌شود.

### چه چیزهایی از نسخه قدیمی حفظ شده‌اند

* اصل وجود domain event به‌عنوان primitive دامنه
* تعلق domain eventها به فضای دامنه، نه application
* ارتباط طبیعی domain event با aggregate root

### چه چیزهایی بازطراحی شده‌اند

* حذف هرگونه behavior یا metadata زودهنگام از primitive پایه
* تعریف حداقلی به‌صورت `IDomainEvent`
* موکول کردن concernهای dispatch و lifecycle به primitiveهای بعدی

---

## naming و structure

### Solution

Kernel.slnx

### Project

ZaminX.BuildingBlocks.Domain

### Namespace

ZaminX.BuildingBlocks.Domain.Events

### مسیر فایل کد

src/00.BuildingBlocks/03.DomainAndApplicationPrimitives/01.Domain/src/ZaminX.BuildingBlocks.Domain/Events/IDomainEvent.cs

### مسیر تست

src/00.BuildingBlocks/03.DomainAndApplicationPrimitives/01.Domain/tests/ZaminX.BuildingBlocks.Domain.Tests/Events/IDomainEventTests.cs

---

## استراتژی تست

حداقل تست لازم برای نسخه اول `DomainEvent` سبک است:

1. امکان پیاده‌سازی `IDomainEvent` باید وجود داشته باشد
2. یک event نمونه باید به‌عنوان `IDomainEvent` قابل شناسایی باشد

در این مرحله تست‌های بیشتری لازم نیست، چون primitive فعلی فقط یک marker contract است.

---

## observationهای باز

موارد زیر در این مرحله هنوز guideline نهایی نیستند و فقط observation محسوب می‌شوند:

* این‌که در آینده `OccurredOn` نیاز شود یا نه
* این‌که base class بهتر از interface شود یا نه
* این‌که metadataهای مشترک برای همه domain eventها لازم شوند یا نه
* این‌که integration بین domain event و mediator چگونه طراحی شود
* این‌که event persistence یا outbox چه نسبتی با این primitive داشته باشند

تا وقتی نیاز واقعی اثبات نشده، این موارد وارد primitive پایه نمی‌شوند.

---

## جمع‌بندی

در زمین X، `DomainEvent` یک primitive ساده، صریح و کم‌مسئولیت است که فقط یک قرارداد پایه برای رخدادهای دامنه ارائه می‌دهد.

این primitive intentionally minimal نگه داشته می‌شود تا:

* reusable بماند
* از over-engineering جلوگیری شود
* پایه‌ای روشن برای `AggregateRoot` فراهم شود
* concernهای application و infrastructure زودتر از موعد وارد دامنه نشوند
