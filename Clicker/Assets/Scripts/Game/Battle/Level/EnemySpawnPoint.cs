using clicker.battle.character;
using clicker.general;
using FrameworkPackage.iTween;
using System;
using System.Text;
using UnityEngine;

namespace clicker.battle.level
{
    public class EnemySpawnPoint : MonoBehaviour
    {
        public Action<EnemySpawnPoint> OnDestroyedAllEnemiesFromSpawn;

        public iTweenPath PathController;

        private float m_SpawnRate;
        private int m_SpawnCount;
        private int m_HP;
        private int m_Speed;

        private DateTime m_SpawnTime;
        private int m_CurSpawnCount = 0;
        private int m_DestroyedEnemiesCount = 0;
        private bool m_CanSpawn = false;
        private datatables.DataTableEnemies.EnemyTypes[] m_EnemyTypes;

        public void Init(float spawnRate, int spawnCount, int hp, int speed, datatables.DataTableEnemies.EnemyTypes[] enemyTypes)
        {
            m_DestroyedEnemiesCount = 0;
            m_CurSpawnCount = 0;

            m_SpawnRate = spawnRate;
            m_SpawnCount = spawnCount;
            m_HP = hp;
            m_Speed = speed;

            m_EnemyTypes = enemyTypes;

            Debug.Log(ToString());
        }

        public void StartSpawn()
        {
            m_SpawnTime = DateTime.Now;
            m_CanSpawn = true;
        }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder(100);
            strBuilder.AppendFormat("Spawn Point\nRate: {0}\nCount: {1}\n", m_SpawnRate, m_SpawnCount);

            return strBuilder.ToString();
        }


        void SpawnEnemy()
        {
            m_CurSpawnCount++;

            if (m_CurSpawnCount >= m_SpawnCount)
                m_CanSpawn = false;

            datatables.DataTableEnemies.EnemyTypes randomEnemyType = m_EnemyTypes[UnityEngine.Random.Range(0, m_EnemyTypes.Length)];

            Enemy enemy = Instantiate(GameManager.Instance.AssetsLibrary.GetPrefab_Enemy(randomEnemyType), transform.position, Quaternion.identity);
            enemy.OnCharacterDestroyed += EnemyDestroyedHandler;
            enemy.Init(m_HP, m_Speed, PathController);

            Debug.Log(string.Format("EnemySpawnPoint: Spawn enemy {0} with HP: {1}", randomEnemyType, m_HP));
        }

        void EnemyDestroyedHandler(Character enemyCharacter)
        {
            Debug.Log("EnemySpawnPoint: Enemy was destroyed " + enemyCharacter.gameObject.name);

            m_DestroyedEnemiesCount++;

            if (m_DestroyedEnemiesCount == m_SpawnCount)
            {
                Debug.Log("EnemySpawnPoint: All enemies destroyed");

                OnDestroyedAllEnemiesFromSpawn?.Invoke(this);
            }
        }

        void Update()
        {
            if (GameManager.Instance.GameIsActive && m_CanSpawn && m_CurSpawnCount < m_SpawnCount)
            {
                if ((m_SpawnTime - DateTime.Now).TotalSeconds <= 0)
                {
                    m_SpawnTime = DateTime.Now.AddSeconds(m_SpawnRate);

                    SpawnEnemy();
                }
            }
        }
    }
}
