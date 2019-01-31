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
        public int BaseSpawnCount = 3;
        public int BaseSpawnCountSpread = 3;
        public int BaseSpawnRate = 2;
        public int BaseSpawnRateSpread = 2;
        [Header("Enemies")]
        public int BaseSpeed = 10;
        public int BaseSpeedSpread = 10;
        public int NewEnemyEachAmountOfLevels = 2;      //CurLvl = 7. 7 / 2 = 3.5 = 4; Enemies [0] [1] [2] [3]
        public EnemyTypes[] EnemyTypes;
    }
}
