# ZaminX.BuildingBlocks.Application

## معرفی

این پروژه مجموعه‌ای از primitiveهای لایه Application در ZaminX را فراهم می‌کند.

هدف این ماژول:

* تعریف الگوی استاندارد برای Command / Query / Event
* ارائه یک Mediator سبک و قابل توسعه
* فراهم کردن pipeline behaviorها
* ارائه Result pattern یکپارچه
* پشتیبانی از معماری modular monolith

---

## قابلیت‌ها

### Messaging

* `ICommand`
* `IQuery`
* `IEvent`

### Handlers

* `ICommandHandler`
* `IQueryHandler`
* `IEventHandler`

### Mediator

* `IMediator`
* `Send` برای requestها
* `Publish` برای eventها

### Result Pattern

* `Result`
* `Result<T>`
* پشتیبانی از چندین error

### Pipeline Behaviors

* اجرای behaviorها قبل/بعد از handler
* پشتیبانی از ordering

### Built-in Behaviors

* RequestTelemetry
* Validation
* ExceptionToResult

---

## نصب و استفاده

### 1. ثبت زیرساخت Application

```csharp
services.AddZaminXApplication(options =>
{
    options.EnableRequestTelemetryBehavior = true;
    options.EnableValidationBehavior = true;
    options.EnableExceptionToResultBehavior = true;
});
```

---

### 2. ثبت handlerها

```csharp
services.AddZaminXApplicationHandlers(typeof(MyModuleMarker).Assembly);
```

در معماری modular monolith، هر ماژول باید handlerهای خودش را register کند.

---

### 3. استفاده از Mediator

```csharp
var result = await mediator.Send(new CreateOrderCommand(...));
```

```csharp
await mediator.Publish(new OrderCreatedEvent(...));
```

---

## Custom Behavior

برای اضافه کردن behavior سفارشی:

```csharp
services.AddZaminXApplication(options =>
{
    options.AddOpenBehavior(typeof(MyCustomBehavior<,>));
});
```

### نکته مهم

* نیاز به register دستی ندارد
* به‌صورت خودکار در DI ثبت می‌شود
* به‌صورت خودکار وارد pipeline می‌شود

---

## Dependencyهای Behavior

اگر behavior شما dependency داشته باشد:

```csharp
public class MyBehavior<TMessage, TResponse>
{
    public MyBehavior(IMyService service) { }
}
```

باید dependency را جدا register کنید:

```csharp
services.AddScoped<IMyService, MyService>();
```

---

## Pipeline Execution

ترتیب پیش‌فرض:

1. RequestTelemetry
2. Validation
3. Custom Behaviors
4. ExceptionToResult

ترتیب نهایی با `Order` تعیین می‌شود.

---

## Validation

این ماژول به FluentValidation وابسته نیست.

از abstraction زیر استفاده می‌کند:

```csharp
IMessageValidator<TMessage>
```

در صورت نیاز می‌توانید adapter برای FluentValidation بنویسید.

---

## Modular Monolith

الگوی پیشنهادی:

```csharp
services.AddZaminXApplication();

services.AddSalesModule();
services.AddIdentityModule();
```

و داخل هر ماژول:

```csharp
services.AddZaminXApplicationHandlers(typeof(ModuleMarker).Assembly);
```

---

## ASP.NET Core Integration

برای تبدیل Result به HTTP response:

از پروژه:

```
ZaminX.BuildingBlocks.Application.AspNetCore
```

استفاده کنید.

---

## مستندات

برای جزئیات بیشتر:

* `mediator.md`
* `result.md`
* `commands.md`
* `queries.md`

---

## خارج از محدوده این ماژول

* message bus
* distributed messaging
* transaction management
* caching
* retry policies

---

## جمع‌بندی

این ماژول یک لایه Application سبک، قابل توسعه و مناسب برای:

* Clean Architecture
* Modular Monolith
* Microservices

فراهم می‌کند.
