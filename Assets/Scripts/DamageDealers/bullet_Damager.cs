using UnityEngine;

namespace RunAndGun.Space
{
    [RequireComponent(typeof(Rigidbody))]
    public class bullet_Damager : MonoBehaviour
    {
        public ProjectileSettings ProjectileSettings;
        public LayerMask DestroyMask;
        [SerializeField] private float destroyTime = 2f;
        public Vector3 target;
        private float destroyTimer;
        private IDamagable damagable;
        private GameObject distinctObject = null;
        private Rigidbody rbody;
        private ParticleSystem.EmissionModule emissionModule;
        private ParticleSystem.MinMaxCurve startLifeTime;

        private void Awake()
        {
            rbody = GetComponent<Rigidbody>();
            rbody.useGravity = false;
            destroyTimer = destroyTime;
        }


        private void Update()
        {
            if (destroyTimer > 0f)
            {
                destroyTimer -= Time.deltaTime;
            }
            else
            {
                DestroyProjectile();
            }
        }

        public void SendProjectile(Vector3 target, bool lowShoot)
        {
            this.target = target;
            this.transform.LookAt(new Vector3(target.x,transform.position.y, transform.position.z));
            rbody.velocity = this.transform.forward * ProjectileSettings.speed;
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((ProjectileSettings.targetMask.value & (1 << other.transform.gameObject.layer)) > 0
                ||
                (DestroyMask.value & (1 << other.transform.gameObject.layer)) > 0)
            {
                BlowUpProjectile();
                DestroyProjectile();
            }
        }

        private void BlowUpProjectile()
        {
            Collider[] colliders = Physics.OverlapSphere(this.transform.position, ProjectileSettings.damageDealSphereRadius, ProjectileSettings.targetMask);
            if (colliders.Length > 1)
            {
                foreach (Collider item in colliders)
                {
                    if (distinctObject != item.gameObject)
                    {
                        distinctObject = item.gameObject;
                    }
                    else
                    {
                        continue;
                    }
                    damagable = item.GetComponent<IDamagable>();
                    if (damagable != null)
                    {
                        damagable.TakeDamage(ProjectileSettings.damageDealValue);
                    }
                }
                distinctObject = null;
            }
        }

        private void DestroyProjectile()
        {
            Destroy(this.gameObject);
        }
    }
}