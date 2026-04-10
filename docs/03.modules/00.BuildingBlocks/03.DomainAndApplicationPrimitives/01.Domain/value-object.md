# ValueObject

## هدف این سند

این سند primitive مربوط به `ValueObject` را در خانواده
`00.BuildingBlocks/03.DomainAndApplicationPrimitives/01.Domain`
تعریف می‌کند.

نقش این سند:

* تعریف دقیق `ValueObject`
* روشن کردن دلیل نیاز به آن در زمین X
* مشخص کردن مرز آن با `Entity`
* ثبت تصمیم‌های طراحی نسخه اول
* مشخص کردن scope و non-goalها
* فراهم کردن مرجع ثابت برای توسعه مدل دامنه در زمین X

---

## تعریف

`ValueObject` یک primitive دامنه برای نمایش مفهومی است که **هویت مستقل ندارد** و بر اساس **مقدار** شناخته می‌شود.

در زمین X، `ValueObject` برای این استفاده می‌شود که:

* value-based equality را در مدل دامنه استاندارد کند
* تفاوت مفهومی بین objectهای دارای هویت و objectهای مقداری را روشن نگه دارد
* یک base type سبک و reusable برای value objectهای دامنه فراهم کند

`ValueObject` در این نسخه intentionally minimal است و فقط concernهای اصلی مربوط به equality و identity-less semantics را پوشش می‌دهد.

---

## جایگاه در taxonomy

* دسته: `BuildingBlocks`
* خانواده: `03.DomainAndApplicationPrimitives`
* زیرخانواده: `01.Domain`
* primitive: `ValueObject`

دلیل قرارگیری در این خانواده این است که `ValueObject` یک primitive دامنه‌ای reusable و self-contained است که می‌تواند مستقل از یک اپلیکیشن خاص مصرف شود.

---

## چرا در زمین X به ValueObject نیاز داریم

در زمین X، وجود `ValueObject` به این دلایل justified است:

1. جلوگیری از تکرار logic مربوط به value-based equality
2. حفظ مرز روشن بین `Entity` و objectهای مقداری
3. جلوگیری از drift در پیاده‌سازی equality در ماژول‌های مختلف
4. فراهم کردن یک primitive پایه برای مدل دامنه

هدف `ValueObject` این نیست که validation framework یا parsing infrastructure را در خود جمع کند؛
هدف فقط استانداردسازی حداقل رفتار مشترک value objectها است.

---

## مدل طراحی نسخه اول

نسخه اول `ValueObject` در زمین X با این مدل طراحی می‌شود:

* `ValueObject` به‌صورت generic و self-referencing پیاده‌سازی می‌شود
* از `IEquatable<TValueObject>` استفاده می‌کند
* equality بر اساس sequence اجزای برابری تعریف می‌شود
* subclass موظف است اجزای برابری را از طریق `GetEqualityComponents()` مشخص کند

### شکل پایه

```csharp
public abstract class ValueObject<TValueObject> : IEquatable<TValueObject>
    where TValueObject : ValueObject<TValueObject>
```

این مدل، strongly-typed equality را فراهم می‌کند و از version غیر generic برای primitive پایه استفاده نمی‌کند.

---

## چرا self-referencing generic انتخاب شد

در زمین X، برای `ValueObject` از الگوی self-referencing generic استفاده می‌شود:

```csharp
where TValueObject : ValueObject<TValueObject>
```

### دلیل این تصمیم

* امکان استفاده از `IEquatable<TValueObject>` را فراهم می‌کند
* equality را strongly-typed می‌کند
* intent طراحی را روشن‌تر می‌کند
* به الگوی موفق استفاده‌شده در پروژه قدیمی زمین نزدیک است
* برای `ValueObject` این generic اضافه، complexity بی‌فایده محسوب نمی‌شود

این تصمیم intentional است و برای `ValueObject` justified است، حتی اگر در بعضی primitiveهای دیگر generic اضافی پذیرفته نشده باشد.

---

## equality semantics

برابری در `ValueObject` از جنس **value-based equality** است.

یعنی اگر:

* دو object از یک type باشند
* و sequence اجزای برابری آن‌ها برابر باشد

آنگاه equal محسوب می‌شوند.

### نکات مهم

* `ValueObject` هویت مستقل ندارد
* برابری بر اساس همه equality componentهای تعریف‌شده انجام می‌شود
* ترتیب اجزا در `GetEqualityComponents()` مهم است
* دو value object از typeهای متفاوت حتی اگر مقادیر ظاهراً مشابهی داشته باشند equal محسوب نمی‌شوند

---

## قرارداد فنی

### اعضای اصلی

* `bool Equals(TValueObject? other)`
* `override bool Equals(object? obj)`
* `override int GetHashCode()`
* `protected abstract IEnumerable<object?> GetEqualityComponents()`
* عملگرهای `==` و `!=`

