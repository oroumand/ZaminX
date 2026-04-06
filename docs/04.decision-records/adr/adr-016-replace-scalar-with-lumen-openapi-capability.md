# ADR: تبدیل Scalar به Lumen

## وضعیت

پذیرفته‌شده

---

## تاریخ

2026-04-07

---

## عنوان

تبدیل capability قبلی Scalar به capability جدید OpenApi با نام محصولی Lumen

---

## زمینه

در نسخه اولیه، ما capabilityای با تمرکز روی `Scalar` داشتیم که هدف اصلی آن ساده‌سازی setup مربوط به OpenAPI document و نمایش UI مبتنی بر Scalar بود.

این پیاده‌سازی اولیه اگرچه برای یک use-case مشخص مفید بود، اما در بازبینی معماری مشخص شد که از نظر مدل capability، نام‌گذاری و scope با taxonomy فعلی زمین X هم‌راستای کامل ندارد.

به‌ویژه این مسائل دیده شد:

* concern اصلی موجود، صرفاً «Scalar» نبود
* بخش مهم‌تر مسئله، ثبت OpenAPI و expose کردن document بود
* Scalar فقط یکی از UIهای قابل استفاده برای نمایش document محسوب می‌شد
* در اکوسیستم ASP.NET Core و ابزارهای پیرامون OpenAPI، UIهای دیگری مانند Swagger UI و ReDoc نیز رایج و قابل پشتیبانی هستند
* متمرکز کردن capability روی یک UI خاص باعث می‌شد مسئله اصلی به‌درستی مدل نشود

در همین بازبینی، این نکته نیز روشن شد که capability موردنظر از جنس CrossCutting نیست، بلکه ماهیت اصلی آن در حوزه setup، registration و runtime composition قرار می‌گیرد.

---

## مسئله

آیا capability فعلی باید با هویت `Scalar` حفظ شود، یا باید به capability عمومی‌تری بازطراحی شود که concern اصلی آن OpenAPI registration و UI composition باشد؟

---

## تصمیم

تصمیم گرفته شد که capability قبلی `Scalar` به capability جدیدی با مشخصات زیر تبدیل شود:

* **نام فنی capability:** `OpenApi`
* **نام محصولی capability:** `Lumen`

در این مدل:

* `Scalar` دیگر capability اصلی نیست
* `Scalar` به‌عنوان یکی از UI integrationهای Lumen در نظر گرفته می‌شود
* Lumen capability مرکزی برای:

  * ثبت OpenAPI
  * expose کردن document endpoint
  * مدیریت wiring مرتبط با API documentation
  * و ترکیب UIهای مختلف
    خواهد بود

---

## دلایل تصمیم

### 1. concern اصلی OpenAPI است، نه Scalar

در بازبینی معماری مشخص شد که بخش اصلی مسئله این نیست که فقط یک UI خاص رجیستر شود، بلکه باید کل چرخه زیر در یک نقطه استاندارد شود:

* ثبت OpenAPI
* expose کردن document
* تعیین document path
* attach کردن UIهای نمایش

در این مدل، `Scalar` تنها یکی از presentation optionها است.

---

### 2. نام Scalar بیش از حد محدودکننده بود

وقتی capability با نام `Scalar` تعریف می‌شود، این تصور ایجاد می‌شود که concern اصلی همان UI است، در حالی که در طراحی جدید:

* document registration
* runtime mapping
* و setup simplification

بخش اصلی capability هستند.

پس نام `Scalar` مسئله را کوچک‌تر و نادقیق‌تر نشان می‌داد.

---

### 3. Lumen به‌عنوان نام محصولی هویت بهتری ایجاد می‌کند

در مقایسه با نام‌های صرفاً فنی، `Lumen`:

* با الگوی naming فعلی پروژه هم‌راستاست
* کنار نام‌هایی مثل Axon، Prism و Parrot می‌نشیند
* حس روشن کردن، نمایش دادن و قابل دیدن کردن API را منتقل می‌کند
* برای capability جدید که دیگر thin utility نیست، product identity مناسبی ایجاد می‌کند

---

### 4. این capability در خانواده RuntimeAndRegistration قرار می‌گیرد

تحلیل معماری نشان داد که concern اصلی این capability:

* startup setup
* runtime wiring
* service registration
* composition

است.

بنابراین این capability باید در خانواده زیر قرار بگیرد:

`00.BuildingBlocks/02.RuntimeAndRegistration`

نه در CrossCutting.

---

### 5. UIها باید integration باشند، نه capability مستقل

در مدل جدید، UIهای مختلف مانند:

* Scalar
* Swagger UI
* ReDoc

concern اصلی capability نیستند، بلکه ابزارهای نمایش روی document تولیدشده هستند.

بنابراین تصمیم گرفته شد که این UIها:

* capability مستقل نباشند
* provider رسمی هم محسوب نشوند
* بلکه به‌عنوان UI integrationهای درون Lumen مدل شوند

---

### 6. abstraction برای این capability پیش‌فرض نیست

این capability:

* service business ارائه نمی‌دهد
* مصرف runtime-level در business logic ندارد
* بیشتر یک registration/setup capability است

بنابراین abstraction مصرفی برای آن ارزش واقعی ایجاد نمی‌کرد.

تصمیم گرفته شد که:

* abstraction عمومی برای مصرف capability تعریف نشود
* builder سبک برای composition کافی باشد
* public API minimal و شفاف بماند

---

### 7. استفاده از Options باید framework-native باشد

در نسخه‌های آزمایشی، مسیرهایی مثل:

* `OptionsWrapper`
* `Options.Create`
* `Replace(IOptions<T>)`

