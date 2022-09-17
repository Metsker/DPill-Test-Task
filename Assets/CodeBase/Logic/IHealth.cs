using System;
namespace CodeBase.Logic
{
    public interface IHealth
    {
        float current { get; }
        float max { get; }

        event Action HealthChanged;
        void TakeDamage(float damage);
    }
}
