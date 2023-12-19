using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RunAndGun.Space
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Slider MusicVolumeSlider;
        [SerializeField] private Slider SoundsVolumeSlider;
        public List<AudioSourceElement> MusicSource;
        public List<AudioSourceElement> AudioEffectsSource;
        private void Awake()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameStateChanged.AddListener(OnGameStateChanged);
            }
            Cursor.visible = true;
            
        }

        private void Start()
        {
            MusicVolumeSlider.value = GlobalBuffer.MusicVolume;
            UpdateMusicVolumeSlider();
            SoundsVolumeSlider.value = GlobalBuffer.SoundVolume;
            UpdateSoundsVolumeSlider();
        }

        public void PauseGameButtonPress()
        {
            GameManager.Instance.UpdateGameState(GameState.InGamePaused);
        }

        public void ResumeGameButtonPress()
        {
            GameManager.Instance.UpdateGameState(GameState.InGameActive);
        }

        public void GoToMainMenuButtonPress()
        {
            SceneManager.LoadScene(0);
        }

        public void LoadGivenScene(int index)
        {
            SceneManager.LoadScene(index);
        }

        public void RestartButtonPress()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void OnGameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.OnMainMenu:

                    break;
                case GameState.InGamePaused:
                    PauseGame();
                    PauseAllSounds();
                    break;
                case GameState.InGameActive:
                    ResumeGame();
                    ResumeAllSounds();
                    break;
                case GameState.PlayerDead:

                    break;
                case GameState.LevelVictory:

                    break;
                case GameState.LevelGameOver:

                    break;
                default: break;
            }
        }

        private void PauseGame()
        {
            Time.timeScale = 0f;
        }

        private void ResumeGame()
        {
            if (GameManager.Instance.State != GameState.PlayerDead || GameManager.Instance.State != GameState.LevelGameOver || GameManager.Instance.State != GameState.LevelVictory)
            {
                Time.timeScale = 1f;
            }
        }

        private void PauseAllSounds()
        {
            foreach (AudioSourceElement item in AudioEffectsSource)
            {
                item.Pause();
            }
        }

        private void ResumeAllSounds()
        {
            foreach (AudioSourceElement item in AudioEffectsSource)
            {
                item.Unpause();
            }
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void UpdateMusicVolumeSlider()
        {
            GlobalBuffer.MusicVolume = MusicVolumeSlider.value;
            foreach (AudioSourceElement item in MusicSource)
            {
                item.Volume = MusicVolumeSlider.value;
            }
        }

        public void UpdateSoundsVolumeSlider()
        {
            GlobalBuffer.SoundVolume = SoundsVolumeSlider.value;
            foreach (AudioSourceElement item in AudioEffectsSource)
            {
                item.Volume = SoundsVolumeSlider.value;
            }
        }
    }
}