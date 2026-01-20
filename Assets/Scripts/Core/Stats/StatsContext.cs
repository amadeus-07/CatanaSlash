using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public sealed class StatsContext : MonoBehaviour
    {
        [SerializeField] private List<StatSO> _stats;

        private readonly StatContainer _container = new StatContainer();
        public StatUpgrader Upgrader {get; private set;}

        public T Get<T>() where T : class, IStat => _container.Get<T>();

        private void Awake()
        {
            _stats.ForEach(e => _container.Add(e.Create()));
            Upgrader = new(_container);
        }

    }
}