using System.Linq;
using CodeBase.Enemy.Debug;
using CodeBase.Infrastructure.Data;
using CodeBase.Logic;
using UnityEngine;
namespace CodeBase.Enemy.Components
{
    [RequireComponent(typeof(Animator), typeof(EnemyAttackRangeChecker))]
    public class EnemyAttack : MonoBehaviour, IDisabledOnDeath
    {
        private const string LayerName = "Player";

        [SerializeField] private EnemyAnimator animator;

        [SerializeField] private float damage = 10;
        [SerializeField] private float effectiveDistance = 0.6f;
        [SerializeField] private float cleavage = 0.5f;
        [SerializeField] private float attackCooldown = 1;

        private readonly Collider[] _hits = new Collider[1];
        private float _attackCooldown;
        private Transform _heroTransform;
        private bool _isAttackActive;
        private bool _isAttacking;
        private int _layerMask;

        private void Awake()
        {
            _layerMask = 1 << LayerMask.NameToLayer(LayerName);
        }

        private void Update()
        {
            if (!CooldownIsUp())
                _attackCooldown -= Time.deltaTime;

            if (CanAttack())
                StartAttack();
        }

        public void Disable()
        {
            enabled = false;
        }

        public void Construct(Transform heroTransform)
        {
            _heroTransform = heroTransform;
        }

        private void OnAttack()
        {
            if (!Hit(out Collider hit))
                return;
            PhysicsDebug.DrawDebug(StartPoint(), cleavage, 1);
            hit.transform.GetComponent<IHealth>().TakeDamage(damage);
        }

        private void OnAttackEnded()
        {
            _attackCooldown = attackCooldown;
            _isAttacking = false;
        }

        public void EnableAttack()
        {
            _isAttackActive = true;
        }

        public void DisableAttack()
        {
            _isAttackActive = false;
        }

        private bool Hit(out Collider hit)
        {
            int hitCount = Physics.OverlapSphereNonAlloc(StartPoint(), cleavage, _hits, _layerMask);

            hit = _hits.FirstOrDefault();

            return hitCount > 0;
        }

        private Vector3 StartPoint()
        {
            return transform.position.AddY(0.5f) + transform.forward * effectiveDistance;
        }

        private bool CooldownIsUp()
        {
            return _attackCooldown <= 0;
        }

        private bool CanAttack()
        {
            return _isAttackActive && !_isAttacking && CooldownIsUp();
        }

        private void StartAttack()
        {
            transform.LookAt(_heroTransform);
            animator.PlayAttack1();

            _isAttacking = true;
        }
    }
}
