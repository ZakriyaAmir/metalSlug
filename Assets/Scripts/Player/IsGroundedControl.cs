using UnityEngine;
namespace RunAndGun.Space
{
    [RequireComponent(typeof(SphereCollider))]
    public class IsGroundedControl : MonoBehaviour
    {
        public bool IsGrounded { get { return isGrounded; } }
        [SerializeField] bool isGrounded = false;
        [SerializeField] private LayerMask groundedMask = 0;
        private LayerMask actualMask = 0;
        private SphereCollider sphereCollider;

        private void Awake()
        {
            GameManager.Instance.isGroundedControl = this;
        }

        private void Start()
        {
            sphereCollider = GetComponent<SphereCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if((groundedMask.value & (1 << other.transform.gameObject.layer)) > 0)
            {
                isGrounded = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if ((groundedMask.value & (1 << other.transform.gameObject.layer)) > 0)
            {
                isGrounded = false;
            }
        }
    }
}