# ZaminX.BuildingBlocks.Data.EntityFrameworkCore.PostgreSql

این پروژه extension مربوط به PostgreSQL را برای Axiom ارائه می‌دهد.

## مسئولیت‌ها

وظیفه اصلی این پروژه:

* ارائه `WithPostgreSql(...)` روی `EntityFrameworkCoreBuilder<TDbContext>`

## هدف

این پروژه concern مربوط به provider PostgreSQL را به جریان registration اضافه می‌کند و consumer را از API سطح پایین provider بی‌نیاز می‌کند.

## نکته مهم

این پروژه:

* فقط provider registration را انجام می‌دهد
* روی builder مشترک EF Core سوار می‌شود
* concernهای عمومی Axiom را دوباره پیاده‌سازی نمی‌کند

## موارد خارج از scope

این پروژه شامل این موارد نیست:

* repository implementation
* paging
* auditing
* sample logic
