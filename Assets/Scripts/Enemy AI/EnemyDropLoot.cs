using UnityEngine;

namespace RunAndGun.Space
{
    public class EnemyDropLoot : MonoBehaviour
    {
        [SerializeField] private Transform HealthPickup;
        [SerializeField] private int HealthDropRateMin = 0;
        [SerializeField] private int HealthDropRateMax = 1;
        [SerializeField] private Transform DNASamplePickUp;
        [SerializeField] private int DNADropRateMin = 1;
        [SerializeField] private int DNADropRateMax = 3;

        private Transform[] dropLoot;
        private Rigidbody[] dropLootRigidBody;
        private int totalDropLootNumber = 0;
        private EnemyComponentsManager enemyComponentsManager;

        private void Awake()
        {
            enemyComponentsManager = GetComponentInParent<EnemyComponentsManager>();
            enemyComponentsManager.OnDeath += OnDeath;
            int healthDropRate = Random.Range(HealthDropRateMin, HealthDropRateMax + 1);
            int dnaDropRate = Random.Range(DNADropRateMin, DNADropRateMax + 1);
            totalDropLootNumber = healthDropRate + dnaDropRate;
            if (totalDropLootNumber > 0)
            {
                dropLoot = new Transform[totalDropLootNumber];
                dropLootRigidBody = new Rigidbody[totalDropLootNumber];
                int x = 0;
                Transform clone = null;
                for (int i = 0; i < healthDropRate; i++)
                {
                    clone = Instantiate(HealthPickup, this.transform.position, Quaternion.identity, this.transform);
                    clone.gameObject.SetActive(false);
                    dropLoot[x] = clone;
                    dropLootRigidBody[x] = clone.GetComponent<Rigidbody>();
                    x++;
                }
                for (int i = 0; i < dnaDropRate; i++)
                {
                    clone = Instantiate(DNASamplePickUp, this.transform.position, Quaternion.identity, this.transform);
                    clone.gameObject.SetActive(false);
                    dropLoot[x] = clone;
                    dropLootRigidBody[x] = clone.GetComponent<Rigidbody>();
                    x++;
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
                for (int i = 0; i < totalDropLootNumber; i++)
                {
                    dropLoot[i].parent = null;
                    dropLoot[i].gameObject.SetActive(true);
                    dropLootRigidBody[i].velocity = new Vector3(Random.Range(-4f, 4f), Random.Range(2f, 4f), 0);
                }
            }
        }
    }
}