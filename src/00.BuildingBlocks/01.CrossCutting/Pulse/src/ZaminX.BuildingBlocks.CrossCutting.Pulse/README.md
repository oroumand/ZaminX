# Pulse

Pulse یک CrossCutting capability در ZaminX است که مجموعه‌ای از **utilityهای سبک، سریع و پرکاربرد** را برای استفاده در کل پروژه فراهم می‌کند.

این capability با تمرکز بر **سادگی، حداقل abstraction و بیشترین کاربرد عملی** طراحی شده است.

---

## 🎯 هدف

Pulse برای حل این نیازها طراحی شده است:

* کار با تاریخ و زمان (خصوصاً تقویم شمسی)
* جلوگیری از تکرار validationهای ساده
* ارائه extension methodهای کاربردی
* حذف نیاز به helperهای پراکنده در پروژه‌ها

---

## 📦 Scope

Pulse فقط شامل این موارد است:

### 1. DateTime (Persian-first)

ابزارهای کامل برای کار با تاریخ شمسی:

* تبدیل و فرمت تاریخ شمسی
* کار با `PersianCalendar`
* متدهای کمکی برای نمایش friendly
* پشتیبانی از فرهنگ فارسی

کلاس‌های اصلی:

* `FriendlyPersianDateUtils`
* `PersianDateTimeUtils`
* `PersianCulture`
* `PersianNumbersUtils`
* `TimeConstants`
* `UnicodeConstants`

---

### 2. Guards

پیاده‌سازی سبک برای guard clauseها:

```csharp
Guard.Against.Null(input);
Guard.Against.NullOrEmpty(input);
```

---

### 3. Extensions

Extension methodهای منتخب:

* `StringExtensions`
* `StringValidatorExtensions`
* `EnumExtensions`

---

## ❌ خارج از scope

موارد زیر عمداً در Pulse وجود ندارند:

* Service Locator (`ZaminServices`)
* Resource / Translator system
* Dependency Injection helpers
* abstractionهای غیرضروری

---

## 🧠 فلسفه طراحی

Pulse بر اساس این اصول طراحی شده است:

### سادگی

APIها باید ساده، قابل فهم و بدون رفتار پنهان باشند.

### عدم over-engineering

هیچ abstraction یا pattern اضافه‌ای بدون نیاز واقعی اضافه نشده است.

### Persian-first

پشتیبانی کامل از تاریخ و فرهنگ فارسی یک هدف اصلی است.

### قابل استفاده فوری

کدها باید بدون setup پیچیده قابل استفاده باشند.

---

## 🚀 نحوه استفاده

### تاریخ شمسی

```csharp
var now = DateTime.Now;
var friendly = FriendlyPersianDateUtils.ToFriendlyDate(now);
```

---

### Guard

```csharp
Guard.Against.Null(user, nameof(user));
```

---

### Extension

```csharp
var isValid = email.IsValidEmail();
```

---

## 📁 ساختار پروژه

```
Pulse/
 └── src/
     └── ZaminX.BuildingBlocks.CrossCutting.Pulse/
         ├── DateTime/
         ├── Globalization/
         ├── Guards/
         └── Extensions/
```

---

## 🧩 جایگاه در معماری

Pulse بخشی از:

```
BuildingBlocks → CrossCutting
```

است و در تمام لایه‌های application قابل استفاده می‌باشد.

---

## ⚠️ نکات

* این capability intentionally ساده نگه داشته شده است
* برای use-caseهای پیچیده‌تر، capability جدا طراحی خواهد شد
* هیچ dependency خارجی ندارد

---

## 📄 لایسنس

MIT
