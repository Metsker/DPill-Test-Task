using CodeBase.Logic;
using UnityEngine;
using UnityEngine.AI;
namespace CodeBase.Enemy.Components
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyHeroChaser : MonoBehaviour, IDisabledOnDeath
    {
        private const float MinimalDistance = 1;

        public NavMeshAgent agent;

        [SerializeField] private float speed = 3;

        private Transform _heroTransform;

        private void Start()
        {
            agent.speed = speed;
        }

        private void Update()
        {
            SetDestinationForAgent();
        }

        public void Disable()
        {
            agent.isStopped = true;
            enabled = false;
        }

        public void Construct(Transform heroTransform)
        {
            _heroTransform = heroTransform;
        }

        private void SetDestinationForAgent()
        {
            if (HeroNotReached())
                agent.destination = _heroTransform.position;
        }

        private bool HeroNotReached()
        {
            return Vector3.Distance(agent.transform.position, _heroTransform.position) >= MinimalDistance;
        }
    }
}
