# Logging Sample

این پروژه نمونه استفاده از Logging در یک ASP.NET Core Web API است.

---

## قابلیت‌ها

* Console logging
* File logging
* Seq integration
* Contextual logging
* UserId / UserName
* Custom properties

---

## 🚀 اجرا

### 1. اجرای Seq با Docker

```bash
docker run --name seq -d ^
  -e ACCEPT_EULA=Y ^
  -e SEQ_FIRSTRUN_ADMINPASSWORD=1234 ^
  -p 5341:80 ^
  datalust/seq
```

---

### 2. اجرای پروژه

```bash
dotnet run
```

---

### 3. تست

مرورگر:

```text
http://localhost:xxxx/
```

---

### 4. مشاهده لاگ

```text
http://localhost:5341
```

username:

```text
admin
```

password:

```text
1234
```

---

## 🧪 سناریوها

### User

```csharp
context.SetUserIdFromClaims("sub");
```

---

### Custom

```csharp
context.Set("TenantId", ctx =>
    ctx.Request.Headers["X-Tenant-Id"]);
```

---

### Service-based

```csharp
context.SetUserId((ctx, sp) =>
{
    var svc = sp.GetRequiredService<IUserService>();
    return svc.UserId;
});
```

---

## 🎯 هدف

این sample برای:

* یادگیری
* تست سریع
* reference implementation

است.
