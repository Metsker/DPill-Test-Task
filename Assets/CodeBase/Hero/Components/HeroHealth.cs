using System;
using CodeBase.Logic;
using UnityEngine;
namespace CodeBase.Hero.Components
{
    [RequireComponent(typeof(HeroAnimator))]
    public class HeroHealth : MonoBehaviour, IHealth, IResettableOnRestart
    {
        [SerializeField] private HeroAnimator animator;
        [SerializeField] private float health = 20;

        private bool _isDead;
        public event Action HealthChanged;


        public float current
        {
            get => health;
            private set
            {
                health = value;
                HealthChanged?.Invoke();
            }
        }

        public float max { get; private set; }
        
        private void Start()
        {
            max = health;
        }

        public void TakeDamage(float damage)
        {
            if (current <= 0)
                return;

            current -= damage;
            animator.PlayHit();
        }
        
        public void Reset()
        {
            current = max;
        }
    }
}
