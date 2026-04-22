# ZaminX.ApplicationPatterns.HandlerExecution

`HandlerExecution` اولین capability از دسته `ApplicationPatterns` در ZaminX است.

این capability یک الگوی reusable برای استانداردسازی پیاده‌سازی handlerها در سطح application ارائه می‌دهد و هدف آن کاهش boilerplate، افزایش یکدستی و ساده‌تر کردن تجربه توسعه‌دهنده در نوشتن command handler و query handler است.

---

## هدف

در پروژه‌های application محور، مخصوصاً زمانی که از mediator و الگوی command/query استفاده می‌شود، معمولاً در handlerها مجموعه‌ای از نیازهای تکراری وجود دارد:

* دسترسی به dependencyهای مشترک مانند mapper، serializer، translator و current user
* ساخت `Result` و `Result<T>`
* مدیریت خطاهای جمع‌شونده در طول execution
* دسترسی به repositoryهای read و write
* دسترسی به `IUnitOfWork` در commandها
* یکدست‌سازی امضای handlerها و کاهش کد تکراری

`HandlerExecution` برای همین مسئله طراحی شده است.

این capability قرار نیست abstraction جدیدی برای mediator، pipeline یا repositoryها بسازد.
همچنین قرار نیست جای primitiveهای موجود در BuildingBlockها را بگیرد.

نقش آن این است که روی capabilityهای موجود سوار شود و یک execution model ساده، روشن و قابل استفاده برای handlerها فراهم کند.

---

## جایگاه در taxonomy

`HandlerExecution` یک **Application Pattern** است و در مسیر زیر قرار می‌گیرد:

```text
src/
  02.ApplicationPatterns/
    HandlerExecution/
```

این capability:

* BuildingBlock نیست
* Foundation نیست
* Integration نیست

بلکه یک الگوی reusable در سطح application است که از BuildingBlockهای موجود استفاده می‌کند.

---

## ساختار سلوشن

```text
src/
  02.ApplicationPatterns/
    HandlerExecution/
      HandlerExecution.slnx
      src/
        ZaminX.ApplicationPatterns.HandlerExecution/
      tests/
        ZaminX.ApplicationPatterns.HandlerExecution.Tests/
```

---

## اجزای capability

نسخه فعلی `HandlerExecution` شامل اجزای زیر است:

### 1. `HandlerServices`

یک dependency bundle تایپ‌شده برای dependencyهای پرتکرار handlerها.

در نسخه فعلی شامل این موارد است:

* `IMapperAdapter`
* `IJsonSerializer`
* `ITranslator`
* `ILoggerFactory`
* `ICurrentUser?`

این کلاس:

* service locator نیست
* dependencyها را explicit نگه می‌دارد
* فقط برای ساده‌سازی constructorهای handler base استفاده می‌شود

### 2. `ResultContext`

یک context کوچک و کنترل‌شده برای جمع‌آوری خطاهای execution همان handler.

ویژگی‌ها:

* lazy ساخته می‌شود
* bag آزاد از objectها نیست
* فقط خطاها را نگه می‌دارد
* می‌تواند به `Result` و `Result<T>` تبدیل شود

### 3. `ApplicationHandlerBase`

کلاس پایه مشترک برای concernهای عمومی handlerها.

مسئولیت‌های آن:

* دسترسی به `Mapper`
* دسترسی به `Serializer`
* دسترسی به `Translator`
* دسترسی به `Logger`
* دسترسی به `CurrentUser`
* نگه‌داری lazy از `ResultContext`
* ارائه helperهای ergonomic برای ساخت `Result`

خود این کلاس handler نهایی نیست و قراردادهای Relay را مستقیم پیاده نمی‌کند.

### 4. `CommandHandlerBase`

کلاس پایه برای command handlerها.

ویژگی‌ها:

* از `ApplicationHandlerBase` ارث می‌برد
* `ICommandHandler<TCommand>` یا `ICommandHandler<TCommand, TResponse>` را پیاده می‌کند
* به repository write وابسته است
* به `IUnitOfWork` وابسته است
* از `AggregateRoot<TId>` برای constrain کردن aggregate استفاده می‌کند

### 5. `QueryHandlerBase`

کلاس پایه برای query handlerها.

ویژگی‌ها:

* از `ApplicationHandlerBase` ارث می‌برد
* `IQueryHandler<TQuery, TResponse>` را پیاده می‌کند
* به repository read وابسته است
* از `Entity<TId>` برای constrain کردن entity استفاده می‌کند

---

## وابستگی‌ها

`HandlerExecution` روی capabilityهای موجود در ZaminX سوار می‌شود و abstraction جدید غیرضروری اضافه نمی‌کند.

وابستگی‌های اصلی:

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

## تصمیم‌های طراحی مهم

### پروژه `Abstractions` جدا نداریم

برای این capability پروژه abstraction جدا نساختیم، چون:

* abstractionهای لازم از قبل در BuildingBlockها وجود دارند
* این capability قرار نیست primitive جدید تعریف کند
* هدف آن convenience و execution pattern است، نه تعریف contract جدید

### repository-aware class جدا نداریم

برای read و write، کلاس‌های جداگانه با نام‌های طولانی مثل این‌ها ایجاد نشدند:

* `ReadRepositoryQueryHandlerBase`
* `WriteRepositoryCommandHandlerBase`

به‌جای آن:

* `CommandHandlerBase` خودش concern مربوط به write repository و `IUnitOfWork` را نگه می‌دارد
* `QueryHandlerBase` خودش concern مربوط به read repository را نگه می‌دارد

این تصمیم برای کاهش شلوغی API و ساده‌تر شدن مصرف capability گرفته شد.

### `ResultContext` حداقلی و کنترل‌شده است

در نسخه فعلی `ResultContext` فقط error accumulation انجام می‌دهد.

عمداً این موارد به آن اضافه نشده‌اند:

* property bag آزاد
* object storage دلخواه
* warning/info channel مستقل
* metadata dictionary
* status model جدا از `Result`

علت این تصمیم این است که ساختار `Result` فعلی پروژه ساده و مینیمال است و `HandlerExecution` باید با همان هم‌راستا بماند.

### `ILoggerFactory` به‌جای `ILogger`

در `HandlerServices` از `ILoggerFactory` استفاده شده است تا هر handler base بتواند logger متناسب با type واقعی خودش را به‌صورت lazy بسازد.

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

برای این capability تست‌های پایه زیر نوشته شده‌اند:

* `ResultContextTests`
* `ApplicationHandlerBaseTests`
* `CommandHandlerBaseTests`
* `QueryHandlerBaseTests`

همچنین برای جلوگیری از وابستگی به implementationهای واقعی، مجموعه‌ای از test doubleها در پروژه تست تعریف شده است.

---

## خارج از scope فعلی

در نسخه فعلی این موارد عمداً خارج از scope نگه داشته شده‌اند:

* pipeline abstraction جدید
* mediator abstraction جدید
* registration extension برای DI
* behavior model مستقل
* property bag آزاد برای context
* مدل چندمرحله‌ای execution state
* sample application رسمی

---

## جمع‌بندی

`HandlerExecution` یک لایه convenience در سطح application است که بدون ساخت abstraction غیرضروری، پیاده‌سازی handlerها را یکدست‌تر، ساده‌تر و قابل استفاده‌تر می‌کند.

این capability به‌جای ساخت primitive جدید، روی BuildingBlockهای موجود ZaminX سوار می‌شود و یک مدل اجرایی روشن برای command handlerها و query handlerها ارائه می‌دهد.

بعدی را هم به همین شکل خام می‌دهم: سند docs capability.
