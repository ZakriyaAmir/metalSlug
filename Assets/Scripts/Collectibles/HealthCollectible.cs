using UnityEngine;

namespace RunAndGun.Space
{
    [RequireComponent(typeof(Collider))]
    public class HealthCollectible : MonoBehaviour, ICollectible
    {
        [SerializeField] private float HealthRegenPoints = 15f;
        [SerializeField] private LayerMask targetMask;
        private IDamagable picker;
        private bool HealthGiven = false;


        private void OnTriggerEnter(Collider other)
        {
            if (!HealthGiven && (targetMask.value & (1 << other.transform.gameObject.layer)) > 0)
            {
                picker = other.transform.GetComponent<IDamagable>();
                if(picker != null)
                {
                    Heal();
                }
            }
        }

        private void Heal()
        {
            picker.TakeDamage(-HealthRegenPoints);
            HealthGiven = true;
            Destroy(this.gameObject);
        }
    }
}