# ZaminX.BuildingBlocks.CrossCutting.Translator.Abstractions

## نمای کلی

این پروژه قراردادهای capability مربوط به Parrot را نگه می‌دارد.

هدف این پروژه این است که:

- API مصرفی capability را تعریف کند
- قراردادهای لازم برای providerها را مشخص کند
- مرز بین core و providerها را شفاف نگه دارد

این پروژه فاقد پیاده‌سازی اجرایی است.

---

## ساختار

این پروژه شامل این بخش‌هاست:

- Builders
- Contracts
- Models

---

## قراردادهای اصلی

### ITranslator

API مصرفی capability است و مصرف‌کننده فقط با این قرارداد کار می‌کند.

مسئولیت‌ها:

- دریافت ترجمه بر اساس key
- دریافت ترجمه بر اساس culture
- پشتیبانی از formatting
- پشتیبانی از concat
- پشتیبانی از fallback

---

### ITranslationDataProvider

قرارداد providerهای داده است.

هر provider باید بتواند داده‌های ترجمه را بارگذاری کند.

---

### ITranslationRefreshService

برای trigger کردن refresh داده‌ها استفاده می‌شود.

این قرارداد برای decouple کردن providerها از core تعریف شده است.

---

### ITranslationMissingKeyRegistrar

برای ثبت اختیاری کلیدهای جاافتاده استفاده می‌شود.

این قرارداد در API مصرفی دیده نمی‌شود و فقط برای wiring داخلی است.

---

### IParrotBuilder

builder مربوط به registration capability است.

providerها از طریق این builder به registration اصلی متصل می‌شوند.

---

## مدل‌ها

### TranslationEntry

نماینده یک رکورد ترجمه است و شامل:

- key
- culture
- value

---

## نکات طراحی

- این پروژه فقط قراردادها را نگه می‌دارد
- نباید شامل منطق اجرایی باشد
- نباید به provider خاصی وابسته شود
- باید پایدار و کم‌تغییر باقی بماند