using CodeBase.Hero.Components;
using CodeBase.Infrastructure.Data;
using TMPro;
using UnityEngine;
namespace CodeBase.UI.Elements
{
    public class HudLootCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI counter;
        
        private HeroZoneInformer _heroZoneInformer;
        private LootData _lootData;

        public void Construct(LootData lootData, HeroZoneInformer heroZoneInformer)
        {
            _lootData = lootData;
            _heroZoneInformer = heroZoneInformer;

            _heroZoneInformer.EnterSafeZone += MoveLootToBank;
            _lootData.Changed += UpdateCounter;

            UpdateCounter();
        }
        
        private void OnDestroy()
        {
            _heroZoneInformer.EnterSafeZone -= MoveLootToBank;
            _lootData.Changed -= UpdateCounter;
        }


        private void MoveLootToBank()
        {
            _lootData.MoveToBank();
        }

        private void UpdateCounter()
        {
            counter.text = $"{_lootData.collectedInBank}";
        }
    }
}
