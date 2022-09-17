using System.Collections;
using CodeBase.Infrastructure.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Logic;
using CodeBase.StaticData;
using UnityEngine;
namespace CodeBase.Hero.Components
{
    [RequireComponent(typeof(HeroAnimator), typeof(HeroZoneInformer), typeof(CharacterController))]
    public class HeroAttack : MonoBehaviour, IDisabledOnDeath
    {
        [SerializeField] private HeroDeath heroDeath;
        [SerializeField] private HeroZoneInformer heroZoneInformer;
        [SerializeField] private CharacterController characterController;
        private IEnumerator _autoAttackCoroutine;
        private IGameFactory _gameFactory;

        private Gun _gun;

        private void Awake()
        {
            _autoAttackCoroutine = AutoAttack();
        }

        private void OnEnable()
        {
            heroZoneInformer.EnterSafeZone += DisableAutoAttack;
            heroZoneInformer.ExitSafeZone += EnableAutoAttack;
        }

        private void OnDisable()
        {
            heroZoneInformer.EnterSafeZone -= DisableAutoAttack;
            heroZoneInformer.ExitSafeZone -= EnableAutoAttack;
        }

        public void Disable()
        {
            enabled = false;
        }

        public void Construct(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        public void SetGun(GunStaticData gunData)
        {
            _gun = new Gun(gunData, _gameFactory);
        }

        private void EnableAutoAttack()
        {
            StartCoroutine(_autoAttackCoroutine);
        }

        private void DisableAutoAttack()
        {
            StopCoroutine(_autoAttackCoroutine);
        }

        private IEnumerator AutoAttack()
        {
            while (!heroDeath.isDead)
            {
                Transform closestEnemy = ClosestEnemy(out float distanceToClosestEnemy);

                if (distanceToClosestEnemy > _gun.Range)
                {
                    yield return new WaitForSeconds(0.1f);
                    continue;
                }

                _gun.Shoot(ProjectileStartPoint(), closestEnemy.position);

                yield return new WaitForSeconds(_gun.Cooldown);
            }
        }

        private Transform ClosestEnemy(out float distanceToClosestEnemy)
        {
            distanceToClosestEnemy = Mathf.Infinity;
            Transform closestEnemy = null;

            foreach (Transform currentEnemy in _gameFactory.monsters)
            {
                float distanceToEnemy = (currentEnemy.transform.position - transform.position).sqrMagnitude;

                if (!(distanceToEnemy < distanceToClosestEnemy))
                    continue;

                distanceToClosestEnemy = distanceToEnemy;
                closestEnemy = currentEnemy;
            }

            return closestEnemy;
        }

        private Vector3 ProjectileStartPoint()
        {
            return transform.position.ChangeY(characterController.center.y);
        }
    }
}
