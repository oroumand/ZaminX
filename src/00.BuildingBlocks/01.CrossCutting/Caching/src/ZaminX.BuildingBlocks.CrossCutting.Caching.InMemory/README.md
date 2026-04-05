# ZaminX.BuildingBlocks.CrossCutting.Caching.InMemory

این پکیج provider مبتنی بر حافظه برای capability فنی `Caching` در زمین X را ارائه می‌کند.

نام محصولی این capability: `StashX`

---

## هدف این پکیج

این پکیج یک implementation مبتنی بر `IMemoryCache` برای `IStashX` فراهم می‌کند.

این provider برای این سناریوها مناسب است:

* توسعه محلی
* سناریوهای سبک
* caching درون‌فرایندی
* use-caseهایی که به backend توزیع‌شده نیاز ندارند

---

## registration

برای فعال‌سازی این provider از registration زیر استفاده می‌شود:

```csharp
builder.Services.AddZaminXCachingWithInMemory();
```

---

## نمونه استفاده

```csharp
app.MapGet("/cache/{key}", async (string key, IStashX stashX, CancellationToken cancellationToken) =>
{
    var value = await stashX.GetAsync<string>(key, cancellationToken);

    return value is null
        ? Results.NotFound()
        : Results.Ok(value);
});
```

---

## رفتار این provider

این provider:

* داده‌ها را در حافظه همان process نگه می‌دارد
* برای scale-out بین چند instance مناسب نیست
* به backend بیرونی نیاز ندارد
* ساده‌ترین provider برای شروع و تست capability است

---

## dependency اصلی

این provider از `IMemoryCache` استفاده می‌کند.

---

## پکیج‌های مرتبط

برای استفاده از contract اصلی capability، این provider به پکیج زیر وابسته است:

* `ZaminX.BuildingBlocks.CrossCutting.Caching.Abstractions`

---

## هویت محصولی

* نام فنی capability: `Caching`
* نام محصولی capability: `StashX`
