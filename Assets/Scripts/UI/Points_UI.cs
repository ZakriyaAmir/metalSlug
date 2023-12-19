using UnityEngine;
using TMPro;

namespace RunAndGun.Space
{
    public class Points_UI : MonoBehaviour
    {
        [SerializeField] private GameResultValueType valueType;
        private TMP_Text text;
        private string value;
        private void Awake()
        {
            text = GetComponent<TMP_Text>();
        }

        private void Start()
        {
            switch (valueType)
            {
                case GameResultValueType.Points:
                    value = GlobalBuffer.gamePoints.Points.ToString();
                    break;
                case GameResultValueType.EnemiesKilled:
                    value = GlobalBuffer.gamePoints.EnemiesKilled.ToString();
                    break;
                case GameResultValueType.CurrentHealthPoints:
                    value = GlobalBuffer.gamePoints.CurrentHealth.ToString();
                    break;
                case GameResultValueType.CurrentAmmoRounds:
                    value = GlobalBuffer.gamePoints.CurrentAmmoCount.ToString();
                    break;
                case GameResultValueType.TimeSpent:
                    //string format = @"dd\:hh\:mm\:ss\.fffffff";
                    value = GlobalBuffer.TimeSpent.ToString(@"hh\:mm\:ss");
                    break;
                default:
                    break;
            }
            text.text = value;
        }
    }
}