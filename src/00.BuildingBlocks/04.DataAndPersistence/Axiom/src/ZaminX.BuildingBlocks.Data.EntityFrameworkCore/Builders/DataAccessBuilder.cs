using System;
using Microsoft.Extensions.DependencyInjection;
using ZaminX.BuildingBlocks.Data.EntityFrameworkCore.Enums;

namespace ZaminX.BuildingBlocks.Data.EntityFrameworkCore.Builders;

public sealed class DataAccessBuilder
{
    internal bool HasWriteRegistration { get; private set; }

    internal bool HasReadRegistration { get; private set; }

    public IServiceCollection Services { get; }

    public DataAccessBuilder(IServiceCollection services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    internal void MarkRegistration(DataAccessRegistrationKind kind)
    {
        switch (kind)
        {
            case DataAccessRegistrationKind.Write:
                if (HasWriteRegistration)
                {
                    throw new InvalidOperationException(
                        "Write EntityFrameworkCore data access has already been registered.");
                }

                HasWriteRegistration = true;
                break;

            case DataAccessRegistrationKind.Read:
                if (HasReadRegistration)
                {
                    throw new InvalidOperationException(
                        "Read EntityFrameworkCore data access has already been registered.");
                }

                HasReadRegistration = true;
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
        }
    }


}
