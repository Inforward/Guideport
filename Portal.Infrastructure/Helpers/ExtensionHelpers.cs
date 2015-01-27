using System.Collections.Specialized;
using Newtonsoft.Json;
using Portal.Model;
using Portal.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Xml.Serialization;
using UtilityEncryption;

namespace Portal.Infrastructure.Helpers
{
    public static class ExtensionHelpers
    {
        public static string SafeTrim(this string text)
        {
            return String.IsNullOrEmpty(text) ? text : text.Trim();
        }

        public static string TrimToLength(this string text, int maxLength, bool addEllipsis = false)
        {
            if (!String.IsNullOrEmpty(text) && text.Length > maxLength)
            {
                return text.Substring(0, maxLength) + (addEllipsis ? "..." : String.Empty);
            }

            return text;
        }

        public static int ToInt32(this object s, int nullValue)
        {
            if (s == null)
                return nullValue;

            int i = nullValue;

            if (Int32.TryParse(s.ToString(), out i))
                return i;

            return nullValue;
        }

        public static bool Contains(this string text, string[] values, StringComparison type = StringComparison.InvariantCultureIgnoreCase)
        {
            if (text == null || values == null)
                return false;

            return values.Any(value => text.IndexOf(value, type) >= 0);
        }

        public static int CountOccurrencesOf(this string text, string pattern, StringComparison type = StringComparison.InvariantCultureIgnoreCase)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(pattern))
                return 0;

            var count = 0;
            var i = 0;

            while ((i = text.IndexOf(pattern, i, type)) != -1)
            {
                i += pattern.Length;
                count++;
            }

