using UnityEngine;

namespace RunAndGun.Space
{
    public class EnemyHealth : MonoBehaviour, IDamagable, IObjectWithHealthBar
    {
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float currentHealth = 100f;
        [SerializeField] private LayerMask hitDetectFromPlayerLayer = 0;
        [SerializeField] private LayerMask playerAttackLayer = 0;
        [SerializeField] private Transform heathBarLocationTransform;

        private Collider _collider;
        private EnemyComponentsManager enemyComponentsManager;
        private IDamager damager;
        private bool alive = true;
        public Vector3 HealthBarPosition { get { return heathBarLocationTransform.position; } }
        public float CurrentHealth { get { return currentHealth; } }
        public float MaxHealth { get { return maxHealth; } }
        public bool Alive { get { return alive; } }

        private void Awake()
        {
            enemyComponentsManager = GetComponentInParent<EnemyComponentsManager>();
            enemyComponentsManager.OnTakeDamage += OnTakeDamage;
            _collider = GetComponent<Collider>();
        }

        private void OnDestroy()
        {
            enemyComponentsManager.OnTakeDamage -= OnTakeDamage;
        }

        private void HealthPointsUpdated(float value1, float value2)
        {
            currentHealth = value1;
            maxHealth = value2;
        }

        private void OnTakeDamage(float damageValue)
        {
            currentHealth -= damageValue;
            if (currentHealth <= 0)
            {
                alive = false;
                _collider.enabled = false;
                enemyComponentsManager.Die();
            }
            else
            {
                enemyComponentsManager.TargetSpotter.AlertOnTheTarget(GameManager.Instance.playerTransform);
            }
        }

        public void TakeDamage(float damageValue)
        {
            enemyComponentsManager.TakeDamage(damageValue);
        }
    }
}