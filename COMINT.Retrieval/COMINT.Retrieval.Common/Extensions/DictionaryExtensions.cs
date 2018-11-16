using System.Collections.Generic;

namespace COMINT.Retrieval.Common.Extensions
{
    public static class DictionaryExtensions
    {
        public static void Increment<TK, TV>(this Dictionary<TK, Dictionary<TV, int>> dictionary, TK key, TV valueKey, int value = 1)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key].Increment(valueKey, value);
            }
            else
            {
                dictionary.Add(key, new Dictionary<TV, int> { { valueKey, value } });
            }
        }

        public static void Increment<TK>(this Dictionary<TK, int> dictionary, TK key, int value = 1)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] += value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }

        public static void Increment<TK>(this Dictionary<TK, double> dictionary, TK key, double value = 1)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] += value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }

        // ReSharper disable once UnusedMember.Global
        public static void Deconstruct<TK, TV>(this KeyValuePair<TK, TV> kvp, out TK key, out TV value)
        {
            key = kvp.Key;
            value = kvp.Value;
        }
    }
}
