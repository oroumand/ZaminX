# Parrot

## نمای کلی

Parrot نام محصولی capability فنی Translator در زمین X است.

این capability برای ترجمه متن‌ها بر اساس key طراحی شده و از مدل «انتزاع + provider» استفاده می‌کند تا:

- API مصرفی از source داده مستقل بماند
- providerهای مختلف پشت یک مرز واحد قرار بگیرند
- داده‌ها در حافظه نگه‌داری شوند
- refresh بدون restart امکان‌پذیر باشد

در نسخه فعلی، provider اولیه Parrot برای SQL Server پیاده‌سازی شده است.

---

## ساختار این capability

این capability از چند پروژه تشکیل شده است:

- ZaminX.BuildingBlocks.CrossCutting.Translator.Abstractions
- ZaminX.BuildingBlocks.CrossCutting.Translator.Parrot
- ZaminX.BuildingBlocks.CrossCutting.Translator.SqlServer
- ZaminX.BuildingBlocks.CrossCutting.Translator.Sample.WebApi

---

## نقش هر پروژه

### Abstractions

قراردادهای capability را نگه می‌دارد، از جمله:

- ITranslator
- ITranslationDataProvider
- ITranslationRefreshService
- ITranslationMissingKeyRegistrar
- IParrotBuilder

---

### Parrot

پیاده‌سازی اصلی capability را نگه می‌دارد، از جمله:

- translator اصلی
- store درون‌حافظه‌ای
- refresh service
- coordination بین providerها
- registration اصلی AddParrot

---

### SqlServer

provider مربوط به SQL Server را نگه می‌دارد، از جمله:

- options
- data provider
- reload hosted service
- missing key registration
- registration مربوط به UseSqlServer

---

### Sample.WebApi

یک نمونه اجرایی برای نمایش registration و مصرف واقعی capability است.

---

## نحوه ثبت در DI

نمونه ثبت Parrot با provider مربوط به SQL Server:

builder.Services.AddParrot(parrot =>
{
    parrot.UseSqlServer(options =>
    {
        options.ConnectionString = "...";
        options.Schema = "dbo";
        options.TableName = "ParrotTranslations";
        options.EnsureTableCreated = true;
    });
});

---

## نکات مهم

- Parrot بدون provider معتبر نیست
- حداقل یک provider باید داخل AddParrot ثبت شود
- providerهای بعدی می‌توانند داده‌های providerهای قبلی را override کنند
- داده‌ها در حافظه نگه‌داری می‌شوند
- refresh بدون restart پشتیبانی می‌شود

---

## مسیرهای formatting

Parrot دو مسیر متفاوت برای formatting دارد:

### آرگومان‌های ترجمه‌شونده

در این مسیر، آرگومان‌ها key ترجمه در نظر گرفته می‌شوند:

translator.GetString("Message.Greeting", "User.DisplayName")

---

### آرگومان‌های خام

در این مسیر، آرگومان‌ها بدون ترجمه در متن قرار می‌گیرند:

translator.GetFormattedString("Message.Price", 1250.75, "IRR", DateTime.UtcNow)

---

## رفتار fallback

در هنگام lookup، Parrot این مسیر را دنبال می‌کند:

1. culture کامل
2. parent culture
3. culture عمومی
4. خود key

---

## مستندات

برای جزئیات معماری و طراحی:

- docs/03.modules/00.BuildingBlocks/01.CrossCutting/Translator.md
- docs/04.decision-records/adr

---

## وضعیت فعلی

در نسخه فعلی:

- طراحی capability کامل شده است
- پیاده‌سازی اولیه core کامل شده است
- provider مربوط به SQL Server پیاده‌سازی شده است
- sample اولیه Web API آماده است

providerهای بعدی در نسخه‌های بعدی اضافه می‌شوند.