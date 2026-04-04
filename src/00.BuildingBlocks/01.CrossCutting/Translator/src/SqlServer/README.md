# ZaminX.BuildingBlocks.CrossCutting.Translator.SqlServer

## نمای کلی

این پروژه provider مربوط به SQL Server برای Parrot را نگه می‌دارد.

---

## registration

builder.Services.AddParrot(parrot =>
{
    parrot.UseSqlServer(options =>
    {
        options.ConnectionString = "...";
        options.Schema = "dbo";
        options.TableName = "ParrotTranslations";
    });
});

---

## options

مهم‌ترین گزینه‌ها:

- ConnectionString
- Schema
- TableName
- CommandTimeoutSeconds
- EnsureTableCreated
- ReloadMode
- ReloadInterval
- RegisterMissingKeys
- MissingKeyDefaultValueTemplate
- SeedTranslations

---

## رفتارها

### load

داده‌ها از دیتابیس خوانده می‌شوند و به TranslationEntry تبدیل می‌شوند.

---

### create table

در صورت فعال بودن، جدول به‌صورت خودکار ساخته می‌شود.

---

### seed

در صورت وجود، داده اولیه درج می‌شود.

---

### reload

در حالت Periodic، داده‌ها به‌صورت دوره‌ای refresh می‌شوند.

---

### missing key

در صورت فعال بودن، keyهای پیدا نشده ثبت می‌شوند.

---

## نکات طراحی

- این پروژه فقط provider است
- API مصرفی ندارد
- فقط از Abstractions استفاده می‌کند