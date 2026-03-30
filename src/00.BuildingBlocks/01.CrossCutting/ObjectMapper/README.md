# Object Mapper

Object Mapper یکی از capabilityهای عمومی و بین‌برشی در زمین X است.

هدف این capability این است که تصمیم استفاده از ابزار mapping در سطح پروژه پخش نشود و مصرف‌کننده‌ها به‌جای وابستگی مستقیم به ابزار بیرونی، به یک قرارداد پایدار وابسته شوند.

## پروژه‌های این capability

این capability در وضعیت فعلی شامل این پروژه‌هاست:

- `ZaminX.BuildingBlocks.CrossCutting.ObjectMapper.Abstractions`
- `ZaminX.BuildingBlocks.CrossCutting.ObjectMapper.AutoMapper`
- `ZaminX.BuildingBlocks.CrossCutting.ObjectMapper.WebApiSample`

## ساختار

### Abstractions
قرارداد اصلی capability را تعریف می‌کند.

### AutoMapper
provider فعلی capability را بر پایه AutoMapper ارائه می‌کند.

### WebApiSample
نمونه اجرایی برای نمایش registration و استفاده از capability است.

## نقش این README

این README برای معرفی سریع capability، ساختار پروژه‌ها و ورود اولیه به آن نگه داشته می‌شود.

برای چرایی وجود capability، جایگاه آن در taxonomy، تصمیم‌های معماری و مرزهای آن، به سند اصلی در docs مراجعه کنید:

- `docs/03.modules/00.BuildingBlocks/01.CrossCutting/object-mappers.md`

## وضعیت فعلی

در وضعیت فعلی:
- قرارداد اصلی capability تعریف شده است
- provider مبتنی بر AutoMapper وجود دارد
- sample اجرایی وجود دارد
- naming فنی در حال هم‌راستاسازی با taxonomy رسمی زمین X است