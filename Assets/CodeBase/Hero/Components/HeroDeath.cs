using System;
using CodeBase.Logic;
using UnityEngine;
namespace CodeBase.Hero.Components
{
    [RequireComponent(typeof(HeroHealth), typeof(HeroAnimator))]
    public class HeroDeath : MonoBehaviour, IResettableOnRestart
    {
        [SerializeField] private HeroHealth health;
        [SerializeField] private HeroAnimator animator;

        public bool isDead { get; private set; }

        private void Start()
        {
            health.HealthChanged += HealthChanged;
        }

        private void OnDestroy()
        {
            health.HealthChanged -= HealthChanged;
        }

        public event Action Happened;

        public void Reset()
        {
            isDead = false;
        }
        
        private void HealthChanged()
        {
            if (AliveOrAlreadyDead())
                return;

            Die();
        }

        private bool AliveOrAlreadyDead()
        {
            return !(health.current <= 0) || isDead;
        }

        private void Die()
        {
            isDead = true;

            DisableComponents();

            animator.PlayDeath();

            Happened?.Invoke();
        }

        private void DisableComponents()
        {
            foreach (IDisabledOnDeath component in GetComponents<IDisabledOnDeath>())
                component.Disable();
        }
    }
}
