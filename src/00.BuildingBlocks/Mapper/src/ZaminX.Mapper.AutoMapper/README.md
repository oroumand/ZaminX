<div dir="rtl">

# ZaminX.Mapper.AutoMapper

پیاده‌سازی Mapper بر پایه AutoMapper برای ZaminX.

## هدف

این پکیج، `IMapperAdapter` را با استفاده از AutoMapper پیاده‌سازی می‌کند.

## نصب

```bash
dotnet add package ZaminX.Mapper.AutoMapper
```

## استفاده

```csharp
builder.Services.AddZaminXAutoMapperAdapter();
```

## تنظیمات پیشرفته

```csharp
builder.Services.AddZaminXAutoMapperAdapter(options =>
{
    options.Assemblies.Add(typeof(Program).Assembly);
});
```

## وابستگی

این پکیج به `ZaminX.Mapper.Abstractions` وابسته است.

</div>
