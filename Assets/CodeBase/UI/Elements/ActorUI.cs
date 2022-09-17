using CodeBase.Logic;
using UnityEngine;
namespace CodeBase.UI.Elements
{
    public class ActorUI : MonoBehaviour
    {
        [SerializeField] private HpBar hpBar;

        private IHealth _health;

        private void OnDestroy()
        {
            _health.HealthChanged -= UpdateHpBar;
        }

        public void Construct(IHealth health)
        {
            _health = health;
            _health.HealthChanged += UpdateHpBar;
        }

        private void UpdateHpBar()
        {
            hpBar.SetValue(_health.current, _health.max);
        }
    }
}
