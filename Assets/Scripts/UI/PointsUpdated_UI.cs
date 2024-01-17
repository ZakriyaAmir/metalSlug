using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace RunAndGun.Space
{
    public class PointsUpdated_UI : MonoBehaviour
    {
        [SerializeField] private GameResultValueType valueType;
        private TMP_Text text;
        private Image img;

        private void Start()
        {
            if (valueType == GameResultValueType.CurrentHealthPoints)
            {
                img = GetComponent<Image>();
            }
            else 
            {
                text = GetComponent<TMP_Text>();
            }
            ManageSubscription();
        }

        private void ManageSubscription()
        {
            switch (valueType)
            {
                case GameResultValueType.Points:
                    GameManager.Instance.OnPointsUpdated.AddListener(UpdatePoints);
                    break;
                case GameResultValueType.EnemiesKilled:
                    GameManager.Instance.OnEnemiesKilledCountUpdated.AddListener(UpdatePoints);
                    break;
                case GameResultValueType.CurrentHealthPoints:
                    GameManager.Instance.OnHealthPointsUpdated.AddListener(UpdatePoints);
                    break;
                case GameResultValueType.CurrentAmmoRounds:
                    GameManager.Instance.OnAmmoUpdated.AddListener(UpdatePoints);
                    break;
                default:
                    break;
            }
            UpdatePoints();
        }

        private void UpdatePoints()
        {
            float value = 0f;
            switch (valueType)
            {
                case GameResultValueType.Points:
                    value = GlobalBuffer.gamePoints.Points;
                    break;
                case GameResultValueType.EnemiesKilled:
                    value = GlobalBuffer.gamePoints.EnemiesKilled;
                    break;
                case GameResultValueType.CurrentHealthPoints:
                    value = GlobalBuffer.gamePoints.CurrentHealth;
                    break;
                case GameResultValueType.CurrentAmmoRounds:
                    if (GameManager.Instance.weapon.infiniteAmmo)
                    {
                        text.text = "∞";
                        return;
                    }
                    else
                    {
                        value = GlobalBuffer.gamePoints.CurrentAmmoCount;
                    }
                    break;
                default:
                    break;
            }
            if (valueType == GameResultValueType.CurrentHealthPoints)
            {
                img.fillAmount = ((float)Mathf.Clamp(value, 0, 100f)) / 100;
            }
            else
            {
                text.text = ((int)Mathf.Clamp(value, 0, 9999f)).ToString();
            }
        }
    }
}