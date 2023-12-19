using UnityEngine;

namespace RunAndGun.Space
{
    public class MeleeDamager : MonoBehaviour
    {
        [SerializeField] private float damageDealSphereRadius = 1f;
        [SerializeField] private float damageDealValue = 5f;
        [SerializeField] private LayerMask targetMask = 0;

        private IDamagable damagable;
        private GameObject distinctObject = null;

        public void InstantiateMeleeDamage()
        {
            Collider[] colliders = Physics.OverlapSphere(this.transform.position, damageDealSphereRadius, targetMask);
            if (colliders.Length > 1)
            {
                foreach (Collider item in colliders)
                {
                    if(distinctObject != item.gameObject)
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
                        damagable.TakeDamage(damageDealValue);
                    }
                }
                distinctObject = null;
            }
        }
    }
}