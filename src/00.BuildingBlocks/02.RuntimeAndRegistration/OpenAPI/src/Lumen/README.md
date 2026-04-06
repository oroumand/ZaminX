# OpenApi.Lumen

این پروژه هسته اجرایی capability `OpenApi` با نام محصولی `Lumen` را ارائه می‌دهد.

---

## مسئولیت‌ها

* registration اولیه OpenAPI در سرویس‌ها
* bind کردن `LumenOptions` از configuration
* فراهم کردن builder برای انتخاب UIها
* اجرای runtime setup در `UseZaminXOpenApi`
* orchestration ماژول‌های UI ثبت‌شده

---

## entry pointها

### registration

```csharp
services.AddZaminXOpenApi(...)
```

### runtime

```csharp
app.UseZaminXOpenApi()
```

---

## اصول طراحی

* سادگی در setup
* استفاده از pipeline استاندارد Options
* عدم ساخت abstraction مصرفی غیرضروری
* جدا نگه‌داشتن concernهای UI از هسته OpenAPI
* نگه‌داشتن runtime composition در owner capability

---

## نکته

این پروژه خودش UI ارائه نمی‌دهد؛
بلکه UIهایی مثل Scalar، Swagger و Redoc را orchestrate می‌کند.
