using UnityEngine;

namespace RunAndGun.Space
{
    [RequireComponent(typeof(SphereCollider))]
    public class SimpleBullet : MonoBehaviour, IDamager
    {
        [SerializeField] private LayerMask targetMask;
        [SerializeField] private LayerMask obstructionMask;
        [SerializeField] private float damageValue = 15f;
        [SerializeField] private float disappearTimeInSeccods = 30f;
        public float DamageValue { get { return damageValue; } set { damageValue = value; } }

        private Rigidbody rbody;
        private TrailRenderer trailRenderer;
        private bool activeBullet = false;
        private SpriteRenderer sprite;
        private Vector3 originalPosition;
        private float disappearTimer;
        private IDamagable damagable;

        private void Start()
        {
            rbody = GetComponent<Rigidbody>();
            trailRenderer = GetComponent<TrailRenderer>();
            sprite = GetComponentInChildren<SpriteRenderer>();
            originalPosition = this.transform.position;
            Disappear();
        }

        private void Update()
        {
            if(disappearTimer > 0)
            {
                disappearTimer -= Time.deltaTime;
                if(disappearTimer < 0)
                {
                    Disappear();
                }
            }
        }

        public void SendBullet(Vector3 direction, float speed)
        {
            Appear();
            rbody.velocity = direction * speed;
        }

        private void Appear()
        {
            rbody.velocity = Vector3.zero;
            disappearTimer = disappearTimeInSeccods;
            trailRenderer.enabled = true;
            sprite.enabled = true;
            activeBullet = true;
        }

        private void Disappear()
        {
            rbody.velocity = Vector3.zero;
            disappearTimer = 0f;
            trailRenderer.enabled = false;
            this.transform.position = originalPosition;
            sprite.enabled = false;
            activeBullet = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(activeBullet)
            {
                if ((targetMask.value & (1 << other.transform.gameObject.layer)) > 0)
                {
                    TryDealDamage(other);
                    Disappear();
                }
                if ((obstructionMask.value & (1 << other.transform.gameObject.layer)) > 0)
                {
                    Disappear();
                }
            }
        }

        private void TryDealDamage(Collider other)
        {
            damagable = other.GetComponent<IDamagable>();
            if(damagable != null)
            {
                DealDamage(damagable);
            }
        }

        public void DealDamage(IDamagable damagable)
        {
            damagable.TakeDamage(damageValue);
        }
    }
}