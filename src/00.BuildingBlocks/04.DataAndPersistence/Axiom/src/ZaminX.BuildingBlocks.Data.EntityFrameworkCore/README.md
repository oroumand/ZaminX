# ZaminX.BuildingBlocks.Data.EntityFrameworkCore

این پروژه هسته مشترک پیاده‌سازی EF Core در Axiom است.

## مسئولیت‌ها

این پروژه مسئول این بخش‌ها است:

* registration entry point
* `AddZaminXDataAccess(...)`
* builderهای پایه DataAccess و EF Core
* provider validation
* ثبت پیش‌فرض `IDataAuditContext`
* ثبت و اعمال interceptorهای مشترک
* auditing infrastructure مشترک
* helperهای model configuration مشترک

## هدف

هدف این پروژه این است که concernهای مشترک EF Core فقط یک‌بار پیاده‌سازی شوند و پروژه‌های read، write و providerها روی آن سوار شوند.

## نکته مهم

این پروژه:

* provider خاصی را مستقیماً انتخاب نمی‌کند
* repositoryهای read و write را مستقیماً پیاده‌سازی نمی‌کند
* concernهای business مانند change log را وارد نمی‌کند

## موارد خارج از scope

این پروژه شامل این موارد نیست:

* `WithSqlServer(...)`
* `WithPostgreSql(...)`
* write repository base
* read repository base
* sample app logic
