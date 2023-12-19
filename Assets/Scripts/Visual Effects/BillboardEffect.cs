using UnityEngine;

namespace RunAndGun.Space
{
    public class BillboardEffect : MonoBehaviour
    {
        private Camera mainCam;
        public bool applyOnlyAtStart = true;
        private void Start()
        {
            mainCam = Camera.main;
            if (applyOnlyAtStart)
            {
                ApplyBillBoardEffect();
            }
        }
        private void LateUpdate()
        {
            if (!applyOnlyAtStart)
            {
                ApplyBillBoardEffect();
            }
        }

        private void ApplyBillBoardEffect()
        {
            // rotate as camera rotates
            transform.rotation = mainCam.transform.rotation;
            // remove rotation on X and Z
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        }
    }
}