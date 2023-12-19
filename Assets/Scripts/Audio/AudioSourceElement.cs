using UnityEngine;

namespace RunAndGun.Space
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSourceElement : MonoBehaviour
    {
        [SerializeField] private AudioType audioType = AudioType.SoundEffectType;
        [SerializeField] private float StartTime = 0f;
        [SerializeField] private float EndTime = 1f;
        [SerializeField] private bool loopForMusic = false;
        private MainMenu mainMenuReference;
        private AudioSource audioSource;
        public float Volume { get { return audioSource.volume; } set { SetVolume(value); } }
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            mainMenuReference = FindObjectOfType<MainMenu>();
            if (audioType == AudioType.SoundEffectType)
            {
                mainMenuReference.AudioEffectsSource.Add(this);
            }
            else if (audioType == AudioType.MusicType)
            {
                mainMenuReference.MusicSource.Add(this);
            }
        }

        private void Update()
        {
            if (audioSource.isPlaying)
            {
                if (audioSource.time > EndTime)
                {
                    if (loopForMusic)
                    {
                        Play();
                    }
                    else
                    {
                        audioSource.Stop();
                    }
                }
            }
        }

        private void OnDestroy()
        {
            if (audioType == AudioType.SoundEffectType)
            {
                mainMenuReference.AudioEffectsSource.Remove(this);
            }
            else if (audioType == AudioType.MusicType)
            {
                mainMenuReference.MusicSource.Remove(this);
            }
        }

        public void Play()
        {
            if (StartTime < audioSource.clip.length)
            {
                audioSource.time = StartTime;
            }
            audioSource.Play();
        }

        public void Pause()
        {
            audioSource.Pause();
        }

        public void Unpause()
        {
            audioSource.UnPause();
        }

        private void SetVolume(float volume)
        {
            audioSource.volume = volume;
        }

    }
}