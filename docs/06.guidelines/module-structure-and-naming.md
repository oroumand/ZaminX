## قواعد ساختار داخلی پروژه‌ها

در capabilityهای زمین X، برای حفظ یکدستی ساختار فنی، این قواعد رعایت می‌شوند:

* کلاس‌های options و تنظیمات در فولدر `Configurations` قرار می‌گیرند
* کلاس‌های پیاده‌سازی در فولدر `Services` قرار می‌گیرند
* extensionهای registration در فولدر `Extensions` قرار می‌گیرند
* فایل `ServiceCollectionExtensions.cs` باید در namespace
  `Microsoft.Extensions.DependencyInjection`
  قرار بگیرد تا متدهای registration با الگوی طبیعی مایکروسافتی در دسترس باشند
* naming فنی پروژه‌ها و namespaceها باید با taxonomy رسمی زمین X هم‌راستا بماند
