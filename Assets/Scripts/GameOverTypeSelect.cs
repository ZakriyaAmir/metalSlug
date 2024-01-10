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
                audioManager.instance.PlayAudio("fail", true, Vector3.zero);
                Failed?.Invoke();
            }
            else
            {
                audioManager.instance.PlayAudio("win", true, Vector3.zero);
                Success?.Invoke();
            }

        }
    }
}