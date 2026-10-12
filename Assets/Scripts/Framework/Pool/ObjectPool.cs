using System.Collections.Generic;

namespace Framework
{
    public class ObjectPool<T> : Singleton<ObjectPool<T>> where T : class, new()
    {
        private Stack<T> cache = new();

        public T Get()
        {
            T t;
            if (cache.Count > 0)
            {
                t = cache.Pop();
            }
            else
            {
                t = new();
            }

            return t;
        }

        public void Release(T t)
        {
            cache.Push(t);
        }
    }
}
