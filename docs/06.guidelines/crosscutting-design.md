# راهنمای طراحی capabilityهای خانواده CrossCutting

## هدف این سند

این سند راهنمای رسمی طراحی capabilityهای خانواده `01.CrossCutting` در زمین X است.

هدف این سند:

- استخراج الگوی طراحی مشترک از capabilityهای مرجع این خانواده
- ایجاد یک استاندارد رسمی برای capabilityهای بعدی
- کاهش drift بین ماژول‌های CrossCutting
- روشن کردن مرز بین تصمیم‌های تثبیت‌شده و observationهای موردی

این سند مرجع طراحی capabilityهای جدید در خانواده CrossCutting است، نه جایگزین docs اختصاصی هر capability.

---

## دامنه سند

این سند برای capabilityهایی نوشته شده است که در مسیر زیر قرار می‌گیرند:

`docs/03.modules/00.BuildingBlocks/01.CrossCutting`

این سند درباره این موضوع‌ها تصمیم‌گیری می‌کند:

- معیار ورود به خانواده CrossCutting
- الگوهای مجاز طراحی capability
- قواعد abstraction و provider
- قواعد registration، options، setup، diagnostics و exception
- قواعد naming، namespace، solution structure و project structure
- قواعد sample، README، docs و ADR

این سند وارد جزئیات local هر capability نمی‌شود.

---

## تعریف خانواده CrossCutting

capabilityهای خانواده CrossCutting قابلیت‌هایی هستند که:

- مسئله‌ای عمومی و بین‌برشی را حل می‌کنند
- به دامنه خاص وابسته نیستند
- به لایه خاص وابسته نیستند
- در چند پروژه و سناریو قابل بازاستفاده‌اند
- می‌توانند مستقل از کل سیستم مصرف شوند
- الگوی flow سطح application ارائه نمی‌کنند
- مسئله اصلی آن‌ها اتصال به boundary بیرونی نیست

بنابراین CrossCutting در زمین X مجموعه‌ای از BuildingBlockهای عمومی و reusable است، نه مجموعه‌ای از integrationها یا application patternها.

---

## معیار ورود یک capability به CrossCutting

یک capability فقط زمانی باید در خانواده CrossCutting قرار بگیرد که همه یا بیشتر معیارهای زیر را داشته باشد:

- مستقل از دامنه خاص باشد
- مستقل از لایه خاص باشد
- reusable باشد
- standalone consumable باشد
- در چند نقطه از سیستم کاربرد داشته باشد
- مسئله اصلی آن مربوط به wiring یا startup نباشد
- مسئله اصلی آن مربوط به integration boundary نباشد

### خارج از این خانواده

در این حالت‌ها capability نباید در CrossCutting قرار بگیرد:

#### اگر مسئله اصلی startup، discovery یا registration است
باید در خانواده RuntimeAndRegistration بررسی شود.

#### اگر مسئله اصلی اتصال به سرویس یا سیستم بیرونی است
باید در Integrations بررسی شود.

#### اگر مسئله اصلی یک flow یا pattern سطح application است
باید در ApplicationPatterns بررسی شود.

---

## اصل پایه: abstraction پیش‌فرض نیست

در زمین X، وجود abstraction برای هر capability یک الزام پیش‌فرض نیست.

abstraction فقط زمانی تعریف می‌شود که نیاز واقعی به مرز capability وجود داشته باشد.

این اصل در خانواده CrossCutting نیز برقرار است.

بنابراین طراحی capability باید با این سؤال شروع شود:

آیا این capability واقعاً به یک مرز abstraction نیاز دارد؟

اگر پاسخ منفی است، ایجاد abstraction صرفاً برای تبعیت شکلی از الگو مجاز نیست.

---

## چه زمانی capability باید provider-based باشد

یک capability زمانی باید با مدل provider-based طراحی شود که یک یا چند مورد از شرایط زیر برقرار باشد:

