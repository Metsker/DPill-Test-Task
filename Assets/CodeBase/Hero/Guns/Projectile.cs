using System;
using CodeBase.Logic;
using DG.Tweening;
using UnityEngine;
namespace CodeBase.Hero
{
    public class Projectile : MonoBehaviour
    {
        private float _damage;
        private Action _onDestroy;

        private void OnTriggerEnter(Collider other)
        {
            other.transform.parent.GetComponent<IHealth>().TakeDamage(_damage);

            DOTween.Kill(transform);

            _onDestroy?.Invoke();
        }

        public void Construct(float damage, Action onDestroy)
        {
            _damage = damage;
            _onDestroy = onDestroy;
        }
    }
}
