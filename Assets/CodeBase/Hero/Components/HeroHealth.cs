using System;
using CodeBase.Logic;
using UnityEngine;
namespace CodeBase.Hero.Components
{
    [RequireComponent(typeof(HeroAnimator))]
    public class HeroHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private HeroAnimator animator;
        [SerializeField] private float health = 20;

        private bool _isDead;

        public void Reset()
        {
            current = max;
        }

        private void Start()
        {
            max = health;
        }

        public event Action HealthChanged;

        public float current
        {
            get
            {
                return health;
            }
            set
            {
                health = value;
                HealthChanged?.Invoke();
            }
        }

        public float max { get; set; }

        public void TakeDamage(float damage)
        {
            if (current <= 0)
                return;

            current -= damage;
            animator.PlayHit();
        }
    }
}