### قرارداد subclass

هر subclass باید اجزای برابری خود را به‌صورت صریح برگرداند:

```csharp
protected override IEnumerable<object?> GetEqualityComponents()
{
    yield return ...;
    yield return ...;
}
```

این تصمیم باعث می‌شود equality definition صریح، قابل بازبینی و قابل تست بماند.

---

## مرز با Entity

`Entity` و `ValueObject` در زمین X دو primitive متفاوت هستند.

### Entity

* هویت دارد
* برابری آن مبتنی بر identity است
* با `Id` شناخته می‌شود

### ValueObject

* هویت مستقل ندارد
* برابری آن مبتنی بر مقدار است
* با مجموعه مقادیرش شناخته می‌شود

### نتیجه

هرجا مفهوم دامنه بدون identity مستقل معنا دارد، باید به‌سمت `ValueObject` رفت، نه `Entity`.

---

## چه چیزهایی عمداً داخل ValueObject نیستند

در نسخه اول، این موارد **جزو scope `ValueObject` نیستند**:

* validation
* parsing
* factory infrastructure
* persistence concernها
* serialization concernها
* empty instance policy
* comparable behavior
* mutation tracking
* domain event concernها

این حذف‌ها intentional هستند و به معنی فراموش شدن این concernها نیستند؛
فقط به این معنی‌اند که این موارد در این مرحله نباید وارد primitive پایه شوند.

---

## Non-goals

این primitive در نسخه اول این هدف‌ها را ندارد:

* تبدیل شدن به framework کامل value object
* اجبار به immutable سازی از طریق زیرساخت
* تعریف contract برای persistence mapping
* حل concernهای validation یا parsing
* ارائه generic baseهای متعدد برای سناریوهای خاص

---

## legacy notes

در پروژه قدیمی زمین، `ValueObject` با الگوی generic self-referencing طراحی شده بود و subtypeها equality components خود را به‌صورت صریح تعریف می‌کردند.

در زمین X، این بخش از legacy حفظ شده است.

### چه چیزهایی از نسخه قدیمی حفظ شده‌اند

* self-referencing generic pattern
* strongly-typed equality
* تعریف equality از طریق equality components
* مرز روشن با `Entity`

### چه چیزهایی بازطراحی شده‌اند

* minimal نگه داشتن primitive پایه
* وارد نکردن concernهای اضافه مثل validation و infrastructure
* محدود کردن scope به semantics اصلی value object

---

## naming و structure

### Solution

Kernel.slnx

### Project

ZaminX.BuildingBlocks.Domain

### Namespace

ZaminX.BuildingBlocks.Domain.ValueObjects

### مسیر فایل کد

src/00.BuildingBlocks/03.DomainAndApplicationPrimitives/01.Domain/src/ZaminX.BuildingBlocks.Domain/ValueObjects/ValueObject.cs

### مسیر تست

src/00.BuildingBlocks/03.DomainAndApplicationPrimitives/01.Domain/tests/ZaminX.BuildingBlocks.Domain.Tests/ValueObjects/ValueObjectTests.cs

---

## استراتژی تست

حداقل تست‌های لازم برای نسخه اول `ValueObject`:

1. دو value object از یک نوع با اجزای برابر باید equal باشند
2. دو value object از یک نوع با اجزای متفاوت نباید equal باشند
3. مقایسه با `null` باید درست کار کند
4. دو reference `null` باید برابر باشند
5. یک instance با خودش باید equal باشد
6. دو value object برابر باید hash code یکسان داشته باشند

---

## observationهای باز

موارد زیر در این مرحله هنوز guideline نهایی نیستند و فقط observation محسوب می‌شوند:

* این‌که در آینده نسخه non-generic لازم شود یا نه
* این‌که helperهای بیشتری برای equality یا normalization نیاز شوند یا نه
* این‌که بعضی value objectها نیاز به `IComparable` پیدا کنند یا نه
* این‌که relation بین `ValueObject` و validation pattern در آینده چه باشد

تا وقتی نیاز واقعی اثبات نشده، این موارد وارد primitive پایه نمی‌شوند.

---

## جمع‌بندی

در زمین X، `ValueObject` یک primitive ساده، صریح و کم‌مسئولیت است که:

* identity مستقل ندارد
* value-based equality را استاندارد می‌کند
* با الگوی generic self-referencing پیاده‌سازی می‌شود
* concernهای اضافه را وارد دامنه نمی‌کند

این primitive intentionally minimal نگه داشته می‌شود تا:

* reusable بماند
* از over-engineering جلوگیری شود
* پایه‌ای روشن برای مدل دامنه فراهم شود
* مرز مفهومی آن با `Entity` شفاف بماند
