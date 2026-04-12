# Mediator

## هدف این سند

این سند primitive مربوط به `Mediator` را در خانواده
`00.BuildingBlocks/03.DomainAndApplicationPrimitives/02.Application`
تعریف می‌کند.

نقش این سند:

* تعریف دقیق `IMediator`
* توضیح نقش `Mediator` در Relay
* ثبت مدل pipeline برای requestها
* ثبت مدل publish برای eventها
* توضیح built-in behaviorها
* توضیح ordering behaviorها
* توضیح registration story ماژول

---

## تعریف

`Mediator` در زمین X یک primitive اپلیکیشنی برای orchestration پیام‌ها است.

در Relay، mediator مسئول این کارها است:

* dispatch کردن `Command` و `Query`
* publish کردن `Event`
* اجرای pipeline behaviorها برای requestها
* resolve کردن handler مناسب از DI container

`Mediator` business logic اجرا نمی‌کند.
مسئولیت آن فقط orchestration و dispatch است.

---

## جایگاه در taxonomy

* دسته: `BuildingBlocks`
* خانواده: `03.DomainAndApplicationPrimitives`
* زیرخانواده: `02.Application`
* primitive: `Mediator`

---

## نقش Mediator در Relay

Relay چند concern اصلی دارد:

* تعریف messageها
* تعریف handlerها
* تعریف result model
* تعریف pipeline behaviorها

`Mediator` نقطه اتصال این concernها است.

### برای requestها

`Mediator`:

1. handler مناسب را پیدا می‌کند
2. behaviorهای مناسب را resolve می‌کند
3. pipeline را می‌سازد
4. request را اجرا می‌کند
5. `Result` یا `Result<T>` برمی‌گرداند

### برای eventها

`Mediator`:

1. event handlerهای ثبت‌شده را پیدا می‌کند
2. event را برای همه آن‌ها publish می‌کند

---

## قرارداد فنی

### IMediator

```csharp
public interface IMediator
{
    Task<TResponse> Send<TResponse>(
        IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
        where TResponse : Result;

    Task Publish<TEvent>(
        TEvent @event,
        CancellationToken cancellationToken = default)
        where TEvent : IEvent;
}
```

---

## مدل request execution

### Send

متد `Send` فقط برای requestها است:

* `Command`
* `Query`

ویژگی‌های مهم:

* خروجی همیشه از جنس `Result` یا `Result<T>` است
* فقط یک handler اصلی برای request اجرا می‌شود
* behaviorها قبل و بعد از handler قابل اجرا هستند

---

## مدل event publishing

### Publish

متد `Publish` فقط برای eventها است.

ویژگی‌های مهم:

* event ممکن است هیچ handler نداشته باشد
* event ممکن است یک یا چند handler داشته باشد
* پاسخ business-level مستقیم ندارد
* در نسخه اول، publish به‌صورت sequential انجام می‌شود

---

## تفاوت request و event در mediator

### Request

* dispatch می‌شود
* یک handler اصلی دارد
* پاسخ دارد
* pipeline behavior دارد

### Event

* publish می‌شود
* صفر تا چند handler دارد
* پاسخ مستقیم ندارد
* در نسخه اول pipeline behavior ندارد

---

## pipeline behavior چیست

pipeline behavior یک لایه اجرایی بین mediator و handler است.

هر behavior می‌تواند:

* قبل از handler کاری انجام دهد
* بعد از handler کاری انجام دهد
* یا اصلاً اجازه ندهد request به handler برسد

### قرارداد پایه

```csharp
public interface IMessageBehavior<in TMessage, TResponse>
    where TMessage : IMessage
    where TResponse : Result
{
    Task<TResponse> Handle(
        TMessage message,
        MessageHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken = default);
}
```

---

## ordering behaviorها

در Relay، ترتیب behaviorها deterministic است.

### قرارداد ordering

```csharp
public interface IOrderedMessageBehavior<in TMessage, TResponse>
    : IMessageBehavior<TMessage, TResponse>
    where TMessage : IMessage
    where TResponse : Result
{
    int Order { get; }
}
```

### قاعده اجرا

* behaviorها بر اساس `Order` به‌صورت صعودی مرتب می‌شوند
* اگر `Order` برابر باشد، registration order به‌عنوان tie-breaker استفاده می‌شود
* اگر behavior ordered نباشد، order پیش‌فرض `1000` می‌گیرد

---

## built-in behaviorهای نسخه اول

### 1. RequestTelemetryBehavior

مسئول:

* logging
* timing

ویژگی‌ها:

* شروع request را log می‌کند
* پایان request را log می‌کند
* زمان اجرای request را ثبت می‌کند
* success/failure را log می‌کند

---

### 2. Validation

در نسخه فعلی، validation بخشی از core Application نیست.

Validation به‌صورت integration جدا ارائه می‌شود:

ZaminX.BuildingBlocks.Application.FluentValidation

این integration:

* FluentValidationBehavior را به pipeline اضافه می‌کند
* validatorها را از DI resolve می‌کند
* failure را به Result تبدیل می‌کند

Core Application هیچ وابستگی‌ای به FluentValidation ندارد.


---

### 3. ExceptionToResultBehavior

مسئول:

* گرفتن exceptionها
* تبدیل آن‌ها به `Result.Failure(...)`

ویژگی‌ها:

* اگر exception دارای property به نام `Code` از نوع `string` باشد، همان code استفاده می‌شود
* اگر code نداشته باشد، مقدار پیش‌فرض:
  `application.unhandled-exception`
* exception خام از mediator خارج نمی‌شود

---

## ترتیب پیش‌فرض built-in behaviorها

1. `RequestTelemetryBehavior`
2. `ValidationBehavior`
3. custom behaviorها
4. `ExceptionToResultBehavior`

---

## custom behaviorها

```csharp
services.AddZaminXApplication(options =>
{
    options.AddOpenBehavior(typeof(MyCustomBehavior<,>));
});
```

### نکته مهم

* نیاز به register دستی روی `IMessageBehavior<,>` ندارند
* خود ماژول آن‌ها را در DI ثبت می‌کند
* خود ماژول آن‌ها را وارد pipeline می‌کند

### اما dependencyها؟

dependencyهای داخلی behavior باید در DI اپلیکیشن ثبت شوند.

---

## registration story

### AddZaminXApplication

* ثبت `IMediator`
* ثبت built-in behaviorها
* ثبت custom behaviorها

### AddZaminXApplicationHandlers

* scan و register handlerها

---

## modular monolith

```csharp
services.AddZaminXApplication();

services.AddZaminXApplicationHandlers(typeof(SalesModuleMarker).Assembly);
services.AddZaminXApplicationHandlers(typeof(IdentityModuleMarker).Assembly);
```

---

## configuration

```csharp
services.AddZaminXApplication(options =>
{
    options.EnableRequestTelemetryBehavior = true;
    options.EnableValidationBehavior = true;
    options.EnableExceptionToResultBehavior = true;

    options.AddOpenBehavior(typeof(HeaderLoggingBehavior<,>));
});
```

---

## چرا وابسته به FluentValidation نیست

Relay از abstraction داخلی استفاده می‌کند:

```csharp
IMessageValidator<TMessage>
```

---

## خارج از scope

* event pipeline
* retry
* transaction
* caching
* distributed tracing
* message bus

---

## جمع‌بندی

Mediator در Relay:

* requestها را dispatch می‌کند
* eventها را publish می‌کند
* pipeline behaviorها را اجرا می‌کند
* built-in و custom behaviorها را پشتیبانی می‌کند
* برای modular monolith طراحی شده است
