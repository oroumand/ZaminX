# ZaminX.BuildingBlocks.CrossCutting.Translator.Parrot

## نمای کلی

این پروژه پیاده‌سازی اصلی capability مربوط به Parrot را نگه می‌دارد.

نقش این پروژه:

- پیاده‌سازی ITranslator
- نگهداری داده‌ها در حافظه
- ترکیب داده‌های providerها
- اعمال fallback
- مدیریت refresh
- ارائه registration اصلی

---

## اجزای اصلی

### ParrotTranslator

پیاده‌سازی API مصرفی capability است.

---

### ParrotTranslationStore

داده‌های ترجمه را در حافظه نگه می‌دارد و lookup را انجام می‌دهد.

---

### ParrotTranslationProviderCoordinator

مسئول بارگذاری داده‌ها از providerها و merge آن‌هاست.

---

### ParrotRefreshService

برای refresh داده‌ها استفاده می‌شود.

---

### ParrotStartupHostedService

در زمان startup، بارگذاری اولیه داده‌ها را انجام می‌دهد.

---

## رفتارها

### کش

تمام داده‌ها پس از load در حافظه نگه‌داری می‌شوند.

---

### merge providerها

providerها به ترتیب registration اجرا می‌شوند.

providerهای بعدی می‌توانند داده‌های قبلی را override کنند.

---

### fallback

ترتیب lookup:

1. culture دقیق
2. parent culture
3. invariant
4. خود key

---

### formatting

دو نوع formatting وجود دارد:

- با آرگومان ترجمه‌شونده
- با آرگومان raw

---

## registration

نقطه ورود اصلی:

services.AddParrot(parrot =>
{
    parrot.UseSqlServer(...);
});

---

## نکات طراحی

- این پروژه به provider خاصی وابسته نیست
- registration provider در پروژه provider انجام می‌شود
- logging در این لایه حداقلی است