using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace RunAndGun.Space
{
    public class PlayerAnimationRigControl : MonoBehaviour
    {
        [SerializeField] private RigBuilder rigBuilder = null;
        public RigBuilder RigBuilder { get { return rigBuilder; } set { rigBuilder = value; } }
        private bool disabled = false;

        private void Awake()
        {
            GameManager.Instance.OnGameStateChanged.AddListener(OnGameStateChanged);
            GameManager.Instance.OnPlayerWeaponReloadStart.AddListener(PauseAnimationRigging);
            GameManager.Instance.OnPlayerWeaponReloadEnd.AddListener(ResumeAnimationRigging);
            rigBuilder = GetComponent<RigBuilder>();
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
                    ResumeAnimationRigging();
                    break;
                case GameState.PlayerDead:
                    DisableAnimationRigging();
                    break;
                case GameState.LevelVictory:
                    DisableAnimationRigging();
                    break;
                case GameState.LevelGameOver:
                    DisableAnimationRigging();
                    break;
                default: break;
            }
        }

        private void PauseAnimationRigging()
        {
            foreach (RigLayer item in rigBuilder.layers)
            {
                item.active = false;
            }
        }

        private void ResumeAnimationRigging()
        {
            if (!disabled)
            {
                foreach (RigLayer item in rigBuilder.layers)
                {
                    item.active = true;
                }
            }
        }

        private void DisableAnimationRigging()
        {
            disabled = true;
            PauseAnimationRigging();
        }

    }
}