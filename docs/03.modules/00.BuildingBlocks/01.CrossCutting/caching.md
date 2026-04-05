# Caching

## هدف این سند

این سند capability فنی `Caching` را در زمین X تعریف می‌کند.

نقش این سند:

* تعریف مسئله‌ای که Caching حل می‌کند
* توضیح چرایی وجود این capability در زمین X
* ثبت جایگاه آن در taxonomy پروژه
* ثبت نام محصولی این capability
* ثبت مدل طراحی، ساختار solution و پروژه‌ها
* مشخص کردن مرز contract، providerها، registration و setup
* ثبت تصمیم‌های محلی این capability بر اساس guideline رسمی CrossCutting

این سند جایگزین guideline عمومی CrossCutting نیست و فقط تصمیم‌های local این capability را ثبت می‌کند.

---

## تعریف capability

Caching یک BuildingBlock از خانواده قابلیت‌های عمومی و بین‌برشی است که یک مرز abstraction ساده و reusable برای نگهداری موقت داده‌ها فراهم می‌کند.

هدف این capability فقط ذخیره و بازیابی داده نیست.
هدف اصلی آن این است که تصمیم استفاده از backend یا provider خاص cache در سطح پروژه پخش نشود و consumerها به‌جای وابستگی مستقیم به frameworkها و providerهای بیرونی، به یک API داخلی پایدار وابسته شوند.

در سطح محصول:

* نام فنی این capability: `Caching`
* نام محصولی این capability: `StashX`

در ساختار taxonomy، نام پروژه‌ها، namespaceها و مستندات معماری از نام فنی `Caching` استفاده می‌شود.
در contract مصرفی و معرفی محصولی از نام `StashX` استفاده می‌شود.

---

## مسئله‌ای که حل می‌کند

در بسیاری از پروژه‌ها نیاز به caching در چند نقطه مختلف سیستم وجود دارد.

نمونه‌های رایج:

* نگهداری موقت داده‌های پرمصرف
* کاهش تعداد فراخوانی به منابع کندتر
* ذخیره نتیجه محاسبات تکراری
* کاهش latency در سناریوهای پرتکرار
* بهبود performance در سطح application و service

اگر این نیاز بدون مرز capability حل شود، معمولاً این مشکلات ایجاد می‌شود:

* پخش شدن استفاده از `IMemoryCache` یا `IDistributedCache` در سطح پروژه
* ناهماهنگی در نحوه تعریف expiration
* وابستگی مستقیم consumerها به providerهای خاص
* سخت شدن جابه‌جایی بین backendها
* نشت concernهای setup و provider به لایه‌های مصرف‌کننده

Caching در زمین X برای جلوگیری از این پراکندگی و ایجاد یک API پایدار داخلی تعریف شده است.

---

## جایگاه در taxonomy

این capability در مسیر زیر قرار می‌گیرد:

```text
00.BuildingBlocks/01.CrossCutting
```

دلیل این جایگذاری:

* مسئله‌ای عمومی و بین‌برشی را حل می‌کند
* به دامنه خاص وابسته نیست
* به لایه خاص وابسته نیست
* در چند پروژه و سناریو قابل بازاستفاده است
* standalone consumable است
* مسئله اصلی آن integration boundary نیست
* مسئله اصلی آن flow سطح application نیست

بنابراین Caching در زمین X یک capability از خانواده CrossCutting است، نه یک Integration و نه یک ApplicationPattern.

---

## مدل طراحی

StashX با الگوی زیر طراحی شده است:

**replacement-based provider capability**

در این مدل:

* یک قرارداد مصرفی واحد وجود دارد
* چند provider جایگزین‌پذیر وجود دارند
* هر provider همان قرارداد را پیاده‌سازی می‌کند
* registration در خود provider انجام می‌شود
* capability core-orchestrated مستقل ندارد

این capability از این نظر به الگوی Object Mapper و Serializer نزدیک است، نه به Translator.

---

## چرا abstraction در این capability لازم است

در زمین X abstraction پیش‌فرض نیست.
اما در Caching، وجود abstraction توجیه واقعی دارد چون:

* بیش از یک provider واقعی برای آن وجود دارد
* providerها API و behavior متفاوت دارند
* نشت API providerها به consumer می‌تواند drift ایجاد کند
* جابه‌جایی backend cache باید پشت یک مرز داخلی جذب شود
* contract مصرفی واحد باعث یکدستی استفاده در سطح پروژه می‌شود

