# ZaminX.BuildingBlocks.CrossCutting.Caching.Redis

این پکیج provider مبتنی بر Redis برای capability فنی `Caching` در زمین X را ارائه می‌کند.

نام محصولی این capability: `StashX`

---

## هدف این پکیج

این پکیج یک implementation مبتنی بر Redis برای `IStashX` فراهم می‌کند.

این provider برای سناریوهایی مناسب است که:

* به caching توزیع‌شده نیاز دارند
* چند instance از application وجود دارد
* state باید بیرون از process نگه داشته شود
* latency پایین و reuse داده بین instanceها اهمیت دارد

---

## registration

برای فعال‌سازی این provider از registration زیر استفاده می‌شود:

```csharp
builder.Services.AddZaminXCachingWithRedis(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "StashX:";
});
```

---

## تنظیمات

### Configuration

آدرس و تنظیمات اتصال Redis

### InstanceName

prefix اختیاری برای کلیدهای cache

---

## نمونه استفاده

```csharp
builder.Services.AddZaminXCachingWithRedis(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "StashX:";
});
```

---

## رفتار این provider

این provider:

* از `IDistributedCache` استفاده می‌کند
* داده‌ها را به‌صورت JSON serialize می‌کند
* برای سناریوهای distributed caching مناسب است
* switching از providerهای دیگر به آن از طریق registration انجام می‌شود

---

## dependencyهای اصلی

این provider از این اجزای اصلی استفاده می‌کند:

* `IDistributedCache`
* Redis
* serialization مبتنی بر `System.Text.Json`

---

## پکیج‌های مرتبط

برای استفاده از contract اصلی capability، این provider به پکیج زیر وابسته است:

* `ZaminX.BuildingBlocks.CrossCutting.Caching.Abstractions`

---

## هویت محصولی

* نام فنی capability: `Caching`
* نام محصولی capability: `StashX`
