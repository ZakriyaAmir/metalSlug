using UnityEngine;

namespace RunAndGun.Space
{
    public class PlayerAnimatorControl : MonoBehaviour
    {
        [SerializeField] private PlayerMovement playerMovement = null;
        [SerializeField] private IsGroundedControl isGroundedControl = null;
        public PlayerMovement PlayerMovement { get { return playerMovement; } set { playerMovement = value; } }
        public IsGroundedControl IsGroundedControl { get { return isGroundedControl; } set { isGroundedControl = value; } }
        private Animator animator = null;

        private void Awake()
        {
            GameManager.Instance.OnGameStateChanged.AddListener(OnGameStateChanged);
            GameManager.Instance.OnPlayerWeaponReloadStart.AddListener(OnPlayerWeaponReload);
        }

        private void Start()
        {
            animator = GetComponent<Animator>();
            playerMovement = GameManager.Instance.playerMovement;
            isGroundedControl = GameManager.Instance.isGroundedControl;
        }

        private void Update()
        {
            if (playerMovement.joystick.Direction.x != 0)
            {
                animator.SetFloat("MoveSpeed", Mathf.Round(playerMovement.HorizontalVelocity * 100f) / 100f);
            }
            else
            {
                //Instantly stop character when joystick is not moving
                animator.SetFloat("MoveSpeed", 0);
                playerMovement.rigidBody.velocity = new Vector3(0, playerMovement.rigidBody.velocity.y, playerMovement.rigidBody.velocity.z);
            }

            animator.SetFloat("VerticalSpeed", Mathf.Round(playerMovement.VecrticalVelocity * 100f) / 100f);
            animator.SetBool("Grounded", isGroundedControl.IsGrounded);
        }

        private void OnPlayerWeaponReload()
        {
            animator.SetTrigger("Reload");
        }

        private void PlayDeathAnimation()
        {
            animator.SetTrigger("Die");
            animator.SetLayerWeight(animator.GetLayerIndex("LowerBody"), 0);
            animator.SetLayerWeight(animator.GetLayerIndex("UpperBody"), 0);
            animator.SetLayerWeight(animator.GetLayerIndex("Body"), 1);
        }

        private void OnGameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.OnMainMenu:

                    break;
                case GameState.InGamePaused:

                    break;
                case GameState.InGameActive:

                    break;
                case GameState.PlayerDead:
                    PlayDeathAnimation();
                    break;
                case GameState.LevelVictory:

                    break;
                case GameState.LevelGameOver:

                    break;
                default: break;
            }
        }
    }
}