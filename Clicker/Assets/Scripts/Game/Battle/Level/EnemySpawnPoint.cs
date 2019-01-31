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

        //HP 
        private int m_HP;
        private int m_HPSpread;

        //Spawn
        private int m_SpawnCount;
        private float m_SpawnRate;

        //Enemies
        private int m_Speed;
        private int m_SpeedSpread;

        private datatables.DataTableEnemies.EnemyTypes[] m_EnemyTypes;

        //Other
        private DateTime m_SpawnTime;
        private int m_CurSpawnCount = 0;
        private int m_DestroyedEnemiesCount = 0;
        private bool m_CanSpawn = false;
 
        public void Init(int hp, int hpSpread,                                  //HP 
                         int spawnCount, int spawnCountSpread,                  //Spawn
                         float spawnRate, int spawnRateSpread, 
                         int speed, int speedSpread,                            //Enemies
                         datatables.DataTableEnemies.EnemyTypes[] enemyTypes) 
        {
            //HP 
            m_HP = hp;
            m_HPSpread = hpSpread;

            //Spawn
            m_SpawnCount = spawnCount;  //TODO: Randomize
            m_SpawnRate = spawnRate;    //TODO: Randomize

            //Enemies
            m_Speed = speed;
            m_SpeedSpread = speedSpread;
            m_EnemyTypes = enemyTypes;

            //Other
            m_DestroyedEnemiesCount = 0;
            m_CurSpawnCount = 0;
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

            int randomHP = m_HP;        //TODO: Randomize
            int randomSpeed = m_Speed;  //TODO: Randomize
            datatables.DataTableEnemies.EnemyTypes randomEnemyType = m_EnemyTypes[UnityEngine.Random.Range(0, m_EnemyTypes.Length)];

            Enemy enemy = Instantiate(GameManager.Instance.AssetsLibrary.GetPrefab_Enemy(randomEnemyType), transform.position, Quaternion.identity);
            enemy.OnCharacterDestroyed += EnemyDestroyedHandler;
            enemy.Init(randomHP, randomSpeed, PathController);

            Debug.Log(string.Format("EnemySpawnPoint: Spawn enemy {0} with HP: {1} and speed: {2}", randomEnemyType, randomHP, randomSpeed));
        }

        void EnemyDestroyedHandler(Character enemyCharacter)
        {
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
