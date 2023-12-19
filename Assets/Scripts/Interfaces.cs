using UnityEngine;

namespace RunAndGun.Space
{
    public class Interfaces
    {

    }

    public interface IDamager
    {
        float DamageValue { get; set; }
        void DealDamage(IDamagable damagable);
    }

    public interface IDamagable
    {
        void TakeDamage(float damageValue);
    }

    public interface IObjectWithHealthBar
    {
        bool Alive { get; }
        Vector3 HealthBarPosition { get; }
        float CurrentHealth { get; }
        float MaxHealth { get; }
    }

    public interface ICollectible
    {

    }
}