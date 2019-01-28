using System;
using UnityEngine;

namespace clicker.battle.level
{
    public class EnemySpawnPoint : MonoBehaviour
    {
        private float m_SpawnRate;
        private int m_SpawnCount;
        private int m_HP;

        private DateTime m_SpawnTime;
        private int m_CurSpawnCount = 0;
        private bool m_CanSpawn = false;

        public void Init(float spawnRate, int spawnCount, int hp, int[] enemies)
        {
            m_SpawnRate = spawnRate;
            m_SpawnCount = spawnCount;
            m_HP = hp;
        }

        public void StartSpawn()
        {
            m_SpawnTime = DateTime.Now;
            m_CanSpawn = true;
        }


        void SpawnEnemy()
        {
            Debug.Log("Spawn enemy: " + m_HP);
        }

        void Update()
        {
            if (m_CanSpawn && m_CurSpawnCount < m_SpawnCount)
            {
                if ((m_SpawnTime - DateTime.Now).TotalSeconds <= 0)
                {
                    m_SpawnTime = DateTime.Now.AddSeconds(m_SpawnRate);
                    m_CurSpawnCount++;

                    SpawnEnemy();
                }
            }
        }
    }
}
