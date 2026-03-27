<div dir="rtl">

# ZaminX.Mapper.Abstractions

این پکیج شامل قراردادهای اصلی Mapper در ZaminX است.

## هدف

این پکیج یک abstraction مستقل برای mapping بین آبجکت‌ها ارائه می‌دهد تا:

* وابستگی به پیاده‌سازی خاص حذف شود
* امکان استفاده از providerهای مختلف فراهم شود
* مصرف‌کننده بتواند پیاده‌سازی سفارشی خود را ایجاد کند

## قرارداد اصلی

```csharp
TDestination Map<TSource, TDestination>(TSource source);
```

## نحوه استفاده

این پکیج به تنهایی استفاده نمی‌شود و باید به همراه یک implementation نصب شود.

مثال:

* ZaminX.Mapper.AutoMapper

</div>