بررسی یا استفاده شدند، اما در نهایت تصمیم گرفته شد که Lumen کاملاً بر پایه pipeline استاندارد Options در ASP.NET Core ساخته شود.

بنابراین رویکرد رسمی Lumen این است:

* `AddOptions<T>()`
* `Bind(...)`
* `Configure(...)`
* `Validate(...)`
* `PostConfigure(...)` در صورت نیاز

و از مسیرهای دستی و overrideمحور پرهیز شود.

---

## پیامدها

### پیامدهای مثبت

* concern capability شفاف‌تر شد
* نام capability محدود به یک UI خاص نماند
* امکان پشتیبانی از چند UI به‌صورت طبیعی به‌وجود آمد
* مدل setup و runtime wiring منسجم‌تر شد
* موقعیت capability در taxonomy پروژه روشن شد
* product identity قوی‌تری برای این حوزه ایجاد شد
* API نهایی قابلیت رشد بدون بازطراحی مفهومی را پیدا کرد

---

### پیامدهای طراحی

بر اساس این تصمیم:

* پروژه core با نام `ZaminX.BuildingBlocks.RuntimeAndRegistration.OpenApi.Lumen` تعریف می‌شود
* UIها در پروژه‌های جدا اما زیرمجموعه همین capability قرار می‌گیرند، مانند:

  * `ZaminX.BuildingBlocks.RuntimeAndRegistration.Scalar`
  * `ZaminX.BuildingBlocks.RuntimeAndRegistration.Swagger`
  * `ZaminX.BuildingBlocks.RuntimeAndRegistration.Redoc`
* entry point اصلی capability از جنس:

  * `AddZaminXOpenApi(...)`
  * `UseZaminXOpenApi()`
    خواهد بود

---

### پیامدهای مستندسازی

این تصمیم ایجاب می‌کند که:

* مستندات قبلی که Scalar را به‌عنوان capability مستقل معرفی می‌کردند، به‌روزرسانی شوند
* Lumen به‌عنوان capability اصلی OpenAPI معرفی شود
* Scalar در اسناد جدید به‌عنوان UI integration توصیف شود
* README و project-state با این مدل جدید هم‌راستا شوند
* سند module-level برای Lumen به‌عنوان مرجع اصلی این capability نگه‌داری شود

---

## گزینه‌های بررسی‌شده

### گزینه 1: حفظ Scalar به‌عنوان capability مستقل

این گزینه رد شد، چون:

* concern اصلی را به‌درستی مدل نمی‌کرد
* نام capability بیش از حد narrow بود
* رشد capability به UIهای دیگر را غیرطبیعی می‌کرد

---

### گزینه 2: absorb کردن Scalar در capability دیگر مانند DI

این گزینه رد شد، چون:

* concernهای متفاوتی با DI دارد
* منجر به آلودگی مسئولیت می‌شود
* OpenAPI setup یک مسئله مستقل و قابل reuse است

---

### گزینه 3: ساخت capability عمومی‌تر با محور OpenAPI

این گزینه پذیرفته شد، چون:

* مسئله اصلی را درست مدل می‌کند
* growth path بهتری دارد
* از نظر taxonomy صحیح است
* امکان naming محصولی معنادار را فراهم می‌کند

---

## آنچه عمداً انتخاب نشد

موارد زیر در این تصمیم آگاهانه انتخاب نشدند:

* provider model رسمی مشابه capabilityهای CrossCutting
* abstraction مصرفی عمومی
* plugin system پیچیده
* multi-document orchestration پیشرفته در v1
* customization سنگین UIها در نسخه اول
* یکی کردن همه UIها در یک پروژه واحد بدون مرزبندی روشن

---

## قواعد ناشی از این تصمیم

از این تصمیم چند guideline عملیاتی حاصل می‌شود:

### 1. concern capability باید بر اساس مسئله اصلی نام‌گذاری شود

نه بر اساس یکی از implementationها یا UIها.

### 2. در capabilityهای RuntimeAndRegistration، abstraction پیش‌فرض نیست

و فقط در صورت نیاز واقعی باید اضافه شود.

### 3. اگر یک UI یا ابزار صرفاً presentation layer باشد

نباید به‌زور به capability مستقل تبدیل شود.

### 4. registration و runtime mapping باید از هم جدا بمانند

تا setup شفاف، قابل فهم و تست‌پذیر بماند.

### 5. options باید با pipeline استاندارد framework مدیریت شوند

نه با تزریق دستی `IOptions<T>`.

---

## تصمیم نهایی

بر این اساس، تصمیم نهایی پروژه این است که:

* capability قبلی `Scalar` به‌عنوان capability اصلی کنار گذاشته شود
* capability جدید با نام فنی `OpenApi` و نام محصولی `Lumen` تعریف شود
* `Scalar`، `Swagger UI` و `ReDoc` به‌عنوان UI integrationهای Lumen مدل شوند
* Lumen در خانواده `RuntimeAndRegistration` قرار بگیرد
* طراحی و پیاده‌سازی آن بر پایه OpenAPI built-in در ASP.NET Core و Options pattern استاندارد انجام شود

---

## وضعیت بازنگری آینده

این ADR در آینده ممکن است در این زمینه‌ها بازنگری شود:

* نیاز به پشتیبانی از multi-document پیشرفته
* نیاز به API versioning integration عمیق‌تر
* نیاز به customization گسترده‌تر برای UIها
* نیاز به package-level decoupling بیشتر میان UI integrations

اما تا این لحظه، تصمیم فعلی مبنای رسمی طراحی و توسعه Lumen در زمین X محسوب می‌شود.
