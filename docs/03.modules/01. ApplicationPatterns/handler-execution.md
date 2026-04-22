# HandlerExecution

## معرفی

`HandlerExecution` یک capability از دسته `ApplicationPatterns` در ZaminX است که برای استانداردسازی پیاده‌سازی handlerها در سطح application طراحی شده است.

این capability روی BuildingBlockهای موجود سوار می‌شود و بدون ساخت abstraction غیرضروری جدید، یک execution model ساده و قابل استفاده برای command handlerها و query handlerها ارائه می‌دهد.

هدف اصلی آن کاهش boilerplate، یکدست‌سازی access به dependencyهای پرتکرار، ساده‌سازی ساخت `Result` و فراهم کردن یک پایه مشترک برای handlerهای application است.

---

## مسئله‌ای که حل می‌کند

در handlerهای application معمولاً الگوهای تکراری زیر دیده می‌شوند:

* constructorهای شلوغ برای dependencyهای مشترک
* تکرار دسترسی به mapper، serializer، translator، logger و current user
* ساخت تکراری `Result` و `Result<T>`
* جمع‌آوری خطاهای execution در طول اجرای handler
* تکرار الگوی دسترسی به repositoryهای read و write
* نیاز به دسترسی مکرر به `IUnitOfWork` در command handlerها

`HandlerExecution` این concerns را به‌صورت یک الگوی reusable و کم‌پیچیدگی استاندارد می‌کند.

---

## جایگاه در taxonomy

`HandlerExecution` در این مسیر قرار می‌گیرد:

```text
src/
  02.ApplicationPatterns/
    HandlerExecution/
```

این capability:

* BuildingBlock نیست
* Foundation نیست
* Integration نیست

بلکه یک application-level reusable pattern است که از capabilityهای موجود در لایه‌های پایین‌تر استفاده می‌کند.

---

## مرز مسئولیت

`HandlerExecution` مسئول این موارد است:

* ارائه dependency bundle برای dependencyهای پرتکرار handlerها
* ارائه base class مشترک برای concerns عمومی handlerها
* ارائه base class برای command handlerها
* ارائه base class برای query handlerها
* ارائه context ساده برای جمع‌آوری خطاهای execution
* ارائه helperهای ergonomic برای ساخت `Result`

`HandlerExecution` مسئول این موارد نیست:

* تعریف mediator جدید
* تعریف pipeline abstraction جدید
* تعریف behavior framework جدید
* تعریف repository abstraction جدید
* registration و DI orchestration
* policy engine یا execution engine پیچیده
* جایگزینی primitiveهای `Result`، `Relay` یا `Axiom`

---

## وابستگی‌ها

نسخه فعلی `HandlerExecution` روی capabilityهای زیر متکی است:

* `ZaminX.BuildingBlocks.Application`
* `ZaminX.BuildingBlocks.Domain`
* `ZaminX.BuildingBlocks.Data.Read.Abstractions`
* `ZaminX.BuildingBlocks.Data.Write.Abstractions`
* `ZaminX.BuildingBlocks.CrossCutting.ObjectMapper.Abstractions`
* `ZaminX.BuildingBlocks.CrossCutting.Serializer.Abstractions`
* `ZaminX.BuildingBlocks.CrossCutting.Translator.Abstractions`
* `ZaminX.BuildingBlocks.IdentityAndUsers.Persona.Abstractions`
* `Microsoft.Extensions.Logging.Abstractions`

---

## اجزای اصلی

### HandlerServices

`HandlerServices` یک dependency bundle تایپ‌شده برای dependencyهای پرتکرار handlerها است.

در نسخه فعلی این dependencyها را نگه می‌دارد:

* `IMapperAdapter`
* `IJsonSerializer`
* `ITranslator`
* `ILoggerFactory`
* `ICurrentUser?`

این کلاس یک service locator نیست، چون dependencyها را:

* explicit دریافت می‌کند
* typed نگه می‌دارد
* و فقط برای ساده‌سازی constructorهای handler base استفاده می‌شود

