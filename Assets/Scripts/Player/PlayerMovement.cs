using UnityEngine;
using System.Collections;
using UnityEngine.Animations.Rigging;

namespace RunAndGun.Space
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] public FixedJoystick joystick;
        [SerializeField] private IsGroundedControl isGroundedControl = null;
        public IsGroundedControl IsGroundedControl { get { return isGroundedControl; } set { isGroundedControl = value; } }
        public Animator playerAnim;
        [SerializeField] private AnimationCurve moveCurve = null;
        [SerializeField] private float jumpForce = 200f;
        [SerializeField] private float maxMoveSpeed = 250f;
        [SerializeField] private float maxMidAirMoveSpeed = 10f;
        [SerializeField] private float jumpForceWhenStuck = 150f;
        [SerializeField] private float stuckTimeDuration = 2f;
        private bool active = true;
        private float moveSpeedRatio = 0f;
        public float moveDirectionX = 1f;
        private float stuckTimer = 0f;
        private bool jumping = false;
        private bool stuckJump = false;
        public Rigidbody rigidBody;
        public Transform gunsParent;
        public TwoBoneIKConstraint leftHandIK;

        public float HorizontalVelocity { get { return transform.InverseTransformVector(rigidBody.velocity).x; } }
        public float VecrticalVelocity { get { return rigidBody.velocity.y; } }

        private void Awake()
        {
            GameManager.Instance.playerMovement = this;
            GameManager.Instance.OnGameStateChanged.AddListener(OnGameStateChanged);
            GameManager.Instance.playerTransform = this.transform;
        }

        private void Start()
        {
            joystick = FindObjectOfType<FixedJoystick>();
            rigidBody = GetComponent<Rigidbody>();
            isGroundedControl = GameManager.Instance.isGroundedControl;
        }

        private void Update()
        {
            ManageStuck();
        }

        public void Move(float direction)
        {
            if (isGroundedControl.IsGrounded || true)
            {
                if (direction < -0.01f)
                {
                    moveDirectionX = -1f;
                }
                else if (direction > 0.01f)
                {
                    moveDirectionX = 1f;
                }
                else
                {
                    moveDirectionX = 0f;   
                }
                moveSpeedRatio = moveCurve.Evaluate(Mathf.Abs(direction)) * moveDirectionX;
            }
        }

        public void Jump()
        {
            if (isGroundedControl.IsGrounded)
            {
                jumping = true;

            }
        }

        public void Crouch()
        {
            if (isGroundedControl.IsGrounded)
            {
                if (playerAnim.GetBool("crouch"))
                {
                    playerAnim.SetBool("crouch", false);
                    crouchUnShrinkCollider();
                }
                else
                {
                    playerAnim.SetBool("crouch", true);
                    crouchShrinkCollider();
                }
            }
        }

        public void crouchShrinkCollider()
        {
            var col = GetComponents<CapsuleCollider>();
            foreach (CapsuleCollider coli in col) 
            {
                coli.center = new Vector3(0, 0.6f, 0);
                coli.height = 0.42f;
            }
        }

        public void crouchUnShrinkCollider()
        {
            var col = GetComponents<CapsuleCollider>();
            foreach (CapsuleCollider coli in col)
            {
                coli.center = new Vector3(0, 1f, 0);
                coli.height = 1.9f;
            }
        }

        private void FixedUpdate()
        {
            if (active)
            {
                if (moveSpeedRatio > 0.01f || moveSpeedRatio < -0.01f)
                {
                    if (isGroundedControl.IsGrounded)
                    {
                        rigidBody.velocity = new Vector3((maxMoveSpeed * moveSpeedRatio * Time.fixedDeltaTime), rigidBody.velocity.y, rigidBody.velocity.z);
                    }
                    else
                    {
                        rigidBody.velocity = new Vector3(
                            Mathf.Clamp(
                                rigidBody.velocity.x + (maxMidAirMoveSpeed * moveSpeedRatio * Time.fixedDeltaTime),
                                    -(maxMidAirMoveSpeed + maxMoveSpeed) * Time.fixedDeltaTime,
                                    (maxMidAirMoveSpeed + maxMoveSpeed) * Time.fixedDeltaTime),
                                rigidBody.velocity.y,
                                rigidBody.velocity.z);
                    }
                    moveSpeedRatio = 0f;

                    if (playerAnim.GetBool("crouch"))
                    {
                        Crouch();
                    }
                }

                if (jumping)
                {
                    rigidBody.velocity = new Vector3(rigidBody.velocity.x, jumpForce * Time.fixedDeltaTime, rigidBody.velocity.z);
                    jumping = false;

                    if (playerAnim.GetBool("crouch"))
                    {
                        Crouch();
                    }
                }
                if (stuckJump)
                {
                    rigidBody.velocity = new Vector3(rigidBody.velocity.x, jumpForceWhenStuck * Time.fixedDeltaTime, rigidBody.velocity.z);
                    stuckJump = false;
                }
            }
        }

        private void ManageStuck()
        {
            if (active)
            {
                if (rigidBody.velocity.magnitude < 0.2f && !isGroundedControl.IsGrounded)
                {
                    if (stuckTimer < stuckTimeDuration)
                    {
                        stuckTimer += Time.deltaTime;
                    }
                    else
                    {
                        stuckJump = true;
                        stuckTimer = 0f;
                    }
                }
                else
                {
                    stuckTimer = 0f;
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

                    break;
                case GameState.InGameActive:

                    break;
                case GameState.PlayerDead:
                    DisablePlayerMovement();            
                    break;
                case GameState.LevelVictory:

                    break;
                case GameState.LevelGameOver:

                    break;
                default: break;
            }
        }

        private void DisablePlayerMovement()
        {
            active = false;
        }

        private void EnablePlayerMovement()
        {
            active = false;
        }
    }
}