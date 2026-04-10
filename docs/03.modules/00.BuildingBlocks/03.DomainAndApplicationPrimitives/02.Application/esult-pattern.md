# Result Pattern

## هدف این سند

این سند primitiveهای مربوط به `Result Pattern` را در خانواده
`00.BuildingBlocks/03.DomainAndApplicationPrimitives/02.Application`
تعریف می‌کند.

نقش این سند:

* تعریف دقیق `Error`
* تعریف دقیق `Result`
* تعریف دقیق `Result<TValue>`
* روشن کردن دلیل نیاز به این pattern در زمین X
* مشخص کردن مرز آن با exceptionهای دامنه
* ثبت تصمیم‌های طراحی نسخه اول
* مشخص کردن scope و non-goalها
* فراهم کردن مرجع ثابت برای `Command`، `Query` و `Handler`

---

## تعریف

`Result Pattern` در زمین X یک primitive application-level برای نمایش outcome یک عملیات است، بدون این‌که flow عادی برنامه بر مبنای exception کنترل شود.

این pattern از سه جزء اصلی تشکیل می‌شود:

* `Error`
* `Result`
* `Result<TValue>`

در این مدل:

* `Error` نمایش‌دهنده یک خطای application-readable است
* `Result` outcome عملیات بدون مقدار را مدل می‌کند
* `Result<TValue>` outcome عملیات دارای مقدار را مدل می‌کند

---

## جایگاه در taxonomy

* دسته: `BuildingBlocks`
* خانواده: `03.DomainAndApplicationPrimitives`
* زیرخانواده: `02.Application`
* primitive: `ResultPattern`

دلیل قرارگیری در این خانواده این است که `Result Pattern` یک primitive اپلیکیشنی reusable و self-contained است که مستقل از transport و frameworkهای بیرونی قابل استفاده است.

---

## چرا در زمین X به Result Pattern نیاز داریم

در زمین X، وجود `Result Pattern` به این دلایل justified است:

1. کاهش وابستگی flow عادی application به exception
2. ارائه outcome صریح و قابل‌پیش‌بینی برای commandها و queryها
3. فراهم کردن مدل یکدست برای success و failure
4. فراهم کردن بستری مناسب برای mapping از domain errors به application-level results
5. آماده‌سازی پایه برای handlerها و mediator

هدف `Result Pattern` این نیست که جایگزین کامل exceptionها شود؛
هدف آن استانداردسازی outcome عملیات در سطح application است.

---

## اجزای اصلی

### Error

`Error` یک primitive کوچک برای نمایش یک خطای application-readable است.

ویژگی‌های اصلی:

* دارای `Code` از نوع `string`
* دارای `Message` از نوع `string`
* immutable
* مناسب برای حمل semantic failure در سطح application

### Result

`Result` outcome عملیات بدون مقدار را مدل می‌کند.

ویژگی‌های اصلی:

* success یا failure بودن را مشخص می‌کند
* مجموعه‌ای read-only از خطاها را نگه می‌دارد
* در حالت success هیچ خطایی ندارد
* در حالت failure حداقل یک خطا دارد

### Result<TValue>

`Result<TValue>` outcome عملیات دارای مقدار را مدل می‌کند.

ویژگی‌های اصلی:

* در حالت success یک `Value` دارد
* در حالت failure فاقد `Value` قابل دسترسی است
* از semantics پایه `Result` استفاده می‌کند
* برای queryها و commandهایی که خروجی دارند مناسب است

---

## مدل طراحی نسخه اول

نسخه اول `Result Pattern` در زمین X با این مدل طراحی می‌شود:

* `Error` به‌صورت `record` تعریف می‌شود
* `Result` یک کلاس پایه برای عملیات بدون مقدار است
* `Result<TValue>` از `Result` ارث می‌برد
* failure می‌تواند شامل یک یا چند `Error` باشد
* success همیشه بدون خطا است
* failure همیشه حداقل یک خطا دارد
* `FirstError` فقط یک convenience member است، نه منبع اصلی حقیقت

---

## چرا multi-error انتخاب شد

در لایه Application، failure همیشه تک‌خطا نیست.

نمونه‌های رایج:

* validation چندفیلدی
* commandهایی با چند rule failure
* batch operationها
* orchestrationها

به همین دلیل، نسخه اول `Result` در زمین X از مدل multi-error استفاده می‌کند.

### invariantهای اصلی

* success → `Errors` خالی
* failure → `Errors` حداقل یک عضو

این مدل هم ساده می‌ماند، هم برای use-caseهای واقعی application کافی است.

---

## چرا Error واحد باقی ماند

با وجود multi-error بودن `Result`، خود `Error` همچنان primitive واحد خطاست.

دلیل این تصمیم:

* ساده نگه داشتن error model
* جلوگیری از ورود زودهنگام به validation bag و metadata
* حفظ انعطاف برای ساخت resultهای تک‌خطا و چندخطا
* امکان توسعه بعدی بدون سنگین کردن primitive پایه

---

## قرارداد فنی

### Error

```csharp
public sealed record Error
```

اعضای اصلی:

* `string Code`
* `string Message`

### Result

```csharp
public class Result
```

اعضای اصلی:

* `bool IsSuccess`
* `bool IsFailure`
* `IReadOnlyCollection<Error> Errors`
* `Error? FirstError`
* `Success()`
* `Failure(Error error)`
* `Failure(params Error[] errors)`
* `Failure(IEnumerable<Error> errors)`

### Result<TValue>

```csharp
public sealed class Result<TValue> : Result
```

اعضای اصلی:

* `TValue Value`
* `Success(TValue value)`
* `Failure(Error error)`
* `Failure(params Error[] errors)`
* `Failure(IEnumerable<Error> errors)`

