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
                                        data[i].BaseSpawnCount, data[i].BaseSpawnCountSpread,                       //Spawn
                                        data[i].BaseSpawnRate, data[i].BaseSpawnRateSpread,
                                        data[i].BaseSpeed, data[i].BaseSpeedSpreadPercent, data[i].BaseMaxSpeed,    //Enemies
                                        data[i].NewEnemyEachAmountOfLevels,
                                        data[i].EnemyTypes);

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

                //base HP = 10;
                // - lvl = 0;
                //Result: 10 + 10 * 0 = 10 + 0 = 10;
                // - lvl = 1;
                //Result: 10 + 10 * 1 = 10 + 10 = 20;
                // - lvl = 5;
                //Result: 10 + 10 * 5 = 10 + 50 = 60;
            }

            return 1;
        }

        public static int GetHPSpreadPercent(AgeTypes age, int lvl)
        {
            if (m_Levels.ContainsKey(age))
            {
                int baseHPSpreadPercent = m_Levels[age].BaseHPSpreadPercent;
                return baseHPSpreadPercent;
            }

            return 0;
        }

        //Spawn
        public static int GetSpawnCount(AgeTypes age, int lvl)
        {
            if (m_Levels.ContainsKey(age))
            {
                int deltaConst = 0;
                int baseSpawnCount = m_Levels[age].BaseSpawnCount;

                int spawnCount = baseSpawnCount + lvl - deltaConst;
                return Mathf.Clamp(spawnCount, 1, spawnCount);
            }

            return 1;
        }

        public static int GetSpawnCountSpread(AgeTypes age, int lvl)
        {
            if (m_Levels.ContainsKey(age))
            {
                int baseSpawnCountSpread = m_Levels[age].BaseSpawnCountSpread;
                int deltaConst = 1;
                int spawnCountSpread = baseSpawnCountSpread + lvl - deltaConst;
                return Mathf.Clamp(spawnCountSpread, 1, spawnCountSpread);
            }

            return 0;
        }

        public static int GetSpawnRate(AgeTypes age, int lvl)
        {
            if (m_Levels.ContainsKey(age))
            {
                int baseSpawnRate = m_Levels[age].BaseSpawnRate;
                return baseSpawnRate;
            }

            return 1;
        }

        public static int GetSpawnRateSpread(AgeTypes age, int lvl)
        {
            if (m_Levels.ContainsKey(age))
            {
                int baseSpawnRateSpread = m_Levels[age].BaseSpawnRateSpread;
                int deltaConst = 1;
                int spawnRateSpread = baseSpawnRateSpread + lvl - deltaConst;
                return Mathf.Clamp(spawnRateSpread, 1, spawnRateSpread);
            }

            return 0;
        }

        //Enemies
        public static float GetSpeed(AgeTypes age, int lvl)
        {
            if (m_Levels.ContainsKey(age))
            {
                float baseSpeed = m_Levels[age].BaseSpeed;
                return baseSpeed + lvl;
            }

            return 1;
        }

        public static int GetSpeedSpreadPercent(AgeTypes age, int lvl)
        {
            if (m_Levels.ContainsKey(age))
            {
                int baseSpeedSpreadPercent = m_Levels[age].BaseSpeedSpreadPercent;
                return baseSpeedSpreadPercent + lvl;
            }

            return 0;
        }

        public static float GetSpeedMax(AgeTypes age)
        {
            if (m_Levels.ContainsKey(age))
            {
                float baseMaxSpeed = m_Levels[age].BaseMaxSpeed;
                int deltaMaxSpeed = 1;
                return baseMaxSpeed * ((int)age + deltaMaxSpeed);
            }

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
                DataTableEnemies.EnemyTypes[] enemies = new DataTableEnemies.EnemyTypes[enemyIndex + 1 <= enemyTypes.Length ? enemyIndex + 1: enemyTypes.Length];

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
            private int m_BaseSpawnCount;
            private int m_BaseSpawnCountSpread;
            private int m_BaseSpawnRate;
            private int m_BaseSpawnRateSpread;
            //Enemies
            private float m_BaseSpeed;
            private int m_BaseSpeedSpreadPercent;
            private float m_BaseMaxSpeed;
            private float m_NewEnemyEachAmountOfLevels;   //CurLvl = 7. 7 / 2 = 3.5 = 4; Enemies [0] [1] [2] [3]
            private DataTableEnemies.EnemyTypes[] m_EnemyTypes;

            public AgeTypes AgeType => m_AgeType;
            //HP
            public int BaseHP => m_BaseHP;
            public int BaseHPSpreadPercent => m_BaseHPSpreadPercent;
            //Spawn
            public int BaseSpawnCount => m_BaseSpawnCount;
            public int BaseSpawnCountSpread => m_BaseSpawnCountSpread;
            public int BaseSpawnRate => m_BaseSpawnRate;
            public int BaseSpawnRateSpread => m_BaseSpawnRateSpread;
            //Enemies
            public float BaseSpeed => m_BaseSpeed;
            public int BaseSpeedSpreadPercent => m_BaseSpeedSpreadPercent;
            public float BaseMaxSpeed => m_BaseMaxSpeed;
            public float NewEnemyEachAmountOfLevels => m_NewEnemyEachAmountOfLevels;
            public DataTableEnemies.EnemyTypes[] EnemyTypes => m_EnemyTypes;

            public Level(AgeTypes ageType,
                        int baseHP, int baseHPSpreadPercent,                            //HP
                        int baseSpawnCount, int baseSpawnCountSpread,                   //Spawn
                        int baseSpawnRate, int baseSpawnRateSpread,
                        float baseSpeed, int baseSpeedSpreadPercent, float baseMaxSpeed,    //Enemies
                        float newEnemyEachAmountOfLevels, 
                        DataTableEnemies.EnemyTypes[] enemyTypes)
            {
                m_AgeType = ageType;

                //HP
                m_BaseHP = baseHP;
                m_BaseHPSpreadPercent = baseHPSpreadPercent;

                //Spawn
                m_BaseSpawnCount = baseSpawnCount;
                m_BaseSpawnCountSpread = baseSpawnCountSpread;

                m_BaseSpawnRate = baseSpawnRate;
                m_BaseSpawnRateSpread = baseSpawnRateSpread;

                //Enemies
                m_BaseSpeed = baseSpeed;
                m_BaseSpeedSpreadPercent = baseSpeedSpreadPercent;
                m_BaseMaxSpeed = baseMaxSpeed;

                m_NewEnemyEachAmountOfLevels = newEnemyEachAmountOfLevels;
                m_EnemyTypes = enemyTypes;
            }
        }
        #endregion
    }
}
