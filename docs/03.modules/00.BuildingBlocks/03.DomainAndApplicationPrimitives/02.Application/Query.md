# Query

## هدف این سند

این سند primitive مربوط به `Query` را در خانواده
`00.BuildingBlocks/03.DomainAndApplicationPrimitives/02.Application`
تعریف می‌کند.

نقش این سند:

* تعریف دقیق `Query`
* روشن کردن تفاوت آن با `Command`
* مشخص کردن مرز آن با `Event`
* ثبت تصمیم‌های طراحی نسخه اول
* فراهم کردن مرجع برای طراحی handlerها و pipelineهای read

---

## تعریف

`Query` یک primitive اپلیکیشنی است که بیان می‌کند:

> «یک درخواست برای خواندن داده بدون تغییر state سیستم»

در زمین X، `Query` برای این استفاده می‌شود که:

* عملیات‌های read-side را مدل کند
* intent صریح برای خواندن داده ایجاد کند
* از تغییر state در مسیر خواندن جلوگیری کند

---

## جایگاه در taxonomy

* دسته: `BuildingBlocks`
* خانواده: `03.DomainAndApplicationPrimitives`
* زیرخانواده: `02.Application`
* primitive: `Query`

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

---

## چرا در زمین X به Query نیاز داریم

وجود `Query` به این دلایل justified است:

1. جداسازی read و write (CQRS-lite)
2. جلوگیری از تغییر state در مسیر خواندن
3. فراهم کردن پایه برای read pipeline
4. شفاف‌سازی intent عملیات‌ها

---

## مدل طراحی نسخه اول

نسخه اول `Query` در زمین X با این مدل طراحی می‌شود:

* `Query` به‌صورت interface تعریف می‌شود
* از `IRequest<TResponse>` ارث می‌برد
* خروجی آن همیشه از جنس `Result<T>` است
* هیچ behavior یا policy در آن قرار داده نمی‌شود

---

## قرارداد فنی

```csharp
public interface IQuery<TResponse> : IRequest<Result<TResponse>>
```

---

## چرا Query کلاس پایه نیست

در زمین X، `Query` به‌صورت interface تعریف شده است.

### دلیل این تصمیم

* queryها معمولاً data carrier هستند
* behavior مشترک ندارند
* استفاده از record یا class آزاد است
* inheritance کلاس باعث محدودیت غیرضروری می‌شود

---

## نسبت با Result Pattern

در زمین X، خروجی query همیشه از جنس `Result<T>` است.

### دلیل این تصمیم

* outcome باید صریح باشد
* failure باید قابل بیان باشد
* contract handlerها یکدست می‌شود

---

## مرز با Command

### Query

* state را تغییر نمی‌دهد
* side effect ندارد
* فقط داده می‌خواند
* خروجی `Result<T>` دارد

### Command

* state را تغییر می‌دهد
* side effect دارد
* خروجی `Result` یا `Result<T>` دارد

---

## مرز با Event

### Query

* درخواست خواندن است
* handler مشخص دارد
* پاسخ دارد

### Event

* اعلان یک اتفاق است
* ممکن است handler نداشته باشد
* ممکن است چند handler داشته باشد
* پاسخ مستقیم ندارد

---

## چرا caching در قرارداد Query نیامده

در زمین X، concernهایی مثل caching وارد `IQuery` نشده‌اند.

### دلیل این تصمیم

* caching یک concern اجرایی است، نه معنایی
* نباید contract را آلوده کند
* بهتر است از طریق attribute و pipeline مدیریت شود

---

## چه چیزهایی عمداً داخل Query نیستند

در نسخه اول، این موارد **جزو scope Query نیستند**:

* caching
* pagination contract
* filtering abstraction
* sorting abstraction
* authorization
* validation
* metadata
* transport concern

---

## Non-goals

این primitive در نسخه اول این هدف‌ها را ندارد:

* تبدیل شدن به framework کامل read model
* تعریف abstraction برای data access
* حل concernهای performance مثل caching در خود contract
* ارائه behavior مشترک

---

## naming و structure

### Solution

Relay.slnx

### Project

ZaminX.BuildingBlocks.Application

### Namespace

ZaminX.BuildingBlocks.Application.Queries

### مسیر فایل کد

src/00.BuildingBlocks/03.DomainAndApplicationPrimitives/02.Application/src/ZaminX.BuildingBlocks.Application/Queries/IQuery.cs

---

## استراتژی تست

حداقل تست‌های لازم:

1. `IQuery` باید `IMessage` را پیاده‌سازی کند
2. `IQuery` باید `IRequest<Result<T>>` باشد
3. هر query باید قابل implement باشد

---

## observationهای باز

موارد زیر هنوز guideline نهایی نیستند:

* نیاز به pagination abstraction
* نیاز به filtering DSL
* نیاز به caching policyهای پیشرفته
* relation با read model architecture

---

## جمع‌بندی

در زمین X، `Query` یک primitive ساده و صریح است که:

* intent خواندن داده را مدل می‌کند
* state را تغییر نمی‌دهد
* روی `Result Pattern` سوار است
* از concernهای اجرایی جدا نگه داشته شده است

این primitive intentionally minimal نگه داشته می‌شود تا:

* reusable بماند
* از over-engineering جلوگیری شود
* پایه‌ای تمیز برای read-side application فراهم کند
