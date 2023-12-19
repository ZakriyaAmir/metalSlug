using UnityEngine;

namespace RunAndGun.Space
{
    public class EnemyAnimator : MonoBehaviour
    {
        private Animator animator = null;
        private Rigidbody _rigidbody = null;
        private EnemyComponentsManager enemyComponentsManager;

        private void Awake()
        {
            enemyComponentsManager = GetComponent<EnemyComponentsManager>();
            enemyComponentsManager.EnemyAnimator = this;
            enemyComponentsManager.OnDeath += AnimateDeath;
        }

        private void Start()
        {
            _rigidbody = enemyComponentsManager.Rigidbody;
            animator = enemyComponentsManager.Animator;
        }

        private void OnDestroy()
        {
            enemyComponentsManager.OnDeath -= AnimateDeath;
        }

        private void Update()
        {
            animator.SetFloat("MoveSpeed", Mathf.Abs(Mathf.Round(_rigidbody.velocity.x * 100f) / 100f));
        }

        public void AnimateAttack()
        {
            animator.SetBool("Attacking", true);
        }

        public void StopAnimateAttack()
        {
            animator.SetBool("Attacking", false);
        }

        public void AnimateIdle()
        {
            animator.SetBool("Idle2", false);
            animator.SetBool("Attacking", false);
        }

        public void AnimateIdle2()
        {
            animator.SetBool("Idle2", true);
        }

        public void StopAnimateIdle2()
        {
            animator.SetBool("Idle2", false);
        }

        public void AnimateDeath()
        {
            animator.SetTrigger("Die");
        }
    }
}