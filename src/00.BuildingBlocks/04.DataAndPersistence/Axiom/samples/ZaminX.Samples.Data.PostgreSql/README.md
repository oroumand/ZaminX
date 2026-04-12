# ZaminX.Samples.Data.PostgreSql

این پروژه نمونه end-to-end مربوط به Axiom روی PostgreSQL است.

## هدف sample

این sample برای نمایش و اعتبارسنجی این flow ساخته شده است:

* registration کامل Axiom
* write DbContext
* read DbContext
* PostgreSQL provider
* write repository
* read repository
* paging و sorting
* auditing

## چیزهایی که این sample نشان می‌دهد

* استفاده از `AddZaminXDataAccess(...)`
* استفاده از `UseEntityFrameworkCore<TDbContext>(...)`
* استفاده از `UseReadEntityFrameworkCore<TDbContext>(...)`
* استفاده از `WithPostgreSql(...)`
* اسکن خودکار repositoryها
* استفاده هم‌زمان از read side و write side

## نکته مهم

این sample برای validate کردن behavior provider-specific روی PostgreSQL ساخته شده و جایگزین مستندات capability نیست.
