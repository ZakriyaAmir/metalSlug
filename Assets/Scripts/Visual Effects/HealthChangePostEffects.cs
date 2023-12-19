using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

namespace RunAndGun.Space
{
    public class HealthChangePostEffects : MonoBehaviour
    {
        [SerializeField] private Volume TakeDamageVolume;
        [SerializeField] private Volume HealVolume;
        [SerializeField] private float effectSpeed = 2f;
        private bool takeDamage = false;
        private bool heal = false;

        private void Awake()
        {
            GameManager.Instance.OnPlayerHealthPointsAdded.AddListener(OnHealthPointsAdded);
        }

        private void Update()
        {
            if (takeDamage)
            {
                TakeDamageVolume.weight = Mathf.Lerp(TakeDamageVolume.weight, 0, effectSpeed * Time.deltaTime);
                if(TakeDamageVolume.weight <= 0)
                {
                    takeDamage = false;
                }
            }
            if (heal)
            {
                HealVolume.weight = Mathf.Lerp(HealVolume.weight, 0, effectSpeed * Time.deltaTime);
                if (HealVolume.weight <= 0)
                {
                    heal = false;
                }
            }
        }

        private void OnHealthPointsAdded(float value)
        {
            if(value > 0)
            {
                HealEffect();
            }
            else
            {
                TakeDamageEffect();
            }
        }

        private void TakeDamageEffect()
        {
            takeDamage = true;
            TakeDamageVolume.weight = 1f;
        }

        private void HealEffect()
        {
            heal = true;
            HealVolume.weight = 1f;
        }
    }
}