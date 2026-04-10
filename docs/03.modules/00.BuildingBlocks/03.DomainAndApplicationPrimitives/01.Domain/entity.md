# Entity

## هدف این سند

این سند primitive مربوط به `Entity` را در خانواده
`00.BuildingBlocks/03.DomainAndApplicationPrimitives/01.Domain`
تعریف می‌کند.

نقش این سند:

* تعریف دقیق `Entity`
* روشن کردن دلیل نیاز به آن در زمین X
* مشخص کردن مرز آن با `AggregateRoot`
* ثبت تصمیم‌های طراحی نسخه اول
* مشخص کردن scope و non-goalها
* فراهم کردن مرجع ثابت برای توسعه primitiveهای بعدی خانواده Domain

---

## تعریف

`Entity` یک primitive پایه دامنه برای نمایش شیئی است که **هویت (Identity)** دارد.

در زمین X:

* هویت از طریق `Id` تعریف می‌شود
* برابری بر اساس هویت است، نه مقدار
* این primitive عمداً ساده و کم‌مسئولیت نگه داشته شده است

---

## جایگاه در taxonomy

* دسته: `BuildingBlocks`
* خانواده: `03.DomainAndApplicationPrimitives`
* زیرخانواده: `01.Domain`
* primitive: `Entity`

---

## چرا به Entity نیاز داریم

* جلوگیری از تکرار تعریف identity در پروژه‌ها
* یکدست کردن equality semantics
* فراهم کردن پایه برای `AggregateRoot`
* جلوگیری از drift در مدل دامنه

---

## مدل طراحی

نسخه اول `Entity`:

* `abstract class`
* generic (`TId`)
* فقط شامل:

  * `Id`
  * equality
  * تشخیص transient بودن

---

## قرارداد فنی

```csharp
public abstract class Entity<TId>
```

### اعضای اصلی

* `TId Id { get; protected set; }`
* constructor های protected
* `IsTransient()`
* `Equals`
* `GetHashCode`
* `==` و `!=`

---

## identity semantics

* identity فقط از طریق `Id`
* اگر `Id` مقدار پیش‌فرض باشد → transient
* entityهای transient برابر نیستند
* نوع (type) در برابری لحاظ می‌شود

---

## equality semantics

برابری:

* same type
* same Id
* non-transient

در غیر این صورت → نابرابر

---

## چرا generic Id

* عدم وابستگی به `long` یا `Guid`
* انعطاف دامنه
* reuse بهتر

### تصمیم مهم

❌ نسخه non-generic نداریم
❌ default id policy نداریم

---

## مرز با AggregateRoot

### Entity

* identity
* equality

### AggregateRoot

* مرز aggregate
* consistency
* domain events

---

## خارج از scope

این‌ها داخل `Entity` نیستند:

* BusinessId
* DomainEvent
* Auditing
* Validation
* Repository
* Persistence concerns
* Mediator / Command / Query

---

## Non-goals

* shared kernel سنگین
* framework دامنه
* repository abstraction
* event pipeline

---

## legacy notes

در پروژه قدیمی:

* BusinessId وجود داشت ❌ حذف شد
* Entity<long> وجود داشت ❌ حذف شد

### حفظ شده‌ها

* identity-based equality
* generic id concept
* separation با AggregateRoot

---

## naming و structure

### Solution

Kernel.slnx

### Project

ZaminX.BuildingBlocks.Domain

### Namespace

ZaminX.BuildingBlocks.Domain.Entities

---

## تست‌ها

حداقل تست‌ها:

* same type + same id → equal
* different type + same id → not equal
* transient → not equal
* same instance → equal
* hash consistency

---

## جمع‌بندی

`Entity` در زمین X:

* ساده
* شفاف
* فقط identity + equality

و intentionally minimal است تا:

* reusable بماند
* over-engineering نشود
* پایه درستی برای AggregateRoot ایجاد کند
