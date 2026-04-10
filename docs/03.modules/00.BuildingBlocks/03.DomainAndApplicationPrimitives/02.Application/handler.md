# Handler

## هدف این سند

این سند primitive مربوط به `Handler` را در خانواده
`00.BuildingBlocks/03.DomainAndApplicationPrimitives/02.Application` تعریف می‌کند.

نقش این سند:

* تعریف قرارداد handler برای `Command` و `Query`
* مشخص کردن نحوه اجرای messageها در application layer
* روشن کردن مرز handler با mediator و pipeline
* ثبت تصمیم‌های طراحی نسخه اول

---

## تعریف

`Handler` یک primitive اپلیکیشنی است که مسئول **اجرای یک message** (Command یا Query) است.

در زمین X:

* هر `Command` دقیقاً توسط یک handler اصلی اجرا می‌شود
* هر `Query` دقیقاً توسط یک handler اصلی اجرا می‌شود
* handler نتیجه را در قالب `Result` یا `Result<T>` برمی‌گرداند

---

## جایگاه در taxonomy

* دسته: `BuildingBlocks`
* خانواده: `03.DomainAndApplicationPrimitives`
* زیرخانواده: `02.Application`
* primitive: `Handler`

---

## جایگاه در مدل پیام‌ها

```text
IMessage
├── IRequest<TResponse>
│   ├── ICommand
│   ├── ICommand<TResponse>
│   └── IQuery<TResponse>
└── IEvent
```

Handlerها روی `Command` و `Query` تعریف می‌شوند.

---

## قراردادهای موجود

### Command Handler (بدون خروجی)

```csharp
public interface ICommandHandler<in TCommand>
    where TCommand : ICommand
{
    Task<Result> Handle(TCommand command, CancellationToken cancellationToken = default);
}
```

---

### Command Handler (با خروجی)

```csharp
public interface ICommandHandler<in TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken = default);
}
```

---

### Query Handler

```csharp
public interface IQueryHandler<in TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken = default);
}
```

---

## چرا CommandHandler و QueryHandler جدا هستند

در زمین X، handlerها بر اساس semantics جدا شده‌اند:

### Command Handler

* تغییر state انجام می‌دهد
* side effect دارد
* concernهایی مثل transaction، validation و authorization به آن نزدیک هستند

### Query Handler

* فقط داده را می‌خواند
* side effect ندارد
* concernهایی مثل caching و projection به آن نزدیک هستند

---

## چرا MessageHandler مشترک نداریم

در این نسخه، `IMessageHandler` تعریف نشده است.

### دلیل این تصمیم

* تفاوت semantics بین command، query و event
* تفاوت مدل handler (single vs multi handler)
* تفاوت response model
* جلوگیری از abstraction زودهنگام

---

## نسبت با Mediator

`Handler` فقط قرارداد اجرای message را تعریف می‌کند.

### نکته مهم

* handler مسئول orchestration نیست
* handler مسئول dispatch نیست
* handler مسئول pipeline نیست

این‌ها مربوط به primitive بعدی یعنی `Mediator` هستند.

---

## نقش CancellationToken

تمام handlerها `CancellationToken` دارند.

### دلیل این تصمیم

* هماهنگی با async programming در .NET
* امکان cancel کردن عملیات‌های طولانی
* هم‌راستایی با الگوهای رایج

---

## چه چیزهایی عمداً داخل Handler نیستند

در نسخه اول، این موارد **جزو scope Handler نیستند**:

* logging
* validation
* authorization
* transaction management
* caching
* retry
* pipeline behavior
* exception handling policy

این concernها در لایه mediator/pipeline مدیریت خواهند شد.

---

## Non-goals

این primitive در نسخه اول این هدف‌ها را ندارد:

* تبدیل شدن به framework کامل handler execution
* ارائه abstraction مشترک برای همه handlerها
* مدیریت lifecycle handlerها
* تعریف dependency injection policy

---

## ساختار فایل‌ها

### Commands

```text
Commands/
  ICommand.cs
  ICommandHandler.cs
```

### Queries

```text
Queries/
  IQuery.cs
  IQueryHandler.cs
```

---

## ساختار تست‌ها

```text
Commands/
  CommandContractsTests.cs
  CommandHandlerContractsTests.cs

Queries/
  QueryContractsTests.cs
  QueryHandlerContractsTests.cs
```

---

## استراتژی تست

حداقل تست‌های لازم:

### CommandHandler

* قابل implement باشد
* خروجی `Result` را برگرداند
* در نسخه generic مقدار را درست برگرداند

### QueryHandler

* قابل implement باشد
* خروجی `Result<T>` را برگرداند

---

## observationهای باز

موارد زیر هنوز guideline نهایی نیستند:

* نیاز به base handler class
* نیاز به decorator pattern
* نحوه اتصال به DI container
* نحوه discovery handlerها
* relation با mediator pipeline

---

## جمع‌بندی

در زمین X، `Handler` یک primitive ساده و صریح است که:

* مسئول اجرای command و query است
* از `Result Pattern` استفاده می‌کند
* به صورت interface تعریف شده است
* از concernهای cross-cutting جدا نگه داشته شده است

این primitive intentionally minimal نگه داشته می‌شود تا:

* قابل استفاده مجدد باشد
* ساده باقی بماند
* پایه‌ای تمیز برای mediator و pipeline فراهم کند