using UnityEngine;
using UnityEngine.Rendering;

namespace RunAndGun.Space
{
    [RequireComponent(typeof(Volume))]
    public class BlurAppear_UI : MonoBehaviour
    {
        [SerializeField] private float startDelay = 0.5f;
        [SerializeField] private float applyDuration = 0.5f;
        
        private Volume postProccessVolume;
        private float durationTimer = 0f;
        private bool started = false;

        private void Start()
        {
            postProccessVolume = GetComponent<Volume>();
            postProccessVolume.weight = 0;
            Invoke(nameof(StartVolume), startDelay);
        }


        private void Update()
        {
            if (started)
            {
                durationTimer += Time.deltaTime;
                if (durationTimer <= applyDuration)
                {
                    postProccessVolume.weight = durationTimer / applyDuration;
                }
                else
                {
                    started = false;
                }
            }
        }

        private void StartVolume()
        {
            started = true;
        }
    }
}