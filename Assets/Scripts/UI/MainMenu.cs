using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RunAndGun.Space
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject loadingPanel;

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
            /* MusicVolumeSlider.value = GlobalBuffer.MusicVolume;
             UpdateMusicVolumeSlider();
             SoundsVolumeSlider.value = GlobalBuffer.SoundVolume;
             UpdateSoundsVolumeSlider();*/
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
            Time.timeScale = 1f;
            showLoading();
            StartCoroutine(delayLoadScene("mainMenu"));

            AdsManager.Instance.RunInterstitialAd();
        }

        public void playNextLevel()
        {
            showLoading();
            PlayerPrefs.SetInt("currentLevel", PlayerPrefs.GetInt("currentLevel", 0) + 1);
            StartCoroutine(delayLoadScene("gameplay"));

            AdsManager.Instance.RunInterstitialAd();
        }

        public IEnumerator delayLoadScene(string scene)
        {
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene(scene);
        }

        public void showLoading()
        {
            Instantiate(loadingPanel);
        }

        public void LoadGivenScene(int index)
        {
            SceneManager.LoadScene(index);
        }

        public void RestartButtonPress()
        {
            Time.timeScale += 1f;
            showLoading();
            StartCoroutine(delayLoadScene("gameplay"));

            AdsManager.Instance.RunInterstitialAd();
        }

        private void OnGameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.OnMainMenu:

                    break;
                case GameState.InGamePaused:
                    PauseGame();
                    break;
                case GameState.InGameActive:
                    ResumeGame();
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

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}