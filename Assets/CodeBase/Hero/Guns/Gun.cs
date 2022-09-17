using System.Threading.Tasks;
using CodeBase.Infrastructure.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.StaticData;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
namespace CodeBase.Hero
{
    public class Gun
    {
        private readonly float _damage;

        private readonly IGameFactory _gameFactory;
        private readonly AssetReferenceGameObject _projectilePrefabReference;
        private readonly float _projectileSpeed;
        public readonly float Cooldown;
        public readonly float Range;

        private ObjectPool<GameObject> _pool;

        public Gun(GunStaticData gunData, IGameFactory gameFactory)
        {
            Cooldown = gunData.cooldown;
            Range = gunData.range;
            _damage = gunData.damage;
            _projectileSpeed = gunData.projectileSpeed;
            _projectilePrefabReference = gunData.projectilePrefabReference;

            _gameFactory = gameFactory;

            InitPool();
        }

        public void Shoot(Vector3 at, Vector3 target)
        {
            GameObject projectile = _pool.Get();

            projectile.transform.position = at;

            projectile.transform.DOMove(target.ChangeY(at.y), _projectileSpeed).SetSpeedBased();
        }

        private void InitPool()
        {
            _pool = new ObjectPool<GameObject>(() =>
                    BuildProjectile().Result,
                pt => { pt.SetActive(true); }, pt => { pt.SetActive(false); },
                Object.Destroy, false, 10, 15);
        }

        private async Task<GameObject> BuildProjectile()
        {
            Projectile projectile = await _gameFactory.CreateProjectile(_projectilePrefabReference);
            projectile.Construct(_damage, () => _pool.Release(projectile.gameObject));
            return projectile.gameObject;
        }
    }
}
