using UnityEngine;

namespace RunAndGun.Space
{
    public class PlayerHealthBar : MonoBehaviour
    {
        [SerializeField] private RectTransform canvas = null;
        private float originalWidth = 0f;
        private void Awake()
        {
            GameManager.Instance.OnPlayerHealthPointsAdded.AddListener(OnHealthPointsAdded);
        }

        private void Start()
        {
            canvas = GetComponent<RectTransform>();
            originalWidth = canvas.sizeDelta.x;
        }

        private void OnHealthPointsAdded(float addedHealthPoints)
        {
            canvas.sizeDelta = new Vector2(
                canvas.sizeDelta.x + originalWidth * (addedHealthPoints / GameManager.Instance.PlayerStartingHealthPoints),
                canvas.sizeDelta.y);
        }
    }
}