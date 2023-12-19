using UnityEngine;
using UnityEngine.UI;

namespace RunAndGun.Space
{
    public class EnemyHealthBar_UI : MonoBehaviour
    {
        [SerializeField] private Image healthImage;
        private Canvas canvas;

        private void Start()
        {
            canvas = GetComponent<Canvas>();
            DeactivateSelf();
        }

        public void DeactivateSelf()
        {
            canvas.enabled = false;
        }

        public void ActivateSelf()
        {
            canvas.enabled = true;
        }

        public void UpdateFill(float value1, float value2)
        {
            healthImage.fillAmount = Mathf.Clamp(value1, 0, value2) / value2;
        }
    }
}