- بیش از یک provider واقعی یا محتمل برای آن وجود داشته باشد
- احتمال تغییر provider در آینده وجود داشته باشد
- نشت API ابزار بیرونی به مصرف‌کننده خطر معماری ایجاد کند
- بخواهیم شیوه استفاده از ابزار بیرونی را در یک نقطه کنترل کنیم
- بخواهیم تغییرات provider یا library بیرونی در یک مرز جذب شوند
- بخواهیم مصرف‌کننده فقط با یک API پایدار داخلی کار کند

نمونه‌های مرجع این الگو در وضعیت فعلی:

- Object Mapper
- Serializer
- Translator

---

## چه زمانی capability نباید provider-based باشد

یک capability نباید فقط به‌صورت پیش‌فرض provider-based شود.

در این وضعیت‌ها provider-based کردن توصیه نمی‌شود:

- فقط یک implementation طبیعی و پایدار وجود دارد
- abstraction ارزش معماری واقعی ایجاد نمی‌کند
- .NET یا اکوسیستم، abstraction کافی و مناسب ارائه می‌دهد
- wrapper ایجادشده فقط یک لایه نازک و کم‌ارزش روی ابزار بیرونی خواهد بود
- هزینه پیچیدگی از ارزش معماری بیشتر است

---

## دو الگوی رسمی طراحی در CrossCutting

در وضعیت فعلی، دو الگوی رسمی برای capabilityهای provider-based در خانواده CrossCutting وجود دارد.

### الگوی اول: replacement-based provider capability

در این الگو:

- یک قرارداد مصرفی وجود دارد
- یک یا چند provider جایگزین‌پذیر وجود دارند
- هر provider همان قرارداد را پیاده‌سازی می‌کند
- registration در خود provider انجام می‌شود
- capability الزاماً core runtime مستقل ندارد

این الگو مناسب capabilityهایی است که:

- providerها نقش جایگزین یکدیگر را دارند
- orchestration پیچیده داخلی ندارند
- consumer فقط یک implementation فعال می‌خواهد

نمونه‌های فعلی:

- Object Mapper
- Serializer

---

### الگوی دوم: core-orchestrated provider capability

در این الگو:

- یک قرارداد مصرفی وجود دارد
- یک core capability وجود دارد
- providerها مستقیماً API مصرفی را پیاده‌سازی نمی‌کنند، بلکه به core سرویس می‌دهند
- registration اصلی در core انجام می‌شود
- providerها از طریق builder یا extension به core متصل می‌شوند
- capability ممکن است lifecycle، cache، refresh، coordination یا merge داشته باشد

این الگو مناسب capabilityهایی است که:

- چند provider می‌توانند هم‌زمان فعال باشند
- providerها نقش data source یا pluggable backend دارند
- core capability منطق اصلی مصرف، merge، fallback یا orchestration را نگه می‌دارد

نمونه فعلی:

- Translator (Parrot)

---

## قرارداد مصرفی و قرارداد درونی

### قرارداد مصرفی

قرارداد مصرفی، API اصلی capability برای consumer است.

این قرارداد باید:

- کوچک باشد
- روشن باشد
- provider-agnostic باشد
- typeهای بیرونی را نشت ندهد
- concernهای registration، diagnostics و options را وارد خود نکند

نمونه‌ها:

- `IMapperAdapter`
- `IJsonSerializer`
- `ITranslator`

---

### قرارداد درونی

قرارداد درونی فقط زمانی تعریف می‌شود که core capability یا providerها برای تعامل داخلی به آن نیاز داشته باشند.

قرارداد درونی زمانی مجاز و لازم است که:

- provider نقش data source یا pluggable backend داشته باشد
- core برای refresh، loading، registration داخلی یا coordination به قرارداد مستقل نیاز داشته باشد
- concern مورد نظر نباید در API مصرفی دیده شود، اما برای extensibility داخلی لازم باشد

نمونه‌های فعلی:

- `ITranslationDataProvider`
- `ITranslationRefreshService`
- `ITranslationMissingKeyRegistrar`
- `IParrotBuilder`

---

### قاعده رسمی

در خانواده CrossCutting:

