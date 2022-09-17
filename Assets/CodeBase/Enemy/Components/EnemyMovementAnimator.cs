using CodeBase.Logic;
using UnityEngine;
using UnityEngine.AI;
namespace CodeBase.Enemy.Components
{
    [RequireComponent(typeof(NavMeshAgent), typeof(EnemyAnimator))]
    public class EnemyMovementAnimator : MonoBehaviour, IDisabledOnDeath
    {
        private const float MinimalVelocity = 0.1f;

        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private EnemyAnimator animator;

        private void Update()
        {
            if (ShouldMove())
                animator.Move(agent.velocity.magnitude);
            else
                animator.StopMoving();
        }

        public void Disable()
        {
            enabled = false;
        }

        private bool ShouldMove()
        {
            return agent.velocity.magnitude > MinimalVelocity && agent.remainingDistance > agent.radius;
        }
    }
}
