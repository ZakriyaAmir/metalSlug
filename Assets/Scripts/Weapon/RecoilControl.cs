using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace RunAndGun.Space
{
    public class RecoilControl : MonoBehaviour
    {
        [SerializeField] private MultiAimConstraint rightHandMultiAimConstraint = null;
        [SerializeField] private float lerpRate = 25f;
        [SerializeField] private float returnRate = 40f;
        [SerializeField] private float recoilUpRate = 5f;
        [SerializeField] private float randomHorizontalRSwayRate = 0.5f;
        [SerializeField] private float maxUpRecoil = 10f;
        [SerializeField] private float maxHorizontalRecoil = 5f;
        private float maxRecoilOffset = 0f;
        private float actualDifference = 0f;
        private Vector3 targetOffset = Vector3.zero;
        private Vector3 initialOffset = Vector3.zero;
        private bool recoilIsActive = false;

        private void Awake()
        {
            GameManager.Instance.recoilControl = this;
        }

        private void Start()
        {
            initialOffset = rightHandMultiAimConstraint.data.offset;
            maxRecoilOffset = initialOffset.z + maxUpRecoil;
        }

        private void Update()
        {
            if(recoilIsActive)
            {
                actualDifference = Vector3.Distance(targetOffset, rightHandMultiAimConstraint.data.offset);
                rightHandMultiAimConstraint.data.offset = Vector3.Lerp(rightHandMultiAimConstraint.data.offset, targetOffset, lerpRate * Time.deltaTime);
                targetOffset = Vector3.MoveTowards(targetOffset, initialOffset, returnRate * Time.deltaTime);
                if(actualDifference < 0.001f)
                {
                    rightHandMultiAimConstraint.data.offset = initialOffset;
                    recoilIsActive = false;
                }
            }
        }

        public void CallRecoil()
        {
            targetOffset = new Vector3(
                    Mathf.Clamp(targetOffset.x + Random.Range(targetOffset.x - randomHorizontalRSwayRate, targetOffset.x + randomHorizontalRSwayRate),
                        initialOffset.x - maxHorizontalRecoil, initialOffset.x + maxHorizontalRecoil),
                    targetOffset.y,
                    Mathf.Clamp(targetOffset.z + recoilUpRate, initialOffset.z, maxRecoilOffset));
            recoilIsActive = true;
        }
    }
}