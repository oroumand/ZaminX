# ZaminX.Samples.Data.SqlServer

این پروژه نمونه end-to-end مربوط به Axiom روی SQL Server است.

## هدف sample

این sample برای نمایش و اعتبارسنجی این flow ساخته شده است:

* registration کامل Axiom
* write DbContext
* read DbContext
* SQL Server provider
* write repository
* read repository
* paging و sorting
* auditing

## چیزهایی که این sample نشان می‌دهد

* استفاده از `AddZaminXDataAccess(...)`
* استفاده از `UseEntityFrameworkCore<TDbContext>(...)`
* استفاده از `UseReadEntityFrameworkCore<TDbContext>(...)`
* استفاده از `WithSqlServer(...)`
* اسکن خودکار repositoryها
* استفاده هم‌زمان از read side و write side

## نکته مهم

این sample برای نمایش behaviorهای پایه Axiom است و هدف آن ارائه architecture کامل application نیست.
