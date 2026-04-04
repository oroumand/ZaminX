# ADR: CrossCutting Capability Design Models

## Status

Accepted

---

## Context

در طراحی capabilityهای خانواده CrossCutting در زمین X، چند capability مرجع پیاده‌سازی شده‌اند:

* Object Mapper (Morpher)
* Serializer (Prism)
* Translator (Parrot)

در بررسی این capabilityها مشخص شد که همه آن‌ها از یک الگوی طراحی یکسان استفاده نمی‌کنند.

برخی capabilityها دارای providerهای جایگزین هستند و consumer فقط با یکی از آن‌ها کار می‌کند.

در مقابل، برخی capabilityها دارای یک core مرکزی هستند که چند provider به آن داده می‌دهند و consumer فقط با core تعامل دارد.

عدم شفافیت این تفاوت می‌تواند منجر به:

* طراحی‌های ناسازگار
* ایجاد abstractionهای غیرضروری
* افزایش پیچیدگی بدون دلیل
* drift در معماری CrossCutting

---

## Decision

در خانواده CrossCutting، دو الگوی رسمی برای طراحی capabilityهای provider-based تعریف می‌شود:

### 1. Replacement-Based Provider Capability

در این الگو:

* یک قرارداد مصرفی وجود دارد
* یک یا چند provider جایگزین‌پذیر وجود دارند
* هر provider همان قرارداد را پیاده‌سازی می‌کند
* consumer فقط با یکی از providerها کار می‌کند
* registration در provider انجام می‌شود

نمونه‌ها:

* Object Mapper
* Serializer (Prism)

---

### 2. Core-Orchestrated Provider Capability

در این الگو:

* یک قرارداد مصرفی وجود دارد
* یک core capability وجود دارد
* providerها به core داده می‌دهند
* consumer فقط با core تعامل دارد
* providerها از طریق builder یا extension به core متصل می‌شوند

نمونه:

* Translator (Parrot)

---

## Consequences

### مزایا

* شفاف شدن مدل طراحی capabilityها
* کاهش ambiguity در تصمیم‌گیری
* جلوگیری از over-engineering
* ایجاد consistency بین capabilityها
* فراهم شدن مسیر واضح برای توسعه capabilityهای جدید

### معایب

* نیاز به درک صحیح تفاوت دو مدل
* افزایش پیچیدگی ذهنی در ابتدای کار

---

## Notes

* انتخاب بین این دو مدل باید بر اساس نیاز واقعی capability انجام شود
* استفاده از core-orchestrated فقط زمانی مجاز است که orchestration واقعی وجود داشته باشد
* استفاده از abstraction بدون نیاز واقعی توصیه نمی‌شود

---

## References

* docs/06.guidelines/crosscutting-design.md
* docs/03.modules/00.BuildingBlocks/01.CrossCutting/object-mappers.md
* docs/03.modules/00.BuildingBlocks/01.CrossCutting/translators.md
