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
                                        data[i].BaseHP, 
                                        data[i].HPSpreadPercent, 
                                        data[i].NewEnemyEachAmountOfLevels, 
                                        data[i].BaseSpawnCount, 
                                        data[i].BaseSpawnRate,
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
            }

            return 1;
        }

        //Spawn
        public static int GetSpawnCount(AgeTypes age, int lvl)
        {
            if (m_Levels.ContainsKey(age))
                return m_Levels[age].BaseSpawnCount;

            return 1;
        }

        public static int GetSpawnRate(AgeTypes age, int lvl)
        {
            if (m_Levels.ContainsKey(age))
                return m_Levels[age].BaseSpawnRate;

            return 1;
        }

        //Enemies
        public static DataTableEnemies.EnemyTypes[] GetEnemiesForLevel(AgeTypes age, int lvl)
        {
            if (m_Levels.ContainsKey(age))
                return m_Levels[age].EnemyTypes;

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
            private int m_BaseHP;                       //HP = HPFunc(curLevel, BaseHP)
            private int m_HPSpreadPercent;              //HPFunc { result +- HPSpreadPercent }
            //Spawn
            private int m_BaseSpawnCount;
            private int m_BaseSpawnRate;
            //Enemies
            private int m_NewEnemyEachAmountOfLevels;   //CurLvl = 7. 7 / 2 = 3.5 = 4; Enemies [0] [1] [2] [3]
            private DataTableEnemies.EnemyTypes[] m_EnemyTypes;

            public AgeTypes AgeType => m_AgeType;
            //HP
            public int BaseHP => m_BaseHP;
            public int HPSpreadPercent => m_HPSpreadPercent;
            //Spawn
            public int BaseSpawnCount => m_BaseSpawnCount;
            public int BaseSpawnRate => m_BaseSpawnRate;
            //Enemies
            public int NewEnemyEachAmountOfLevels => m_NewEnemyEachAmountOfLevels;
            public DataTableEnemies.EnemyTypes[] EnemyTypes => m_EnemyTypes;

            public Level(AgeTypes ageType, int baseHP, int hpSpreadPercent, int newEnemyEachAmountOfLevels, int baseSpawnCount, int baseSpawnRate, DataTableEnemies.EnemyTypes[] enemyTypes)
            {
                m_AgeType = ageType;
                //HP
                m_BaseHP = baseHP;
                m_HPSpreadPercent = hpSpreadPercent;
                //Spawn
                m_BaseSpawnCount = baseSpawnCount;
                m_BaseSpawnRate = baseSpawnRate;
                //Enemies
                m_NewEnemyEachAmountOfLevels = newEnemyEachAmountOfLevels;
                m_EnemyTypes = enemyTypes;
            }
        }
        #endregion
    }
}
