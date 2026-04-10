# DomainException

## هدف این سند

این سند primitive مربوط به `DomainException` را در خانواده
`00.BuildingBlocks/03.DomainAndApplicationPrimitives/01.Domain`
تعریف می‌کند.

نقش این سند:

* تعریف دقیق `DomainException`
* روشن کردن دلیل نیاز به آن در زمین X
* مشخص کردن مرز آن با error handling در لایه Application
* ثبت تصمیم‌های طراحی نسخه اول
* مشخص کردن scope و non-goalها
* فراهم کردن مرجع ثابت برای توسعه primitiveهای بعدی دامنه و اپلیکیشن

---

## تعریف

`DomainException` یک primitive دامنه برای نمایش خطاها و rule violationهای معنادار در سطح دامنه است.

در زمین X، `DomainException` برای این استفاده می‌شود که:

* خطاهای دامنه را به‌صورت صریح و قابل‌تشخیص مدل کند
* یک قرارداد پایدار و machine-readable برای خطاهای دامنه فراهم کند
* از وابسته شدن منطق دامنه به messageهای متنی جلوگیری کند
* پایه‌ای روشن برای mappingهای بعدی در لایه Application و API ایجاد کند

در نسخه اول، `DomainException` intentionally minimal است و فقط concernهای اصلی مربوط به خطای دامنه را پوشش می‌دهد.

---

## جایگاه در taxonomy

* دسته: `BuildingBlocks`
* خانواده: `03.DomainAndApplicationPrimitives`
* زیرخانواده: `01.Domain`
* primitive: `DomainException`

دلیل قرارگیری در این خانواده این است که `DomainException` یک primitive دامنه‌ای reusable و self-contained است که می‌تواند مستقل از یک اپلیکیشن خاص مصرف شود.

---

## چرا در زمین X به DomainException نیاز داریم

در زمین X، وجود `DomainException` به این دلایل justified است:

1. فراهم کردن یک مدل صریح برای خطاهای دامنه
2. جدا کردن خطاهای دامنه از خطاهای زیرساختی و برنامه‌ای
3. جلوگیری از reliance روی message برای تصمیم‌گیری نرم‌افزار
4. فراهم کردن یک شناسه پایدار برای error mapping در لایه‌های بالاتر

هدف `DomainException` این نیست که مدل نهایی همه خطاهای application را تعریف کند؛
هدف فقط استانداردسازی حداقل قرارداد خطاهای دامنه است.

---

## مدل طراحی نسخه اول

نسخه اول `DomainException` در زمین X با این مدل طراحی می‌شود:

* `DomainException` از `Exception` ارث می‌برد
* دارای property به نام `Code` از نوع `string` است
* `Code` مقدار اجباری و پایدار برای شناسایی خطای دامنه است
* `Message` همچنان برای نمایش و توضیح خطا استفاده می‌شود
* نسخه اول این primitive فاقد metadata، validation details و concernهای application-level است

---

## قرارداد فنی

### شکل پایه

```csharp
public class DomainException : Exception
```

### اعضای اصلی

* `string Code { get; }`
* constructor با `code`
* constructor با `code` و `message`
* constructor با `code` و `message` و `innerException`

---

## چرا Code از نوع string است

در زمین X، `Code` به‌صورت `string` تعریف می‌شود، نه عددی.

### دلیل این تصمیم

* self-descriptive است
* به registry عددی نیاز ندارد
* بدون context خارجی هم معنا دارد
* برای logging، mapping و result handling مناسب‌تر است
* از پیچیدگی اضافه جلوگیری می‌کند

### مثال

```text
order.invalid-state
user.email-already-exists
general.domain-rule-violation
```

---

## چرا Key جداگانه نداریم

در نسخه اول، `DomainException` فقط یک شناسه پایدار به نام `Code` دارد.

وجود property جداگانه‌ای مثل `Key` در این مرحله پذیرفته نشده است.

### دلیل این تصمیم

* از ایجاد دو منبع حقیقت جلوگیری می‌شود
* ambiguity بین semantic identifier و localization/resource key ایجاد نمی‌شود
* concernهای presentation و translation وارد دامنه نمی‌شوند
* طراحی minimal باقی می‌ماند

اگر در آینده نیاز واقعی به localization key یا resource key اثبات شود، آن concern در لایه مناسب‌تری مدیریت خواهد شد.

---

## مرز با Exception معمولی

هر `Exception` لزوماً domain exception نیست.

### Exception معمولی

* ممکن است زیرساختی، برنامه‌ای یا فنی باشد
* الزاماً semantic مشخص دامنه ندارد

### DomainException

* مربوط به rule violation یا خطای معنادار دامنه است
* دارای `Code` پایدار و machine-readable است
* برای مصرف در لایه‌های بالاتر قابل تشخیص است

