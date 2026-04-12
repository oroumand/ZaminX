# ZaminX.BuildingBlocks.Data.Abstractions

این پروژه قراردادها و مدل‌های مشترک لایه Data را در زمین X نگه می‌دارد.

## مسئولیت‌ها

این پروژه شامل موارد مشترکی است که هم در read side و هم در write side استفاده می‌شوند:

* `IDataAuditContext`
* `DefaultDataAuditContext`
* `PagedQuery`
* `PagedQuery<TSearch>`
* `PagedResult<TData>`

## نکته مهم

این پروژه:

* به EF Core وابسته نیست
* به Domain package وابسته نیست
* concernهای provider-specific ندارد
* فقط قراردادها و مدل‌های عمومی و reusable را نگه می‌دارد

## موارد خارج از scope

این پروژه شامل این موارد نیست:

* Repository implementation
* UnitOfWork implementation
* DbContext registration
* provider configuration
* auditing infrastructure در EF Core

## کاربرد

این پروژه به‌عنوان پایه مشترک برای پروژه‌های زیر استفاده می‌شود:

* `ZaminX.BuildingBlocks.Data.Write.Abstractions`
* `ZaminX.BuildingBlocks.Data.Read.Abstractions`
* `ZaminX.BuildingBlocks.Data.EntityFrameworkCore`
