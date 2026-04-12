using System;
using System.Collections.Generic;
using System.Text;

namespace ZaminX.BuildingBlocks.Data.Write.Abstractions.Contracts;

public interface IUnitOfWork
{
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    Task<int> CommitAsync(CancellationToken cancellationToken = default);

    Task<int> RollbackAsync(CancellationToken cancellationToken = default);

}
