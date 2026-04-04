# Translator (Parrot)

## نمای کلی

Translator در زمین X یک capability قابل بازاستفاده در خانواده CrossCutting است که مسئول ترجمه و تبدیل کلیدهای معنایی به متن نهایی است.

این capability با نام محصولی Parrot ارائه می‌شود.

Parrot این امکان را فراهم می‌کند که:

- متن‌ها به‌صورت key-based تعریف شوند
- ترجمه‌ها از sourceهای مختلف تأمین شوند
- رفتار fallback به‌صورت استاندارد اعمال شود
- formatting و ترکیب متن‌ها به‌صورت ساده انجام شود
- بدون restart، داده‌های ترجمه به‌روزرسانی شوند

---

## مسئله‌ای که حل می‌کند

در بسیاری از سیستم‌ها:

- متن‌ها به‌صورت hard-coded نوشته می‌شوند
- ترجمه‌ها به‌سختی مدیریت می‌شوند
- تغییر ترجمه نیاز به deploy دارد
- چند source داده به‌سختی ترکیب می‌شوند

Parrot این مشکلات را حل می‌کند و یک لایه انتزاعی برای ترجمه فراهم می‌کند که:

- مستقل از storage است
- قابل توسعه است
- و در runtime قابل تغییر است

---

## مفاهیم اصلی

### کلید ترجمه

هر متن با یک key شناخته می‌شود:

Common.Hello  
User.DisplayName  
Message.Greeting  

---

### Culture

Parrot از culture پشتیبانی می‌کند:

- en-US
- fa-IR
- یا culture خالی (default)

---

### رفتار fallback

اگر ترجمه‌ای پیدا نشود:

1. ابتدا culture کامل بررسی می‌شود
2. سپس parent culture
3. در نهایت culture عمومی (بدون culture)
4. اگر همچنان پیدا نشد، خود key برگردانده می‌شود

---

## API مصرفی

مصرف‌کننده فقط با ITranslator کار می‌کند.

### دریافت ساده

translator["Common.Hello"]  
translator.GetString("Common.Hello")  

---

### با culture

translator.GetString(new CultureInfo("fa-IR"), "Common.Hello")

---

### با آرگومان‌های ترجمه‌شونده

translator.GetString(
    "Message.Greeting",
    "User.DisplayName")

در این حالت:

- آرگومان‌ها ابتدا ترجمه می‌شوند
- سپس در متن قرار می‌گیرند

---

### با آرگومان‌های خام

translator.GetFormattedString(
    "Message.Price",
    1250.75,
    "IRR",
    DateTime.UtcNow)

در این حالت:

- آرگومان‌ها مستقیم استفاده می‌شوند
- ترجمه نمی‌شوند

---

### ترکیب چند کلید

translator.GetConcatString('-', "Common.Hello", "Common.World")

---

## مدل provider

Parrot از مدل provider-based استفاده می‌کند.

- مصرف‌کننده فقط به ITranslator وابسته است
- داده‌ها توسط providerها تأمین می‌شوند
- چند provider می‌توانند هم‌زمان فعال باشند

---

### ترتیب providerها

- providerها به ترتیب registration اجرا می‌شوند
- providerهای بعدی می‌توانند داده‌های قبلی را override کنند

---

## کش و به‌روزرسانی

- داده‌ها در حافظه نگه‌داری می‌شوند
- دسترسی به ترجمه‌ها بسیار سریع است
- امکان refresh بدون restart وجود دارد

providerها می‌توانند:

- به‌صورت دوره‌ای refresh انجام دهند
- یا در آینده مبتنی بر تغییر باشند

---

## ثبت کلیدهای جاافتاده

Parrot به‌صورت اختیاری از ثبت کلیدهای جاافتاده پشتیبانی می‌کند.

اگر کلیدی پیدا نشود:

- می‌تواند در source داده ثبت شود
- مقدار اولیه برای آن تعیین شود

این رفتار:

- اختیاری است
- وابسته به provider است
- در API مصرفی دیده نمی‌شود

---

## ساختار پروژه

این capability شامل این پروژه‌هاست:

- Abstractions → قراردادها
- Parrot → پیاده‌سازی اصلی
- SqlServer → provider نمونه
- Sample → مثال استفاده

---

## نحوه استفاده

### ثبت در DI

builder.Services.AddParrot(parrot =>
{
    parrot.UseSqlServer(options =>
    {
        options.ConnectionString = "...";
        options.EnsureTableCreated = true;
    });
});

---

### استفاده در کد

public class MyService
{
    private readonly ITranslator _translator;

    public MyService(ITranslator translator)
    {
        _translator = translator;
    }

    public string GetMessage()
    {
        return _translator["Common.Hello"];
    }
}

---

## نکات طراحی

- API مصرفی ساده نگه داشته شده است
- dependency به ابزارهای بیرونی نشت نمی‌کند
- providerها مستقل هستند
- logging در سطح capability انجام نمی‌شود
- خطاها از طریق exception مدیریت می‌شوند

---

## جمع‌بندی

Parrot یک capability سبک، سریع و قابل توسعه برای ترجمه در زمین X است که:

- abstraction مناسب ارائه می‌دهد
- از چند source پشتیبانی می‌کند
- runtime-friendly است
- و developer experience خوبی فراهم می‌کند