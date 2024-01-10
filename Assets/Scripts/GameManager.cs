using Cinemachine;
using Firebase.Analytics;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace RunAndGun.Space
{
    public class GameManager : MonoBehaviour
    {
        // singleton
        public static GameManager Instance;
        public GameState State;
        // state to see on inspector
        [SerializeField] private GameState StartingState;
        [SerializeField] private GamePoints gamePointsExposed;
        // game data
        public float PlayerStartingHealthPoints;
        // events
        public UnityEvent<GameState> OnGameStateChanged;
        public UnityEvent OnPlayerWeaponReloadStart;
        public UnityEvent OnPlayerWeaponReloadEnd;

        public UnityEvent<float> OnPlayerHealthPointsAdded = new UnityEvent<float>();
        public UnityEvent OnAmmoUpdated = new UnityEvent();
        public UnityEvent OnPointsUpdated = new UnityEvent();
        public UnityEvent OnEnemiesKilledCountUpdated = new UnityEvent();
        public UnityEvent OnHealthPointsUpdated = new UnityEvent();
        public UnityEvent<string> OnAnnounce = new UnityEvent<string>();
        public UnityEvent OnAllEnemiesKilled = new UnityEvent();

        // necessary components for player related scripts
        public PlayerMovement playerMovement;
        public PlayerInput playerInput;
        public IsGroundedControl isGroundedControl;
        public Transform aimTarget;
        public RecoilControl recoilControl;
        public Weapon weapon;
        public Weapon defaultWeapon;
        private bool isFireButtonDown;
        public GameObject player;
        public GameObject[] playerPrefabs;
        public GameObject[] allLevels;
        public levelBehavior currentLevel;
        public int totalEarnings;
        public GameObject tutorialPanel;

        // necessary components for other classes
        public Transform playerTransform;
        private List<Transform> enemies = new List<Transform>();

        // reference to selected enemy
        public EnemyHealthBar_UI EnemyHealthBar_UI;

        public static GameManager instance;

        private void Awake()
        {
            Instance = this;
            GlobalBuffer.Reset();

            if (instance != null)
            {
                Destroy(this);
            }
            else
            {
                instance = this;
            }

            //Reset current level if the total levels count exceeds
            if (PlayerPrefs.GetInt("currentLevel", 0) >= allLevels.Length)
            {
                Debug.Log("Levels Reset");
                PlayerPrefs.SetInt("currentLevel", 0);

                //GA Event
                FirebaseAnalytics.LogEvent("All_Levels" + "_Cleared");
            }
            GameObject obj = Instantiate(allLevels[PlayerPrefs.GetInt("currentLevel", 0)]);
            currentLevel = obj.GetComponent<levelBehavior>();

            player = Instantiate(playerPrefabs[PlayerPrefs.GetInt("selectedPlayer", 0)], currentLevel.spawnPoint.position, Quaternion.identity);
            FindObjectOfType<CinemachineVirtualCamera>().Follow = player.transform;
            FindObjectOfType<CinemachineVirtualCamera>().LookAt = player.transform;
        }

        public void claimLevelReward()
        {
            economyManager.Instance.addMoney(totalEarnings);
            //GA Event
            FirebaseAnalytics.LogEvent("Level_Reward_" + totalEarnings);
        }

        private void Start()
        {
            EnemyHealthBar_UI = GameObject.FindObjectOfType<EnemyHealthBar_UI>();
            UpdateGameState(StartingState);

            if (audioManager.instance) 
            {
                audioManager.instance.PlayAudio("gameplayBGM", false, Vector3.zero);
            }

            if (PlayerPrefs.GetInt("tutorial", 1) == 1)
            {
                tutorialPanel.SetActive(true);
            }

            //GA Event
            FirebaseAnalytics.LogEvent("Level_Started_" + PlayerPrefs.GetInt("currentLevel", 0));
        }

        public void acceptTutorial() 
        {
            tutorialPanel.SetActive(false);
            PlayerPrefs.SetInt("tutorial", 0);
        }

        public void selectDefaultWeapon() 
        {
            weapon = defaultWeapon;
            playerInput.weapon = defaultWeapon;
            defaultWeapon.gameObject.SetActive(true);
        }

        public void UpdateGameState(GameState newState)
        {
            Instance.State = newState;
            switch (Instance.State)
            {
                case GameState.OnMainMenu:
                    Cursor.visible = true;
                    break;
                case GameState.InGamePaused:
                    Cursor.visible = true;
                    break;
                case GameState.InGameActive:
                    Cursor.visible = false;
                    break;
                case GameState.PlayerDead:
                    Cursor.visible = true;
                    GlobalBuffer.CalculateTimeSpent();
                    GlobalBuffer.failed = true;
                    if (audioManager.instance.vibrationBool)
                    {
                        Handheld.Vibrate();
                    }
                    //GoToEndScene();
                    break;
                case GameState.LevelVictory:
                    Cursor.visible = true;
                    GlobalBuffer.CalculateTimeSpent();
                    if (audioManager.instance.vibrationBool)
                    {
                        Handheld.Vibrate();
                    }
                    GoToEndScene();
                    break;
                case GameState.LevelGameOver:
                    Cursor.visible = false;
                    break;
                default: break;
            }
            Instance.OnGameStateChanged?.Invoke(newState);
        }

        public void PlayerHealthPointsAdded(float value)
        {
            Instance.OnPlayerHealthPointsAdded?.Invoke(value);
            GlobalBuffer.gamePoints.CurrentHealth += value;
            Instance.UpdateHealthPoints();
        }

        public void PointsAdded(int value)
        {
            GlobalBuffer.gamePoints.Points += value;
            Instance.UpdatePoints();
        }


        private void Update()
        {
            gamePointsExposed = GlobalBuffer.gamePoints;
        }

        public void ReloadWeaponStart()
        {
            Instance.OnPlayerWeaponReloadStart?.Invoke();
        }

        public void ReloadWeaponEnd()
        {
            Instance.OnPlayerWeaponReloadEnd?.Invoke();
        }

        public void UpdateAmmo()
        {
            Instance.OnAmmoUpdated?.Invoke();
        }

        public void UpdatePoints()
        {
            Instance.OnPointsUpdated?.Invoke();
        }

        public void UpdateKilledCount()
        {
            Instance.OnEnemiesKilledCountUpdated?.Invoke();
        }
        private void UpdateHealthPoints()
        {
            Instance.OnHealthPointsUpdated?.Invoke();
        }

        public void GoToEndScene()
        {
            SceneManager.LoadScene(2);
        }

        public void LevelVictory()
        {
            Instance.UpdateGameState(GameState.LevelVictory);
        }

        public void AnnounceText(string text)
        {
            Instance.OnAnnounce?.Invoke(text);
        }

        public void AddEnemy(Transform enemy)
        {
            enemies.Add(enemy);
        }

        public void RemoveEnemy(Transform enemy)
        {
            enemies.Remove(enemy);
            if (!enemies.Any())
            {
                OnAllEnemiesKilled?.Invoke();
            }
        }

        public void triggerJump()
        {
            playerMovement.Jump();
        }

        public void triggerCrouch()
        {
            playerMovement.Crouch();
        }


        public void OnFirePressed()
        {
            isFireButtonDown = true;
            InvokeRepeating("FireEvent", 0f, 0.01f); // Adjust the repeat rate as needed
        }

        // Attach this method to the button's OnPointerUp event in the Unity Editor
        public void OnFireReleased()
        {
            isFireButtonDown = false;
            CancelInvoke("FireEvent");
        }

        // This method will be repeatedly invoked as long as the button is pressed
        private void FireEvent()
        {
            if (isFireButtonDown)
            {
                playerInput.weapon.TryShoot();
            }
        }
    }
}