---

### ResultContext

`ResultContext` یک context کوچک و کنترل‌شده برای جمع‌آوری خطاهای execution همان handler است.

ویژگی‌های آن:

* lazy ساخته می‌شود
* فقط خطا را نگه می‌دارد
* bag آزاد از objectها نیست
* می‌تواند به `Result` یا `Result<T>` تبدیل شود

این context عمداً ساده نگه داشته شده است تا با مدل فعلی `Result` در ZaminX هم‌راستا بماند.

---

### ApplicationHandlerBase

`ApplicationHandlerBase` پایه مشترک concernهای عمومی handlerها است.

این کلاس مسئول این موارد است:

* دسترسی به `Mapper`
* دسترسی به `Serializer`
* دسترسی به `Translator`
* دسترسی به `Logger`
* دسترسی به `CurrentUser`
* نگه‌داری lazy از `ResultContext`
* ارائه helperهای ergonomic برای ساخت `Result`

این کلاس خودش handler نهایی نیست و قراردادهای Relay را مستقیم پیاده نمی‌کند.

---

### CommandHandlerBase

`CommandHandlerBase` پایه استاندارد برای command handlerها است.

ویژگی‌های اصلی:

* از `ApplicationHandlerBase` ارث می‌برد
* قراردادهای `ICommandHandler<TCommand>` و `ICommandHandler<TCommand, TResponse>` را پیاده می‌کند
* به write repository وابسته است
* به `IUnitOfWork` وابسته است
* aggregate را با `AggregateRoot<TId>` constrain می‌کند

این کلاس concern مربوط به write side را در همان خانواده command نگه می‌دارد و class اضافی جداگانه برای repository-aware شدن تولید نمی‌کند.

---

### QueryHandlerBase

`QueryHandlerBase` پایه استاندارد برای query handlerها است.

ویژگی‌های اصلی:

* از `ApplicationHandlerBase` ارث می‌برد
* قرارداد `IQueryHandler<TQuery, TResponse>` را پیاده می‌کند
* به read repository وابسته است
* entity را با `Entity<TId>` constrain می‌کند

این کلاس concern مربوط به read side را در همان خانواده query نگه می‌دارد و class اضافی جداگانه برای repository-aware شدن تولید نمی‌کند.

---

## ساختار نهایی capability

```text
ApplicationHandlerBase
  ├─ CommandHandlerBase<TCommand, TRepository, TAggregate, TId>
  ├─ CommandHandlerBase<TCommand, TResponse, TRepository, TAggregate, TId>
  └─ QueryHandlerBase<TQuery, TResponse, TRepository, TEntity, TId>
```

---

## تصمیم‌های طراحی کلیدی

### 1. پروژه Abstractions جدا نداریم

برای `HandlerExecution` پروژه `Abstractions` جدا ساخته نشد، چون:

* abstractionهای لازم از قبل در BuildingBlockها وجود دارند
* این capability قرار نیست primitive جدید تعریف کند
* هدف آن convenience layer و execution pattern است

---

### 2. repository-aware base class جدا نداریم

در این capability عمداً classهای جدا با نام‌های طولانی مثل این‌ها ساخته نشدند:

* `ReadRepositoryQueryHandlerBase`
* `WriteRepositoryCommandHandlerBase`

به‌جای آن:

* concern مربوط به write repository و `IUnitOfWork` داخل `CommandHandlerBase` قرار گرفت
* concern مربوط به read repository داخل `QueryHandlerBase` قرار گرفت

علت این تصمیم:

* کاهش شلوغی API
* کاهش تعداد base classها
* ساده‌تر شدن تصمیم‌گیری برای مصرف‌کننده capability

---

### 3. ResultContext حداقلی است

`ResultContext` در نسخه فعلی فقط error accumulation انجام می‌دهد.

عمداً این موارد به آن اضافه نشده‌اند:

* metadata dictionary
* warning/info channel مستقل
* object bag آزاد
* execution status model جدا
* custom state pipeline

