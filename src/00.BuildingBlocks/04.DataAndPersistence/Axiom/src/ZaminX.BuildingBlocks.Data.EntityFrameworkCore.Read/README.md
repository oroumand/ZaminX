# ZaminX.BuildingBlocks.Data.EntityFrameworkCore.Read

این پروژه پیاده‌سازی read side روی EF Core را در Axiom ارائه می‌دهد.

## مسئولیت‌ها

این پروژه شامل این اجزا است:

* `EfReadRepository<TEntity, TId, TDbContext>`
* paging helper روی `IQueryable`
* sorting helper روی `IQueryable`
* read repository scanning
* registration extensionهای مربوط به read side

## مدل طراحی

در این پروژه:

* queryها به‌صورت پیش‌فرض `NoTracking` هستند
* اگر `SortBy` مشخص شده باشد، sorting بر همان اساس اعمال می‌شود
* اگر `SortBy` مشخص نشده باشد، repository می‌تواند default sorting اعمال کند
* fallback پیش‌فرض sorting بر اساس `Id` و سپس `{EntityName}Id` به‌صورت نزولی انجام می‌شود

## نکته مهم

این پروژه read side را ساده نگه می‌دارد و contract پایه را روی entity یا read model صفحه‌بندی‌شده بنا می‌کند.

## موارد خارج از scope

این پروژه شامل این موارد نیست:

* specification pattern
* filtering DSL عمومی
* projection engine
* provider registration
