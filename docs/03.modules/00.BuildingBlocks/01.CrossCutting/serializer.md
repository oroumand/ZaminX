# Serializer

## نام محصولی

Prism

## نام فنی

Serializer

---

## نمای کلی

Serializer یکی از capabilityهای خانواده قابلیت‌های عمومی و بین‌برشی در زمین X است.

این capability یک مرز یکدست برای serialization و deserialization داده فراهم می‌کند تا:

* مصرف‌کننده به ابزار بیرونی وابسته نشود
* تصمیم انتخاب provider در یک نقطه متمرکز بماند
* شیوه استفاده از serializer در سطح سیستم یکدست شود
* تغییر provider یا تغییر رفتار فنی در سراسر پروژه پخش نشود

در سطح محصول، این capability با نام Prism شناخته می‌شود.

---

## دلیل انتخاب نام Prism

نام Prism به چیزی اشاره می‌کند که یک ورودی را به نمایی روشن، ساخت‌یافته و قابل تفسیر تبدیل می‌کند.

این تصویر با نقش این capability تناسب مفهومی دارد:

* داده درون‌برنامه‌ای به یک نمایش قابل انتقال تبدیل می‌شود
* نمایش متنی دوباره به یک ساختار قابل استفاده در برنامه بازگردانده می‌شود
* capability نقش یک مرز روشن و کنترل‌شده برای این تبدیل را ایفا می‌کند

این نام در سطح محصول:

* تصویری است
* متمایز است
* با روایت capability سازگار است

در عین حال، naming فنی capability در پروژه‌ها و namespaceها همچنان Serializer باقی می‌ماند تا با taxonomy رسمی زمین X هم‌راستا باشد.

---

## مسئله‌ای که این capability حل می‌کند

در پروژه‌های واقعی، serialization و deserialization در نقاط متعددی استفاده می‌شوند:

* endpointها
* integrationها
* caching
* message bus
* eventing
* سناریوهای ذخیره‌سازی یا تبادل داده

اگر هر مصرف‌کننده مستقیماً از یک library مانند System.Text.Json یا Newtonsoft.Json استفاده کند، معمولاً این مشکلات ایجاد می‌شود:

* وابستگی مستقیم به ابزار بیرونی در سراسر سیستم پخش می‌شود
* تغییر provider یا تغییر API ابزار بیرونی سخت می‌شود
* تنظیمات و رفتارهای مشترک در چند نقطه تکرار می‌شوند
* مرز capability از بین می‌رود
* مصرف‌کننده به‌جای مسئله، با جزئیات ابزار درگیر می‌شود

Serializer این مسئله را با تعریف یک قرارداد مصرفی مینیمال و providerهای جایگزین حل می‌کند.

---

## جایگاه در taxonomy

این capability در دسته BuildingBlocks و در خانواده 01.CrossCutting قرار می‌گیرد.

دلیل این جایگاه:

* مسئله آن عمومی و بین‌برشی است
* به یک دامنه خاص وابسته نیست
* به‌صورت مستقل قابل مصرف است
* در پروژه‌های مختلف قابلیت بازاستفاده دارد

---

## دامنه فعلی capability

در فاز فعلی، این capability فقط روی JSON متمرکز است.

این تصمیم به این دلیل گرفته شده است که:

* providerهای فعلی همگی JSON-based هستند
* مسئله فعلی پروژه روی serialization عمومی داده در قالب JSON متمرکز است
* تعمیم زودهنگام capability به همه فرمت‌ها فعلاً ارزش کافی ندارد
* مینیمال ماندن قرارداد از عمومی‌سازی زودتر از نیاز مهم‌تر است

---

## نیاز به مرز انتزاعی

برای این capability، مرز انتزاعی لازم است.

دلیل:

* بیش از یک provider واقعی برای آن وجود دارد
* نشت API ابزار بیرونی باید کنترل شود
* رفتار serialization باید از دید مصرف‌کننده یکدست بماند
* registration و تنظیمات provider باید در مرز capability محصور شوند

در نتیجه، Serializer از الگوی «انتزاع + provider» پیروی می‌کند.

---

## قرارداد اصلی capability

قرارداد مصرفی این capability باید کوچک، روشن و provider-agnostic بماند.

شکل مطلوب قرارداد:

public interface IJsonSerializer
{
string Serialize<T>(T? value);
T? Deserialize<T>(string json);
object? Deserialize(string json, Type type);
}

این قرارداد فقط نیازهای عمومی و پرتکرار را پوشش می‌دهد:

* تبدیل object به JSON
* تبدیل JSON به object به‌صورت generic
* تبدیل JSON به object بر اساس Type

---

## چیزهایی که فعلاً وارد قرارداد نمی‌شوند

این موارد فعلاً وارد قرارداد مصرفی capability نمی‌شوند:

* overloadهای مبتنی بر options
* stream-based API
* متدهای async
* pretty print یا formatting-specific overload
* typeهای library-specific
* APIهای DOM مانند JsonDocument یا JToken
* ثبت converter در سطح قرارداد
* naming policy و contract resolver در سطح مصرف‌کننده
* تنظیمات provider در متدهای serialize و deserialize

---

## providerهای فعلی

در فاز فعلی، این capability با دو provider طراحی می‌شود:

### provider مایکروسافتی

این provider بر پایه System.Text.Json ساخته می‌شود.

### provider نیوتن‌سافتی

این provider بر پایه Newtonsoft.Json ساخته می‌شود.

---

## registration

registration هر provider در خود همان provider نگه داشته می‌شود.

نمونه:

services.AddMicrosoftJsonSerializer();

services.AddMicrosoftJsonSerializer(options =>
{
options.PropertyNameCaseInsensitive = true;
});

services.AddNewtonsoftJsonSerializer();

services.AddNewtonsoftJsonSerializer(options =>
{
options.UseCamelCase = true;
});

---

## policy مربوط به options

در این capability، options provider فقط در registration مجازند.

این یعنی:

* JsonSerializerOptions
* JsonSerializerSettings

نباید وارد قرارداد مصرفی capability شوند.

---

## policy مربوط به exception

providerهای این capability باید exceptionهای ابزار بیرونی را جذب کنند و آن‌ها را به exception capability-level و یکدست تبدیل کنند.

این exception باید حداقل این اطلاعات را داشته باشد:

* نوع عملیات
* نوع هدف
* نام provider
* inner exception

---

## policy مربوط به logging و diagnostics

provider اصلی این capability نباید برای هر فراخوانی serialize یا deserialize لاگ روتین تولید کند.

در این capability:

* payload خام نباید در logging عمومی ثبت شود
* debug اصلی باید در caller و boundary انجام شود
* در صورت نیاز به troubleshooting عمیق، می‌توان از decorator تشخیصی استفاده کرد

---

## sample

برای این capability، وجود یک sample کوچک مفید است.

---

## naming فنی مورد انتظار

* ZaminX.BuildingBlocks.CrossCutting.Serializer.Abstractions
* ZaminX.BuildingBlocks.CrossCutting.Serializer.Microsoft
* ZaminX.BuildingBlocks.CrossCutting.Serializer.Newtonsoft

نام solution این capability می‌تواند Prism باشد.

---

## ارتباط با سایر اسناد

* docs/03.modules/00.BuildingBlocks/01.CrossCutting/index.md
* docs/04.decision-records/adr/adr-001-cross-cutting-capabilities-provider-model.md
* docs/04.decision-records/adr/adr-006-product-naming-vs-technical-naming.md
* docs/04.decision-records/adr/adr-009-provider-options-diagnostics-and-exception-boundary.md