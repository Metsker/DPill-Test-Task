using System;
using CodeBase.Logic.Animations;
using UnityEngine;
namespace CodeBase.Enemy.Components
{
    public class EnemyAnimator : MonoBehaviour, IAnimationStateReader
    {
        private static readonly int Attack1 = Animator.StringToHash("Attack_1");
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int Hit = Animator.StringToHash("Hit");
        private static readonly int Die = Animator.StringToHash("Die");
        private static readonly int Victory = Animator.StringToHash("Win");
        private readonly int _attackStateHash = Animator.StringToHash("attack01");
        private readonly int _deathStateHash = Animator.StringToHash("die");

        private readonly int _idleStateHash = Animator.StringToHash("idle");
        private readonly int _victoryStateHash = Animator.StringToHash("victory");
        private readonly int _walkingStateHash = Animator.StringToHash("Move");

        private Animator animator { get; set; }


        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public AnimatorState State { get; private set; }

        public void EnteredState(int stateHash)
        {
            State = StateFor(stateHash);
            StateEntered?.Invoke(State);
        }

        public void ExitedState(int stateHash)
        {
            StateExited?.Invoke(State);
        }

        public event Action<AnimatorState> StateEntered;
        public event Action<AnimatorState> StateExited;

        public void PlayDeath()
        {
            animator.SetTrigger(Die);
        }

        public void PlayAttack1()
        {
            animator.SetTrigger(Attack1);
        }

        public void PlayVictory()
        {
            animator.SetTrigger(Victory);
        }

        public void PlayHit()
        {
            animator.SetTrigger(Hit);
        }

        public void Move(float speed)
        {
            animator.SetBool(IsMoving, true);
            animator.SetFloat(Speed, speed);
        }

        public void StopMoving()
        {
            animator.SetBool(IsMoving, false);
        }

        private AnimatorState StateFor(int stateHash)
        {
            AnimatorState state;
            if (stateHash == _idleStateHash)
                state = AnimatorState.Idle;
            else if (stateHash == _attackStateHash)
                state = AnimatorState.Attack;
            else if (stateHash == _walkingStateHash)
                state = AnimatorState.Walking;
            else if (stateHash == _victoryStateHash)
                state = AnimatorState.Victory;
            else if (stateHash == _deathStateHash)
                state = AnimatorState.Died;
            else
                state = AnimatorState.Unknown;

            return state;
        }
    }
}