- قرارداد مصرفی برای همه capabilityهای abstraction-based الزامی است
- قرارداد درونی فقط در صورت وجود نیاز واقعی به orchestration یا extensibility داخلی مجاز است
- وجود قرارداد درونی به معنی public API برای consumer نیست

---

## قواعد registration

در capabilityهای CrossCutting، registration باید از قواعد زیر پیروی کند:

### 1. registration باید extension-based باشد
نقطه ورود registration باید extension method باشد.

### 2. registration باید در مالک خودش بماند
هر provider مسئول registration خودش است.
اگر capability core دارد، registration اصلی capability در core قرار می‌گیرد.

### 3. registration نباید در consumer app پخش شود
consumer نباید مجبور شود wiring capability را به‌صورت دستی و پراکنده انجام دهد.

### 4. namespace registration باید استاندارد باشد
فایل registration باید در namespace زیر قرار بگیرد:

`Microsoft.Extensions.DependencyInjection`

### 5. سبک registration باید متناسب با الگوی capability باشد

#### در replacement-based capability
registration مستقیم provider کافی است.

نمونه:
- `AddMicrosoftJsonSerializer`
- `AddNewtonsoftJsonSerializer`

#### در core-orchestrated capability
registration اصلی در core انجام می‌شود و providerها از طریق builder به آن متصل می‌شوند.

نمونه:
- `AddParrot(...).UseSqlServer(...)`

### 6. misconfigurationهای حیاتی باید زودهنگام شناسایی شوند
اگر capability بدون provider معتبر نیست، این موضوع باید در startup validation کنترل شود و خطای شفاف ایجاد کند.

---

## قواعد options

### اصل پایه

options provider بخشی از قرارداد مصرفی capability نیستند.

### قواعد رسمی

- options در فولدر `Configurations` قرار می‌گیرند
- options فقط در registration یا runtime داخلی provider استفاده می‌شوند
- options provider-specific نباید وارد abstraction شوند
- typeهای بیرونی provider نباید وارد API مصرفی شوند
- consumer نباید برای استفاده روزمره capability مجبور به کار با options provider-specific شود

### سبک فنی options

در خانواده CrossCutting، یک سبک فنی واحد برای همه providerها اجباری نیست.

دو سبک مجاز هستند:

#### options object ساده
وقتی options فقط در registration یا ساخت provider استفاده می‌شوند

#### الگوی `IOptions<T>`
وقتی provider در runtime به options نیاز دارد یا lifecycle داخلی provider به آن وابسته است

### قاعده مهم
یکسان بودن مرز معماری options مهم‌تر از یکسان بودن مکانیکی پیاده‌سازی فنی آن‌هاست.

---

## قواعد setup

- setup باید کوتاه، ساده و discoverable باشد
- setup capability نباید wiring پراکنده در چند نقطه تحمیل کند
- registration باید نقطه ورود واضح داشته باشد
- consumer باید بداند از کدام extension method شروع کند
- setupهای runtime-specific فقط در صورت نیاز واقعی مجازند
- capabilityهایی که بدون provider معتبر نیستند باید validation روشن داشته باشند

---

## قواعد logging و diagnostics

در خانواده CrossCutting:

- logging و diagnostics بخشی از قرارداد مصرفی capability نیستند
- providerهای اصلی نباید برای هر call لاگ روتین per-call تولید کنند
- payload خام یا داده حساس نباید در logging عمومی ثبت شود
- troubleshooting عمیق باید با decorator، wrapper یا observability بیرونی انجام شود
- capability نباید API مصرفی را با concernهای diagnostics آلوده کند

### پیامد طراحی
consumer باید بتواند capability را بدون dependency به concernهای logging و tracing مصرف کند.

---

## قواعد exception handling

### اصل پایه
در capabilityهای provider-based، exception ابزار یا library بیرونی نباید بدون کنترل از مرز capability عبور کند، اگر آن exception به consumer-facing behavior مربوط است.

### قواعد رسمی

- provider باید خطاهای بیرونی را در مرز capability normalize کند
- exception capability-level باید context حداقلی مفید برای debugging را نگه دارد
- misconfigurationهای startup باید exception شفاف و زودهنگام داشته باشند
- exception handling باید بین providerهای یک capability تا حد ممکن یکدست باشد

