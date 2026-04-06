# Axon

Axon نام محصولی capability فنی `DependencyInjection` در زمین X است.

این ماژول برای ساده‌سازی registration سرویس‌ها در DI container و حذف wiring دستی طراحی شده است.

---

## چرا Axon؟

در پروژه‌های واقعی:

* registration سرویس‌ها پراکنده می‌شود
* naming و convention یکسان نیست
* duplicateها رخ می‌دهد
* و نگهداری آن سخت می‌شود

Axon این مشکل را با یک مدل ساخت‌یافته حل می‌کند.

---

## قابلیت‌ها

* marker-based registration
* assembly scanning
* convention-based registration (opt-in)
* duplicate handling policy
* filtering
* startup validation
* open generic support

---

## quick start

```csharp
services.AddZaminXDependencyInjection(options =>
{
    options.AddAssemblyContaining<Program>();
});
```

---

## marker example

```csharp
public class OrderService : IOrderService, IScopedDependency
{
}
```

---

## docs

مستند کامل:

```
docs/03.modules/00.BuildingBlocks/02.RuntimeAndRegistration/dependency-injection.md
```

---

## جایگاه

* BuildingBlock
* RuntimeAndRegistration

---

## نکته

Axon جایگزین DI container نیست.
بلکه فقط registration را استاندارد و ساده می‌کند.
