using Microsoft.AspNetCore.Mvc;
using ZaminX.BuildingBlocks.CrossCutting.Serializer.Abstractions.Contracts;
using ZaminX.BuildingBlocks.CrossCutting.Serializer.Abstractions.Exceptions;
using ZaminX.BuildingBlocks.CrossCutting.Serializer.WebApiSample.Models;

namespace ZaminX.BuildingBlocks.CrossCutting.Serializer.WebApiSample.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class SerializerController(IJsonSerializer jsonSerializer) : ControllerBase
{
    private readonly IJsonSerializer _jsonSerializer = jsonSerializer;

    [HttpPost("serialize")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<object> Serialize([FromBody] PersonDto model)
    {
        var json = _jsonSerializer.Serialize(model);

        return Ok(new
        {
            provider = "Microsoft",
            json
        });
    }

    [HttpPost("deserialize")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<object> Deserialize([FromBody] DeserializeRequest request)
    {
        try
        {
            var result = _jsonSerializer.Deserialize<PersonDto>(request.Json);

            return Ok(new
            {
                provider = "Microsoft",
                data = result
            });
        }
        catch (JsonSerializationException ex)
        {
            return BadRequest(new
            {
                error = ex.Message,
                ex.Operation,
                ex.ProviderName,
                targetType = ex.TargetType?.FullName
            });
        }
    }

    [HttpPost("round-trip")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<object> RoundTrip([FromBody] PersonDto model)
    {
        try
        {
            var json = _jsonSerializer.Serialize(model);
            var result = _jsonSerializer.Deserialize<PersonDto>(json);

            return Ok(new
            {
                provider = "Microsoft",
                serialized = json,
                deserialized = result
            });
        }
        catch (JsonSerializationException ex)
        {
            return BadRequest(new
            {
                error = ex.Message,
                ex.Operation,
                ex.ProviderName,
                targetType = ex.TargetType?.FullName
            });
        }
    }
}