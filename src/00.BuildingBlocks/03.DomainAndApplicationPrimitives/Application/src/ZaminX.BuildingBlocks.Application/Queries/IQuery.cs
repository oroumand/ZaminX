using ZaminX.BuildingBlocks.Application.Requests;
using ZaminX.BuildingBlocks.Application.Results;

namespace ZaminX.BuildingBlocks.Application.Queries;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}