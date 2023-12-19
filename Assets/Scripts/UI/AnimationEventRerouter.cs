using UnityEngine;
using UnityEngine.Events;

namespace RunAndGun.Space
{
    public class AnimationEventRerouter : MonoBehaviour
    {
        public UnityEvent trigger;
        public UnityEvent trigger2;
        public UnityEvent trigger3;
        public UnityEvent trigger4;
        public UnityEvent trigger5;

        public void Trigger()
        {
            trigger?.Invoke();
        }

        public void Trigger2()
        {
            trigger2?.Invoke();
        }

        public void Trigger3()
        {
            trigger3?.Invoke();
        }

        public void Trigger4()
        {
            trigger4?.Invoke();
        }

        public void Trigger5()
        {
            trigger5?.Invoke();
        }
    }
}