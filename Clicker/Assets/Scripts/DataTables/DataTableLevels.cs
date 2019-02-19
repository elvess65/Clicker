using System.Collections.Generic;
using UnityEngine;

namespace clicker.datatables
{
    public class DataTableLevels
    {
        private static Dictionary<AgeTypes, Level> m_Levels;

        public static void SetData(LevelData[] data)
        {
            if (data == null)
                return;

            m_Levels = new Dictionary<AgeTypes, Level>();

            for (int i = 0; i < data.Length; i++)
            {
                Level level = new Level(data[i].AgeType,
                                        data[i].BaseHP, data[i].BaseHPSpreadPercent,                                //HP
                                        data[i].BaseSpawnCount, data[i].BaseSpawnCountSpreadPercent,                //Spawn
                                        data[i].BaseSpawnRate, data[i].BaseSpawnRateSpreadPercent,
                                        data[i].MinSpawnRate, data[i].RateStepEachAmountOfLevels,
                                        data[i].BaseSpeed, data[i].BaseSpeedSpreadPercent, data[i].MaxSpeed,        //Enemies
                                        data[i].NewEnemyEachAmountOfLevels,
                                        data[i].EnemyTypes,
                                        GetWinConditionControllerForAge(data[i].AgeType));                          //Win condition

                m_Levels.Add(data[i].AgeType, level);
            }
        }

        //HP
        public static int GetHP(AgeTypes age, int lvl)
        {
            if (m_Levels.ContainsKey(age))
            {
                int baseHP = m_Levels[age].BaseHP;
                return baseHP + baseHP * lvl;
            }

            return 1;
        }

        public static int GetHPSpreadPercent(AgeTypes age, int lvl, int levelHP)
        {
            if (m_Levels.ContainsKey(age))
            {
                float baseHPSpreadPercent = m_Levels[age].BaseHPSpreadPercent / 100f;
                return (int)(baseHPSpreadPercent * levelHP);
            }

            return 0;
        }

        //Spawn
        // - Spawn count
        public static int GetSpawnCount(AgeTypes age, int lvl)
        {
            if (m_Levels.ContainsKey(age))
            {
                int baseSpawnCount = m_Levels[age].BaseSpawnCount;
                return baseSpawnCount + lvl;
            }

            return 1;
        }

        public static int GetSpawnCountSpread(AgeTypes age, int lvl, int levelSpawnCount)
        {
            if (m_Levels.ContainsKey(age))
            {
                float baseSpawnCountSpreadPercent = m_Levels[age].BaseSpawnCountSpreadPercent / 100f;
                return (int)(baseSpawnCountSpreadPercent * levelSpawnCount);
            }

            return 0;
        }

        // - Spawn rate
        public static float GetSpawnRate(AgeTypes age, int lvl)
        {
            if (m_Levels.ContainsKey(age))
            {
                float baseSpawnRate = m_Levels[age].BaseSpawnRate;
                float result = baseSpawnRate - (float)lvl / m_Levels[age].RateStepEachAmountOfLevels;

                return Mathf.Clamp(result, m_Levels[age].MinSpawnRate, baseSpawnRate);
            }

            return 1;
        }

        public static float GetSpawnRateSpread(AgeTypes age, int lvl, float spawnRate)
        {
            if (m_Levels.ContainsKey(age))
            {
                float constValue = 2f;

                int baseSpawnRateSpread = m_Levels[age].BaseSpawnRateSpread;
                float baseSpawnPercent = m_Levels[age].BaseSpawnRateSpread / 100f;

                float result = baseSpawnPercent * (spawnRate / constValue);
                return result;
            }

            return 0;
        }

        //Enemies
        public static float GetSpeed(AgeTypes age, int lvl)
        {
            if (m_Levels.ContainsKey(age))
            {
                float constValue = 0.07f;

                float baseSpeed = m_Levels[age].BaseSpeed;
                return baseSpeed + (baseSpeed * (lvl * constValue));
            }

            return 1;
        }

