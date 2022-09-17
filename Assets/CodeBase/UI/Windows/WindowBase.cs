using UnityEngine;
using UnityEngine.UI;
namespace CodeBase.UI.Windows
{
    public abstract class WindowBase : MonoBehaviour
    {
        [SerializeField] protected Button closeButton;

        private void Awake()
        {
            OnAwake();
        }

        protected virtual void OnAwake()
        {
            closeButton.onClick.AddListener(() => Destroy(gameObject));
        }
    }
}
