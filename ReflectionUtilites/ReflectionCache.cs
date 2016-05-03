using System.Runtime.Caching;

namespace ReflectionUtilites
{
    #region Using Directives

    using ReflectionUtilites.Exceptions;
    using System;
    using System.Collections.Generic;

    #endregion Using Directives

    public static class ReflectionCache
    {
        #region Constants and Fields

        private static readonly Dictionary<Type, ReflectionClass> Cache = new Dictionary<Type, ReflectionClass>();

        #endregion Constants and Fields

        #region Public Methods

        public static ReflectionClass GetReflection(Type t)
        {
            if (t == null)
            {
                throw new NullReferenceReflectionException();
            }

            if (!Cache.ContainsKey(t))
            {
                Cache[t] = new ReflectionClass(t);
            }

            return Cache[t];
        }

        public static ReflectionClass GetReflectionMemoryCache(string t, CacheItemPolicy itemPolicy = null)
        {
            if (t == null)
            {
                throw new NullReferenceReflectionException();
            }

            Type type = Type.GetType(t);
            if (type == null)
            {
                throw new ArgumentException("Type " + t + " does not exist.");
            }
            if (itemPolicy == null)
            {
                itemPolicy = new CacheItemPolicy();
                DateTime dt1 = DateTime.Now.Date + new TimeSpan(30, 00, 00, 00);
                itemPolicy.AbsoluteExpiration = dt1;
            }
            ObjectCache memoryCache = MemoryCache.Default;

            if (!memoryCache.Contains(t))
            {
                memoryCache.Add(t, new ReflectionClass(type), itemPolicy);
            }

            return (ReflectionClass)memoryCache[t];
        }

        #endregion Public Methods
    }
}