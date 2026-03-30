# ADR 007: جایگاه pattern-like primitiveها در BuildingBlocks

## وضعیت
پذیرفته‌شده

---

## زمینه

بعضی اجزا مانند:
- Entity
- AggregateRoot
- ValueObject
- DomainEvent
- Command
- Query
- CommandHandler
- QueryHandler
- Mediator

ماهیت pattern-like دارند.  
اما این موضوع به‌تنهایی تعیین نمی‌کند که باید در ApplicationPatterns قرار بگیرند.

اگر صرف pattern-like بودن را معیار قرار دهیم:
- مرز BuildingBlocks و ApplicationPatterns مخدوش می‌شود
- capabilityهای مستقل به‌اشتباه از BuildingBlocks خارج می‌شوند

---

## تصمیم

اگر یک primitive یا الگوی پایه به‌صورت مستقل، خودبسنده و قابل بازاستفاده ارائه شود، در BuildingBlocks قرار می‌گیرد؛ حتی اگر ماهیت pattern-like داشته باشد.

---

## دلایل تصمیم

این تصمیم برای این گرفته می‌شود که:
- معیار اصلی BuildingBlocks حفظ شود
- pattern-like بودن با وابستگی به flow سطح اپلیکیشن اشتباه گرفته نشود
- primitiveهای پایه در جای درست خود قرار بگیرند

---

## پیامدها

### پیامدهای مثبت
- مرز ApplicationPatterns روشن‌تر می‌شود
- primitiveهای پایه در taxonomy درست قرار می‌گیرند

### هزینه‌ها
- بعضی اجزا همچنان نیاز به بحث مرزی خواهند داشت

---

## ارتباط با سایر اسناد

- `docs/02.architecture/index.md`
- `docs/03.modules/index.md`
- `docs/03.modules/00.BuildingBlocks/index.md`