### نکته مهم

متدهای static `Failure` در `Result<TValue>` عمداً متدهای static کلاس والد را hide می‌کنند و با `new` مشخص می‌شوند.

این تصمیم intentional است و برای حفظ API strongly-typed در `Result<TValue>` انجام شده است.

---

## چرا Code و Message در Error لازم‌اند

### Code

`Code` برای مصرف machine-readable و پایدار استفاده می‌شود.

ویژگی‌های مورد انتظار:

* پایدار
* قابل استفاده در mapping
* مستقل از متن نمایشی
* قابل استفاده در logging و integration

مثال:

* `orders.invalid-state`
* `orders.not-found`
* `validation.required-field`

### Message

`Message` برای توضیح انسانی خطا استفاده می‌شود.

نتیجه:

* `Code` برای برنامه
* `Message` برای انسان

---

## مرز با DomainException

`DomainException` و `Result Pattern` دو primitive متفاوت هستند.

### DomainException

* primitive دامنه است
* rule violationهای دامنه را مدل می‌کند
* concern آن domain semantics است

### Result Pattern

* primitive اپلیکیشن است
* outcome عملیات را مدل می‌کند
* concern آن application flow است

### نتیجه

`Result Pattern` جایگزین `DomainException` نیست.

اما در لایه‌های بالاتر، ممکن است `DomainException` به یک یا چند `Error` map شود.
این mapping concern خود `Result` نیست.

---

## چه چیزهایی عمداً داخل Result Pattern نیستند

در نسخه اول، این موارد **جزو scope `Result Pattern` نیستند**:

* HTTP status
* validation dictionary
* metadata bag
* severity
* localization concern
* UI concern
* mediator concern
* retry concern
* exception serialization concern
* transport-specific concernها

این حذف‌ها intentional هستند و به معنی فراموش شدن این concernها نیستند؛
فقط به این معنی‌اند که این موارد در این مرحله نباید وارد primitive پایه application شوند.

---

## Non-goals

این primitive در نسخه اول این هدف‌ها را ندارد:

* تبدیل شدن به یک framework کامل error handling
* مدل کردن مستقیم API response
* جایگزین کردن کامل exceptionها
* ارائه abstraction برای validation frameworkها
* تعریف contract transport-level برای failure

---

## naming و structure

### Solution

Relay.slnx

### Project

ZaminX.BuildingBlocks.Application

### Namespace

ZaminX.BuildingBlocks.Application.Results

### مسیر فایل‌های کد

* `src/00.BuildingBlocks/03.DomainAndApplicationPrimitives/02.Application/src/ZaminX.BuildingBlocks.Application/Results/Error.cs`
* `src/00.BuildingBlocks/03.DomainAndApplicationPrimitives/02.Application/src/ZaminX.BuildingBlocks.Application/Results/Result.cs`
* `src/00.BuildingBlocks/03.DomainAndApplicationPrimitives/02.Application/src/ZaminX.BuildingBlocks.Application/Results/ResultOfT.cs`

### مسیر تست

* `src/00.BuildingBlocks/03.DomainAndApplicationPrimitives/02.Application/tests/ZaminX.BuildingBlocks.Application.Tests/Results/ErrorTests.cs`
* `src/00.BuildingBlocks/03.DomainAndApplicationPrimitives/02.Application/tests/ZaminX.BuildingBlocks.Application.Tests/Results/ResultTests.cs`
* `src/00.BuildingBlocks/03.DomainAndApplicationPrimitives/02.Application/tests/ZaminX.BuildingBlocks.Application.Tests/Results/ResultOfTTests.cs`

---

## استراتژی تست

حداقل تست‌های لازم برای نسخه اول `Result Pattern`:

1. `Error` باید `Code` و `Message` را درست نگه دارد
2. `Error` نباید `Code` یا `Message` خالی یا null بپذیرد
3. `Result.Success()` باید success بدون خطا بسازد
4. `Result.Failure(...)` باید failure با یک یا چند خطا بسازد
5. failure نباید با collection خالی ساخته شود
6. failure نباید خطای null بپذیرد
7. `Result<TValue>.Success(...)` باید value را در حالت success نگه دارد
8. دسترسی به `Value` در حالت failure باید خطا بدهد
9. `FirstError` باید خطای اول را برگرداند
10. hiding متدهای static در `Result<TValue>` باید API strongly-typed را حفظ کند

---

## observationهای باز

موارد زیر در این مرحله هنوز guideline نهایی نیستند و فقط observation محسوب می‌شوند:

* این‌که در آینده `ValidationError` تخصصی نیاز شود یا نه
* این‌که metadata ساخت‌یافته روی `Error` لازم شود یا نه
* این‌که implicit conversionها اضافه شوند یا نه
* این‌که `Result` extension methodهای chaining و mapping بگیرد یا نه
* این‌که relation دقیق آن با mediator pipeline چه باشد

تا وقتی نیاز واقعی اثبات نشده، این موارد وارد primitive پایه نمی‌شوند.

---

## جمع‌بندی

در زمین X، `Result Pattern` یک primitive application-level ساده، صریح و کم‌مسئولیت است که:

* outcome عملیات را مدل می‌کند
* success و failure را شفاف نگه می‌دارد
* از یک error model پایدار استفاده می‌کند
* از multi-error پشتیبانی می‌کند
* concernهای transport و framework را وارد لایه application نمی‌کند

این primitive intentionally minimal نگه داشته می‌شود تا:

* reusable بماند
* برای command، query و handler مناسب باشد
* از over-engineering جلوگیری شود
* پایه‌ای روشن برای primitiveهای بعدی لایه Application فراهم شود