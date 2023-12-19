using UnityEngine;
using UnityEngine.Events;

namespace RunAndGun.Space
{
    public class EnterTrigger : MonoBehaviour
    {
        [SerializeField] private LayerMask targetMask;
        [SerializeField] private bool runOnce = false;
        public UnityEvent triggerEnterEvent;
        private GameObject uniqueObject = null;

        private void OnTriggerEnter(Collider other)
        {
            if ((targetMask.value & (1 << other.transform.gameObject.layer)) > 0)
            {
                if (uniqueObject == null)
                {
                    uniqueObject = other.gameObject;
                    ActivateTrigger();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if ((targetMask.value & (1 << other.transform.gameObject.layer)) > 0)
            {
                uniqueObject = null;
            }
        }

        private void ActivateTrigger()
        {
            triggerEnterEvent?.Invoke();
            if (runOnce)
            {
                DisableSelf();
            }
        }

        private void DisableSelf()
        {
            this.gameObject.SetActive(false);
        }
    }
}