### context حداقلی پیشنهادی
در صورت نیاز، exception capability-level باید بتواند این اطلاعات را نگه دارد:

- نوع عملیات
- نوع هدف
- نام provider
- inner exception

### نکته
این قاعده درباره همه خطاهای internal و local به‌صورت مکانیکی اعمال نمی‌شود.
تمرکز این guideline روی مرزهایی است که از capability به consumer نشت می‌کنند.

---

## قواعد naming محصولی و naming فنی

در خانواده CrossCutting، naming دو لایه دارد.

### نام محصولی
نامی است که capability در سطح محصول با آن شناخته می‌شود.

نمونه‌ها:

- Prism
- Parrot

### نام فنی
نامی است که در taxonomy، namespace، project naming و docs path استفاده می‌شود.

نمونه‌ها:

- Serializer
- Translator

### قواعد رسمی

- نام محصولی مجاز است و بخشی از روایت محصول است
- نام فنی باید با taxonomy رسمی پروژه هم‌راستا بماند
- namespaceها و project nameها باید بر اساس نام فنی باشند
- docs باید mapping بین نام محصولی و نام فنی را شفاف ثبت کنند
- نام محصولی جایگزین نام فنی در ساختار فنی نمی‌شود

### solution naming

در capabilityهایی که برند محصولی تثبیت شده دارند، نام solution می‌تواند نام محصولی باشد.

نمونه:
- `Prism.slnx`
- `Parrot.slnx`

اما project nameها و namespaceها باید همچنان technical-name-aligned باقی بمانند.

---

## قواعد namespace

الگوی رسمی namespace در capabilityهای CrossCutting به این صورت است:

### Abstractions
`ZaminX.BuildingBlocks.CrossCutting.<TechnicalCapability>.Abstractions`

### Provider
`ZaminX.BuildingBlocks.CrossCutting.<TechnicalCapability>.<ProviderName>`

### Core در صورت وجود
`ZaminX.BuildingBlocks.CrossCutting.<TechnicalCapability>.<CoreOrProductName>`

### Sample
namespace sample باید با نام پروژه sample هم‌راستا باشد و از naming رسمی capability پیروی کند.

### Registration
فایل‌های registration باید در namespace زیر قرار بگیرند:

`Microsoft.Extensions.DependencyInjection`

---

## قواعد ساختار solution

هر capability در خانواده CrossCutting باید ساختاری روشن و قابل پیش‌بینی داشته باشد.

الگوی پایه:

- solution در ریشه capability
- فولدر `src`
- فولدر `samples`

### پروژه‌های محتمل در `src`

- `Abstractions`
- یک یا چند provider
- در صورت نیاز یک core project

### پروژه‌های محتمل در `samples`

- یک sample کوچک و روشن
- در صورت نیاز چند sample با سناریوهای متفاوت

### قواعد رسمی

- solution باید فقط پروژه‌های مربوط به همان capability را نگه دارد
- artifactهای اضافی یا موقت نباید در ساختار مرجع باقی بمانند
- naming solution و naming پروژه‌ها باید با guideline رسمی هم‌راستا باشند

---

## قواعد ساختار پروژه‌ها

### پروژه Abstractions

مسئولیت‌ها:

- قراردادهای مصرفی
- قراردادهای درونی در صورت نیاز
- مدل‌های مشترک
- exceptionهای capability-level
- builder interfaceها در صورت نیاز

محدودیت‌ها:

- نباید به provider-specific library وابسته شود
- نباید منطق اجرایی capability را نگه دارد

---

### پروژه Core

فقط در capabilityهای core-orchestrated ایجاد می‌شود.

مسئولیت‌ها:

- implementation اصلی capability برای consumer
- orchestration داخلی
- lifecycle و startup behavior
- validation اصلی configuration
- store، coordinator و runtime behavior

محدودیت‌ها:

- نباید به provider خاص قفل شود
- نباید منطق provider-specific را در خود نگه دارد

---

### پروژه Provider

مسئولیت‌ها:

