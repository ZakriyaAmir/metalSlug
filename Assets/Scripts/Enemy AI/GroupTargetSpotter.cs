using System.Collections.Generic;
using UnityEngine;

namespace RunAndGun.Space
{
    public class GroupTargetSpotter : MonoBehaviour
    {
        [SerializeField] private Transform spotNotifier = null;
        private TargetSpotData spotData;
        public TargetSpotData SpotData { get { return spotData; } set { UpdateSpotData(value); } }
        public List<TargetSpotter> spottersList;
        private void UpdateSpotData(TargetSpotData spotData)
        {
            switch (spotData.enemySpotState)
            {
                case EnemySpotState.NoTarget:
                    if (CheckIfAllHaveThisState(EnemySpotState.NoTarget))
                    {
                        this.spotData = spotData;
                    }
                    break;
                case EnemySpotState.TargetIsVisible:
                    this.spotData = spotData;
                    break;
                case EnemySpotState.AlertedOnTarget:
                    if (!CheckIfAnyHasThisState(EnemySpotState.TargetIsVisible))
                    {
                        this.spotData = spotData;
                    }
                    break;
                case EnemySpotState.TargetLost:
                    if (!CheckIfAnyHasThisState(EnemySpotState.TargetIsVisible))
                    {
                        this.spotData = spotData;
                    }
                    break;
                default: break;
            }
            this.spotData = spotData;
            if (spotNotifier != null && (this.spotData.enemySpotState == EnemySpotState.AlertedOnTarget || this.spotData.enemySpotState == EnemySpotState.TargetLost))
            {
                UpdateNotifierLocation();
            }
        }

        private bool CheckIfAnyHasThisState(EnemySpotState spotState)
        {
            bool ans = false;
            foreach (TargetSpotter item in spottersList)
            {
                if (item.SpotData.enemySpotState == spotState)
                {
                    ans = true;
                }
            }
            return (ans);
        }

        private bool CheckIfAllHaveThisState(EnemySpotState spotState)
        {
            bool ans = true;
            foreach (TargetSpotter item in spottersList)
            {
                if (item.SpotData.enemySpotState != spotState)
                {
                    ans = false;
                }
            }
            return (ans);
        }

        private void UpdateNotifierLocation()
        {
            if (spotNotifier != null)
            {
                spotNotifier.transform.position = spotData.lastKnownPosition;
            }
        }
    }
}