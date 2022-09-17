using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PlayerData;
using CodeBase.StaticData;
using CodeBase.UI.Services.Factory;
namespace CodeBase.Infrastructure.States
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _state;
        private IExitableState _activeState;

        public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain loadingCurtain, AllServices services)
        {
            _state = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)] = new BootstrapState(
                    this,
                    sceneLoader,
                    services),

                [typeof(LoadLevelState)] = new LoadLevelState(
                    this,
                    sceneLoader,
                    loadingCurtain,
                    services.Single<IGameFactory>(),
                    services.Single<IStaticDataService>(),
                    services.Single<IUIFactory>()),

                [typeof(GameLoopState)] = new GameLoopState(this),

                [typeof(RestartState)] = new RestartState(
                    this,
                    services.Single<IStaticDataService>(),
                    services.Single<IPlayerProgressService>(),
                    loadingCurtain,
                    services.Single<IGameFactory>())
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            TState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayLoadedState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();

            TState state = GetState<TState>();
            _activeState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState
        {
            return _state[typeof(TState)] as TState;
        }
    }
}
