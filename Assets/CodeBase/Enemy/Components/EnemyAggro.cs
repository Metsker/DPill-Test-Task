using CodeBase.Hero.Components;
using CodeBase.Logic;
using UnityEngine;
using UnityEngine.AI;
namespace CodeBase.Enemy.Components
{
    [RequireComponent(typeof(NavMeshAgent), typeof(EnemyHeroChaser))]
    public class EnemyAggro : MonoBehaviour, IDisabledOnDeath
    {
        [SerializeField] private EnemyHeroChaser chaser;
        [SerializeField] private TriggerObserver safeZoneObserver;
        private bool _hasAggroTarget;

        private HeroZoneInformer _heroZoneInformer;

        private void Start()
        {
            EnableAggro();
        }

        private void OnEnable()
        {
            safeZoneObserver.TriggerEnter += SafeZoneEnter;
        }

        private void OnDisable()
        {
            safeZoneObserver.TriggerEnter -= SafeZoneEnter;

            _heroZoneInformer.EnterSafeZone -= DisableAggro;
            _heroZoneInformer.ExitSafeZone -= EnableAggro;
        }

        public void Disable()
        {
            enabled = false;
        }

        public void Construct(HeroZoneInformer heroZoneInformer)
        {
            _heroZoneInformer = heroZoneInformer;

            _heroZoneInformer.EnterSafeZone += DisableAggro;
            _heroZoneInformer.ExitSafeZone += EnableAggro;
        }

        private void SafeZoneEnter(Collider obj)
        {
            DisableAggro();
        }

        private void EnableAggro()
        {
            if (_hasAggroTarget)
                return;

            _hasAggroTarget = true;

            SwitchFollowOn();
        }

        private void DisableAggro()
        {
            if (!_hasAggroTarget)
                return;

            _hasAggroTarget = false;

            SwitchFollowOff();
        }

        private void SwitchFollowOff()
        {
            chaser.agent.isStopped = true;
            chaser.enabled = false;
        }

        private void SwitchFollowOn()
        {
            chaser.agent.isStopped = false;
            chaser.enabled = true;
        }
    }
}
