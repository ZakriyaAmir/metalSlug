using UnityEngine;

namespace RunAndGun.Space
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 50f;
        private Rigidbody _rigidbody = null;
        private EnemyComponentsManager enemyComponentsManager;
        private int moveDirection = 0;
        private bool stopCalled = false;

        private void Awake()
        {
            enemyComponentsManager = GetComponent<EnemyComponentsManager>();
            enemyComponentsManager.EnemyMovement = this;
        }

        private void Start()
        {
            _rigidbody = enemyComponentsManager.Rigidbody;
        }

        public void Moving(int movingDirection = 1)
        {
            moveDirection = movingDirection;
        }

        public void StopMoving()
        {
            moveDirection = 0;
            stopCalled = true;
        }

        private void FixedUpdate()
        {
            if(moveDirection != 0)
            {
                _rigidbody.velocity = new Vector3(moveSpeed * transform.forward.x * moveDirection * Time.fixedDeltaTime, _rigidbody.velocity.y, _rigidbody.velocity.z);
            }
            else if(stopCalled)
            {
                _rigidbody.velocity = new Vector3(0f, _rigidbody.velocity.y, _rigidbody.velocity.z);
                stopCalled = false;
            }
        }
    }
}