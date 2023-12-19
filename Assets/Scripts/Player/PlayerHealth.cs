using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace RunAndGun.Space
{
    public class PlayerHealth : MonoBehaviour, IDamagable
    {
        [SerializeField] private float healthPoints = 100f;
        public float HealthPoints { get { return healthPoints; } set { OnHealthPointsAdded(value); } }
        public float StartingHealthPoints;
        private bool isDead = false;
        public bool IsDead { get { return isDead; } set { isDead = value; } }

        private void Awake()
        {
            GameManager.Instance.OnGameStateChanged.AddListener(OnGameStateChanged);
            GameManager.Instance.OnPlayerHealthPointsAdded.AddListener(OnHealthPointsAdded);
            StartingHealthPoints = healthPoints;
            GameManager.Instance.PlayerStartingHealthPoints = StartingHealthPoints;
            GlobalBuffer.gamePoints.CurrentHealth = healthPoints;
        }

        private void Start()
        {
            // testing death event
            //InvokeRepeating(nameof(TestPlayeDeath),1.5f, 1f);
        }

        public void TakeDamage(float damagePoints)
        {
            GameManager.Instance.PlayerHealthPointsAdded(-damagePoints);
        }

        private void OnHealthPointsAdded(float addedHealthPoints)
        {
            if (!isDead)
            {
                healthPoints += addedHealthPoints;
                //healthPoints = Mathf.Clamp(healthPoints, -StartingHealthPoints ,StartingHealthPoints);
                if (healthPoints <= 0)
                {
                    GameManager.Instance.UpdateGameState(GameState.PlayerDead);
                }
            }
        }

        private void OnPlayerDeath()
        {
            isDead = true;  // mark as dead in current class
        }
        private void TestPlayeDeath()
        {
            TakeDamage(-25);
        }

        private void OnGameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.OnMainMenu:

                    break;
                case GameState.InGamePaused:

                    break;
                case GameState.InGameActive:

                    break;
                case GameState.PlayerDead:
                    OnPlayerDeath();
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