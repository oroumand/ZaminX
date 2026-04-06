# Modules

## معرفی

این بخش شامل مستندات مربوط به تمام capabilityهای زمین X است.

در این لایه، ساختار ماژول‌ها، دسته‌بندی آن‌ها، و نحوه قرارگیری capabilityها در taxonomy پروژه مشخص می‌شود.

هدف این بخش این است که:

* دیدی یکپارچه از ساختار ماژول‌ها ارائه دهد
* مسیر درست استفاده از capabilityها را مشخص کند
* رابطه بین BuildingBlockها، الگوها و integrationها را روشن کند
* و به‌عنوان نقطه ورود برای درک معماری ماژولار زمین X عمل کند

---

## جایگاه Modules در معماری

در ساختار کلی پروژه:

```
docs/
  01.vision/
  02.architecture/
  03.modules/
```

لایه Modules مسئول پاسخ به این سؤال است:

> «چه چیزهایی در سیستم داریم و چگونه دسته‌بندی شده‌اند؟»

در حالی که:

* `vision` به «چرا» پاسخ می‌دهد
* `architecture` به «چگونه» پاسخ می‌دهد
* `modules` به «چه چیزهایی» پاسخ می‌دهد

---

## ساختار Modules

```
03.modules/
  00.BuildingBlocks/
  01.ApplicationPatterns/
  02.Integrations/
```

---

## 1. BuildingBlocks

BuildingBlockها هسته اصلی زمین X هستند.

ویژگی‌ها:

* مستقل
* self-contained
* قابل reuse
* بدون وابستگی به application خاص

این اجزا:

* رفتار پایه سیستم را فراهم می‌کنند
* foundation سایر لایه‌ها هستند
* می‌توانند به‌صورت جداگانه publish شوند

---

## 2. ApplicationPatterns

ApplicationPatternها:

* از چند BuildingBlock تشکیل می‌شوند
* یک الگوی رایج در سطح اپلیکیشن را استاندارد می‌کنند
* developer experience را بهبود می‌دهند

مثال:

* CRUD pattern
* CQRS pattern
* validation flow

---

## 3. Integrations

Integrationها مسئول اتصال سیستم به بیرون هستند:

* سرویس‌های third-party
* APIهای خارجی
* message brokerها
* storageها

---

## رابطه بین لایه‌ها

```
BuildingBlocks → ApplicationPatterns → Applications
                     ↓
                 Integrations
```

* BuildingBlocks پایه هستند
* ApplicationPatterns روی آن‌ها ساخته می‌شوند
* Integrations به بیرون متصل می‌کنند

---

## اصول دسته‌بندی capabilityها

برای اینکه یک capability در این لایه‌ها قرار بگیرد، باید بر اساس **مسئله اصلی خود** دسته‌بندی شود، نه تکنولوژی یا implementation.

معیار اصلی:

> «این capability چه مسئله‌ای را حل می‌کند؟»

---

## مسیر مطالعه پیشنهادی

### برای درک معماری

1. BuildingBlocks
2. CrossCutting
3. RuntimeAndRegistration
4. Axon
5. Lumen

---

### برای شروع توسعه

1. انتخاب BuildingBlock مناسب
2. استفاده از ApplicationPattern (در صورت نیاز)
3. اتصال Integrationها

---

## وضعیت فعلی

در حال حاضر:

* BuildingBlockهای اصلی در حال تثبیت هستند
* CrossCutting تا حد زیادی پیاده‌سازی شده
* RuntimeAndRegistration در حال تکمیل است
* Lumen به‌عنوان capability جدید اضافه شده است

---

## نکته مهم

Modules فقط یک فهرست نیست.

این لایه:

* مدل ذهنی پروژه را تعریف می‌کند
* نحوه رشد سیستم را مشخص می‌کند
* و پایه consistency در طراحی است

بنابراین تغییر در این لایه باید با دقت و مبتنی بر تصمیم‌های معماری انجام شود.
