using UnityEngine;

namespace RunAndGun.Space
{
    [RequireComponent(typeof(Rigidbody))]
    public class ProjectileDamager : MonoBehaviour
    {
        public ProjectileSettings ProjectileSettings;
        public LayerMask DestroyMask;
        [SerializeField] private ParticleSystem tailPS;
        [SerializeField] private ParticleSystem blowPS;
        [SerializeField] private float destroyTime = 5f;
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
                BlowUpProjectile();
                DestroyProjectile();
            }
        }

        public void SendProjectile(Vector3 target, bool lowShoot)
        {
            this.target = target;
            float? angle = CalculateAnge(target, lowShoot);
            if (angle != null)
            {
                float _angle = (float)angle;
                this.transform.LookAt(new Vector3(target.x, this.transform.position.y, target.z));
                this.transform.RotateAround(this.transform.position, this.transform.right, -_angle);
                rbody.useGravity = true;
                rbody.velocity = this.transform.forward * ProjectileSettings.speed;
            }
            else
            {
                Debug.Log("COULDN'T REACH");
            }
        }

        private float? CalculateAnge(Vector3 target, bool low = false)
        {
            Vector3 targetDirection = target - this.transform.position;
            float y = targetDirection.y;
            targetDirection.y = 0f;
            float x = targetDirection.magnitude;
            float gravity = Physics.gravity.y * -1f;
            float speedSqr = ProjectileSettings.speed * ProjectileSettings.speed;
            float underTheSqrRoot = (speedSqr * speedSqr) - gravity * (gravity * x * x + 2 * y * speedSqr);
            if (underTheSqrRoot >= 0f)
            {
                float root = Mathf.Sqrt(underTheSqrRoot);
                float highAngle = speedSqr + root;
                float lowAngle = speedSqr - root;
                if (low)
                {
                    return Mathf.Atan2(lowAngle, gravity * x) * Mathf.Rad2Deg;
                }
                else
                {
                    return Mathf.Atan2(highAngle, gravity * x) * Mathf.Rad2Deg;
                }
            }
            else
            {
                return null;
            }
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
            tailPS.transform.parent = null;
            emissionModule = tailPS.emission;
            emissionModule.enabled = false;
            startLifeTime = tailPS.main.startLifetime;
            Destroy(tailPS.gameObject, startLifeTime.constant);
            blowPS.transform.parent = null;
            blowPS.Play();
            Destroy(this.gameObject);
        }

    }
}