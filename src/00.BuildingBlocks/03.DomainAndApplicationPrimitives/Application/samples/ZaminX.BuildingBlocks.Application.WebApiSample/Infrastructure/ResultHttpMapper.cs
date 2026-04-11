using ZaminX.BuildingBlocks.Application.Results;

namespace Relay.SampleWebApi.Infrastructure;

public static class ResultHttpMapper
{
    public static IResult ToHttpResult(this Result result)
    {
        if (result.IsSuccess)
            return Results.Ok();

        return Results.BadRequest(new
        {
            success = false,
            errors = result.Errors.Select(x => new { x.Code, x.Message })
        });
    }

    public static IResult ToHttpResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Results.Ok(new
            {
                success = true,
                data = result.Value
            });
        }

        return Results.BadRequest(new
        {
            success = false,
            errors = result.Errors.Select(x => new { x.Code, x.Message })
        });
    }
}