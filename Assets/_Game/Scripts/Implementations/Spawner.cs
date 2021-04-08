using System.Collections.Generic;
using UnityEngine;

namespace WOBH
{
    public class Spawner<T> where T : Component
    {
        private readonly T prefab;

        private Stack<T> stack = new Stack<T>();

        public Spawner(T prefab) => this.prefab = prefab;

        public T Spawn()
        {
            if (stack.Count > 0)
                return stack.Pop();

            return Object.Instantiate(prefab);
        }

        public void Recycle(T obj) => stack.Push(obj);
    }
}