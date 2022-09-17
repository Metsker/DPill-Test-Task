using System;
using System.Collections;
using CodeBase.Logic;
using UnityEngine;
namespace CodeBase.Enemy.Components
{
    public class EnemyDeath : MonoBehaviour
    {
        private const int DestroyAfterSeconds = 3;

        [SerializeField] private EnemyHealth health;
        [SerializeField] private EnemyAnimator animator;

        private void Start()
        {
            health.HealthChanged += HealthChanged;
        }

        private void OnDestroy()
        {
            health.HealthChanged -= HealthChanged;
        }

        public event Action Happened;

        private void HealthChanged()
        {
            if (health.current <= 0)
                Die();
        }

        private void Die()
        {
            health.HealthChanged -= HealthChanged;

            DisableComponents();

            animator.PlayDeath();
            Happened?.Invoke();

            StartCoroutine(DestroyTimer());
        }

        private void DisableComponents()
        {
            foreach (IDisabledOnDeath component in GetComponents<IDisabledOnDeath>())
                component.Disable();
        }

        private IEnumerator DestroyTimer()
        {
            yield return new WaitForSeconds(DestroyAfterSeconds);
            Destroy(gameObject);
        }
    }
}
