using TMPro;
using UnityEngine;

namespace RunAndGun.Space
{
    [RequireComponent(typeof(TMP_Text))]
    public class AnnouncementText_UI : MonoBehaviour
    {
        [SerializeField]
        private float announceDuration = 5f;
        private float announceDurationTimer;
        private TextMeshProUGUI textMeshProUGUI;
        
        private void Awake()
        {
            GameManager.Instance.OnAnnounce.AddListener(AnnounceText);
            textMeshProUGUI = GetComponent<TextMeshProUGUI>();
            HideText();
        }

        private void AnnounceText(string text)
        {
            textMeshProUGUI.enabled = true;
            textMeshProUGUI.text = text;
            announceDurationTimer = announceDuration;
        }

        private void Update()
        {
            if (announceDurationTimer > 0)
            {
                announceDurationTimer -= Time.deltaTime;

            }
            else if (announceDurationTimer < 0)
            {
                HideText();
            }
        }

        private void HideText()
        {
            textMeshProUGUI.enabled = false;
        }
    }
}