علت این تصمیم این است که ساختار `Result` فعلی پروژه ساده است و `HandlerExecution` باید با همان سادگی هم‌راستا بماند.

---

### 4. ILoggerFactory به‌جای ILogger

در `HandlerServices` از `ILoggerFactory` استفاده شده است، نه `ILogger` خام.

علت:

* logger باید متناسب با type واقعی handler ساخته شود
* ساخت logger باید lazy باشد
* `ApplicationHandlerBase` بتواند logger typed خودش را بسازد

---

### 5. dependence on repository contract, not service locator

repositoryها به‌صورت explicit و constructor-based وارد base classها می‌شوند.
هیچ service locator یا lookup runtime برای پیدا کردن repository استفاده نشده است.

---

## نمونه استفاده

### command handler

```csharp
public sealed class CreateOrderHandler
    : CommandHandlerBase<CreateOrderCommand, IOrderWriteRepository, Order, long>
{
    public CreateOrderHandler(
        IOrderWriteRepository repository,
        IUnitOfWork unitOfWork,
        HandlerServices services)
        : base(repository, unitOfWork, services)
    {
    }

    public override async Task<Result> Handle(
        CreateOrderCommand command,
        CancellationToken cancellationToken = default)
    {
        var order = new Order();

        await Repository.AddAsync(order, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);

        return Success();
    }
}
```

### command handler با response

```csharp
public sealed class CreateOrderHandler
    : CommandHandlerBase<CreateOrderCommand, long, IOrderWriteRepository, Order, long>
{
    public CreateOrderHandler(
        IOrderWriteRepository repository,
        IUnitOfWork unitOfWork,
        HandlerServices services)
        : base(repository, unitOfWork, services)
    {
    }

    public override async Task<Result<long>> Handle(
        CreateOrderCommand command,
        CancellationToken cancellationToken = default)
    {
        var order = new Order();

        await Repository.AddAsync(order, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);

        return Success(order.Id);
    }
}
```

### query handler

```csharp
public sealed class GetOrderByIdHandler
    : QueryHandlerBase<GetOrderByIdQuery, OrderDto, IOrderReadRepository, OrderReadModel, long>
{
    public GetOrderByIdHandler(
        IOrderReadRepository repository,
        HandlerServices services)
        : base(repository, services)
    {
    }

    public override async Task<Result<OrderDto>> Handle(
        GetOrderByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var entity = await Repository.GetByIdAsync(query.OrderId, cancellationToken);

        if (entity is null)
            return NotFound<OrderDto>("order.not-found", "سفارش مورد نظر پیدا نشد.");

        var dto = Mapper.Map<OrderReadModel, OrderDto>(entity);

        return Success(dto);
    }
}
```

---

## تست‌ها

در نسخه فعلی برای این capability تست‌های پایه زیر پیاده‌سازی شده‌اند:

* `ResultContextTests`
* `ApplicationHandlerBaseTests`
* `CommandHandlerBaseTests`
* `QueryHandlerBaseTests`

همچنین مجموعه‌ای از test doubleها برای mapper، serializer، translator، current user، repositoryها و unit of work در پروژه تست ساخته شده‌اند.

---

## خارج از scope فعلی

در این نسخه موارد زیر عمداً خارج از scope نگه داشته شده‌اند:

* registration extension برای DI
* mediator abstraction جدید
* pipeline behavior framework
* property bag آزاد در execution context
* status model پیچیده برای handler execution
* sample application رسمی
* policy-based execution orchestration

---

## جمع‌بندی

`HandlerExecution` یک application-level reusable pattern در ZaminX است که با تکیه بر BuildingBlockهای موجود، پیاده‌سازی handlerها را ساده‌تر، یکدست‌تر و کم‌تکرارتر می‌کند.

این capability به‌جای ساخت abstractionهای جدید غیرضروری، روی primitiveها و contractهای موجود پروژه سوار می‌شود و یک execution model روشن برای command handlerها و query handlerها ارائه می‌دهد.