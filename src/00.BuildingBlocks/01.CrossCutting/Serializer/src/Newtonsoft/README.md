# ZaminX.BuildingBlocks.CrossCutting.Serializer.Newtonsoft

## نمای کلی

این پروژه provider نیوتن‌سافتی capability `Serializer` را بر پایه `Newtonsoft.Json` ارائه می‌دهد.

---

## مسئولیت‌ها

* پیاده‌سازی `IJsonSerializer`
* ارائه registration در `ServiceCollectionExtensions`
* نگهداری options مربوط به provider نیوتن‌سافتی

---

## نکات طراحی

* registration در خود provider نگه داشته می‌شود
* options فقط در registration استفاده می‌شوند
* exceptionهای بیرونی به exception مشترک capability تبدیل می‌شوند
* provider لاگ روتین per-call تولید نمی‌کند

---

## استفاده

نمونه registration:

```csharp
services.AddNewtonsoftJsonSerializer();

services.AddNewtonsoftJsonSerializer(options =>
{
    options.UseCamelCase = true;
});
```