---

## مرز با ResultPattern

`DomainException` و `ResultPattern` دو primitive متفاوت هستند.

### DomainException

* یک exception دامنه‌ای است
* برای مدل کردن rule violation یا failure دامنه‌ای استفاده می‌شود

### ResultPattern

* یک الگوی application-level برای بازگرداندن نتیجه است
* concern آن handling و flow در سطح application است

### نتیجه

`DomainException` نباید concernهای `ResultPattern` را در خود جذب کند.
همچنین `ResultPattern` هم جایگزین کامل `DomainException` نیست.

در لایه‌های بالاتر، ممکن است `DomainException` به یک result مناسب map شود، اما این mapping concern خود `DomainException` نیست.

---

## چه چیزهایی عمداً داخل DomainException نیستند

در نسخه اول، این موارد **جزو scope `DomainException` نیستند**:

* `Key`
* `int Code`
* `Metadata`
* `ValidationErrors`
* `HttpStatusCode`
* localization concern
* translation concern
* UI concern
* application concern
* result concern

این حذف‌ها intentional هستند و به معنی فراموش شدن این concernها نیستند؛
فقط به این معنی‌اند که این موارد در این مرحله نباید وارد primitive پایه دامنه شوند.

---

## Non-goals

این primitive در نسخه اول این هدف‌ها را ندارد:

* تعریف مدل کامل error handling برای application
* پوشش دادن validation failureها به‌صورت ساخت‌یافته
* mapping مستقیم به API response
* تعریف localization/resource contract
* تعریف error catalog عددی
* حل concernهای transport یا presentation

---

## legacy notes

در پروژه قدیمی زمین، family خطاهای دامنه richer بود و کلاس‌هایی مثل
`DomainStateException`،
`InvalidEntityStateException`
و
`InvalidValueObjectStateException`
وجود داشتند.

در زمین X، نسخه اول با یک primitive پایه و ساده‌تر شروع می‌شود.

### چه چیزهایی از نسخه قدیمی حفظ شده‌اند

* اصل وجود exceptionهای دامنه
* جداسازی خطاهای دامنه از خطاهای فنی
* نقش دامنه‌ای exception در بیان rule violation

### چه چیزهایی بازطراحی شده‌اند

* شروع از یک base primitive ساده‌تر
* استفاده از `Code` متنی پایدار
* حذف hierarchy زودهنگام و بیش‌ازحد
* موکول کردن specializationهای بیشتر به زمانی که نیاز واقعی اثبات شود

---

## naming و structure

### Solution

Kernel.slnx

### Project

ZaminX.BuildingBlocks.Domain

### Namespace

ZaminX.BuildingBlocks.Domain.Exceptions

### مسیر فایل کد

src/00.BuildingBlocks/03.DomainAndApplicationPrimitives/01.Domain/src/ZaminX.BuildingBlocks.Domain/Exceptions/DomainException.cs

### مسیر تست

src/00.BuildingBlocks/03.DomainAndApplicationPrimitives/01.Domain/tests/ZaminX.BuildingBlocks.Domain.Tests/Exceptions/DomainExceptionTests.cs

---

## استراتژی تست

حداقل تست‌های لازم برای نسخه اول `DomainException`:

1. constructor با `code` باید `Code` و `Message` را درست مقداردهی کند
2. constructor با `code` و `message` باید propertyها را درست تنظیم کند
3. constructor با `innerException` باید مقدار inner exception را نگه دارد
4. `code` نباید `null` باشد
5. `code` نباید empty یا whitespace باشد

---

## observationهای باز

موارد زیر در این مرحله هنوز guideline نهایی نیستند و فقط observation محسوب می‌شوند:

* این‌که در آینده family تخصصی‌تری از domain exceptionها نیاز شود یا نه
* این‌که metadata ساخت‌یافته برای بعضی domain errorها لازم شود یا نه
* این‌که relation دقیق `DomainException` با `ResultPattern` چه شکل نهایی بگیرد
* این‌که localization key در لایه‌های بالاتر چگونه مدیریت شود

تا وقتی نیاز واقعی اثبات نشده، این موارد وارد primitive پایه نمی‌شوند.

---

## جمع‌بندی

در زمین X، `DomainException` یک primitive ساده، صریح و کم‌مسئولیت است که:

* خطاهای دامنه را مدل می‌کند
* دارای `Code` پایدار و machine-readable است
* از message برای توضیح انسانی استفاده می‌کند
* concernهای application و presentation را وارد دامنه نمی‌کند

این primitive intentionally minimal نگه داشته می‌شود تا:

* reusable بماند
* از over-engineering جلوگیری شود
* پایه‌ای روشن برای `ResultPattern` و error mapping در لایه‌های بالاتر فراهم شود
