using CodeBase.Hero.Components;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Windows;
namespace CodeBase.UI.Services.Windows
{
    public class WindowService : IWindowService
    {
        private readonly IUIFactory _uiFactory;

        private RestartWindow _restartWindow;

        public WindowService(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public void Subscribe(HeroDeath heroDeath)
        {
            heroDeath.Happened += OpenRestart;
        }

        public async void Open(WindowId windowId)
        {
            switch (windowId)
            {
                case WindowId.Unknown:
                    break;
                case WindowId.Restart:
                    if (!_restartWindow)
                        _restartWindow = await _uiFactory.CreateRestart();
                    break;
            }
        }

        private void OpenRestart()
        {
            Open(WindowId.Restart);
        }
    }
}
