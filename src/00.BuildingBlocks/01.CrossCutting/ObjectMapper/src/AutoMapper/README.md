# Object Mapper AutoMapper Provider

این پروژه provider فعلی Object Mapper را بر پایه AutoMapper ارائه می‌کند.

## مسئولیت این پروژه

مسئولیت این پروژه:
- پیاده‌سازی قرارداد اصلی Object Mapper
- ارائه setup و registration مربوط به provider
- فراهم کردن configuration لازم برای scan و پیکربندی mapping

## registration

این provider registration خودش را درون خودش نگه می‌دارد و از طریق extension method ثبت می‌شود.

## نکته

مصرف‌کننده‌ها باید به قرارداد capability وابسته شوند، نه به API مستقیم ابزار بیرونی.

این پروژه provider فعلی است، نه تعریف‌کننده مرز capability.

## مستندات بیشتر

برای توضیح جایگاه capability و چرایی وجود آن، به این سند مراجعه کنید:

- `docs/03.modules/00.BuildingBlocks/01.CrossCutting/object-mappers.md`