# StashX

`StashX` نام محصولی capability فنی `Caching` در زمین X است.

این capability در خانواده `00.BuildingBlocks/01.CrossCutting` قرار می‌گیرد و یک مرز abstraction ساده و reusable برای caching فراهم می‌کند.

---

## هدف

StashX برای این طراحی شده است که:

* استفاده از caching در سطح پروژه پخش نشود
* مصرف‌کننده‌ها به provider یا framework خاص وابسته نشوند
* جابه‌جایی بین providerها ساده باشد
* registration و setup هر provider در مالک خودش باقی بماند
* یک API پایدار، کوچک و قابل فهم برای cache در اختیار consumer قرار گیرد

---

## نام‌گذاری

در این capability:

* نام فنی: `Caching`
* نام محصولی: `StashX`

در taxonomy، namespace، نام پروژه‌ها و ساختار فنی از نام `Caching` استفاده می‌شود.
در contract مصرفی و معرفی محصولی از نام `StashX` استفاده می‌شود.

قرارداد اصلی این capability:

```csharp
IStashX
```

---

## مدل طراحی

StashX با مدل زیر طراحی شده است:

* abstraction-based
* provider-based
* replacement-based

یعنی:

* یک قرارداد مصرفی واحد وجود دارد
* providerهای مختلف همان قرارداد را پیاده‌سازی می‌کنند
* consumer فقط یک provider فعال می‌خواهد
* این capability core-orchestrated نیست

---

## ساختار solution

```text
StashX.slnx
```

---

## پروژه‌ها

### 1. `ZaminX.BuildingBlocks.CrossCutting.Caching.Abstractions`

شامل:

* `IStashX`
* `CacheEntryOptions`
* exception پایه capability

---

### 2. `ZaminX.BuildingBlocks.CrossCutting.Caching.InMemory`

provider مبتنی بر `IMemoryCache`

registration:

```csharp
services.AddZaminXCachingWithInMemory();
```

---

### 3. `ZaminX.BuildingBlocks.CrossCutting.Caching.Redis`

provider مبتنی بر Redis و `IDistributedCache`

registration:

```csharp
services.AddZaminXCachingWithRedis(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "StashX:";
});
```

---

### 4. `ZaminX.BuildingBlocks.CrossCutting.Caching.SqlServer`

provider مبتنی بر SQL Server و `IDistributedCache`

registration:

```csharp
services.AddZaminXCachingWithSqlServer(options =>
{
    options.ConnectionString = "...";
    options.SchemaName = "dbo";
    options.TableName = "StashXCache";
    options.EnsureStorageOnStartup = true;
});
```

---

### 5. `ZaminX.BuildingBlocks.CrossCutting.Caching.Sample`

یک sample واحد برای نمایش نحوه استفاده و switching بین providerها.

در این sample:

* هر سه provider حضور دارند
* یکی active است
* دو تای دیگر comment شده‌اند

---

## قرارداد مصرفی

API فعلی capability:

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

---

## CacheEntryOptions

تنظیمات entry در نسخه فعلی عمداً minimal نگه داشته شده‌اند:

* `AbsoluteExpiration`
* `SlidingExpiration`

---

## قواعد registration

الگوی registration در این capability:

```csharp
AddZaminXCachingWithInMemory()
AddZaminXCachingWithRedis(...)
AddZaminXCachingWithSqlServer(...)
```

این الگو برای حفظ discoverability و یکدستی capabilityهای زمین X انتخاب شده است.

---

## رفتار SQL Server

در provider مربوط به SQL Server، setup assistance پشتیبانی می‌شود اما:

* باید explicit باشد
* باید opt-in باشد
* نباید به‌صورت پیش‌فرض side effect پنهان داشته باشد

گزینه زیر این رفتار را کنترل می‌کند:

```csharp
EnsureStorageOnStartup
```

اگر این گزینه فعال باشد، storage موردنیاز در startup ایجاد می‌شود.
اگر غیرفعال باشد، provider فرض می‌کند storage از قبل وجود دارد.

---

## نمونه استفاده

```csharp
builder.Services.AddZaminXCachingWithInMemory();

app.MapGet("/cache/{key}", async (string key, IStashX stashX, CancellationToken cancellationToken) =>
{
    var value = await stashX.GetAsync<string>(key, cancellationToken);

    return value is null
        ? Results.NotFound()
        : Results.Ok(value);
});
```

---

## نکات طراحی

در نسخه فعلی StashX این تصمیم‌ها اعمال شده‌اند:

* abstraction وجود دارد چون چند provider واقعی وجود دارد
* contract مصرفی کوچک و provider-agnostic نگه داشته شده
* provider-specific setup وارد API مصرفی نشده
* registration در مالک provider باقی مانده
* از builder pattern استفاده نشده
* internal contract اضافه تعریف نشده
* docs مرجع اصلی capability هستند و README فقط entry point سریع است

---

## موارد خارج از scope فعلی

در نسخه فعلی این capability، این موارد عمداً در scope نیستند:

* hybrid cache
* multi-provider orchestration
* distributed invalidation policy
* tagging
* stampede protection
* serializer abstraction مستقل
* diagnostics package جدا
* advanced key abstraction

---

## ارجاع به docs اصلی

برای مستند اصلی capability به این فایل مراجعه کنید:

```text
docs/03.modules/00.BuildingBlocks/01.CrossCutting/caching.md
```
