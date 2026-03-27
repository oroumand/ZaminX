# ADR 0002: قرارگیری Patternها در BuildingBlocks

## وضعیت
پذیرفته شده

## زمینه
در نسخه قبلی (Zamin)، بسیاری از الگوهای مهم مانند:
- Entity
- AggregateRoot
- Result
- Command / Query
- Mediator
- Pipeline Behavior

در داخل ساختار معماری Onion پیاده‌سازی شده بودند.

این در حالی است که این مفاهیم:
- وابسته به Onion نیستند
- در چندین سبک معماری قابل استفاده هستند
- جزو building blockهای reusable محسوب می‌شوند

قرار گرفتن این الگوها داخل Onion باعث می‌شد:
- استفاده از آن‌ها در معماری‌های دیگر سخت شود
- وابستگی ناخواسته به Onion ایجاد شود
- تفکیک بین «الگو» و «ساختار معماری» از بین برود

## تصمیم
تمام patternهای reusable از ساختارهای معماری جدا می‌شوند و زیر BuildingBlocks قرار می‌گیرند.

برای سازمان‌دهی این بخش، یک فولدر مفهومی به نام Patterns زیر BuildingBlocks ایجاد می‌شود:

src/00.BuildingBlocks/Patterns/

داخل این فولدر، یک solution مجزا برای patternها ایجاد می‌شود و پروژه‌ها به‌صورت فنی و شفاف نام‌گذاری می‌شوند.

Patternها شامل دسته‌هایی مانند:
- Domain patterns
- Application patterns
- Result model
- Mediator و pipeline
- abstractionهای مشترک

## نکته مهم
Patterns یک مفهوم طراحی هستند، نه الزاماً یک نوع package مستقل.

بنابراین:
- نام فولدر می‌تواند Patterns باشد (مفهومی)
- اما نام پروژه‌ها باید فنی و دقیق باشند (مثلاً Domain، Application و ...)

## پیامدها
- patternها از معماری‌های خاص مستقل می‌شوند
- امکان استفاده مجدد در Onion و Modular Monolith فراهم می‌شود
- وابستگی‌های ناخواسته کاهش پیدا می‌کند
- ساختار محصول قابل فهم‌تر و قابل توسعه‌تر می‌شود