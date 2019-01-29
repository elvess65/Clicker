using clicker.battle.character;
using System.Collections.Generic;
using UnityEngine;
using static clicker.datatables.DataTableEnemies;

namespace clicker.general
{
    public class AssetsLibrary : MonoBehaviour
    {
        public EnemyData[] Enemies;

        private Dictionary<EnemyTypes, EnemyData> m_Enemies;

        void Awake()
        {
            InitEnemies();
        }

        void InitEnemies()
        {
            m_Enemies = new Dictionary<EnemyTypes, EnemyData>();
            for (int i = 0; i < Enemies.Length; i++)
            {
                if (!m_Enemies.ContainsKey(Enemies[i].Type))
                    m_Enemies.Add(Enemies[i].Type, Enemies[i]);
            }
        }


        public Enemy GetPrefab_Enemy(EnemyTypes type)
        {
            if (m_Enemies.ContainsKey(type))
                return m_Enemies[type].Prefab;

            return null;
        }


        [System.Serializable]
        public struct EnemyData
        {
            public EnemyTypes Type;
            public Enemy Prefab;
        }
    }
}
