using UnityEngine;
using UnityEngine.Rendering;

namespace RunAndGun.Space
{
    [RequireComponent(typeof(Volume))]
    public class PlayerDeadPostProccess_UI : MonoBehaviour
    {
        [SerializeField] private float effectStartDelay = 0.5f;
        [SerializeField] private float effectDuration = 2f;
        private Volume playerDeadPostProccessVolume;
        private bool playerDead = false;
        private float startDelayTimer = 0f;
        private float durationTimer = 0f;

        private void Awake()
        {
            playerDeadPostProccessVolume = GetComponent<Volume>();
            playerDeadPostProccessVolume.weight = 0;
            GameManager.Instance.OnGameStateChanged.AddListener(OnGameStateChanged);
        }

        private void Update()
        {
            if (playerDead)
            {
                if (startDelayTimer < effectStartDelay)
                {
                    startDelayTimer += Time.deltaTime;
                }
                else if (durationTimer < effectDuration)
                {
                    durationTimer += Time.deltaTime;
                    playerDeadPostProccessVolume.weight = durationTimer / effectDuration;
                }
                else
                {
                    playerDead = false;
                    GameManager.Instance.GoToEndScene();
                }
            }
        }

        private void OnGameStateChanged(GameState newState)
        {
            switch (newState)
            {
                case GameState.OnMainMenu:
                    
                    break;
                case GameState.InGamePaused:
                    
                    break;
                case GameState.InGameActive:
                    
                    break;
                case GameState.PlayerDead:
                    playerDead = true;
                    break;
                case GameState.LevelVictory:
                    
                    break;
                case GameState.LevelGameOver:
                    
                    break;
                default: break;
            }
        }
    }
}