using clicker.battle.character;
using clicker.general;
using FrameworkPackage.PathCreation;
using PathCreation;
using System;
using System.Text;
using UnityEngine;

namespace clicker.battle.level
{
    public class EnemySpawnPoint : MonoBehaviour
    {
        public Action<EnemySpawnPoint> OnDestroyedAllEnemiesFromSpawn;
        public Action OnDestroyEnemy;

        public PathCreator PathController;

        //HP 
        private int m_HP;
        private int m_HPSpreadPercent;

        //Spawn
        private float m_SpawnRate;

        //Enemies
        private float m_Speed;
        private float m_SpeedSpreadPercent;
        private float m_MaxSpeed;

        private datatables.DataTableEnemies.EnemyTypes[] m_EnemyTypes;

        private const int m_RANDOM_VALUE_FOR_ADD = 35;
        private const int m_RANDOM_VALUE_FOR_REMOVE = 70;

        //Other
        private DateTime m_SpawnTime;
        private int m_CurSpawnCount = 0;
        private int m_DestroyedEnemiesCount = 0;
        private bool m_CanSpawn = false;

        public int ActualSpawnCount { get; private set; }

        public void Init(//HP 
                         int hp, int hpSpreadPercent,
                         //Spawn 
                         // - Spawn count
                         int spawnCount, int spawnCountSpread,
                         // - Spawn rate
                         float spawnRate, float spawnRateSpread,
                         //Enemies
                         // - Speed
                         float speed, float speedSpreadPercent, float maxSpeed,
                         // - Enemy
                         datatables.DataTableEnemies.EnemyTypes[] enemyTypes) 
        {
            //HP 
            m_HP = hp;
            m_HPSpreadPercent = hpSpreadPercent;

            //Spawn
            ActualSpawnCount = GetSpread(spawnCount, spawnCountSpread);
            m_SpawnRate =  GetSpread(spawnRate, spawnRateSpread);

            //Enemies
            m_Speed = speed;
            m_SpeedSpreadPercent = speedSpreadPercent;
            m_MaxSpeed = maxSpeed;

            m_EnemyTypes = enemyTypes;

            //Other
            m_DestroyedEnemiesCount = 0;
            m_CurSpawnCount = 0;

            Debug.Log(string.Format("SpawnPoint. Name: {0}. SpawnCount: {1}. SpawnRate: {2}", gameObject.name, ActualSpawnCount, m_SpawnRate));
        }

        public void StartSpawn()
        {
            m_SpawnTime = DateTime.Now;
            m_CanSpawn = true;
        }

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder(100);
            strBuilder.AppendFormat("Spawn Point\nRate: {0}\nCount: {1}\n", m_SpawnRate, ActualSpawnCount);

            return strBuilder.ToString();
        }


        void SpawnEnemy()
        {
            m_CurSpawnCount++;

            if (m_CurSpawnCount >= ActualSpawnCount)
                m_CanSpawn = false;

            int randomHP = GetSpreadByPercent(m_HP, m_HPSpreadPercent);
            float randomSpeed = GetSpreadByPercent(m_Speed, m_SpeedSpreadPercent);
            randomSpeed = Mathf.Clamp(randomSpeed, 0, m_MaxSpeed);
            datatables.DataTableEnemies.EnemyTypes randomEnemyType = m_EnemyTypes[UnityEngine.Random.Range(0, m_EnemyTypes.Length)];

            Enemy enemy = Instantiate(GameManager.Instance.AssetsLibrary.GetPrefab_Enemy(randomEnemyType), transform.position, Quaternion.identity);
            enemy.OnCharacterDestroyed += EnemyDestroyedHandler;
            enemy.Init(randomHP, randomSpeed, PathController);

            Debug.Log(string.Format("EnemySpawnPoint: Spawn enemy {0} with HP: {1} and speed: {2}", randomEnemyType, randomHP, randomSpeed));
        }

        void EnemyDestroyedHandler(Character enemyCharacter)
        {
            m_DestroyedEnemiesCount++;

            OnDestroyEnemy?.Invoke();

            if (m_DestroyedEnemiesCount == ActualSpawnCount)
                OnDestroyedAllEnemiesFromSpawn?.Invoke(this);
        }

        void Update()
        {
            if (GameManager.Instance.GameIsActive && m_CanSpawn && m_CurSpawnCount < ActualSpawnCount)
            {
                if ((m_SpawnTime - DateTime.Now).TotalSeconds <= 0)
                {
                    m_SpawnTime = DateTime.Now.AddSeconds(m_SpawnRate);

                    SpawnEnemy();
                }
            }
        }


        /// <summary>
        /// Разброс процентного значения. Версия для int
        /// </summary>
        int GetSpreadByPercent(int val, int percent)
        {
            float rawVal = val * (percent / 100f);
            int percentVal = (int)rawVal;

            return val + percentVal * GetSign();
        }

        /// <summary>
        /// Разброс процентного значения. Версия для float
        /// </summary>
        float GetSpreadByPercent(float val, float percent)
        {
            float rawVal = val * (percent / 100f);
            int percentVal = (int)rawVal;

            return val + percentVal * GetSign();
        }

        /// <summary>
        /// Разброс установленного значения. Версия функции для int
        /// </summary>
        /// <returns></returns>
        int GetSpread(int val, int spread)
        {
            int rawResult = val + spread * GetSign();            //val = 2, spread = 1. Result = 2 +- 1(0);
            return Mathf.Clamp(rawResult, 1, rawResult);
        }

        /// <summary>
        /// Разброс установленного значения. Версия функции для float
        /// </summary>
        float GetSpread(float val, float spread)
        {
            float rawResult = val + spread * GetSign();          //val = 2, spread = 1. Result = 2 +- 1(0);
            return Mathf.Clamp(rawResult, 1, rawResult);
        }

        int GetSign()
        {
            int sign = 0;
            int random = UnityEngine.Random.Range(0, 100);
            if (random > m_RANDOM_VALUE_FOR_ADD)
                sign = 1;
            else if (random > 70)
                sign = -1;

            return sign;
        }
    }
}
