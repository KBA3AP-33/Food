using System.Linq.Expressions;

namespace Catalog.Static
{
    public static class LinqStatic
    {
        public static IQueryable<TEntity> DynamicOrderBy<TEntity>(this IQueryable<TEntity> source, string orderByProperty, bool isDescending = false)
        {
            string command = isDescending ? "OrderByDescending" : "OrderBy";
            var type = typeof(TEntity);
            var property = type.GetProperty(orderByProperty);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property!);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property!.PropertyType }, source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }

        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            var param = Expression.Parameter(typeof(T));
            var body = Expression.AndAlso(Expression.Invoke(left, param), Expression.Invoke(right, param));
            return Expression.Lambda<Func<T, bool>>(body, param);
        }

        public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            var param = Expression.Parameter(typeof(T));
            var body = Expression.OrElse(Expression.Invoke(left, param), Expression.Invoke(right, param));
            return Expression.Lambda<Func<T, bool>>(body, param);
        }
    }
}
