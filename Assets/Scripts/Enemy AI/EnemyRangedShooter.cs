using UnityEngine;

namespace RunAndGun.Space
{
    public class EnemyRangedShooter : MonoBehaviour
    {
        [SerializeField] private float targetHeight = 2f;
        [SerializeField] private float damageDealSphereRadius = 1f;
        [SerializeField] private float damageDealValue = 5f;
        [SerializeField] private float speed = 25f;
        [SerializeField] private LayerMask targetMask = 0;
        [SerializeField] private Transform projectilePrefab;
        [SerializeField] private bool LowShoot = false;
        [SerializeField] public Transform shootingPoint;
        [SerializeField] public bool isBullet;

        private EnemyComponentsManager enemyComponentsManager;
        private Vector3 target;

        private void Awake()
        {
            enemyComponentsManager = GetComponentInParent<EnemyComponentsManager>();
            enemyComponentsManager.EnemyRangedShooter = this;
        }

        public void ShootAtTarget()
        {
            //Play enemy Attack sound
            if (transform.parent.GetComponent<EnemyComponentsManager>().enemyAttackAudioName != null && transform.parent.GetComponent<EnemyComponentsManager>().enemyAttackAudioName != "")
            {
                audioManager.instance.PlayAudio(transform.parent.GetComponent<EnemyComponentsManager>().enemyAttackAudioName, true, transform.position);
            }
            //
            Transform _projectile = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.identity);
            if (isBullet)
            {
                bullet_Damager _projectileDamager = _projectile.GetComponent<bullet_Damager>();
                _projectileDamager.ProjectileSettings = new ProjectileSettings(damageDealSphereRadius, damageDealValue, speed, targetMask);
                _projectileDamager.SendProjectile(target + new Vector3(0f, targetHeight, 0f), LowShoot);
            }
            else 
            {
                ProjectileDamager _projectileDamager = _projectile.GetComponent<ProjectileDamager>();
                _projectileDamager.ProjectileSettings = new ProjectileSettings(damageDealSphereRadius, damageDealValue, speed, targetMask);
                _projectileDamager.SendProjectile(target + new Vector3(0f, targetHeight, 0f), LowShoot);
            }
        }

        public void SetTarget(Vector3 target)
        {
            this.target = target;
        }
    }
}