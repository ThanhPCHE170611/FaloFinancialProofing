using Elfie.Serialization;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq.Expressions;

namespace FALOFinancialProofing.Utilities
{
    public static class FilterHelper
    {
        // filter by string
        public static IEnumerable<T> Filter<T>(IEnumerable<T> query, string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return query;
            }
            var properties = typeof(T).GetProperties();
            var result = query.Where(x =>
            {
                foreach (var property in properties)
                {
                    var value = property.GetValue(x);
                    if (value != null && value.ToString().Contains(filter))
                    {
                        return true;
                    }
                }
                return false;
            });
            return result;
        }
        public static IEnumerable<T> Filter<T>(this IEnumerable<T> source, Expression<Func<T, bool>> predicate)
        {
            return source.AsQueryable().Where(predicate).ToList();
        }
    }
}
