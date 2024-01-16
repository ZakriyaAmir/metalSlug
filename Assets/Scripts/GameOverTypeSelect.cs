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

        private void Start()
        {
            navigationButtons.SetActive(false);
            multiplierWheel.SetActive(false);

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
                if (PlayerPrefs.GetInt("levelsCompleted", 0) <= PlayerPrefs.GetInt("currentLevel", 0))
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
        }
        
        public void showLose()
        {
            navigationButtons.SetActive(true);
        }

        public void claimLevelReward()
        {
            economyManager.Instance.addMoney(GlobalBuffer.gamePoints.Points);
            //GA Event
            FirebaseAnalytics.LogEvent("Level_Reward_" + GlobalBuffer.gamePoints.Points);
        }
    }
}