using UnityEngine;

namespace RunAndGun.Space
{
    [RequireComponent(typeof(Collider))]
    public class DNASampleCollectible : MonoBehaviour, ICollectible
    {
        [SerializeField] private LayerMask targetMask;
        [SerializeField] private int points = 1;
        private bool taken = false;

        private void OnTriggerEnter(Collider other)
        {
            if (!taken && (targetMask.value & (1 << other.transform.gameObject.layer)) > 0)
            {
                GivePoints();
            }
        }

        private void GivePoints()
        {
            taken = true;
            GameManager.Instance.PointsAdded(points);
            Destroy(this.gameObject);
        }
    }
}