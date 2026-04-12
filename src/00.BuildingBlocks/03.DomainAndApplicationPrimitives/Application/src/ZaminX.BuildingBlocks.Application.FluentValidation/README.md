# ZaminX.BuildingBlocks.Application.FluentValidation

## معرفی

این پروژه integration رسمی بین ZaminX Application و FluentValidation را فراهم می‌کند.

---

## هدف

* اجرای validation در pipeline
* حذف وابستگی FluentValidation از core
* ارائه API ساده برای فعال‌سازی validation

---

## نصب

```csharp
services.AddZaminXApplicationFluentValidation(options =>
{
    options.AddAssembly(typeof(ModuleMarker).Assembly);
});
```

---

## چه اتفاقی می‌افتد

* validatorها register می‌شوند
* FluentValidationBehavior به pipeline اضافه می‌شود
* requestها قبل از handler validate می‌شوند

---

## ساخت Validator

```csharp
public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.Amount).GreaterThan(0);
    }
}
```

---

## ترتیب pipeline

1. RequestTelemetry
2. FluentValidation
3. Custom
4. ExceptionToResult

---

## نکات مهم

* نیاز به register دستی behavior نیست
* dependencyهای validator باید در DI باشند
* validatorها می‌توانند در هر ماژول باشند

---

## جمع‌بندی

این پروژه:

* validation را به‌صورت clean و modular ارائه می‌دهد
* integration استاندارد FluentValidation را فراهم می‌کند
* کاملاً از core جداست
