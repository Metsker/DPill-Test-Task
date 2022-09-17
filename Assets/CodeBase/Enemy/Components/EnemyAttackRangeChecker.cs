using CodeBase.Logic;
using UnityEngine;
namespace CodeBase.Enemy.Components
{
    [RequireComponent(typeof(EnemyAttack))]
    public class EnemyAttackRangeChecker : MonoBehaviour, IDisabledOnDeath
    {
        [SerializeField] private EnemyAttack enemyAttack;
        [SerializeField] private TriggerObserver triggerObserver;

        private void Start()
        {
            enemyAttack.DisableAttack();
        }

        private void OnEnable()
        {
            triggerObserver.TriggerEnter += TriggerEnter;
            triggerObserver.TriggerExit += TriggerExit;
        }

        private void OnDisable()
        {
            triggerObserver.TriggerEnter -= TriggerEnter;
            triggerObserver.TriggerExit -= TriggerExit;
        }

        public void Disable()
        {
            enabled = false;
        }

        private void TriggerEnter(Collider obj)
        {
            enemyAttack.EnableAttack();
        }

        private void TriggerExit(Collider obj)
        {
            enemyAttack.DisableAttack();
        }
    }
}
