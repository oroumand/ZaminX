# DependencyInjection.Abstractions

این پروژه شامل contractهای marker برای تعیین lifetime سرویس‌ها است.

---

## markerها

* ITransientDependency
* IScopedDependency
* ISingletonDependency

---

## هدف

این markerها:

* intent ثبت سرویس را مشخص می‌کنند
* dependency به implementation را حذف می‌کنند
* و decoupling ایجاد می‌کنند

---

## مثال

```csharp
public class UserService : IUserService, IScopedDependency
{
}
```

---

## نکته

این پروژه intentionally lightweight است و هیچ logic اجرایی ندارد.
