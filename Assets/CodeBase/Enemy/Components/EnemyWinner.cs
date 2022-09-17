using CodeBase.Hero.Components;
using CodeBase.Logic;
using UnityEngine;
namespace CodeBase.Enemy.Components
{
    public class EnemyWinner : MonoBehaviour, IDisabledOnDeath
    {
        [SerializeField] private EnemyAnimator animator;
        [SerializeField] private EnemyHeroChaser chaser;

        private HeroDeath _heroDeath;

        private void OnDestroy()
        {
            _heroDeath.Happened -= OnVictory;
        }

        public void Disable()
        {
            enabled = false;
        }

        public void Construct(HeroDeath heroDeath)
        {
            _heroDeath = heroDeath;
            _heroDeath.Happened += OnVictory;
        }

        private void OnVictory()
        {
            DisableMovement();
            animator.PlayVictory();
        }

        private void DisableMovement()
        {
            chaser.enabled = false;
        }
    }
}
