using UnityEngine;
using TMPro;

namespace RunAndGun.Space
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextSlideDown_UI : MonoBehaviour
    {
        [SerializeField] private float slideTimeInSeconds = 1f;
        [SerializeField] private float slideStartOffset = 50f;
        [SerializeField] private float slideStartDelay = 0.5f;

        private TextMeshProUGUI textMeshPro;
        private RectTransform rectTransform;
        private Vector2 originalTextMeshProPosition;
        private Vector2 currentTextMeshProPosition;
        private float originalTextMeshProAlpha;
        private float slideTimer = 0f;
        private bool slideStarted = false;

        private void Start()
        {
            textMeshPro = GetComponent<TextMeshProUGUI>();
            rectTransform = GetComponent<RectTransform>();
            originalTextMeshProPosition = rectTransform.anchoredPosition;
            originalTextMeshProAlpha = textMeshPro.color.a;
            SettOffset();
            Invoke(nameof(Slide), slideStartDelay);
        }

        private void Update()
        {
            if (slideStarted)
            {
                if (slideTimer < slideTimeInSeconds)
                {
                    slideTimer += Time.deltaTime;
                    textMeshPro.color = new Color(textMeshPro.color.r, textMeshPro.color.g, textMeshPro.color.b, Mathf.Lerp(0f, originalTextMeshProAlpha, slideTimer / slideTimeInSeconds));
                    rectTransform.anchoredPosition = Vector2.Lerp(currentTextMeshProPosition, originalTextMeshProPosition, slideTimer / slideTimeInSeconds);
                }
                else
                {
                    slideStarted = false;
                }
            }
        }

        private void SettOffset()
        {
            textMeshPro.color = new Color(textMeshPro.color.r, textMeshPro.color.g, textMeshPro.color.b, 0f);
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + slideStartOffset);
            currentTextMeshProPosition = rectTransform.anchoredPosition;
        }

        private void Slide()
        {
            slideStarted = true;
        }
    }
}