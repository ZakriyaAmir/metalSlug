using UnityEngine;

namespace RunAndGun.Space
{
    public class EnemyAIControl : MonoBehaviour
    {
        [SerializeField] private float lostDuration = 30f;
        [SerializeField] private float distanceToInteract = 1f;
        [SerializeField] private float distanceToInvestigate = 1f;
        [SerializeField] private Vector3 originalPosition;

        private EnemyComponentsManager enemyComponentsManager;
        private TargetSpotter targetSpotter = null;
        private EnemyMovement enemyMovement = null;
        private EnemyAnimator enemyAnimator = null;
        private EnemyRangedShooter enemyRangedShooter = null;
        private float _distanceToTarget;
        private float _distanceToLastKnownPosition;
        private float _distanceToOriginalPost;
        private float _xDifference;
        private bool _lookLeft = true;
        private EnemySpotState _enemySpotState;
        private bool alive = true;

        private void Awake()
        {
            enemyComponentsManager = GetComponent<EnemyComponentsManager>();
            enemyComponentsManager.OnSpotStateChanged += OnSpotDataUpdated;
            enemyComponentsManager.OnDeath += OnDeath;
        }

        private void Start()
        {
            enemyMovement = enemyComponentsManager.EnemyMovement;
            enemyAnimator = enemyComponentsManager.EnemyAnimator;
            targetSpotter = enemyComponentsManager.TargetSpotter;
            enemyRangedShooter = enemyComponentsManager.EnemyRangedShooter;
            originalPosition = transform.position;
            CheckLookDirectionAtStart();
        }

        private void OnDestroy()
        {
            enemyComponentsManager.OnSpotStateChanged -= OnSpotDataUpdated;
            enemyComponentsManager.OnDeath -= OnDeath;
        }

        private void Update()
        {
            if(alive)
            {
                ManageSpotData();
            }
        }

        private void CheckLookDirectionAtStart()
        {
            if (this.transform.localRotation.eulerAngles.y == 90)
            {
                _lookLeft = false;
            }
            else if (this.transform.localRotation.eulerAngles.y == 270)
            {
                _lookLeft = true;
            }
            else
            {
                this.transform.Rotate(0, 270, 0);
                _lookLeft = true;
            }
        }

        private void OnSpotDataUpdated(EnemySpotState enemySpotState)
        {
            _enemySpotState = enemySpotState;
        }

        private void ManageSpotData()
        {
            switch (_enemySpotState)
            {
                case EnemySpotState.NoTarget:
                    ReturnToOriginalPost();
                    break;
                case EnemySpotState.TargetIsVisible:
                    ActionOnTarget();
                    break;
                case EnemySpotState.AlertedOnTarget:
                    GoToTargetsLastKnowPosition();
                    break;
                case EnemySpotState.TargetLost:
                    GoToTargetsLastKnowPosition();
                    break;
                default: break;
            }
        }

        private void OnDeath()
        {
            alive = false;
            enemyMovement.StopMoving();
        }

        private void ReturnToOriginalPost()
        {
            _distanceToOriginalPost = Mathf.Abs(originalPosition.x - this.transform.position.x);
            if (_distanceToOriginalPost > distanceToInvestigate)
            {
                MoveTowards(originalPosition);
            }
            else
            {
                enemyMovement.StopMoving();
                enemyAnimator.StopAnimateAttack();
                enemyAnimator.AnimateIdle();
            }
        }

        private void ActionOnTarget()
        {
            _distanceToTarget = Mathf.Abs(transform.position.x - targetSpotter.SpotData.targetTransform.position.x);
            if (_distanceToTarget > distanceToInteract)
            {
                MoveTowards(targetSpotter.SpotData.targetTransform.position);
            }
            else
            {
                enemyMovement.StopMoving();
                enemyAnimator.StopAnimateIdle2();
                enemyAnimator.AnimateAttack();
                if(enemyRangedShooter != null && targetSpotter.SpotData.enemySpotState == EnemySpotState.TargetIsVisible)
                {
                    enemyRangedShooter.SetTarget(
                        targetSpotter.SpotData.targetTransform.position);
                }
            }
        }

        private void GoToTargetsLastKnowPosition()
        {
            if (targetSpotter.SpotData.spotTime + lostDuration > Time.time)
            {
                _distanceToLastKnownPosition = Mathf.Abs(transform.position.x - targetSpotter.SpotData.lastKnownPosition.x);
                if (_distanceToLastKnownPosition > distanceToInvestigate)
                {
                    MoveTowards(targetSpotter.SpotData.lastKnownPosition);
                }
                else
                {
                    enemyMovement.StopMoving();
                    enemyAnimator.StopAnimateAttack();
                    enemyAnimator.AnimateIdle2();
                }
            }
            else
            {
                targetSpotter.ForgetTheTarget();
            }
        }

        private void MoveTowards(Vector3 destination)
        {
            _xDifference = destination.x - this.transform.position.x;
            if (_xDifference > 0.01f && _lookLeft)
            {
                this.transform.Rotate(0, 180, 0);
                _lookLeft = false;
            }
            else if (_xDifference < -0.01f && !_lookLeft)
            {
                this.transform.Rotate(0, 180, 0);
                _lookLeft = true;
            }
            enemyAnimator.StopAnimateAttack();
            enemyAnimator.StopAnimateIdle2();
            enemyMovement.Moving();
        }
    }
}
