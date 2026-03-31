# ZaminX.BuildingBlocks.CrossCutting.Serializer.WebApiSample

## نمای کلی

این پروژه یک Web API sample برای capability `Serializer` است.

هدف این sample این است که نشان دهد:

* provider چگونه register می‌شود
* مصرف‌کننده فقط `IJsonSerializer` را می‌بیند
* قابلیت `Serializer` در یک سناریوی واقعی‌تر از console sample چگونه مصرف می‌شود

---

## هدف sample

این sample برای این طراحی شده است که:

* registration provider را در یک Web API نشان دهد
* serialization و deserialization را از طریق endpointهای ساده نمایش دهد
* ثابت کند که مصرف‌کننده به library بیرونی وابسته نمی‌شود

---

## نکته مهم

این پروژه برای نمایش استفاده از capability طراحی شده است، نه برای آموزش مستقیم `System.Text.Json` یا `Newtonsoft.Json`.

---

## endpointهای اصلی

### serialize

یک object ساده را می‌گیرد و JSON تولید می‌کند.

### deserialize

یک JSON را می‌گیرد و آن را به object تبدیل می‌کند.

### round-trip

یک object را serialize و سپس deserialize می‌کند تا رفتار کامل capability دیده شود.
