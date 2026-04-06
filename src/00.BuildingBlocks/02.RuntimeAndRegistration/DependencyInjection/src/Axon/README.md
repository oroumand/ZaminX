# Axon

این پروژه implementation capability Axon را ارائه می‌دهد.

---

## مسئولیت‌ها

* scanning اسمبلی‌ها
* تحلیل typeها
* registration سرویس‌ها
* اعمال policyها
* validation

---

## entry point

```csharp
services.AddZaminXDependencyInjection(...)
```

---

## dependencyها

* Microsoft.Extensions.DependencyInjection
* Abstractions project

---

## نکات طراحی

* facade ساده روی IServiceCollection
* بدون abstraction اضافه
* marker-first approach
* policy-driven behavior

---

## نتیجه

این پروژه هسته اجرایی Axon است.
