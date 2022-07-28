using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public class ObjectPool<T>
        where T : Object
    {
        Queue<T> pool = new Queue<T>();

        public T getNewItem(T origin)
        {
            if (pool.Count > 0)
                return pool.Dequeue();

            return GameObject.Instantiate<T>(origin);
        }

        public void release(T item) => pool.Enqueue(item);
    }
}