- implementation provider-specific
- registration provider
- options provider
- adapter/serviceهای فنی
- hosted serviceهای provider-specific در صورت نیاز
- scriptها یا assetهای فنی provider در صورت نیاز

محدودیت‌ها:

- نباید API مصرفی capability را مالک شود، مگر در الگوی replacement-based
- نباید docs capability را با docs library بیرونی جایگزین کند

---

### پروژه Sample

مسئولیت‌ها:

- نمایش registration
- نمایش inject شدن قرارداد مصرفی
- نمایش usage واقعی capability

محدودیت‌ها:

- sample مرجع اصلی معماری نیست
- sample جایگزین docs نیست
- sample نباید به آموزش مستقیم library بیرونی تبدیل شود

---

## قواعد ساختار فولدرهای داخلی

برای حفظ یکدستی فنی، این فولدرها در capabilityهای CrossCutting به‌عنوان نام‌های استاندارد پذیرفته می‌شوند:

- `Contracts`
- `Models`
- `Builders`
- `Configurations`
- `Services`
- `Extensions`
- `Exceptions`
- `Adapters`
- `Scripts`

### قاعده مهم
وجود همه این فولدرها در همه پروژه‌ها الزامی نیست.

فقط در صورت نیاز استفاده می‌شوند، اما در صورت استفاده باید از همین naming استاندارد پیروی کنند.

---

## قواعد builder pattern

builder pattern در خانواده CrossCutting فقط زمانی استفاده می‌شود که capability از نوع core-orchestrated باشد.

### builder pattern مناسب است وقتی:
- registration اصلی در core انجام می‌شود
- providerها باید به core attach شوند
- چند provider می‌توانند با هم فعال باشند
- registration providerها بخشی از composition capability است

### builder pattern مناسب نیست وقتی:
- capability فقط providerهای جایگزین دارد
- consumer فقط یک provider را مستقیماً register می‌کند
- core orchestration مستقلی وجود ندارد

بنابراین builder pattern یک guideline عمومی برای همه capabilityهای provider-based نیست.

---

## قواعد sample

sample در خانواده CrossCutting باید از قواعد زیر پیروی کند:

- minimal باشد
- واقعی باشد
- registration capability را نشان دهد
- inject شدن قرارداد مصرفی را نشان دهد
- usage روزمره capability را نمایش دهد
- naming منظم و taxonomy-aligned داشته باشد

### چیزهایی که sample نباید باشد
- جایگزین docs
- dump پراکنده از امکانات library بیرونی
- پروژه‌ای با naming مبهم یا namespace ناهماهنگ
- محلی برای باقی ماندن artifactهای آزمایشی

---

## قواعد README و docs

### اصل پایه
docs مرجع اصلی capability است.  
README ورودی سریع و مکمل است.

### README ریشه capability باید:
- capability را معرفی کند
- نام محصولی و نام فنی را روشن کند
- پروژه‌های capability را فهرست کند
- نقش هر پروژه را بگوید
- مرجع اصلی docs را معرفی کند

### README هر پروژه باید:
- مسئولیت همان پروژه را توضیح دهد
- مرز آن پروژه را روشن کند
- از تکرار غیرضروری docs خودداری کند

### docs capability باید:
- تعریف capability
- مسئله‌ای که حل می‌کند
- جایگاه در taxonomy
- چرایی وجود در زمین X
- مدل طراحی
- ساختار capability
- policyهای مرزی
- نحوه استفاده در سطح مفهومی
- محدودیت‌ها
- تصمیم‌های باز
- ارتباط با ADRها
را پوشش دهد

### قاعده مهم
README نباید جای docs را بگیرد و docs نباید به تکرار README تبدیل شود.

---

## قواعد ADR

در خانواده CrossCutting، ADR فقط زمانی لازم است که تصمیم از سطح local implementation عبور کند.

### ADR لازم است وقتی:
- تصمیم روی چند capability اثر می‌گذارد
- یک الگوی رسمی طراحی تثبیت می‌شود
- cost of change بالا است
- تصمیم معماری برگشت‌پذیری کمی دارد
- guideline رسمی جدید ایجاد می‌شود