بنابراین abstraction در این capability یک تصمیم معماری توجیه‌شده است، نه یک wrapper شکلی.

---

## قرارداد مصرفی

قرارداد اصلی این capability:

```csharp
IStashX
```

API فعلی:

```csharp
public interface IStashX
{
    Task SetAsync<T>(
        string key,
        T value,
        CacheEntryOptions? options = null,
        CancellationToken cancellationToken = default);

    Task<T?> GetAsync<T>(
        string key,
        CancellationToken cancellationToken = default);

    Task<T> GetOrSetAsync<T>(
        string key,
        Func<CancellationToken, Task<T>> factory,
        CacheEntryOptions? options = null,
        CancellationToken cancellationToken = default);

    Task RemoveAsync(
        string key,
        CancellationToken cancellationToken = default);
}
```

### ویژگی‌های این contract

* کوچک است
* async-first است
* provider-agnostic است
* concernهای registration و setup را وارد API نمی‌کند
* typeهای بیرونی providerها را نشت نمی‌دهد
* از key abstraction اضافی استفاده نمی‌کند
* فعلاً internal contract جدا ندارد

---

## CacheEntryOptions

در نسخه فعلی، تنظیمات entry عمداً minimal نگه داشته شده‌اند:

```csharp
public sealed class CacheEntryOptions
{
    public TimeSpan? AbsoluteExpiration { get; set; }

    public TimeSpan? SlidingExpiration { get; set; }
}
```

این مدل فعلاً برای use-caseهای اصلی کافی در نظر گرفته شده و از اضافه کردن optionهای پیچیده‌تر خودداری شده است.

---

## policy مربوط به nullability

در contract فعلی:

```csharp
GetAsync<T>
```

مقدار `T?` برمی‌گرداند.

در این مدل:

* cache miss با `null` نمایش داده می‌شود
* wrapper نتیجه جداگانه برای hit/miss تعریف نشده
* API برای استفاده روزمره ساده نگه داشته شده است

این یک تصمیم local برای نسخه فعلی capability است و هنوز guideline عمومی سطح پروژه محسوب نمی‌شود.

---

## policy مربوط به exception

در نسخه فعلی capability:

* exception پایه capability وجود دارد: `StashXException`
* providerها API بیرونی را مستقیماً در surface مصرفی نشت نمی‌دهند
* misconfiguration باید در setup providerها روشن و قابل تشخیص باشد

با این حال، hierarchy کامل exceptionها در این نسخه عمداً minimal نگه داشته شده و فعلاً توسعه داده نشده است.

بنابراین:

* وجود exception پایه، guideline محلی capability است
* policy دقیق exception mapping در این نسخه هنوز minimal است و می‌تواند در نسخه‌های بعدی تکمیل شود

---

## providerها

در نسخه فعلی، سه provider برای StashX پیاده‌سازی شده‌اند:

### 1. InMemory

project:

```text
ZaminX.BuildingBlocks.CrossCutting.Caching.InMemory
```

مسئولیت:

* پیاده‌سازی `IStashX` روی `IMemoryCache`
* نگهداری داده‌ها در حافظه فرایند
* ارائه registration ساده برای use-caseهای local و lightweight

---

### 2. Redis

project:

```text
ZaminX.BuildingBlocks.CrossCutting.Caching.Redis
```

مسئولیت:

* پیاده‌سازی `IStashX` روی Redis با استفاده از `IDistributedCache`
* نگهداری داده‌ها در backend توزیع‌شده
* ارائه options مربوط به connection و instance name

---

### 3. SqlServer

project:

```text
ZaminX.BuildingBlocks.CrossCutting.Caching.SqlServer
```

مسئولیت:

* پیاده‌سازی `IStashX` روی SQL Server با استفاده از `IDistributedCache`
* نگهداری داده‌ها در storage مبتنی بر جدول
* ارائه setup assistance اختیاری برای storage

---

## registration model

الگوی registration در این capability:

```csharp
AddZaminXCachingWithInMemory()
AddZaminXCachingWithRedis(...)
AddZaminXCachingWithSqlServer(...)
```

ویژگی‌های این الگو:

