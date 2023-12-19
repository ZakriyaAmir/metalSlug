using UnityEngine;

namespace RunAndGun.Space
{
    public class TargetSpotter : MonoBehaviour
    {
        [SerializeField] private LayerMask targetMask = 0;
        [SerializeField] private LayerMask obstructionMask = 0;

        private EnemyComponentsManager enemyComponentsManager;
        private GroupTargetSpotter groupTargetSpotter = null;
        private TargetSpotData spotData;
        public TargetSpotData SpotData { get { return spotData; } set { spotData = value; } }
        private Ray ray;
        private Vector3 directionToTarget;
        private float distanceToTarget;
        private bool alive = true;

        private void Awake()
        {
            enemyComponentsManager = GetComponentInParent<EnemyComponentsManager>();
            enemyComponentsManager.OnDeath += OnDeath;
            enemyComponentsManager.TargetSpotter = this;
        }

        private void Start()
        {
            groupTargetSpotter = GetComponentInParent<GroupTargetSpotter>();
            if (groupTargetSpotter != null)
            {
                groupTargetSpotter.spottersList.Add(this);
            }
        }

        private void OnDestroy()
        {
            enemyComponentsManager.OnDeath -= OnDeath;
            if (groupTargetSpotter != null)
            {
                groupTargetSpotter.spottersList.Remove(this);
            }
        }

        private void Update()
        {
            if (alive)
            {
                RetrieveFromGroupSpotter();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (alive)
            {
                if ((targetMask.value & (1 << other.transform.gameObject.layer)) > 0)
                {
                    directionToTarget = other.transform.position - transform.position;
                    distanceToTarget = directionToTarget.magnitude;
                    directionToTarget = directionToTarget.normalized;
                    ray = new Ray(transform.position, directionToTarget);
                    if (!Physics.Raycast(ray, distanceToTarget, obstructionMask))
                    {
                        SpotTheTarget(other.transform);
                    }
                    else
                    {
                        if (spotData.enemySpotState == EnemySpotState.TargetIsVisible)
                        {
                            LooseTheTarget();
                        }
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (alive)
            {
                if ((targetMask.value & (1 << other.transform.gameObject.layer)) > 0)
                {
                    if (spotData.enemySpotState == EnemySpotState.TargetIsVisible)
                    {
                        LooseTheTarget();
                    }
                }
            }
        }

        private void SpotTheTarget(Transform targetTransform)
        {
            spotData.enemySpotState = EnemySpotState.TargetIsVisible;
            spotData.targetTransform = targetTransform;
            spotData.spotTime = Time.time;
            PushToGroupSpotter();
            enemyComponentsManager.UpdateSpotState(EnemySpotState.TargetIsVisible);
        }

        private void LooseTheTarget()
        {
            spotData.enemySpotState = EnemySpotState.TargetLost;
            if (spotData.targetTransform != null)
            {
                spotData.lastKnownPosition = spotData.targetTransform.position;
                spotData.targetTransform = null;
            }
            PushToGroupSpotter();
            enemyComponentsManager.UpdateSpotState(EnemySpotState.TargetLost);
        }

        public void AlertOnTheTarget(Transform targetTransform)
        {
            //Debug.Log("Alerted!");
            if (spotData.enemySpotState != EnemySpotState.TargetIsVisible)
            {
                //Debug.Log("ACT!");
                spotData.enemySpotState = EnemySpotState.AlertedOnTarget;
                spotData.lastKnownPosition = targetTransform.position;
                spotData.spotTime = Time.time;
                PushToGroupSpotter();
                enemyComponentsManager.UpdateSpotState(EnemySpotState.AlertedOnTarget);
            }
        }

        public void ForgetTheTarget()
        {
            if (spotData.enemySpotState == EnemySpotState.AlertedOnTarget || spotData.enemySpotState == EnemySpotState.TargetLost)
            {
                spotData.enemySpotState = EnemySpotState.NoTarget;
                spotData.targetTransform = null;
                PushToGroupSpotter();
                enemyComponentsManager.UpdateSpotState(EnemySpotState.NoTarget);
            }
        }

        private void RetrieveFromGroupSpotter()
        {
            if (groupTargetSpotter != null || (spotData.enemySpotState == EnemySpotState.NoTarget || groupTargetSpotter.SpotData.enemySpotState == EnemySpotState.TargetIsVisible))
            {
                PullFromGroupSpotter();
            }
        }

        private void PullFromGroupSpotter()
        {
            if (groupTargetSpotter != null)
            {
                spotData = groupTargetSpotter.SpotData;
                enemyComponentsManager.UpdateSpotState(spotData.enemySpotState);
            }
        }

        private void PushToGroupSpotter()
        {
            if (groupTargetSpotter != null)
            {
                groupTargetSpotter.SpotData = spotData;
            }
        }

        private void OnDeath()
        {
            if (spotData.enemySpotState != EnemySpotState.NoTarget)
            {
                LooseTheTarget();
            }
            alive = false;
        }
    }
}