        public static float GetSpeedSpreadPercent(AgeTypes age, int lvl, float levelSpeed)
        {
            if (m_Levels.ContainsKey(age))
            {
                float baseSpeedSpreadPercent = m_Levels[age].BaseSpeedSpreadPercent / 100f;
                return baseSpeedSpreadPercent * levelSpeed;
            }

            return 0;
        }

        public static float GetSpeedMax(AgeTypes age)
        {
            if (m_Levels.ContainsKey(age))
                return m_Levels[age].MaxSpeed;

            return 10;
        }

        public static DataTableEnemies.EnemyTypes[] GetEnemiesForLevel(AgeTypes age, int lvl)
        {
            if (m_Levels.ContainsKey(age))
            {
                //CurLvl = 0. 0 / 2 = 0 = 0; Enemies [0] 
                //CurLvl = 0. 1 / 2 = 0.5 = 1; Enemies [0] [1]
                //CurLvl = 7. 7 / 2 = 3.5 = 4; Enemies [0] [1] [2] [3]
                //CurLvl = 15. 15 / 2 = 7.5 = 8; Enemies [0] [1] [2] [3] [4]

                float amountOfEnemies = (float)lvl / m_Levels[age].NewEnemyEachAmountOfLevels;
                int enemyIndex = Mathf.CeilToInt(amountOfEnemies);

                DataTableEnemies.EnemyTypes[] enemyTypes = m_Levels[age].EnemyTypes;

                //Размер массива ограничеваеться размером массива  доступных врагов
                DataTableEnemies.EnemyTypes[] enemies = new DataTableEnemies.EnemyTypes[enemyIndex + 1 <= enemyTypes.Length ? enemyIndex + 1 : enemyTypes.Length];

                for (int i = 0; i < enemyTypes.Length; i++)
                {
                    //EnemyTypes (j):           [0] [1] [2] [3] [4] 
                    //Enemies:   (enemyIndex):  [0] [1]  

                    if (i <= enemyIndex)
                        enemies[i] = enemyTypes[i];
                }

                return enemies;
            }

            return new DataTableEnemies.EnemyTypes[] { DataTableEnemies.EnemyTypes.Enemy1, DataTableEnemies.EnemyTypes.Enemy2, DataTableEnemies.EnemyTypes.Enemy3 };
        }

        //Win condition
        public static AgeWinConditionController GetWinConditionController(AgeTypes age)
        {
            if (m_Levels.ContainsKey(age))
                return m_Levels[age].WinConditionController;

            return null;
        }

        static AgeWinConditionController GetWinConditionControllerForAge(AgeTypes ageType)
        {
            switch (ageType)
            {
                case AgeTypes.FirstAge:
                {
                    Dictionary<DataTableItems.ItemTypes, int> itemsToWin = new Dictionary<DataTableItems.ItemTypes, int>();
                        itemsToWin.Add(DataTableItems.ItemTypes.Stick, 3);

                    return new FirstAge_WinConditionController(itemsToWin);
                }
            }

            Debug.LogError("ERROR: Win condition controller for age " + ageType + " not found");
            return null;
        }


        #region Data Structures
        public enum AgeTypes
        {
            FirstAge,
            SecondAge
        }

        public class Level
        {
            private AgeTypes m_AgeType;
            //HP
            private int m_BaseHP;                           //HP = HPFunc(curLevel, BaseHP)
            private int m_BaseHPSpreadPercent;              //HPFunc { result +- HPSpreadPercent }
            //Spawn
            // - Spawn count
            private int m_BaseSpawnCount;
            private int m_BaseSpawnCountSpreadPercent;
            // - Spawn rate
            private float m_BaseSpawnRate;
            private int m_BaseSpawnRateSpreadPercent;
            private float m_MinSpawnRate;
            private int m_RateStepEachAmountOfLevels;
            //Enemies
            // - Speed
            private float m_BaseSpeed;
            private int m_BaseSpeedSpreadPercent;
            private float m_MaxSpeed;
            // - Enemy
            private float m_NewEnemyEachAmountOfLevels;   //CurLvl = 7. 7 / 2 = 3.5 = 4; Enemies [0] [1] [2] [3]
            private DataTableEnemies.EnemyTypes[] m_EnemyTypes;

