# Redoc

این پروژه integration مربوط به `ReDoc` را برای capability `Lumen` ارائه می‌دهد.

---

## مسئولیت‌ها

* ثبت `RedocUiOptions`
* فعال‌سازی UI از طریق `UseRedoc`
* map کردن ReDoc در runtime
* استفاده از document endpoint تولیدشده توسط OpenAPI built-in

---

## نمونه استفاده

```csharp
builder.Services.AddZaminXOpenApi(
    builder.Configuration,
    build: lumen =>
    {
        lumen.UseRedoc(options =>
        {
            options.RoutePrefix = "redoc";
        });
    });
```

---

## نکات طراحی

* این پروژه generator جدیدی اضافه نمی‌کند
* concern آن صرفاً ارائه UI برای document موجود است
* options آن از configuration و code-based setup هر دو پشتیبانی می‌کند

---

## نتیجه

این پروژه مناسب سناریوهایی است که UI خواندنی‌تر و documentation-oriented نسبت به Swagger UI مدنظر باشد.