* discoverable است
* با الگوی کلی capabilityهای زمین X هم‌راستاست
* registration در مالک provider باقی می‌ماند
* consumer wiring پراکنده انجام نمی‌دهد

فایل‌های registration در namespace استاندارد زیر قرار می‌گیرند:

```text
Microsoft.Extensions.DependencyInjection
```

---

## SQL setup model

در provider مربوط به SQL Server، setup assistance مجاز است، اما باید:

* explicit باشد
* opt-in باشد
* default behavior بدون side effect پنهان باقی بماند

برای همین، گزینه زیر تعریف شده است:

```csharp
EnsureStorageOnStartup
```

### رفتار این گزینه

اگر این گزینه `true` باشد:

* hosted service مربوط به initialization فعال می‌شود
* storage موردنیاز در startup ایجاد می‌شود

اگر این گزینه `false` باشد:

* هیچ initializationی انجام نمی‌شود
* provider فرض می‌کند storage از قبل وجود دارد

### دلیل این تصمیم

این مدل برای ایجاد تعادل بین این دو هدف انتخاب شده است:

* تجربه مصرف مناسب و onboarding ساده
* پرهیز از side effect پنهان و implicit

---

## ساختار solution و پروژه‌ها

solution:

```text
StashX.slnx
```

پروژه‌ها:

```text
src/
  ZaminX.BuildingBlocks.CrossCutting.Caching.Abstractions
  ZaminX.BuildingBlocks.CrossCutting.Caching.InMemory
  ZaminX.BuildingBlocks.CrossCutting.Caching.Redis
  ZaminX.BuildingBlocks.CrossCutting.Caching.SqlServer

samples/
  ZaminX.BuildingBlocks.CrossCutting.Caching.Sample
```

### مسئولیت پروژه‌ها

#### Abstractions

* قرارداد مصرفی
* مدل‌های مشترک
* exception پایه capability

#### InMemory

* provider مبتنی بر `IMemoryCache`

#### Redis

* provider مبتنی بر Redis و `IDistributedCache`

#### SqlServer

* provider مبتنی بر SQL Server و `IDistributedCache`
* setup assistance اختیاری storage

#### Sample

* نمایش نحوه registration و مصرف capability
* نمایش switching ساده بین providerها

---

## sample

در نسخه فعلی فقط یک sample برای capability وجود دارد.

این sample:

* هر سه provider را نشان می‌دهد
* یکی از providerها را active نگه می‌دارد
* دو provider دیگر را به‌صورت comment نگه می‌دارد
* switching بین providerها را برای onboarding ساده می‌کند

این تصمیم برای جلوگیری از پراکندگی sampleها و تکرار غیرضروری گرفته شده است.

---

## موارد خارج از scope فعلی

در نسخه فعلی StashX این موارد عمداً در scope نیستند:

* hybrid caching
* multi-provider orchestration
* fallback بین providerها
* distributed invalidation policy
* tagging
* stampede protection
* serializer abstraction مستقل
* advanced key abstraction
* diagnostics package جدا
* builder pattern
* internal provider contract

---

## نسبت با guideline عمومی CrossCutting

این capability با guideline رسمی CrossCutting در این بخش‌ها align است:

* در خانواده `01.CrossCutting` قرار گرفته
* abstraction پیش‌فرض فرض نشده و فقط با دلیل واقعی پذیرفته شده
* provider-based بودن بر اساس نیاز واقعی چند provider انتخاب شده
* مدل طراحی آن replacement-based است
* registration در مالک provider باقی مانده
* contract مصرفی کوچک و provider-agnostic نگه داشته شده
* setup concernها وارد API مصرفی نشده‌اند
* docs مرجع اصلی capability هستند

---

## وضعیت فعلی

وضعیت این capability در این مرحله:

* طراحی capability تثبیت شده است
* naming فنی و محصولی تثبیت شده است
* solution structure تثبیت شده است
* سه provider اولیه پیاده‌سازی شده‌اند
* sample واحد پیاده‌سازی و تست شده است
* providerهای InMemory، Redis و SqlServer در sample تست شده‌اند

در نتیجه، StashX اکنون به‌عنوان یکی از capabilityهای پیاده‌سازی‌شده خانواده CrossCutting در زمین X در نظر گرفته می‌شود.
