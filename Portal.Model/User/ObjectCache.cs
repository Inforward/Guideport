using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Portal.Model
{
    public partial class ObjectCache
    {
        public int UserID { get; set; }
        public string Key { get; set; }
        public string ValueSerialized { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }

        [IgnoreDataMember]
        public virtual User User { get; set; }

        public override bool Equals(object obj)
        {
            var item = obj as ObjectCache;

            if (item != null)
            {
                return item.UserID == UserID && item.Key == Key;
            }

            return false;
        }

        public override int GetHashCode()
        {
            if (!string.IsNullOrEmpty(Key))
            {
                return UserID + Key.GetHashCode();
            }

            return base.GetHashCode();
        }

        public static string SerializeItem(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }

    public static class ObjectCacheExtensions
    {
        public static T GetValue<T>(this ObjectCache cacheItem, T defaultValue = default(T))
        {
            var value = defaultValue;

            if (!string.IsNullOrEmpty(cacheItem.ValueSerialized))
            {
                value = JsonConvert.DeserializeObject<T>(cacheItem.ValueSerialized);
            }

            return value != null ? value : defaultValue;
        }

        public static void Update(this ICollection<ObjectCache> list, ObjectCache cacheItem)
        {
            if (list.Any(o => o.UserID == cacheItem.UserID && o.Key == cacheItem.Key))
            {
                list.Remove(cacheItem);
                list.Add(cacheItem);
            }
            else
            {
                list.Add(cacheItem);
            }
        }
    }
}
