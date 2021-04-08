using System;
using System.Collections.Generic;

namespace WOBH
{
    public class DIContainer
    {
        public static DIContainer Instance = new DIContainer();

        private Dictionary<Type, object> singles = new Dictionary<Type, object>();

        private DIContainer() { }

        public T Resolve<T>() where T : class, new()
        {
            Type type = typeof(T);
            if (singles.TryGetValue(type, out var value))
            {
                return value as T;
            }

            T t = new T();
            singles.Add(type, t);
            
            return t;
        }
    }
}