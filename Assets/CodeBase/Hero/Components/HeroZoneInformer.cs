using System;
using CodeBase.Logic;
using UnityEngine;
namespace CodeBase.Hero.Components
{
    public class HeroZoneInformer : MonoBehaviour
    {
        [SerializeField] private TriggerObserver triggerObserver;

        private void OnEnable()
        {
            triggerObserver.TriggerEnter += TriggerEnter;
            triggerObserver.TriggerExit += TriggerExit;
        }

        private void OnDisable()
        {
            triggerObserver.TriggerEnter -= TriggerEnter;
            triggerObserver.TriggerExit -= TriggerExit;
        }

        public event Action EnterSafeZone;
        public event Action ExitSafeZone;

        private void TriggerEnter(Collider other)
        {
            EnterSafeZone?.Invoke();
        }

        private void TriggerExit(Collider other)
        {
            ExitSafeZone?.Invoke();
        }
    }
}
