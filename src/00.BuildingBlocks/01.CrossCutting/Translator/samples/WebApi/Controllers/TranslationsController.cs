using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using ZaminX.BuildingBlocks.CrossCutting.Translator.Abstractions.Contracts;

namespace ZaminX.BuildingBlocks.CrossCutting.Translator.Sample.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class TranslationsController(ITranslator translator) : ControllerBase
{
    [HttpGet("{key}")]
    public IActionResult Get(string key, [FromQuery] string? culture = null)
    {
        var selectedCulture = CreateCultureOrDefault(culture);
        var value = translator.GetString(selectedCulture, key);

        return Ok(new
        {
            Key = key,
            Culture = selectedCulture.Name,
            Value = value
        });
    }

    [HttpGet("{key}/formatted")]
    public IActionResult GetFormatted(
        string key,
        [FromQuery] string? culture = null,
        [FromQuery] decimal amount = 1250.75m,
        [FromQuery] string currency = "IRR")
    {
        var selectedCulture = CreateCultureOrDefault(culture);

        var value = translator.GetFormattedString(
            selectedCulture,
            key,
            amount,
            currency,
            DateTime.UtcNow);

        return Ok(new
        {
            Key = key,
            Culture = selectedCulture.Name,
            Value = value
        });
    }

    [HttpGet("{key}/translated-args")]
    public IActionResult GetWithTranslatedArguments(
        string key,
        [FromQuery] string? culture = null,
        [FromQuery] string firstArgument = "User.DisplayName",
        [FromQuery] string secondArgument = "Common.Welcome")
    {
        var selectedCulture = CreateCultureOrDefault(culture);

        var value = translator.GetString(
            selectedCulture,
            key,
            firstArgument,
            secondArgument);

        return Ok(new
        {
            Key = key,
            Culture = selectedCulture.Name,
            Value = value
        });
    }

    [HttpGet("concat")]
    public IActionResult Concat(
        [FromQuery] string? culture = null,
        [FromQuery] char separator = ' ',
        [FromQuery] string[]? keys = null)
    {
        var selectedCulture = CreateCultureOrDefault(culture);
        var selectedKeys = keys is { Length: > 0 }
            ? keys
            : new[] { "Common.Hello", "Common.World" };

        var value = translator.GetConcatString(selectedCulture, separator, selectedKeys);

        return Ok(new
        {
            Culture = selectedCulture.Name,
            Keys = selectedKeys,
            Value = value
        });
    }

    [HttpGet("missing/{key}")]
    public IActionResult GetMissing(string key, [FromQuery] string? culture = null)
    {
        var selectedCulture = CreateCultureOrDefault(culture);
        var value = translator.GetString(selectedCulture, key);

        return Ok(new
        {
            Key = key,
            Culture = selectedCulture.Name,
            Value = value,
            Note = "If missing-key registration is enabled, this key will be inserted into the SQL table when it is not found."
        });
    }

    private static CultureInfo CreateCultureOrDefault(string? culture)
    {
        return string.IsNullOrWhiteSpace(culture)
            ? CultureInfo.CurrentCulture
            : CultureInfo.GetCultureInfo(culture);
    }
}