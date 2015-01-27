using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace Portal.Infrastructure.Helpers
{
    public static class ListHelper
    {
        public static void ForEach<TItem>(this IEnumerable<TItem> items, Action<TItem> action)
        {
            foreach (var item in items)
            {
                action(item);
            }
        }

        public static Dictionary<TResult, List<TItem>> ToIndexedGroups<TItem, TResult>(this IEnumerable<TItem> list, Func<TItem, TResult> keyFunction)
        {
            var retVal = new Dictionary<TResult, List<TItem>>();

            var groupedItems = list.GroupBy(keyFunction);

            foreach (var groupItem in groupedItems)
            {
                retVal.Add(groupItem.Key, groupItem.ToList());
            }

            return retVal;
        }

        public static string ToCsv<T>(this IEnumerable<T> list)
        {
            return ToCsv<T>(list, o => o.ToString());
        }

        public static string ToCsv<T>(this IEnumerable<T> list, Func<T, string> getValue)
        {
            return Join<T>(list, ",", getValue).TrimEnd(',');
        }

        public static string Join<T>(this IEnumerable<T> list, string separator, Func<T, string> getValue)
        {
            var retVal = string.Empty;

            if (list != null && separator != null)
            {
                var buffer = new StringBuilder();

                list.ForEach(item =>
                {
                    var toString = getValue(item);

                    if (toString != separator)
                    {
                        buffer.AppendFormat("{0}{1}", toString, separator);
                    }
                });

                if (buffer.Length > 0)
                {
                    buffer.Remove(buffer.Length - separator.Length, separator.Length);
                }

                retVal = buffer.ToString();
            }

            return retVal;
        }

        public enum OrderDirection
        {
            Ascending,
            Descending
        }

        public static IEnumerable<dynamic> OrderByDynamic(this IEnumerable<dynamic> source, string propertyName, OrderDirection direction = OrderDirection.Ascending)
        {
            if (source == null)
                return null;

            if (direction == OrderDirection.Ascending)
            {
                return source.OrderBy(x =>
                {
                    var o = x as ExpandoObject;

                    if (o != null)
                        return ((IDictionary<string, object>)x)[propertyName];

                    return x.GetType().GetProperty(propertyName).GetValue(x, null);
                });
            }

            return source.OrderByDescending(x =>
            {
                var o = x as ExpandoObject;

                if (o != null)
                    return ((IDictionary<string, object>)x)[propertyName];

                return x.GetType().GetProperty(propertyName).GetValue(x, null);
            });
        }
    }
}
