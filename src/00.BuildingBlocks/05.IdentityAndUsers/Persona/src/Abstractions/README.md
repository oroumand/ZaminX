# Persona.Abstractions

این پروژه قراردادهای پایه Persona را نگه می‌دارد.

هدف آن این است که مصرف‌کننده بتواند بدون وابستگی مستقیم به ASP.NET Core یا جزئیات host، به اطلاعات کاربر جاری دسترسی داشته باشد.

---

## مسئولیت‌ها

این پروژه مسئول این موارد است:

* تعریف `ICurrentUser`
* تعریف `IWebCurrentUser`
* نگه‌داری API مصرفی Persona
* حفظ مرز abstraction از implementation

---

## قراردادها

### ICurrentUser

قرارداد پایه برای دسترسی به اطلاعات هویتی کاربر جاری.

اعضای اصلی:

* `UserId`
* `UserName`
* `FirstName`
* `LastName`
* `IsAuthenticated`
* `GetClaim(string claimType)`
* `GetClaims(string claimType)`

---

### IWebCurrentUser

قرارداد وبی Persona که از `ICurrentUser` ارث می‌برد و اطلاعات زیر را اضافه می‌کند:

* `IpAddress`
* `UserAgent`

---

## اصل طراحی

این پروژه نباید:

* به ASP.NET Core وابسته باشد
* implementation در خود داشته باشد
* concernهای runtime یا host-specific را در surface API پخش کند

هدف این پروژه فقط تعریف مرز مصرفی روشن برای current user access است.

---

## چه چیزهایی در این پروژه نیست

این پروژه عمداً شامل این موارد نیست:

* پیاده‌سازی مبتنی بر `HttpContext`
* registration
* options
* helperهای ASP.NET Core
* login / logout
* authorization concernها
* profile concernها

این موارد در پروژه implementation قرار می‌گیرند یا اساساً خارج از scope Persona هستند.

---

## نقش در معماری

`Persona.Abstractions` مرز stable و قابل‌ارجاع Persona است.

هر بخشی که فقط به current user access نیاز دارد باید تا حد ممکن به این پروژه وابسته شود، نه به implementation وبی.

این تصمیم باعث می‌شود:

* coupling کاهش یابد
* تست‌پذیری بهتر شود
* مرز capability روشن بماند
* host-specific concerns به مصرف‌کننده نشت نکنند

---

## جمع‌بندی

این پروژه کوچک اما کلیدی است، چون قرارداد اصلی Persona را تعریف می‌کند و مبنای استفاده یکدست از current user در سطح پروژه را فراهم می‌سازد.
