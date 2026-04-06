# Scalar

این پروژه integration مربوط به UI ابزار `Scalar` را برای Lumen ارائه می‌دهد.

---

## مسئولیت‌ها

* ثبت `ScalarUiOptions`
* bind کردن تنظیمات Scalar از configuration
* فعال‌سازی UI در builder از طریق `UseScalar`
* map کردن Scalar UI در runtime

---

## نمونه استفاده

```csharp
builder.Services.AddZaminXOpenApi(
    builder.Configuration,
    build: lumen =>
    {
        lumen.UseScalar(options =>
        {
            options.Title = "ZaminX API Reference";
        });
    });
```

---

## نکات طراحی

* این پروژه owner تولید OpenAPI document نیست
* این پروژه document endpoint را از Lumen core دریافت می‌کند
* options آن با pipeline استاندارد فریم‌ورک ثبت می‌شوند
* این پروژه مستقل از Swagger و Redoc باقی می‌ماند

---

## نتیجه

اگر فقط Scalar UI بخواهید، کافی است همین پروژه را کنار Lumen reference کنید.
