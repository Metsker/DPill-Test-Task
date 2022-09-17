using System;
using CodeBase.Logic;
using UnityEngine;
namespace CodeBase.Enemy.Components
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class EnemyHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private EnemyAnimator animator;
        [SerializeField] private float health = 10;

        private void Start()
        {
            max = health;
        }

        public float current
        {
            get
            {
                return health;
            }
            private set
            {
                health = value;
            }
        }

        public float max { get; private set; }

        public event Action HealthChanged;

        public void TakeDamage(float damage)
        {
            current -= damage;

            animator.PlayHit();

            HealthChanged?.Invoke();
        }
    }
}