### ADR لازم نیست وقتی:
- اصلاح صرفاً محلی naming مطرح است
- cleanup ساختاری کوچک انجام می‌شود
- یک جزئیات local implementation تغییر می‌کند

### استاندارد ADR
ADRها باید بر اساس MADR نوشته شوند و هر ADR فقط یک تصمیم را پوشش دهد.

---

## تصمیم‌های تثبیت‌شده این خانواده

در وضعیت فعلی، این تصمیم‌ها برای خانواده CrossCutting تثبیت‌شده در نظر گرفته می‌شوند:

- abstraction فقط در صورت نیاز واقعی ایجاد می‌شود
- provider-based بودن فقط برای capabilityهای مناسب استفاده می‌شود
- registration در provider یا core مالک آن نگه داشته می‌شود
- API مصرفی باید provider-agnostic باقی بماند
- options فقط در registration و runtime داخلی provider مجازند
- diagnostics بخشی از قرارداد مصرفی capability نیست
- exception بیرونی نباید در مرز consumer-facing بدون کنترل نشت کند
- نام محصولی و نام فنی از هم تفکیک می‌شوند
- docs مرجع اصلی است و README مکمل
- ساختار داخلی پایه پروژه‌ها با naming استاندارد فولدرها حفظ می‌شود
- builder pattern فقط برای capabilityهای core-orchestrated مناسب است

---

## چیزهایی که هنوز guideline رسمی نیستند

موارد زیر در وضعیت فعلی guideline رسمی خانواده CrossCutting نیستند و فقط observation یا تصمیم local به‌شمار می‌آیند:

- وجود indexer در قرارداد مصرفی
- داشتن دو مسیر formatting
- داشتن in-memory cache در همه capabilityها
- داشتن refresh runtime در همه capabilityها
- پشتیبانی از multi-provider override در همه capabilityها
- استفاده اجباری از `IOptions<T>` در همه providerها
- استفاده از hosted service در همه capabilityهای provider-based

این موارد فقط در capabilityهایی اعمال می‌شوند که نیاز واقعی به آن‌ها داشته باشند.

---

## معیار ارزیابی capability جدید در این خانواده

برای طراحی capability جدید در CrossCutting، این پرسش‌ها باید پاسخ داده شوند:

1. آیا این capability واقعاً عمومی و بین‌برشی است؟
2. آیا مستقل از دامنه و لایه خاص است؟
3. آیا نیاز واقعی به abstraction دارد؟
4. آیا replacement-based است یا core-orchestrated؟
5. قرارداد مصرفی آن چیست؟
6. آیا قرارداد درونی هم لازم است؟
7. registration آن کجا باید قرار بگیرد؟
8. options آن چگونه بدون نشت به consumer مدیریت می‌شوند؟
9. exceptionهای بیرونی چگونه در مرز capability کنترل می‌شوند؟
10. sample، README و docs آن چگونه از هم تفکیک می‌شوند؟

تا زمانی که این پرسش‌ها روشن نشده‌اند، طراحی capability نهایی تلقی نمی‌شود.

---

## ارتباط با سایر اسناد

- `docs/03.modules/00.BuildingBlocks/01.CrossCutting/index.md`
- `docs/04.decision-records/adr/adr-001-cross-cutting-capabilities-provider-model.md`
- `docs/04.decision-records/adr/adr-006-product-naming-vs-technical-naming.md`
- `docs/04.decision-records/adr/adr-009-provider-options-diagnostics-and-exception-boundary.md`
- `docs/06.guidelines/adr.md`

---

## نگهداری سند

این سند باید در این حالت‌ها بازبینی شود:

- وقتی capability مرجع جدیدی به خانواده CrossCutting اضافه می‌شود
- وقتی ADR جدیدی الگوی طراحی این خانواده را تغییر می‌دهد
- وقتی driftهای ساختاری چند capability تکرار می‌شوند
- وقتی observationهای محلی به guideline رسمی تبدیل می‌شوند

هدف این سند تثبیت طراحی است، نه محدود کردن رشد طبیعی capabilityها.