using CodeBase.Infrastructure.Data;
using TMPro;
using UnityEngine;
namespace CodeBase.Hero.Components
{
    public class HeroLootCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI counter;

        private LootData _lootData;

        private void OnDestroy()
        {
            _lootData.Changed -= UpdateCounter;
        }

        public void Construct(LootData lootData)
        {
            _lootData = lootData;
            _lootData.Changed += UpdateCounter;

            UpdateCounter();
        }

        private void UpdateCounter()
        {
            counter.text = $"{_lootData.collectedInField}";
        }
    }
}
