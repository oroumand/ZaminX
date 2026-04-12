using System;
using System.Collections.Generic;
using System.Text;
using ZaminX.BuildingBlocks.Data.Abstractions.Models;

namespace ZaminX.BuildingBlocks.Data.Read.Abstractions.Contracts;

public interface IReadRepository<TEntity, TId>
{
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);

    Task<PagedResult<TEntity>> GetPagedListAsync(
        PagedQuery request,
        CancellationToken cancellationToken = default);

}