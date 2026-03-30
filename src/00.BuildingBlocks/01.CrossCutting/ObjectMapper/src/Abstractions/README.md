# Object Mapper Abstractions

این پروژه قرارداد اصلی capability مربوط به Object Mapper را نگه می‌دارد.

## مسئولیت این پروژه

مسئولیت این پروژه:
- تعریف مرز مصرف capability
- جلوگیری از وابستگی مستقیم مصرف‌کننده به providerهای بیرونی
- فراهم کردن قرارداد پایدار برای پیاده‌سازی‌های مختلف

## محتوای فعلی

در وضعیت فعلی، این پروژه شامل قرارداد اصلی Object Mapper است.

## نکته استفاده

این پروژه به‌تنهایی provider ارائه نمی‌کند.  
برای استفاده واقعی از capability باید یکی از providerها نیز در کنار آن استفاده شود.

## provider فعلی

provider فعلی:
- `ZaminX.BuildingBlocks.CrossCutting.ObjectMapper.AutoMapper`

## مستندات بیشتر

برای توضیح معماری capability و چرایی وجود آن، به این سند مراجعه کنید:

- `docs/03.modules/00.BuildingBlocks/01.CrossCutting/object-mappers.md`