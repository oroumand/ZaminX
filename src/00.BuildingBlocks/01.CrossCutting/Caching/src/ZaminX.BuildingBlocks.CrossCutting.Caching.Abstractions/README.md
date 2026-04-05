# ZaminX.BuildingBlocks.CrossCutting.Caching.Abstractions

این پکیج انتزاع‌ها و قراردادهای اصلی capability فنی `Caching` در زمین X را نگه می‌دارد.

نام محصولی این capability: `StashX`

---

## هدف این پکیج

هدف این پکیج این است که یک مرز پایدار، کوچک و provider-agnostic برای caching تعریف کند تا مصرف‌کننده‌ها به backendها و providerهای مشخص وابسته نشوند.

این پکیج فقط قرارداد عمومی capability را نگه می‌دارد و شامل هیچ provider مشخصی نیست.

---

## محتویات این پکیج

این پکیج شامل این اجزای اصلی است:

* `IStashX`
* `CacheEntryOptions`
* `StashXException`

---

## قرارداد اصلی

قرارداد اصلی این capability:

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

## تنظیمات entry

در نسخه فعلی، تنظیمات entry عمداً minimal نگه داشته شده‌اند:

* `AbsoluteExpiration`
* `SlidingExpiration`

---

## نکات مهم

* این پکیج فقط contract capability را نگه می‌دارد
* نباید implementation مشخص provider داخل آن قرار بگیرد
* providerهای concrete باید به این پکیج reference بدهند
* consumerها نیز می‌توانند فقط برای استفاده از contract به این پکیج reference بدهند

---

## پکیج‌های مرتبط

providerهای concrete این capability در پکیج‌های جداگانه ارائه می‌شوند:

* `ZaminX.BuildingBlocks.CrossCutting.Caching.InMemory`
* `ZaminX.BuildingBlocks.CrossCutting.Caching.Redis`
* `ZaminX.BuildingBlocks.CrossCutting.Caching.SqlServer`

---

## هویت محصولی

* نام فنی capability: `Caching`
* نام محصولی capability: `StashX`
