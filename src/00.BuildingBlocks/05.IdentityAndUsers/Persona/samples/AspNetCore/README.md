# Persona.AspNetCore Sample

این sample برای نمایش استفاده واقعی از Persona در یک application وبی ساده طراحی شده است.

هدف sample این نیست که یک سیستم احراز هویت کامل ارائه دهد. هدف آن فقط validate کردن این capability در یک flow ساده و قابل‌فهم است.

---

## هدف sample

این sample این سناریوها را نشان می‌دهد:

* registration Persona در ASP.NET Core
* دسترسی به `ICurrentUser`
* دسترسی به `IWebCurrentUser`
* خواندن claimها
* مشاهده رفتار در حالت anonymous
* مشاهده رفتار در حالت authenticated

---

## مسیرهای اصلی

### `/`

این endpoint اطلاعات current user را برمی‌گرداند، شامل:

* `IsAuthenticated`
* `UserId`
* `UserName`
* `FirstName`
* `LastName`
* `IpAddress`
* `UserAgent`

---

### `/claims/{claimType}`

این endpoint مقدار یک claim و لیست تمام مقادیر همان claim type را برمی‌گرداند.

برای بررسی رفتار `GetClaim` و `GetClaims` استفاده می‌شود.

---

### `/login`

این endpoint یک sign-in ساده نمونه‌ای انجام می‌دهد و چند claim تستی روی user قرار می‌دهد.

این فقط برای validate کردن sample است و نباید به‌عنوان implementation نهایی authentication در نظر گرفته شود.

---

### `/logout`

این endpoint sign-out را انجام می‌دهد و نمونه را به حالت anonymous برمی‌گرداند.

---

## نحوه اجرا

نمونه:

```bash
dotnet run --project ./samples/ZaminX.Samples.IdentityAndUsers.Persona.AspNetCore/ZaminX.Samples.IdentityAndUsers.Persona.AspNetCore.csproj
```

بعد از اجرا می‌توان این flow را تست کرد:

1. درخواست به `/`
2. درخواست به `/login`
3. درخواست دوباره به `/`
4. درخواست به `/claims/department`
5. درخواست به `/logout`

---

## چرا این sample minimal نگه داشته شده است

این sample عمداً minimal است تا:

* focus روی خود Persona بماند
* پیچیدگی‌های unrelated وارد نشوند
* debugging و validate کردن capability ساده باشد
* sample به showcase یک سیستم identity کامل تبدیل نشود

---

## جمع‌بندی

این sample یک reference سبک برای validation و یادگیری Persona است و نشان می‌دهد capability در سناریوی واقعی وب چگونه مصرف می‌شود.
