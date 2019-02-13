using UnityEngine;
using static clicker.datatables.DataTableEnemies;
using static clicker.datatables.DataTableLevels;

namespace clicker.datatables
{
    interface iLevelDataLoader
    {
        LevelData[] GetData(int dataParam);
    }

    public class LevelDataLoader_Local : iLevelDataLoader
    {
        public LevelData[] GetData(int dataParam)
        {
            return GameObject.FindObjectOfType<LocalLevelDataEditor>().Data_Levels;
        }
    }

    /// <summary>
    /// Локальная структура для передачи данных в локальную структуру
    /// </summary>
    [System.Serializable]
    public class LevelData
    {
        public AgeTypes AgeType = AgeTypes.FirstAge;
        [Header("HP")]
        public int BaseHP = 10;                         //HP = HPFunc(curLevel, BaseHP)
        public int BaseHPSpreadPercent = 15;            //HPFunc { result +- HPSpreadPercent }
        [Header("Spawn")]
        [Header(" - Spawn count")]
        public int BaseSpawnCount = 3;
        public int BaseSpawnCountSpreadPercent = 3;
        [Header(" - Spawn rate")]
        public float BaseSpawnRate = 2f;
        public float MinSpawnRate = 0.7f;
        public int RateStepEachAmountOfLevels = 7;
        public int BaseSpawnRateSpreadPercent = 2;
        [Header("Enemies")]
        [Header(" - Speed")]
        public float BaseSpeed = 10;
        public int BaseSpeedSpreadPercent = 10;
        public float MaxSpeed = 15;
        [Header(" - Enemy")]
        public float NewEnemyEachAmountOfLevels = 2;      //CurLvl = 7. 7 / 2 = 3.5 = 4; Enemies [0] [1] [2] [3]
        public EnemyTypes[] EnemyTypes;
    }
}
