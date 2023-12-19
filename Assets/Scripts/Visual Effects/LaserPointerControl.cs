using UnityEngine;

namespace RunAndGun.Space
{
    public class LaserPointerControl : MonoBehaviour
    {
        [SerializeField] private float maxDistance = 50f;
        [SerializeField] private LayerMask hitMask = 0;
        [SerializeField] private LayerMask enemyLayerMask = 0;
        [SerializeField] private Transform laserBeamDirectionalScaler = null;
        [SerializeField] private Transform laserDotSprite = null;
        [SerializeField] private string hitName = "";
        private Ray ray;
        private RaycastHit hitInfo;
        private Transform hitTransform;
        private SpriteRenderer spriteRenderer;
        private IObjectWithHealthBar objectWithHealthBar;
        private bool isOnEnemyHitBox = false;

        private void Start()
        {
            ray = new Ray(transform.position, Vector3.left);
            spriteRenderer = laserDotSprite.GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = false;
        }

        private void LateUpdate()
        {
            RayCastLaserBeam();
            UpdateEnemyHealthBar_UI();
        }

        private void RayCastLaserBeam()
        {
            ray = new Ray(transform.position, transform.TransformDirection(Vector3.left));
            if (Physics.Raycast(ray, out hitInfo, maxDistance, hitMask))
            {
                RayHasHit();
            }
            else
            {
                RayHasNoHit();
            }
        }

        private void RayHasHit()
        {
            spriteRenderer.enabled = true;
            laserDotSprite.transform.position = hitInfo.point;
            if (hitTransform != hitInfo.transform)
            {
                hitTransform = hitInfo.transform;
                hitName = hitTransform.name;
                CheckIfOnEnemyHitBox();
            }
            laserBeamDirectionalScaler.transform.localScale = new Vector3(hitInfo.distance,
                                                                            laserBeamDirectionalScaler.transform.localScale.y,
                                                                            laserBeamDirectionalScaler.transform.localScale.z);
        }

        private void RayHasNoHit()
        {
            laserDotSprite.transform.position = this.transform.position;
            spriteRenderer.enabled = false;
            laserBeamDirectionalScaler.transform.localScale = new Vector3(maxDistance,
                                                                            laserBeamDirectionalScaler.transform.localScale.y,
                                                                            laserBeamDirectionalScaler.transform.localScale.z);
            ClearEnemyHealthBar_UI();
            hitTransform = null;
            hitName = "";
        }

        private void CheckIfOnEnemyHitBox()
        {
            isOnEnemyHitBox = false;
            if ((enemyLayerMask.value & (1 << hitInfo.collider.transform.gameObject.layer)) > 0)
            {
                objectWithHealthBar = hitInfo.collider.transform.GetComponent<IObjectWithHealthBar>();
                if (objectWithHealthBar != null && objectWithHealthBar.Alive)
                {
                    isOnEnemyHitBox = true;
                }
            }
            if (isOnEnemyHitBox)
            {
                GameManager.Instance.EnemyHealthBar_UI.ActivateSelf();
            }
            else
            {
                GameManager.Instance.EnemyHealthBar_UI.DeactivateSelf();
            }
        }

        private void UpdateEnemyHealthBar_UI()
        {
            if (isOnEnemyHitBox)
            {
                GameManager.Instance.EnemyHealthBar_UI.transform.position = objectWithHealthBar.HealthBarPosition;
                GameManager.Instance.EnemyHealthBar_UI.UpdateFill(objectWithHealthBar.CurrentHealth, objectWithHealthBar.MaxHealth);
            }
        }

        private void ClearEnemyHealthBar_UI()
        {
            GameManager.Instance.EnemyHealthBar_UI.DeactivateSelf();
        }
    }
}