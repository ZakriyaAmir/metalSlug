using UnityEngine;
using Cinemachine;

namespace RunAndGun.Space
{
    public class UserAimPointerTracker : MonoBehaviour
    {
        [SerializeField] private float minDistance = 0.5f;
        [SerializeField] private Transform playerCenter = null;
        [SerializeField] private CinemachineVirtualCamera cinemachine = null;
        [SerializeField] private LayerMask hitmask = 0;
        [SerializeField] private Transform AimCrossHairGameObject = null;
        [SerializeField] private Transform rightHandHoldTransform = null;
        private bool activated = true;
        private Vector3 mousePointerPosition = Vector3.zero;
        public Vector3 MousePointerPosition { get { return mousePointerPosition; } }
        private Camera mainCamera = null;
        private Ray ray;
        private RaycastHit hitInfo;

        private void Awake()
        {
            GameManager.Instance.OnGameStateChanged.AddListener(OnGameStateChanged);
        }

        private void Start()
        {
            mainCamera = Camera.main;
        }

        private void LateUpdate()
        {
            TrackMousePointer();
        }

        private void OnGameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.OnMainMenu:

                    break;
                case GameState.InGamePaused:
                    DisableAimCrossHairUpdater();
                    break;
                case GameState.InGameActive:
                    EnableAimCrossHairUpdater();
                    break;
                case GameState.PlayerDead:
                    DisableAimCrossHairUpdater();
                    break;
                case GameState.LevelVictory:
                    DisableAimCrossHairUpdater();
                    break;
                case GameState.LevelGameOver:
                    DisableAimCrossHairUpdater();
                    break;
                default: break;
            }
        }

        private void DisableAimCrossHairUpdater()
        {
            activated = false;
        }

        private void EnableAimCrossHairUpdater()
        {
            activated = true;
        }

        private void TrackMousePointer()
        {
            /*if (cinemachine != null && cinemachine.isActiveAndEnabled)
            {
                ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hitInfo, 99999f, hitmask))
                {
                    if (Vector3.Distance(hitInfo.point, playerCenter.transform.position) > minDistance)
                    {
                        mousePointerPosition = hitInfo.point;
                        if (AimCrossHairGameObject != null)
                        {
                            UpdateAimCrossHairPoistion();
                        }
                    }
                }
            }*/
            UpdateAimCrossHairPoistion();
        }

        private void UpdateAimCrossHairPoistion()
        {
            if (activated)
            {
                //AimCrossHairGameObject.transform.position = hitInfo.point;
                var joystick = GameManager.Instance.playerMovement.joystick;
                if (joystick.Direction.magnitude > 0.2f)
                {
                    var xDifference = playerCenter.position.x + joystick.Direction.x * 50;
                    var yDifference = playerCenter.position.y + joystick.Direction.y * 50;

                    AimCrossHairGameObject.transform.position = new Vector3(xDifference, yDifference,
                   (rightHandHoldTransform != null) ?
                       rightHandHoldTransform.position.z :
                       0
                   );
                }
            }
        }
    }
}