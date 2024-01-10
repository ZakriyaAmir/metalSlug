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
            //Play enemy Attack sound
            if (transform.parent.GetComponent<EnemyComponentsManager>().enemyAttackAudioName != null && transform.parent.GetComponent<EnemyComponentsManager>().enemyAttackAudioName != "")
            {
                audioManager.instance.PlayAudio(transform.parent.GetComponent<EnemyComponentsManager>().enemyAttackAudioName, true, transform.position);
            }
            //
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