using System;
using UnityEngine;

namespace RunAndGun.Space
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(EnemyMovement))]
    [RequireComponent(typeof(EnemyAnimator))]
    [RequireComponent(typeof(EnemyAIControl))]
    public class EnemyComponentsManager : MonoBehaviour
    {
        private Rigidbody _rigidbody = null;
        public Rigidbody Rigidbody { get { return _rigidbody; } }
        private EnemyMovement enemyMovement = null;
        public EnemyMovement EnemyMovement { get { return enemyMovement; } set { enemyMovement = value; } }
        private Animator animator = null;
        public Animator Animator { get { return animator; } set { animator = value; } }
        private TargetSpotter targetSpotter = null;
        public TargetSpotter TargetSpotter { get { return targetSpotter; } set { targetSpotter = value; } }
        private EnemyAnimator enemyAnimator;
        public EnemyAnimator EnemyAnimator { get { return enemyAnimator; } set { enemyAnimator = value; } }
        private EnemyRangedShooter enemyRangedShooter;
        public EnemyRangedShooter EnemyRangedShooter { get { return enemyRangedShooter; } set { enemyRangedShooter = value; } }

        public event Action<EnemySpotState> OnSpotStateChanged;
        public event Action<float> OnTakeDamage;
        public event Action OnDeath;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            animator = GetComponentInChildren<Animator>();
            GameManager.Instance.AddEnemy(this.transform);
        }

        public void UpdateSpotState(EnemySpotState enemySpotState)
        {
            OnSpotStateChanged?.Invoke(enemySpotState);
        }
        
        public void TakeDamage(float damageValue)
        {
            OnTakeDamage?.Invoke(damageValue);
        }

        public void Die()
        {
            OnDeath?.Invoke();
            GlobalBuffer.gamePoints.EnemiesKilled += 1;
            this.gameObject.layer = LayerMask.NameToLayer(GlobalStringVars.PLAYER_PHASE_THROUGH_LAYER);
            GameManager.Instance.RemoveEnemy(this.transform);
        }
    }
}