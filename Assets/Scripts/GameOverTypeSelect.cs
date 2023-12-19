using UnityEngine;
using UnityEngine.Events;

namespace RunAndGun.Space
{
    public class GameOverTypeSelect : MonoBehaviour
    {
        public UnityEvent Failed;
        public UnityEvent Success;

        private void Start()
        {
            if (GlobalBuffer.failed)
            {
                Failed?.Invoke();
            }
            else
            {
                Success?.Invoke();
            }

        }
    }
}