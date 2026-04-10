# Command

## هدف این سند

این سند primitive مربوط به `Command` را در خانواده
`00.BuildingBlocks/03.DomainAndApplicationPrimitives/02.Application`
تعریف می‌کند.

نقش این سند:

* تعریف دقیق `Command`
* روشن کردن دلیل نیاز به آن در زمین X
* مشخص کردن مرز آن با `Query` و `Event`
* مشخص کردن نسبت آن با `Result Pattern`
* ثبت تصمیم‌های طراحی نسخه اول
* فراهم کردن مرجع برای طراحی handlerها و mediator

---

## تعریف

`Command` یک primitive اپلیکیشنی است که بیان می‌کند:

> «یک درخواست برای تغییر state سیستم»

در زمین X، `Command` برای این استفاده می‌شود که:

* عملیات‌های تغییردهنده state را مدل کند
* intent صریح برای write-side ایجاد کند
* پایه‌ای برای handlerها و pipelineهای application فراهم کند

---

## جایگاه در taxonomy

* دسته: `BuildingBlocks`
* خانواده: `03.DomainAndApplicationPrimitives`
* زیرخانواده: `02.Application`
* primitive: `Command`

---

## جایگاه در مدل پیام‌ها

در زمین X، `Command` بخشی از خانواده messageها است:

```text
IMessage
├── IRequest<TResponse>
│   ├── ICommand
│   ├── ICommand<TResponse>
│   └── IQuery<TResponse>
└── IEvent
```

---

## چرا در زمین X به Command نیاز داریم

وجود `Command` به این دلایل justified است:

1. جداسازی read و write (CQRS-lite)
2. جلوگیری از mix شدن semantics عملیات‌ها
3. فراهم کردن پایه برای handler pipeline
4. ساده‌سازی reasoning درباره تغییر state

---

## مدل طراحی نسخه اول

نسخه اول `Command` در زمین X با این مدل طراحی می‌شود:

* `Command` به‌صورت interface تعریف می‌شود
* از `IRequest<TResponse>` ارث می‌برد
* خروجی آن همیشه از جنس `Result` است
* نسخه generic برای commandهایی که خروجی دارند استفاده می‌شود

---

## قرارداد فنی

### ICommand

```csharp
public interface ICommand : IRequest<Result>
```

### ICommand<TResponse>

```csharp
public interface ICommand<TResponse> : IRequest<Result<TResponse>>
```

---

## چرا Command کلاس پایه نیست

در زمین X، `Command` به‌صورت interface تعریف شده است، نه کلاس.

### دلیل این تصمیم

* commandها معمولاً data carrier هستند
* نیازی به behavior مشترک ندارند
* inheritance کلاس باعث محدودیت غیرضروری می‌شود
* flexibility برای استفاده از record یا class حفظ می‌شود

---

## نسبت با Result Pattern

در زمین X، خروجی command همیشه از جنس `Result` است.

### دلیل این تصمیم

* outcome عملیات باید صریح باشد
* failure باید بدون exception قابل بیان باشد
* handlerها باید قرارداد یکدست داشته باشند

---

## مرز با Query

### Command

* state را تغییر می‌دهد
* side effect دارد
* معمولاً idempotent نیست (مگر طراحی شود)
* خروجی آن `Result` است

### Query

* state را تغییر نمی‌دهد
* فقط data را می‌خواند
* side effect ندارد
* خروجی آن `Result<T>` است

---

## مرز با Event

### Command

* درخواست انجام یک کار است
* معمولاً یک handler اصلی دارد
* پاسخ (Result) دارد

### Event

* اعلان وقوع یک اتفاق است
* ممکن است هیچ handler نداشته باشد
* ممکن است چند handler داشته باشد
* پاسخ مستقیم ندارد

---

## چرا Event زیر Request قرار نگرفت

در زمین X، `Event` از `IRequest` ارث نمی‌برد.

### دلیل این تصمیم

* event درخواست نیست، اعلان است
* semantics متفاوتی دارد
* handler model آن متفاوت است
* response model ندارد

---

## چه چیزهایی عمداً داخل Command نیستند

در نسخه اول، این موارد **جزو scope Command نیستند**:

* handler
* validation
* authorization
* transaction
* idempotency
* metadata مثل userId یا correlationId
* persistence concern
* transport concern

---

## Non-goals

این primitive در نسخه اول این هدف‌ها را ندارد:

* تبدیل شدن به abstraction کامل برای command handling
* تعریف pipeline
* حل concernهای cross-cutting
* ارائه behavior مشترک

---

## naming و structure

### Solution

Relay.slnx

### Project

ZaminX.BuildingBlocks.Application

### Namespace

ZaminX.BuildingBlocks.Application.Commands

### مسیر فایل کد

src/00.BuildingBlocks/03.DomainAndApplicationPrimitives/02.Application/src/ZaminX.BuildingBlocks.Application/Commands/ICommand.cs

---

## استراتژی تست

حداقل تست‌های لازم:

1. `ICommand` باید `IMessage` را پیاده‌سازی کند
2. `ICommand` باید `IRequest<Result>` باشد
3. `ICommand<T>` باید `IRequest<Result<T>>` باشد
4. هر command باید قابل implement باشد

---

## observationهای باز

موارد زیر هنوز guideline نهایی نیستند:

* نیاز به `CommandBase` در آینده
* نیاز به metadata مشترک
* relation با validation pipeline
* relation با authorization

---

## جمع‌بندی

در زمین X، `Command` یک primitive ساده و صریح است که:

* intent تغییر state را مدل می‌کند
* روی `Result Pattern` سوار است
* بخشی از خانواده messageها است
* behavior یا infrastructure اضافی ندارد

این primitive intentionally minimal نگه داشته می‌شود تا:

* reusable بماند
* از over-engineering جلوگیری شود
* پایه‌ای تمیز برای handler و mediator فراهم کند
