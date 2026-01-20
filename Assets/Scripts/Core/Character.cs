using UnityEngine;
using UnityEngine.AI;

namespace Core
{
    [RequireComponent(typeof(StatsContext), typeof(NavMeshAgent))]
    public class Character : MonoBehaviour
    {
        public StatsContext Stats {get; private set;}

        protected void Awake()
        {
            Stats = GetComponent<StatsContext>();
        }

        protected void Start()
        {
            Stats.Get<Health>().OnDead += OnDead;
        }

        private void OnDead()
        {
            Destroy(gameObject);
        }

        protected void OnDestroy()
        {
            if ( Stats.Get<Health>() != null)
                Stats.Get<Health>().OnDead -= OnDead;
        }
    }
}