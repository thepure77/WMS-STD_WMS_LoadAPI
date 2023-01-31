using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using static LoadBusiness.Load.SearchDetailModel;

namespace LoadBusiness
{

    //public static class QueryableExtensions
    //{
    //    //public static IQueryable<T> KWOrderBy<T>(this IQueryable<T> source, IEnumerable<SortModel> sortModels)
    //    //{
    //    //    var expression = source.Expression;
    //    //    int count = 0;
    //    //    foreach (var item in sortModels)
    //    //    {
    //    //        var parameter = Expression.Parameter(typeof(T), "x");
    //    //        var selector = Expression.PropertyOrField(parameter, item.ColId);
    //    //        var method = string.Equals(item.Sort, "desc", StringComparison.OrdinalIgnoreCase) ?
    //    //            (count == 0 ? "OrderByDescending" : "ThenByDescending") :
    //    //            (count == 0 ? "OrderBy" : "ThenBy");
    //    //        expression = Expression.Call(typeof(Queryable), method,
    //    //            new Type[] { source.ElementType, selector.Type },
    //    //            expression, Expression.Quote(Expression.Lambda(selector, parameter)));
    //    //        count++;
    //    //    }
    //    //    return count > 0 ? source.Provider.CreateQuery<T>(expression) : source;
    //    //}
    //}

    public static class QueryableExtensions
    {
        public static IQueryable<T> KWOrderBy<T>(this IQueryable<T> source, IEnumerable<SortModel> sortModels)
        {
            var expression = source.Expression;
            int count = 0;
            foreach (var item in sortModels)
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var selector = Expression.PropertyOrField(parameter, item.ColId);
                var method = string.Equals(item.Sort, "desc", StringComparison.OrdinalIgnoreCase) ?
                    (count == 0 ? "OrderByDescending" : "ThenByDescending") :
                    (count == 0 ? "OrderBy" : "ThenBy");
                expression = Expression.Call(typeof(Queryable), method,
                    new Type[] { source.ElementType, selector.Type },
                    expression, Expression.Quote(Expression.Lambda(selector, parameter)));
                count++;
            }
            return count > 0 ? source.Provider.CreateQuery<T>(expression) : source;
        }
    }
}
