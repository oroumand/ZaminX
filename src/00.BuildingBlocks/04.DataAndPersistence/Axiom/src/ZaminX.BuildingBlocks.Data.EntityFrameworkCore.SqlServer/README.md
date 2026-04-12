# ZaminX.BuildingBlocks.Data.EntityFrameworkCore.SqlServer

این پروژه extension مربوط به SQL Server را برای Axiom ارائه می‌دهد.

## مسئولیت‌ها

وظیفه اصلی این پروژه:

* ارائه `WithSqlServer(...)` روی `EntityFrameworkCoreBuilder<TDbContext>`

## هدف

این پروژه فقط concern مربوط به provider SQL Server را به جریان registration اضافه می‌کند و جزئیات provider را از لایه‌های بالاتر پنهان نگه می‌دارد.

## نکته مهم

این پروژه:

* روی builder مشترک EF Core سوار می‌شود
* concernهای read و write را دوباره پیاده‌سازی نمی‌کند
* implementation عمومی EF Core را تکرار نمی‌کند

## موارد خارج از scope

این پروژه شامل این موارد نیست:

* repository implementation
* paging
* auditing
* sample logic
