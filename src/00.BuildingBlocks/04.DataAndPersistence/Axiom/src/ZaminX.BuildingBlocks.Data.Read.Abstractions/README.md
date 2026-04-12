# ZaminX.BuildingBlocks.Data.Read.Abstractions

این پروژه قراردادهای read side را در Axiom نگه می‌دارد.

## مسئولیت‌ها

قرارداد اصلی این پروژه:

* `IReadRepository<TEntity, TId>`

این قرارداد:

* دریافت یک آیتم تکی را پوشش می‌دهد
* دریافت لیست صفحه‌بندی‌شده را پوشش می‌دهد
* از `PagedQuery` و `PagedResult<TData>` استفاده می‌کند

## هدف

هدف این پروژه ارائه یک contract ساده، قابل فهم و CQRS-friendly برای query side است، بدون اینکه implementation یا concernهای EF Core وارد آن شوند.

## نکته مهم

پیش‌فرض این قرارداد بر پایه entity یا read model است. اگر مصرف‌کننده نیاز به query اختصاصی یا DTO projection داشته باشد، باید آن را در repository اختصاصی خود اضافه کند.

## موارد خارج از scope

این پروژه شامل این موارد نیست:

* paging implementation
* sorting implementation
* dynamic query building
* DbContext registration
* provider-specific behavior
