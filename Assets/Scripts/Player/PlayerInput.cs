using UnityEngine;
namespace RunAndGun.Space
{
    public class PlayerInput : MonoBehaviour
    {
        public bool Activated { get { return activated; } set { activated = value; } }
        private bool activated = false;
        [SerializeField] private float deadZone = 0.01f;
        private PlayerMovement playerMovement = null;
        private RecoilControl recoilControl = null;
        public Weapon weapon;
        private float horizontalInput = 0f;
        private float horizontalInput2 = 0f;

        private void Awake()
        {
            GameManager.Instance.OnGameStateChanged.AddListener(OnGameStateChanged);
            GameManager.Instance.playerInput = this;
        }

        private void Start()
        {
            activated = true;
            playerMovement = GameManager.Instance.playerMovement;
            recoilControl = GameManager.Instance.recoilControl;
            weapon = GameManager.Instance.weapon;
        }

        private void Update()
        {
            if (activated)
            {
                // tracking horizontal inputs with keyboard buttons
                horizontalInput = 0f;
                horizontalInput += Input.GetAxis(GlobalStringVars.HORIZONTAL_AXIS);
                if (horizontalInput > deadZone || horizontalInput < -deadZone)
                {
                    playerMovement.Move(horizontalInput);
                }

                // tracking horizontal inputs with joystick
                horizontalInput2 = 0f;
                horizontalInput2 += GameManager.Instance.playerMovement.joystick.Direction.x;
                if (horizontalInput2 > deadZone || horizontalInput2 < -deadZone)
                {
                    playerMovement.Move(horizontalInput2);
                }

                // tracking jump button press
                if (Input.GetButtonDown(GlobalStringVars.JUMP_BUTTON))
                {
                    playerMovement.Jump();
                }
                // tracking fire button press
                if (Input.GetButton(GlobalStringVars.FIRE_1))
                {
                    weapon.TryShoot();
                }
                // tracking reload button pressed
                if (Input.GetKeyDown(GlobalStringVars.WEAPON_RELOAD_KEY))
                {
                    weapon.ReloadWeaponStart();
                }
                if (Input.GetKeyDown(GlobalStringVars.CROUCH))
                {
                    playerMovement.Crouch();
                }
            }
            else
            {
                horizontalInput = 0f;
            }
        }

        private void DisableInput()
        {
            activated = false;
        }

        private void EnableInput()
        {
            activated = true;
        }

        private void OnGameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.OnMainMenu:

                    break;
                case GameState.InGamePaused:
                    DisableInput();
                    break;
                case GameState.InGameActive:
                    EnableInput();
                    break;
                case GameState.PlayerDead:
                    DisableInput();
                    break;
                case GameState.LevelVictory:
                    DisableInput();
                    break;
                case GameState.LevelGameOver:
                    DisableInput();
                    break;
                default: break;
            }
        }
    }
}