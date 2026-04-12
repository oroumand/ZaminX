# Fluent Validation Integration

## هدف این سند

این سند نحوه integration بین ZaminX Application و FluentValidation را توضیح می‌دهد.

هدف:

* ارائه validation به‌صورت behavior در pipeline
* جداسازی کامل validation از core Application
* فراهم کردن integration رسمی و reusable

---

## جایگاه در معماری

FluentValidation به‌عنوان یک integration layer در نظر گرفته شده است.

* Core Application → بدون validation
* FluentValidation → در پروژه جدا

پروژه:

```
ZaminX.BuildingBlocks.Application.FluentValidation
```

---

## چرا validation در core نیست؟

در طراحی ZaminX:

* core Application باید minimal باشد
* وابسته به framework خاص نباشد
* قابل reuse در سناریوهای مختلف باشد

به همین دلیل:

* validation از core حذف شده
* به‌صورت integration مستقل پیاده‌سازی شده

---

## نحوه فعال‌سازی

برای فعال‌سازی FluentValidation:

```csharp
services.AddZaminXApplication();

services.AddZaminXApplicationFluentValidation(options =>
{
    options.AddAssembly(typeof(SalesModuleMarker).Assembly);
});
```

---

## چه اتفاقی می‌افتد؟

با این configuration:

1. validatorها scan و در DI ثبت می‌شوند
2. behavior validation به pipeline اضافه می‌شود
3. requestها قبل از رسیدن به handler validate می‌شوند

---

## FluentValidationBehavior

این behavior:

* تمام validatorهای مربوط به message را پیدا می‌کند
* آن‌ها را اجرا می‌کند
* در صورت وجود خطا:

  * `Result.Failure(...)` برمی‌گرداند
  * از اجرای handler جلوگیری می‌کند

---

## ترتیب اجرای pipeline

ترتیب پیش‌فرض:

1. RequestTelemetryBehavior
2. FluentValidationBehavior
3. Custom Behaviors
4. ExceptionToResultBehavior

---

## ثبت Validatorها

دو روش پشتیبانی می‌شود:

### روش 1: از طریق extension

```csharp
options.AddAssembly(typeof(SalesModuleMarker).Assembly);
```

---

### روش 2: دستی (اختیاری)

```csharp
services.AddValidatorsFromAssembly(typeof(SalesModuleMarker).Assembly);
```

Behavior در هر صورت validatorها را consume می‌کند.

---

## ساخت Validator

نمونه:

```csharp
public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.CustomerName)
            .NotEmpty();

        RuleFor(x => x.Amount)
            .GreaterThan(0);
    }
}
```

---

## نحوه کار در runtime

برای هر request:

1. mediator behavior را اجرا می‌کند
2. behavior validatorها را resolve می‌کند
3. validatorها اجرا می‌شوند
4. اگر خطا بود:

   * handler اجرا نمی‌شود
   * Result.Failure برمی‌گردد
5. اگر موفق بود:

   * execution ادامه پیدا می‌کند

---

## Error Mapping

هر خطای FluentValidation به این شکل تبدیل می‌شود:

```json
{
  "code": "validation.error-code",
  "message": "error message"
}
```

---

## dependencyها

FluentValidationBehavior وابسته به:

```
IValidator<TMessage>
```

است.

این validatorها:

* از DI resolve می‌شوند
* توسط FluentValidation registration extension ثبت می‌شوند

---

## مزایای این طراحی

* جداسازی کامل concerns
* عدم وابستگی core به FluentValidation
* reusable بودن integration
* pipeline-based validation
* قابل توسعه برای frameworkهای دیگر

---

## Non-goals

این integration شامل این موارد نیست:

* cross-field orchestration پیچیده
* async external validation orchestration
* validation caching
* validation distribution

---

## جمع‌بندی

در ZaminX:

* validation بخشی از core Application نیست
* validation به‌صورت behavior در pipeline اجرا می‌شود
* FluentValidation integration رسمی و مستقل دارد
* مصرف‌کننده با یک extension ساده آن را فعال می‌کند
