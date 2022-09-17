using CodeBase.Infrastructure.States;
using UnityEngine;
using UnityEngine.UI;
namespace CodeBase.UI.Windows
{
    public class RestartWindow : WindowBase
    {
        [SerializeField] private Button restartButton;

        private IGameStateMachine _stateMachine;

        public void Construct(IGameStateMachine sceneLoader)
        {
            _stateMachine = sceneLoader;
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            restartButton.onClick.AddListener(Restart);
            restartButton.onClick.AddListener(() => Destroy(gameObject));
        }

        private void Restart()
        {
            _stateMachine.Enter<RestartState>();
        }
    }
}
