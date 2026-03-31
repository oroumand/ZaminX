# ZaminX.BuildingBlocks.CrossCutting.Serializer.Microsoft

## نمای کلی

این پروژه provider مایکروسافتی capability `Serializer` را بر پایه `System.Text.Json` ارائه می‌دهد.

---

## مسئولیت‌ها

* پیاده‌سازی `IJsonSerializer`
* ارائه registration در `ServiceCollectionExtensions`
* نگهداری options مربوط به provider مایکروسافتی

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
services.AddMicrosoftJsonSerializer();

services.AddMicrosoftJsonSerializer(options =>
{
    options.PropertyNameCaseInsensitive = true;
});
```
