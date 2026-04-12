# ZaminX.BuildingBlocks.Data.EntityFrameworkCore.Write

این پروژه پیاده‌سازی write side روی EF Core را در Axiom ارائه می‌دهد.

## مسئولیت‌ها

این پروژه شامل این اجزا است:

* `EfUnitOfWork<TDbContext>`
* `EfWriteRepository<TEntity, TId, TDbContext>`
* write repository scanning
* registration extensionهای مربوط به write side

## مدل طراحی

در این پروژه:

* `IUnitOfWork` توسط `DbContext` پیاده‌سازی می‌شود
* repository پایه برای aggregate access استفاده می‌شود
* graph loading از طریق `CreateAggregateQuery()` کنترل می‌شود
* application نیازی به آگاهی از Include و ThenInclude ندارد

## نکته مهم

این پروژه scanning را بدون وابستگی به library خارجی انجام می‌دهد و فقط repositoryهایی را ثبت می‌کند که:

* از `EfWriteRepository<,,>` ارث ببرند
* و interfaceای را پیاده‌سازی کنند که از `IWriteRepository<,>` مشتق شده باشد

## موارد خارج از scope

این پروژه شامل این موارد نیست:

* provider registration
* auditing registration
* transaction abstraction پیشرفته
* domain event dispatch
