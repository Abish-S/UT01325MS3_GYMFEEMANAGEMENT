using System.Linq.Expressions;

namespace UT01325MS3_GYMFEEMANAGEMENT.Extensions
{
    public static class PredicateExtensions
    {
        public static Expression<Func<T, bool>> AndAlso<T>(
           this Expression<Func<T, bool>> first,
           Expression<Func<T, bool>> second)
        {
            if (first == null) return second;
            if (second == null) return first;

            var parameter = Expression.Parameter(typeof(T));
            var combined = Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(
                    Expression.Invoke(first, parameter),
                    Expression.Invoke(second, parameter)
                ),
                parameter
            );

            return combined;
        }
    }
}
