using UnityEngine;

namespace RunAndGun.Space
{
    public class FlipFaceControl : MonoBehaviour
    {
        [SerializeField] private Transform aimTarget = null;
        public Transform AimTarget { get { return aimTarget; } set { aimTarget = value; } }
        [SerializeField] private Transform parentTransform = null;
        public Transform ParentTransform { get { return parentTransform; } set { parentTransform = value; } }

        private float xDifference = 0f;
        private bool lookLeft = false;
        private bool active = true;

        private void Awake()
        {
            GameManager.Instance.OnGameStateChanged.AddListener(OnGameStateChanged);
        }

        private void Start()
        {
            parentTransform = this.transform;
            if (aimTarget != null)
            {
                xDifference = aimTarget.position.x - parentTransform.position.x;
                if (xDifference < 0)
                {
                    lookLeft = true;
                }
            }

            active = true;
        }

        private void Update()
        {
            if (active)
            {
                //xDifference = aimTarget.position.x - parentTransform.position.x;
                xDifference = GameManager.Instance.playerMovement.moveDirectionX;
                if (xDifference < -0.01f && !lookLeft)
                {
                    parentTransform.Rotate(0, 180, 0);
                    lookLeft = true;
                }
                else if (xDifference > 0.01f && lookLeft)
                {
                    parentTransform.Rotate(0, 180, 0);
                    lookLeft = false;
                }
            }
        }
        private void OnGameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.OnMainMenu:

                    break;
                case GameState.InGamePaused:
                    PauseFlipFace();
                    break;
                case GameState.InGameActive:
                    ResumeFlipFace();
                    break;
                case GameState.PlayerDead:
                    DisableFlipFace();
                    break;
                case GameState.LevelVictory:
                    DisableFlipFace();
                    break;
                case GameState.LevelGameOver:
                    DisableFlipFace();
                    break;
                default: break;
            }
        }

        private void PauseFlipFace()
        {
            enabled = false;
        }

        private void ResumeFlipFace()
        {
            enabled = true;
        }
        private void DisableFlipFace()
        {
            active = false;
        }

        private void EnableFlipFace()
        {
            active = true;
        }
    }
}