            return count;
        }

        public static string SubstringByWord(this string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            var retVal = text;

            if (text.Length > maxLength)
            {
                var lastSpace = text.LastIndexOf(' ', maxLength, maxLength - 1);
                if (lastSpace > -1)
                    retVal = text.Substring(0, lastSpace);
            }

            return retVal;
        }

        public static string StripTags(this string source, bool removeScriptsAndStyles = true)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;

            if (removeScriptsAndStyles)
                source = Regex.Replace(source, "<(script|style)\\b[^>]*?>.*?</\\1>", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Singleline);                

            var array = new char[source.Length];
            var arrayIndex = 0;
            var inside = false;

            for (var i = 0; i < source.Length; i++)
            {
                var let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }

                if (let == '>')
                {
                    inside = false;
                    continue;
                }

                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }

            return new string(array, 0, arrayIndex);
        }

        public static T Clone<T>(this T source)
        {
            using (var ms = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
                binaryFormatter.Serialize(ms, source);
                ms.Seek(0, SeekOrigin.Begin);

                return (T)binaryFormatter.Deserialize(ms);
            }
        }

        public static void With<T>(this T instance, Action<T> action)
        {
            action(instance);
        }

        public static R With<T, R>(this T instance, Func<T, R> func)
        {
            return (R)func(instance);
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
        {
            return list == null || !list.Any();
        }

        public static bool Equals(this string stringA, string stringB)
        {
            if (stringA != null && stringB != null)
                return String.Compare(stringA, stringB, StringComparison.InvariantCultureIgnoreCase) == 0;

            return false;
        }

        public static string Protect(this string text, string purpose)
        {
            if (String.IsNullOrEmpty(text))
                return null;

            byte[] stream = Encoding.UTF8.GetBytes(text);
            byte[] encodedValue = MachineKey.Protect(stream, purpose);
            return HttpServerUtility.UrlTokenEncode(encodedValue);
        }

        public static string Unprotect(this string text, string purpose)
        {
            if (String.IsNullOrEmpty(text))
                return null;

            byte[] stream = HttpServerUtility.UrlTokenDecode(text);
            byte[] decodedValue = MachineKey.Unprotect(stream, purpose);
            return Encoding.UTF8.GetString(decodedValue);
        }

        public static bool IsNotNullOrEmpty(this string text)
        {
            return !String.IsNullOrWhiteSpace(text);
        }

        public static bool IsNullOrEmpty(this string text)
        {
            return String.IsNullOrWhiteSpace(text);
        }

        public static T FindRecursive<T>(this List<T> list, Predicate<T> match) where T : IHierarchy<T>
        {
            var item = list.Find(match);

            if (item == null)
            {
                foreach (var menuItem in list.Where(l => !l.Children.IsNullOrEmpty()))
                {
                    item = menuItem.Children.FindRecursive(match);

                    if (item != null)
                        break;
                }
            }

            return item;
        }

        public static bool AnyRecursive<T>(this List<T> list, Func<T, bool> match) where T : IHierarchy<T>
        {
            var exists = list.Any(match);

            if (!exists)
            {
                foreach (var menuItem in list.Where(l => !l.Children.IsNullOrEmpty()))
                {
                    exists = menuItem.Children.AnyRecursive(match);

                    if (exists)
                        break;
                }
            }

            return exists;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();

            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static string FullMessage(this Exception ex)
        {
            var sb = new StringBuilder();

            while (ex != null)
            {
                sb.Append(ex.Message);
                ex = ex.InnerException;
            }

            return sb.ToString();
        }

        public static bool In<T>(this T o, params T[] items)
        {
            return In(new[] { o }, items);
        }

        public static bool In<T>(this T[] c, params T[] items)
        {
            var retVal = false;

            if (c == null || items == null)
                return false;

            if (c.Any(items.Contains))
            {
                retVal = true;
            }

            return retVal;
        }

        public static TARGETENUM ToEnum<SOURCETYPE, TARGETENUM>(this SOURCETYPE i)
        {
            if (Enum.IsDefined(typeof(TARGETENUM), i))
            {
                if (i is string)
                {
                    return (TARGETENUM)Enum.Parse(typeof(TARGETENUM), i.ToString());
                }

                return (TARGETENUM)Enum.ToObject(typeof(TARGETENUM), i);
            }

            throw new IndexOutOfRangeException(String.Format("Value {0} does not exist in enumeration {1}.", i, typeof(TARGETENUM).FullName));
        }

        public static string ToXml<T>(this T o)
        {
            var serializer = new XmlSerializer(typeof(T));
            var sb = new StringBuilder();

            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, o);
                return sb.ToString();
            }

        }

        public static T FromXml<T>(this string xml)
        {
            var serializer = new XmlSerializer(typeof(T));

            using (var reader = new StringReader(xml))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        public static string ToJsonBase64String(this object obj)
        {
            return obj == null ? string.Empty : obj.ToJson().ToBase64String();
        }

        public static string ToJson(this object obj, Formatting formatting = Formatting.Indented)
        {
            string json;

            var scriptSerializer = JsonSerializer.Create(new JsonSerializerSettings()
            {
                Formatting = formatting,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            using (var sw = new StringWriter())
            {
                scriptSerializer.Serialize(sw, obj);
                json = sw.ToString();
            }

            return json;
        }

        public static string ToBase64String(this string obj)
        {
            return obj == null ? null : Convert.ToBase64String(Encoding.UTF8.GetBytes(obj));
        }

        public static DateTime ToStartDate(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        }

        public static DateTime ToEndDate(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
        }

        public static string Slugify(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;

            var slug = text.ToLower();

            // Strip Invalid chars           
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");

            // Convert multiple spaces into one space   
            slug = Regex.Replace(slug, @"\s+", " ").Trim();

            // Cut and trim 
            slug = slug.Substring(0, slug.Length <= 45 ? slug.Length : 45).Trim();

            // Replace spaces with hyphens
            slug = Regex.Replace(slug, @"\s", "-");

            // Convert multiple hyphens into one hyphen   
            slug = Regex.Replace(slug, @"-+", "-").Trim();

            // Ensure it does not end with a hyphen
            if (slug.EndsWith("-"))
                slug = slug.TrimEnd('-');

            return slug;
        }

        public static T GetValue<T>(this NameValueCollection formCollection, string fieldName)
        {
            var value = default(T);

            if (formCollection == null || formCollection[fieldName] == null)
                return value;

            var s = formCollection[fieldName];

            if (!string.IsNullOrWhiteSpace(s))
            {
                try
                {
                    value = (T)Convert.ChangeType(s, typeof(T));
                }
                catch { }
            }

            return value;
        }

        public static SiteMapItem GetRootPage(this List<SiteMapItem> items, int currentSiteContentId)
        {
            var currentPage = items.FindRecursive(s => s.SiteContentID == currentSiteContentId);

            while (currentPage != null && currentPage.SiteContentParentID.HasValue)
            {
                currentPage = items.FindRecursive(s => s.SiteContentID == currentPage.SiteContentParentID);
            }

            return currentPage;
        }
    }
}
