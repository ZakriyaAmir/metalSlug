using Firebase.Analytics;
using UnityEngine;
using UnityEngine.Events;

namespace RunAndGun.Space
{
    public class GameOverTypeSelect : MonoBehaviour
    {
        public UnityEvent Failed;
        public UnityEvent Success;

        public GameObject multiplierWheel;
        public GameObject navigationButtons;
        public GameObject leftPanel;
        public GameObject CenteredPanel;
        public GameObject NextLevelButton;

        private void Start()
        {
            navigationButtons.SetActive(false);
            multiplierWheel.SetActive(false);
            CenteredPanel.SetActive(false);
            leftPanel.SetActive(false);
            NextLevelButton.SetActive(false);

            if (GlobalBuffer.failed)
            {
                Failed?.Invoke();

                showLose();
                
                audioManager.instance.PlayAudio("fail", true, Vector3.zero);

                //GA Event
                FirebaseAnalytics.LogEvent("Level_Failed_" + PlayerPrefs.GetInt("currentLevel", 0));
            }
            else
            {
                Success?.Invoke();
                //Save win progress
                if (PlayerPrefs.GetInt("levelsCompleted", 0) <= PlayerPrefs.GetInt("currentLevel", 0) + 1)
                {
                    PlayerPrefs.SetInt("levelsCompleted", PlayerPrefs.GetInt("levelsCompleted", 0) + 1);
                }

                showWin();

                audioManager.instance.PlayAudio("win", true, Vector3.zero);

                //GA Event
                FirebaseAnalytics.LogEvent("Level_Completed_" + PlayerPrefs.GetInt("currentLevel", 0));
            }

        }
        public void showWin()
        {
            multiplierWheel.SetActive(true);
            CenteredPanel.SetActive(false);
            leftPanel.SetActive(true);
            NextLevelButton.SetActive(true);
        }

        public void showLose()
        {
            navigationButtons.SetActive(true);
            CenteredPanel.SetActive(true);
            leftPanel.SetActive(false);
            NextLevelButton.SetActive(false);
        }

        public void claimLevelReward()
        {
            economyManager.Instance.addMoney(GlobalBuffer.gamePoints.Points);
            //GA Event
            FirebaseAnalytics.LogEvent("Level_Reward_" + GlobalBuffer.gamePoints.Points);
        }
    }
}