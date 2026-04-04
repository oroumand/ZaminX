# Object Mapper

Object Mapper یکی از capabilityهای خانواده CrossCutting در زمین X است.

نام فنی این capability Object Mapper و نام محصولی آن Morpher است.

Morpher برای هم‌راستایی با الگوی naming محصولی در capabilityهای مرجع CrossCutting انتخاب شده است و در عین حال به‌گونه‌ای نام‌گذاری شده که با مفهوم Reflection در اکوسیستم .NET اشتباه گرفته نشود.

این capability یک مرز انتزاعی برای عملیات mapping بین objectها فراهم می‌کند و وابستگی به ابزارهای بیرونی mapping را از مصرف‌کننده جدا می‌کند.

---

## هدف

هدف Object Mapper:

* ساده‌سازی mapping بین objectها
* جلوگیری از نشت API کتابخانه‌های mapping به لایه‌های بالاتر
* فراهم کردن امکان جایگزینی providerهای مختلف
* ایجاد یک API یکنواخت و پایدار برای مصرف‌کننده

---

## پروژه‌ها

این capability در وضعیت فعلی شامل پروژه‌های زیر است:

* ZaminX.BuildingBlocks.CrossCutting.ObjectMapper.Abstractions
  قراردادهای اصلی capability

* ZaminX.BuildingBlocks.CrossCutting.ObjectMapper.AutoMapper
  provider مبتنی بر AutoMapper

* ZaminX.BuildingBlocks.CrossCutting.ObjectMapper.WebApiSample
  نمونه مصرف capability در یک Web API

---

## نحوه استفاده (خلاصه)

ابتدا provider مورد نظر را register کنید:

services.AddAutoMapperAdapter(...);

سپس در کد مصرف‌کننده:

public class MyService
{
private readonly IMapperAdapter _mapper;

```
public MyService(IMapperAdapter mapper)
{
    _mapper = mapper;
}

public Target Map(Source source)
{
    return _mapper.Map<Target>(source);
}
```

}

جزئیات کامل usage و سناریوهای مختلف در docs و sample ارائه شده است.

---

## مستندات اصلی

مرجع اصلی طراحی و مستندات این capability در docs پروژه نگهداری می‌شود:

* docs/03.modules/00.BuildingBlocks/01.CrossCutting/object-mappers.md
* docs/06.guidelines/crosscutting-design.md

این README فقط برای معرفی سریع capability و ساختار آن است و جایگزین docs اصلی نیست.

---

## وضعیت فعلی

این capability به‌عنوان یکی از capabilityهای مرجع خانواده CrossCutting در زمین X در نظر گرفته می‌شود و مبنایی برای طراحی capabilityهای مشابه در این خانواده است.
