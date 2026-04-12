# ZaminX.BuildingBlocks.Data.Write.Abstractions

این پروژه قراردادهای write side را در Axiom نگه می‌دارد.

## مسئولیت‌ها

قراردادهای اصلی این پروژه:

* `IUnitOfWork`
* `IWriteRepository<TAggregate, TId>`

## هدف

هدف این پروژه این است که application و domain serviceها بتوانند بدون وابستگی به EF Core یا provider خاص، با قراردادهای write کار کنند.

## نکته مهم

این پروژه:

* implementation ندارد
* به EF Core وابسته نیست
* transaction orchestration پیشرفته را وارد نمی‌کند
* فقط write contractهای پایه را تعریف می‌کند

## موارد خارج از scope

این پروژه شامل این موارد نیست:

* repository scanning
* unit of work implementation
* graph loading implementation
* save interceptor
* provider registration
