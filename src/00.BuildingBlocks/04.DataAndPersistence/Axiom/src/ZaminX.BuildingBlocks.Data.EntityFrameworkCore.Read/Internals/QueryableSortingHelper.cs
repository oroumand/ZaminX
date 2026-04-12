using System.Linq.Expressions;
using System.Reflection;

namespace ZaminX.BuildingBlocks.Data.EntityFrameworkCore.Read.Internals;

internal static class QueryableSortingHelper
{
    public static IQueryable<TEntity> ApplySorting<TEntity>(
    IQueryable<TEntity> query,
    string sortBy,
    bool sortDescending)
    {
        ArgumentNullException.ThrowIfNull(query);

    if (string.IsNullOrWhiteSpace(sortBy))
        {
            return query;
        }

        var property = FindProperty(typeof(TEntity), sortBy) ?? throw new InvalidOperationException(
                $"Property '{sortBy}' was not found on type '{typeof(TEntity).FullName}'.");
        var parameter = Expression.Parameter(typeof(TEntity), "entity");
        var propertyAccess = Expression.Property(parameter, property);
        var orderByLambda = Expression.Lambda(propertyAccess, parameter);

        var methodName = sortDescending ? "OrderByDescending" : "OrderBy";

        var methodCallExpression = Expression.Call(
            typeof(Queryable),
            methodName,
            [typeof(TEntity), property.PropertyType],
            query.Expression,
            Expression.Quote(orderByLambda));

        return query.Provider.CreateQuery<TEntity>(methodCallExpression);
    }

    private static PropertyInfo? FindProperty(Type type, string propertyName)
    {
        return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .FirstOrDefault(property =>
                string.Equals(property.Name, propertyName, StringComparison.OrdinalIgnoreCase));
    }


}