            public AgeTypes AgeType => m_AgeType;
            //HP
            public int BaseHP => m_BaseHP;
            public int BaseHPSpreadPercent => m_BaseHPSpreadPercent;
            //Spawn
            // - Spawn count
            public int BaseSpawnCount => m_BaseSpawnCount;
            public int BaseSpawnCountSpreadPercent => m_BaseSpawnCountSpreadPercent;
            // - Spawn rate
            public float BaseSpawnRate => m_BaseSpawnRate;
            public int BaseSpawnRateSpread => m_BaseSpawnRateSpreadPercent;
            public float MinSpawnRate => m_MinSpawnRate;
            public int RateStepEachAmountOfLevels => m_RateStepEachAmountOfLevels;
            //Enemies
            // - Speed
            public float BaseSpeed => m_BaseSpeed;
            public int BaseSpeedSpreadPercent => m_BaseSpeedSpreadPercent;
            public float MaxSpeed => m_MaxSpeed;
            // - Enemy
            public float NewEnemyEachAmountOfLevels => m_NewEnemyEachAmountOfLevels;
            public DataTableEnemies.EnemyTypes[] EnemyTypes => m_EnemyTypes;
            //Win condition
            public AgeWinConditionController WinConditionController { get; private set; }

            public Level(AgeTypes ageType,
                        //HP
                        int baseHP, int baseHPSpreadPercent,
                        //Spawn
                        // - Spawn count
                        int baseSpawnCount, int baseSpawnCountSpreadPercent,
                        // - Spawn rate
                        float baseSpawnRate, int baseSpawnRateSpreadPercent,
                        float minSpawnRate, int rateStepEachAmountOfLevels,
                        //Enemies
                        // - Speed
                        float baseSpeed, int baseSpeedSpreadPercent, float maxSpeed,
                        // - Enemy
                        float newEnemyEachAmountOfLevels,
                        DataTableEnemies.EnemyTypes[] enemyTypes,
                        //Win condition
                        AgeWinConditionController winConditionController)
            {
                m_AgeType = ageType;

                //HP
                m_BaseHP = baseHP;
                m_BaseHPSpreadPercent = baseHPSpreadPercent;

                //Spawn
                // - Spawn count
                m_BaseSpawnCount = baseSpawnCount;
                m_BaseSpawnCountSpreadPercent = baseSpawnCountSpreadPercent;
                // - Spawn rate
                m_BaseSpawnRate = baseSpawnRate;
                m_BaseSpawnRateSpreadPercent = baseSpawnRateSpreadPercent;
                m_MinSpawnRate = minSpawnRate;
                m_RateStepEachAmountOfLevels = rateStepEachAmountOfLevels;

                //Enemies
                // - Speed
                m_BaseSpeed = baseSpeed;
                m_BaseSpeedSpreadPercent = baseSpeedSpreadPercent;
                m_MaxSpeed = maxSpeed;
                // - Enemy
                m_NewEnemyEachAmountOfLevels = newEnemyEachAmountOfLevels;
                m_EnemyTypes = enemyTypes;
                //Win condition
                WinConditionController = winConditionController;
            }
        }

        #region Win Condition Controllers
        public abstract class AgeWinConditionController
        {
            public abstract bool AgeIsFinished(account.Account account);
        }

        public class FirstAge_WinConditionController : AgeWinConditionController
        {
            //Пример: Для прохождения епохи нужно собрать 10 камней

            private Dictionary<DataTableItems.ItemTypes, int> m_ItemsToWin;

            public FirstAge_WinConditionController(Dictionary<DataTableItems.ItemTypes, int> itemsToWin) : base()
            {
                m_ItemsToWin = new Dictionary<DataTableItems.ItemTypes, int>();
                m_ItemsToWin = itemsToWin;
            }

            public override bool AgeIsFinished(account.Account account)
            {
                int count = 0;
                foreach (DataTableItems.ItemTypes itemType in m_ItemsToWin.Keys)
                {
                    if (account.Inventory.HasAmountOfItem(itemType, m_ItemsToWin[itemType]))
                        count++;
                }

                return count == m_ItemsToWin.Count;
            }
        }
        #endregion
        #endregion
    }
}
