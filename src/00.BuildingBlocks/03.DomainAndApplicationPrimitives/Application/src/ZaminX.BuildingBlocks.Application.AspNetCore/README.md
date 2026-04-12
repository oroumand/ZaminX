# ZaminX.BuildingBlocks.Application.AspNetCore

## معرفی

این پروژه integration بین `ZaminX.BuildingBlocks.Application` و ASP.NET Core را فراهم می‌کند.

هدف این پروژه این است که concernهای مربوط به وب و HTTP را از core Application جدا نگه دارد و در عین حال استفاده از primitiveهای Application را در Web API ساده‌تر کند.

---

## مسئولیت‌ها

این پروژه مسئول این موارد است:

* تبدیل `Result` و `Result<T>` به `IResult`
* فراهم کردن integration مناسب برای Minimal API و ASP.NET Core
* نگه داشتن concernهای HTTP خارج از core Application

---

## چرا این پروژه جدا است

در ZaminX، core Application نباید به ASP.NET Core وابسته باشد.

به همین دلیل:

* `ZaminX.BuildingBlocks.Application`
  فقط primitiveهای لایه Application را نگه می‌دارد
* `ZaminX.BuildingBlocks.Application.AspNetCore`
  concernهای مربوط به وب را پیاده‌سازی می‌کند

این جداسازی intentional است و باعث می‌شود:

* core reusable بماند
* coupling کاهش پیدا کند
* integrationهای دیگر در آینده راحت‌تر اضافه شوند

---

## قابلیت فعلی

### ResultHttpMapper

این پروژه در نسخه فعلی شامل mapper مربوط به HTTP result است.

هدف آن:

* تبدیل `Result` به `IResult`
* تبدیل `Result<T>` به `IResult`

---

## نمونه استفاده

### برای Result بدون data

```csharp
var result = await mediator.Send(command, cancellationToken);
return result.ToHttpResult();
```

### برای Result دارای data

```csharp
var result = await mediator.Send(query, cancellationToken);
return result.ToHttpResult();
```

---

## رفتار فعلی mapping

### Success

* `Result` موفق → `200 OK`
* `Result<T>` موفق → `200 OK`

### Failure

* `Result` ناموفق → `400 BadRequest`
* `Result<T>` ناموفق → `400 BadRequest`

---

## شکل خروجی

### Success بدون data

```json
{
  "success": true
}
```

### Success با data

```json
{
  "success": true,
  "data": {}
}
```

### Failure

```json
{
  "success": false,
  "errors": [
    {
      "code": "some.error",
      "message": "Some error message."
    }
  ]
}
```

---

## نحوه اضافه کردن reference

در پروژه وب:

```xml
<ProjectReference Include="...\ZaminX.BuildingBlocks.Application.AspNetCore.csproj" />
```

---

## نحوه استفاده در Minimal API

```csharp
app.MapPost("/orders", async (
    CreateOrderCommand command,
    IMediator mediator,
    CancellationToken cancellationToken) =>
{
    var result = await mediator.Send(command, cancellationToken);
    return result.ToHttpResult();
});
```

```csharp
app.MapGet("/orders/{id:guid}", async (
    Guid id,
    IMediator mediator,
    CancellationToken cancellationToken) =>
{
    var result = await mediator.Send(new GetOrderByIdQuery(id), cancellationToken);
    return result.ToHttpResult();
});
```

---

## نسبت با Application Core

این پروژه از:

* `ZaminX.BuildingBlocks.Application`

استفاده می‌کند، اما برعکس آن برقرار نیست.

یعنی:

* AspNetCore integration به Application وابسته است
* Application به AspNetCore integration وابسته نیست

---

## مناسب برای چه سناریوهایی است

این پروژه برای این سناریوها مناسب است:

* ASP.NET Core Web API
* Minimal API
* endpointهایی که از `IMediator` استفاده می‌کنند
* سیستم‌هایی که response آن‌ها بر مبنای `Result` است

---

## Non-goals

این پروژه در نسخه فعلی این هدف‌ها را ندارد:

* exception handling middleware
* problem details mapping
* status code policyهای پیشرفته
* integration با MVC filters
* OpenAPI response conventions
* endpoint base classes

این موارد در صورت نیاز می‌توانند در integrationهای بعدی اضافه شوند.

---

## جمع‌بندی

`ZaminX.BuildingBlocks.Application.AspNetCore` یک integration سبک و مشخص برای استفاده از primitiveهای Application در ASP.NET Core فراهم می‌کند.

این پروژه کمک می‌کند که:

* endpointها ساده‌تر شوند
* mapping `Result` به HTTP یکدست شود
* concernهای وب از core Application جدا بمانند
