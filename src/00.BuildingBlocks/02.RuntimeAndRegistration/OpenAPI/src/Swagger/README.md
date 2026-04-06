# Swagger

این پروژه integration مربوط به `Swagger UI` را برای capability `Lumen` ارائه می‌دهد.

---

## مسئولیت‌ها

* ثبت `SwaggerUiOptions`
* فعال‌سازی UI از طریق builder
* map کردن Swagger UI بر پایه document تولیدشده توسط OpenAPI built-in
* جدا نگه‌داشتن concernهای UI از هسته Lumen

---

## نمونه استفاده

```csharp
builder.Services.AddZaminXOpenApi(
    builder.Configuration,
    build: lumen =>
    {
        lumen.UseSwagger(options =>
        {
            options.RoutePrefix = "swagger";
        });
    });
```

---

## نکات طراحی

* این پروژه از generator خود Swashbuckle استفاده نمی‌کند
* document generation همچنان در Lumen core و روی `Microsoft.AspNetCore.OpenApi` باقی می‌ماند
* این پروژه فقط UI را روی document endpoint سوار می‌کند

---

## نتیجه

این پروژه مناسب سناریوهایی است که Swagger UI برای تست محلی، ad-hoc exploration یا onboarding تیمی لازم باشد.
