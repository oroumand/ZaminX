<div dir="rtl">
# Mapper

Mapper یکی از Building Block های ZaminX است که یک قرارداد مستقل و قابل توسعه برای نگاشت بین آبجکت‌ها ارائه می‌کند.

در این capability، قرارداد mapping از پیاده‌سازی آن جدا شده است تا:

* امکان استفاده از پیاده‌سازی‌های مختلف وجود داشته باشد
* مصرف‌کننده بتواند پیاده‌سازی سفارشی خودش را بنویسد
* لایه‌های بالاتر فقط به قرارداد وابسته باشند، نه به provider مشخص

## ساختار

این capability در حال حاضر شامل این پروژه‌ها است:

* `ZaminX.Mapper.Abstractions`
* `ZaminX.Mapper.AutoMapper`
* `ZaminX.Mapper.WebApiSample`

## پروژه‌ها

### ZaminX.Mapper.Abstractions

شامل قراردادهای اصلی Mapper است.

در حال حاضر قرارداد اصلی این capability:

* `IMapperAdapter`

### ZaminX.Mapper.AutoMapper

پیاده‌سازی `IMapperAdapter` بر پایه AutoMapper.

این پروژه شامل این بخش‌ها است:

* Adapter
* Registration
* Options

### ZaminX.Mapper.WebApiSample

نمونه استفاده واقعی از Mapper در یک Web API.

این پروژه برای این استفاده می‌شود:

* نمایش نحوه registration
* نمایش نحوه inject شدن `IMapperAdapter`
* نمایش انجام mapping واقعی
* بررسی صحت کارکرد OpenAPI و Scalar

## نحوه استفاده

برای استفاده از پیاده‌سازی AutoMapper:

```csharp
builder.Services.AddZaminXAutoMapperAdapter();
```

برای استفاده با تنظیمات بیشتر:

```csharp
builder.Services.AddZaminXAutoMapperAdapter(options =>
{
    options.Assemblies.Add(typeof(Program).Assembly);
});
```

## تصمیم‌های مهم طراحی

### جداسازی قرارداد از پیاده‌سازی

قراردادها در پروژه `ZaminX.Mapper.Abstractions` قرار گرفته‌اند و پیاده‌سازی‌ها به آن وابسته هستند.

### مالکیت registration

هر پروژه registration خودش را درون خودش نگه می‌دارد.

### الگوی naming برای registration

همه متدهای registration با پیشوند `AddZaminX` شروع می‌شوند تا به راحتی قابل کشف باشند.

## وضعیت فعلی

در این مرحله:

* قرارداد Mapper تعریف شده است
* پیاده‌سازی AutoMapper ایجاد شده است
* registration اولیه آماده است
* sample عملیاتی وجود دارد

## گام‌های بعدی احتمالی

* اضافه کردن پیاده‌سازی Mapster
* بازبینی naming در صورت نیاز
* بررسی سناریوی custom implementation توسط مصرف‌کننده
* آماده‌سازی packageها برای انتشار
</div>