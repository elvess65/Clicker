using clicker.battle;
using UnityEngine;
using static clicker.datatables.DataTableEnemies;

namespace clicker.datatables
{
    public class LocalLevelDataEditor : MonoBehaviour
    {
        public LevelData[] LevelsList;

        [System.Serializable]
        public class LevelData
        {
            public float DelayBeforeStart;
            public float DelayBtwSpawn;

            public StepData[] SpawnList;    
        }

        [System.Serializable]
        public class StepData
        {
            [Header("HP")]
            public int MinHP = 1;
            public int MaxHP = 10;
            [Header("Speed")]
            public int MinSpeed = 1;
            public int MaxSpeed = 5;
            [Header("Spawn")]
            public SpawnData[] SpawnPointsData;
        }

        [System.Serializable]
        public class SpawnData
        {
            public LevelController.SpawnPointTypes SpawnPoint;

            [Header("Enemies")]
            public EnemyTypes[] PossibleEnemies;
        }
    }
}
