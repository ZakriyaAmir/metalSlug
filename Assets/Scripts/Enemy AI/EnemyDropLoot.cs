using System.Collections.Generic;
using UnityEngine;

namespace RunAndGun.Space
{
    public class EnemyDropLoot : MonoBehaviour
    {
        [SerializeField] private Transform HealthPickup;
        [SerializeField] private int HealthDropRateMin = 0;
        [SerializeField] private int HealthDropRateMax = 1;
        [SerializeField] private Transform CoinPickUp;
        [SerializeField] private int CoinDropRateMin = 1;
        [SerializeField] private int CoinDropRateMax = 3;
        [SerializeField] private Transform WeaponPickUp;
        [SerializeField] private int WeaponDropRateMin = 1;
        [SerializeField] private int WeaponDropRateMax = 3;

        public List<Transform> dropLoot;
        private int totalDropLootNumber = 0;
        private EnemyComponentsManager enemyComponentsManager;

        private void Awake()
        {
            enemyComponentsManager = GetComponentInParent<EnemyComponentsManager>();
            enemyComponentsManager.OnDeath += OnDeath;
            int healthDropRate = Random.Range(HealthDropRateMin, HealthDropRateMax + 1);
            int coinDropRate = Random.Range(CoinDropRateMin, CoinDropRateMax + 1);
            int weaponDropRate = Random.Range(WeaponDropRateMin, WeaponDropRateMax + 1);
            int totalCount = 0;
            if (HealthPickup) 
            {
                totalCount++;
            }
            if (CoinPickUp)
            {
                totalCount++;
            }
            if (WeaponPickUp)
            {
                totalCount++;
            }

            totalDropLootNumber = totalCount;
            if (totalDropLootNumber > 0)
            {
                //dropLoot = new Transform[totalDropLootNumber];
                int x = 0;
                Transform clone = null;
                if (HealthPickup != null)
                {
                    for (int i = 0; i < healthDropRate; i++)
                    {
                        clone = Instantiate(HealthPickup, this.transform.position, Quaternion.identity, this.transform);
                        clone.gameObject.SetActive(false);
                        dropLoot.Add(clone);
                        x++;
                    }
                }
                if (CoinPickUp != null)
                {
                    for (int i = 0; i < coinDropRate; i++)
                    {
                        clone = Instantiate(CoinPickUp, this.transform.position, Quaternion.identity, this.transform);
                        clone.gameObject.SetActive(false);
                        dropLoot.Add(clone);
                        x++;
                    }
                }
                if (WeaponPickUp != null)
                {
                    for (int i = 0; i < weaponDropRate; i++)
                    {
                        clone = Instantiate(WeaponPickUp, this.transform.position, Quaternion.identity, this.transform);
                        clone.gameObject.SetActive(false);
                        dropLoot.Add(clone);
                        x++;
                    }
                }
            }
        }

        private void OnDestroy()
        {
            enemyComponentsManager.OnDeath -= OnDeath;
        }
        private void OnDeath()
        {
            if (totalDropLootNumber > 0)
            {
                foreach (var item in dropLoot) 
                {
                    item.parent = null;
                    item.gameObject.SetActive(true);
                    item.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-4f, 4f), Random.Range(2f, 4f), 0);
                }
            }
        }
    }
}