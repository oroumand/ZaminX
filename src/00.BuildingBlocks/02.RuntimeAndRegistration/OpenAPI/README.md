# Lumen

Lumen نام محصولی capability فنی `OpenApi` در زمین X است.

این solution برای ساده‌سازی registration و runtime setup مربوط به OpenAPI و UIهای مشاهده و تست آن طراحی شده است.

در نسخه فعلی، Lumen این مسئولیت‌ها را پوشش می‌دهد:

* ثبت OpenAPI با استفاده از `Microsoft.AspNetCore.OpenApi`
* ارائه entry point یکپارچه برای setup در `IServiceCollection`
* اجرای runtime setup از طریق `UseZaminXOpenApi`
* پشتیبانی از UIهای مستقل:
  * Scalar
  * Swagger
  * Redoc

---

## چرا Lumen؟

در ASP.NET Core جدید، خود فریم‌ورک تولید document را انجام می‌دهد، اما UIها به‌صورت جداگانه اضافه می‌شوند.
در پروژه‌های واقعی، wiring این بخش معمولاً در `Program.cs` پخش می‌شود و خیلی زود consistency خود را از دست می‌دهد.

Lumen این مسئله را با یک composition layer سبک در خانواده `RuntimeAndRegistration` حل می‌کند.

---

## ساختار solution

```text
src/
  Lumen/
  Scalar/
  Swagger/
  Redoc/

samples/
  WebApiSample/
```

---

## مدل طراحی

* `OpenApi.Lumen` هسته capability است
* هر UI project مستقل خودش را دارد
* options با pipeline استاندارد `Microsoft.Extensions.Options` ثبت می‌شوند
* هیچ `OptionsWrapper` یا `Options.Create` یا `Replace(IOptions<>)` در طراحی وجود ندارد
* wiring runtime در یک entry point متمرکز باقی می‌ماند

---

## quick start

```csharp
builder.Services.AddZaminXOpenApi(
    builder.Configuration,
    build: lumen =>
    {
        lumen.UseScalar();
        lumen.UseSwagger();
        lumen.UseRedoc();
    });

var app = builder.Build();

app.UseZaminXOpenApi();
```

---

## docs

مستند capability و تصمیم‌های معماری در پوشه `Docs` این بسته قرار داده شده‌اند.
مسیرهای مهم:

* `Docs/README.md`
* `Docs/docs/02.architecture/project-state.md`
* `Docs/docs/03.modules/00.BuildingBlocks/02.RuntimeAndRegistration/lumen.md`

---

## نکته مهم

این solution بر پایه `.NET 10` و `sdk 10.0.200` تنظیم شده است و با `Directory.Build.props` و `Directory.Packages.props` ریشه پروژه هم‌راستا است.
