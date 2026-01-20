using System;
using System.Collections.Generic;

namespace Core
{
    public sealed class StatContainer
    {
        private readonly Dictionary<Type, IStat> _stats = new();

        public void Add<T> (T stat) where T : class, IStat
            => _stats[stat.GetType()] = stat;

        public T Get<T>() where T : class, IStat 
            => _stats[typeof(T)] as T ?? throw new InvalidOperationException($"Stat {typeof(T).Name} not found"); 
    }

}