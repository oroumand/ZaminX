# ZaminX.BuildingBlocks.CrossCutting.Translator.Sample.WebApi

## نمای کلی

این پروژه یک نمونه Web API برای نمایش استفاده از Parrot است.

---

## اجرا

ابتدا connection string را در appsettings.json تنظیم کن.

سپس:

dotnet run --project samples/ZaminX.BuildingBlocks.CrossCutting.Translator.Sample.WebApi

---

## endpointها

### lookup

GET /api/translations/{key}?culture=en-US

---

### formatted

GET /api/translations/{key}/formatted

---

### translated args

GET /api/translations/{key}/translated-args

---

### concat

GET /api/translations/concat

---

### missing key

GET /api/translations/missing/{key}

---

## هدف

این پروژه فقط برای نمایش نحوه استفاده است و برای production طراحی نشده است.