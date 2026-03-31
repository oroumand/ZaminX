# Prism

## نمای کلی

Prism نام محصولی capability فنی `Serializer` در زمین X است.

این capability در خانواده `01.CrossCutting` قرار می‌گیرد و یک مرز یکدست برای serialization و deserialization داده فراهم می‌کند تا:

* مصرف‌کننده به ابزار بیرونی وابسته نشود
* provider در یک نقطه محصور بماند
* registration در خود provider نگه داشته شود
* تفاوت providerها در سطح مصرف‌کننده نشت نکند

در فاز فعلی، Prism روی JSON متمرکز است.

---

## ساختار solution

این solution شامل این پروژه‌هاست:

* `ZaminX.BuildingBlocks.CrossCutting.Serializer.Abstractions`
* `ZaminX.BuildingBlocks.CrossCutting.Serializer.Microsoft`
* `ZaminX.BuildingBlocks.CrossCutting.Serializer.Newtonsoft`
* `ZaminX.BuildingBlocks.CrossCutting.Serializer.WebApiSample`

---

## نقش هر پروژه

### Abstractions

قرارداد مصرفی capability و exception مشترک را نگه می‌دارد.

### Microsoft

provider مبتنی بر `System.Text.Json` را ارائه می‌دهد.

### Newtonsoft

provider مبتنی بر `Newtonsoft.Json` را ارائه می‌دهد.

### WebApiSample

نمونه‌ای کوچک برای نمایش registration و مصرف capability ارائه می‌دهد.

---

## قواعد طراحی

* قرارداد مصرفی باید provider-agnostic بماند
* options فقط در registration مجازند
* exceptionهای بیرونی باید به exception capability-level تبدیل شوند
* provider اصلی logging روتین per-call انجام نمی‌دهد
* payload خام نباید log شود

---

## ارتباط با docs

مرجع اصلی تصمیم‌های معماری و محصولی این capability در docs نگه داشته می‌شود، به‌ویژه:

* `docs/03.modules/00.BuildingBlocks/01.CrossCutting/serializer.md`
* `docs/04.decision-records/adr/adr-009-provider-options-diagnostics-and-exception-boundary.md`
