using Microsoft.AspNetCore.Http;
using ZaminX.BuildingBlocks.Application.Results;
namespace ZaminX.BuildingBlocks.Application.AspNetCore.HttpResults;

public static class ResultHttpMapper
{
    public static IResult ToHttpResult(this Result result)
    {
        ArgumentNullException.ThrowIfNull(result);

        if (result.IsSuccess)
        {
            return Microsoft.AspNetCore.Http.Results.Ok(new
            {
                success = true
            });
        }

        return Microsoft.AspNetCore.Http.Results.BadRequest(new
        {
            success = false,
            errors = result.Errors.Select(x => new
            {
                x.Code,
                x.Message
            })
        });
    }

    public static IResult ToHttpResult<TValue>(this Result<TValue> result)
    {
        ArgumentNullException.ThrowIfNull(result);

        if (result.IsSuccess)
        {
            return Microsoft.AspNetCore.Http.Results.Ok(new
            {
                success = true,
                data = result.Value
            });
        }        
        return Microsoft.AspNetCore.Http.Results.BadRequest(new
        {
            success = false,
            errors = result.Errors.Select(x => new
            {
                x.Code,
                x.Message
            })
        });
    }
}