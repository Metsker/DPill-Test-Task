using CodeBase.Infrastructure.Data;
using CodeBase.Logic;
using UnityEngine;
namespace CodeBase.Enemy.Components
{
    public class EnemyHeroObserver : MonoBehaviour, IDisabledOnDeath
    {
        [SerializeField] private float speed = 3;

        private Transform _heroTransform;

        private Vector3 _positionToLook;

        private void Update()
        {
            RotateTowardsHero();
        }

        public void Disable()
        {
            enabled = false;
        }

        public void Construct(Transform heroTransform)
        {
            _heroTransform = heroTransform;
        }

        private void RotateTowardsHero()
        {
            UpdatePositionToLookAt();
            transform.rotation = SmoothedRotation(transform.rotation, _positionToLook);
        }

        private Quaternion SmoothedRotation(Quaternion rotation, Vector3 positionToLook)
        {
            return Quaternion.Lerp(rotation, Quaternion.LookRotation(positionToLook), SpeedFactor());
        }

        private float SpeedFactor()
        {
            return speed * Time.deltaTime;
        }

        private void UpdatePositionToLookAt()
        {
            Vector3 positionDiff = _heroTransform.position - transform.position;
            _positionToLook = positionDiff.ChangeY(transform.position.y);
        }
    }
}
