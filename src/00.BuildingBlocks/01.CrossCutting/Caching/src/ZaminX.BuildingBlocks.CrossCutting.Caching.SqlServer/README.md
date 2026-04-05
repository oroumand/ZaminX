# ZaminX.BuildingBlocks.CrossCutting.Caching.SqlServer

این پکیج provider مبتنی بر SQL Server برای capability فنی `Caching` در زمین X را ارائه می‌کند.

نام محصولی این capability: `StashX`

---

## هدف این پکیج

این پکیج یک implementation مبتنی بر SQL Server برای `IStashX` فراهم می‌کند.

این provider برای سناریوهایی مناسب است که:

* SQL Server در زیرساخت موجود است
* تیم ترجیح می‌دهد cache در همان بستر SQL نگه داشته شود
* استفاده از Redis در دسترس نیست یا انتخاب نشده است
* یک backend متمرکز برای cache موردنیاز است

---

## registration

برای فعال‌سازی این provider از registration زیر استفاده می‌شود:

```csharp
builder.Services.AddZaminXCachingWithSqlServer(options =>
{
    options.ConnectionString = "...";
    options.SchemaName = "dbo";
    options.TableName = "StashXCache";
    options.EnsureStorageOnStartup = true;
});
```

---

## تنظیمات

### ConnectionString

رشته اتصال SQL Server

### SchemaName

نام schema مربوط به جدول cache

### TableName

نام جدول cache

### EnsureStorageOnStartup

اگر این گزینه فعال باشد، storage موردنیاز در startup ایجاد می‌شود.

---

## رفتار setup

در این provider، setup storage پشتیبانی می‌شود اما:

* explicit است
* opt-in است
* به‌صورت پیش‌فرض side effect پنهان ندارد

یعنی اگر `EnsureStorageOnStartup` فعال نشود، provider فرض می‌کند storage از قبل وجود دارد.

---

## نمونه استفاده

```csharp
builder.Services.AddZaminXCachingWithSqlServer(options =>
{
    options.ConnectionString = "...";
    options.SchemaName = "dbo";
    options.TableName = "StashXCache";
    options.EnsureStorageOnStartup = true;
});
```

---

## رفتار این provider

این provider:

* از `IDistributedCache` استفاده می‌کند
* داده‌ها را به‌صورت JSON serialize می‌کند
* برای سناریوهای distributed caching مبتنی بر SQL Server قابل استفاده است
* setup storage را فقط در صورت فعال بودن گزینه مربوطه انجام می‌دهد

---

## dependencyهای اصلی

این provider از این اجزای اصلی استفاده می‌کند:

* `IDistributedCache`
* SQL Server
* `Microsoft.Extensions.Caching.SqlServer`
* serialization مبتنی بر `System.Text.Json`

---

## پکیج‌های مرتبط

برای استفاده از contract اصلی capability، این provider به پکیج زیر وابسته است:

* `ZaminX.BuildingBlocks.CrossCutting.Caching.Abstractions`

---

## هویت محصولی

* نام فنی capability: `Caching`
* نام محصولی capability: `StashX`
