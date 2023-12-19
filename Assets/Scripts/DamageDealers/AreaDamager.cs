using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace RunAndGun.Space
{
    public class AreaDamager : MonoBehaviour
    {
        [SerializeField] private LayerMask targetMask;
        [SerializeField] private float damageValue = 15f;
        [SerializeField] private float damageRateInSeconds = 1f;
        Dictionary<IDamagable, float> damageTakers;
        private void Start()
        {
            damageTakers = new Dictionary<IDamagable, float>();
        }

        
        private void Update()
        {
            if(damageTakers.Count > 0)
            {
                ManageDamageTakers();
            }
        }

        private void ManageDamageTakers()
        {
            for (int i = 0; i < damageTakers.Count; i++)
            {
                if (damageTakers.ElementAt(i).Value < 0)
                {
                    damageTakers.ElementAt(i).Key.TakeDamage(damageValue);
                    damageTakers[damageTakers.ElementAt(i).Key] = damageRateInSeconds;
                }
                else
                {
                    damageTakers[damageTakers.ElementAt(i).Key] -= Time.deltaTime;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((targetMask.value & (1 << other.transform.gameObject.layer)) > 0)
            {
                IDamagable damagable = other.GetComponent<IDamagable>();
                if (damagable != null)
                {
                    damageTakers.TryAdd(damagable, damageRateInSeconds);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if ((targetMask.value & (1 << other.transform.gameObject.layer)) > 0)
            {
                IDamagable damagable = other.GetComponent<IDamagable>();
                if (damagable != null)
                {
                    damageTakers.Remove(damagable);
                }
            }
        }
    }
}