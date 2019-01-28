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
                                        data[i].Enemies);

                m_Levels.Add(data[i].AgeType, level);
            }
        }

        //HP
        public static int GetHP(int age, int lvl)
        {
            return 1;
        }

        //Spawn
        public static int GetSpawnCount(int age, int lvl)
        {
            return 1;
        }

        public static int GetSpawnRate(int age, int lvl)
        {
            return 1;
        }

        //Enemies
        public static int[] GetEnemiesForLevel(int age, int lvl)
        {
            return new int[] { 1, 2, 3 };
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
            private int[] m_Enemies;

            public AgeTypes AgeType => m_AgeType;
            //HP
            public int BaseHP => m_BaseHP;
            public int HPSpreadPercent => m_HPSpreadPercent;
            //Spawn
            public int BaseSpawnCount => m_BaseSpawnCount;
            public int BaseSpawnRate => m_BaseSpawnRate;
            //Enemies
            public int NewEnemyEachAmountOfLevels => m_NewEnemyEachAmountOfLevels;
            public int[] Enemies => m_Enemies;

            public Level(AgeTypes ageType, int baseHP, int hpSpreadPercent, int newEnemyEachAmountOfLevels, int baseSpawnCount, int baseSpawnRate, int[] enemies)
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
                m_Enemies = enemies;
            }
        }
        #endregion
    }
}
