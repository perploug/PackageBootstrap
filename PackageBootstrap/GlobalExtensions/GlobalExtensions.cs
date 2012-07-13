using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;

namespace PackageBootstrap.Extensions
{
    /// <summary>The global extensions.</summary>
    public static class GlobalExtensions
    {
        public static void Raise<T>(this EventHandler<T> handler, object sender, T args) where T : EventArgs
        {
            if (handler != null) handler(sender, args);
        }   


        #region Public Methods
        public static void RemoveEventHandlerListeners(this object instance, string eventName)
        {
            RemoveEventHandlerListeners(instance, instance.GetType().GetEvent(eventName, BindingFlags.Public | BindingFlags.Instance));
        }

        public static void RemoveEventHandlerListeners(this object instance, EventInfo info)
        {
            if (info != null)
            {
                var field = instance.GetType().GetField(info.Name, BindingFlags.NonPublic | BindingFlags.Instance);
                if (field != null)
                {
                    var eh = field.GetValue(instance) as EventHandler;

                    if (eh != null)
                    {
                        //Remove events one by one
                        foreach (var del in eh.GetInvocationList())
                            info.GetRemoveMethod().Invoke(instance, new object[] {del});
                    }
                }
            }
        }
        
        public static void RemoveAllEventHandlerListeners(this object instance)
        {
            var events = instance.GetType().GetEvents(BindingFlags.Public | BindingFlags.Instance);
            foreach (var eventInfo in events)
                instance.RemoveEventHandlerListeners(eventInfo);
        }
        
        public static bool ContainsKeyIgnoreCase<TValue>(this IDictionary<string, TValue> dictionary, string key)
        {
            return dictionary.Keys.Any(i => i.Equals(key, StringComparison.CurrentCultureIgnoreCase));
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var knownKeys = new HashSet<TKey>();
            return source.Where(element => knownKeys.Add(keySelector(element)));
        }

        public static bool EnumTryParse<T>(this string strType, bool ignoreCase, out T result)
        {
            try
            {
                result = (T)Enum.Parse(typeof(T), strType, ignoreCase);
                return true;
            }
            catch
            {
                result = default(T);
                return false;
            }
        }

        public static IEnumerable<TItem> FlattenList<TItem>(this IEnumerable<TItem> items, Func<TItem, IEnumerable<TItem>> selectChild)
        {
            IEnumerable<TItem> children = items != null && items.Any()
                                              ? items.SelectMany(selectChild).FlattenList(selectChild)
                                              : Enumerable.Empty<TItem>();

            if (items != null)
            {
                return items.Concat(children);
            }

            return null;
        }

        public static IEnumerable<TItem> ForEach<TItem>(this IEnumerable<TItem> items, Action<TItem> action)
        {
            if (items != null)
            {
                foreach (TItem item in items)
                {
                    action(item);
                }
            }

            return items;
        }

        public static TResult[] ForEach<TItem, TResult>(this IEnumerable<TItem> items, Func<TItem, TResult> func)
        {
            return items.Select(func).ToArray();
        }

        public static string Format(this TimeSpan timeSpan, string format)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                format,
                timeSpan.Hours,
                timeSpan.Minutes,
                timeSpan.Seconds);
        }

        public static TValue GetEntryIgnoreCase<TValue>(this IDictionary<string, TValue> dictionary, string key)
        {
            return dictionary.GetEntryIgnoreCase(key, default(TValue));
        }

        public static TValue GetEntryIgnoreCase<TValue>(this IDictionary<string, TValue> dictionary, string key, TValue defaultValue)
        {
            key = dictionary.Keys.Where(i => i.Equals(key, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            return !key.IsNullOrWhiteSpace()
                       ? dictionary[key]
                       : defaultValue;
        }

        public static void IfNotNull<TItem>(this TItem item, Action<TItem> action) where TItem : class
        {
            if (item != null)
            {
                action(item);
            }
        }

        public static void IfNotNull<TItem>(this IEnumerable<TItem> items, Action<TItem> action) where TItem : class
        {
            if (items != null)
            {
                foreach (TItem item in items)
                {
                    item.IfNotNull(action);
                }
            }
        }

        public static void IfTrue(this bool predicate, Action action)
        {
            if (predicate)
            {
                action();
            }
        }

        public static bool IsNullOrWhiteSpace(this string str)
        {
            return (str == null) || (str.Trim().Length == 0);
        }

        public static TResult IfNotNull<TResult, TItem>(this TItem item, Func<TItem, TResult> action)
            where TItem : class
        {
            return item != null ? action(item) : default(TResult);
        }

        public static TResult IfNotNull<TResult, TItem>(this TItem item, Func<TItem, TResult> action, TResult defaultValue)
            where TItem : class
        {
            return item != null ? action(item) : defaultValue;
        }

        public static IList<string> ToDelimitedList(this string list, string delimiter = ",")
        {
            var delimiters = new[] { delimiter };
            return !list.IsNullOrWhiteSpace()
                       ? list.Split(delimiters, StringSplitOptions.RemoveEmptyEntries)
                             .Select(i => i.Trim())
                             .ToList()
                       : new List<string>();
        }

        #endregion
    }
}