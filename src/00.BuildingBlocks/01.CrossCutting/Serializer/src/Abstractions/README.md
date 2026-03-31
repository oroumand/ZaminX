# ZaminX.BuildingBlocks.CrossCutting.Serializer.Abstractions

## نمای کلی

این پروژه قرارداد مصرفی capability `Serializer` و اجزای مشترک provider-agnostic آن را نگه می‌دارد.

---

## مسئولیت‌ها

* تعریف `IJsonSerializer`
* تعریف exception مشترک capability
* جلوگیری از نشت dependencyهای provider-specific به مصرف‌کننده

---

## نکات مهم

* این پروژه نباید به `System.Text.Json` وابسته شود
* این پروژه نباید به `Newtonsoft.Json` وابسته شود
* این پروژه فقط مرز capability را نگه می‌دارد

---

## اجزای اصلی

* `IJsonSerializer`
* `JsonSerializationException`
