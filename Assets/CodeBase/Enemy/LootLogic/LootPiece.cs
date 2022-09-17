using System;
using System.Collections;
using CodeBase.Infrastructure.Data;
using TMPro;
using UnityEngine;
namespace CodeBase.Enemy.LootLogic
{
    public class LootPiece : MonoBehaviour
    {
        private const float DestroyTime = 1.5f;

        [SerializeField] private GameObject skull;
        [SerializeField] private GameObject pickupPopup;
        [SerializeField] private TextMeshPro lootText;

        private Loot _loot;
        private LootData _lootData;
        private bool _picked;

        private void OnDestroy()
        {
            Destroyed?.Invoke();
        }

        private void OnTriggerEnter(Collider other)
        {
            Pickup();
        }

        public event Action Destroyed;

        public void Construct(LootData lootData)
        {
            _lootData = lootData;
        }

        public void Initialize(Loot loot)
        {
            _loot = loot;
        }

        private void Pickup()
        {
            if (_picked)
                return;

            _picked = true;

            UpdateLootData();
            HideSkull();
            ShowText();
            StartCoroutine(DestroyTimer());
        }

        private void UpdateLootData()
        {
            _lootData.Collect(_loot);
        }

        private void HideSkull()
        {
            skull.SetActive(false);
        }

        private void ShowText()
        {
            lootText.text = _loot.value.ToString();
            pickupPopup.SetActive(true);
        }

        private IEnumerator DestroyTimer()
        {
            yield return new WaitForSeconds(DestroyTime);

            Destroy(gameObject);
        }
    }
}
