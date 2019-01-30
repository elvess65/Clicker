using UnityEngine;
using clicker.datatables;
using static clicker.datatables.DataTableEnemies;
using static clicker.datatables.DataTableLevels;

namespace clicker.battle.level
{
    public class LevelController : MonoBehaviour
    {
        public System.Action OnLevelFinished;

        public EnemySpawnPoint[] SpawnPoints;

        private int m_SpawnpointsDestroyedEnemiesCount = 0;


        public void Init(AgeTypes age, int level)
        {
            Debug.LogWarning(string.Format("LevelController: Start level. Age: {0}. Level: {1}", age, level));

            //HP
            int hp = DataTableLevels.GetHP(age, level);             //TODO: add spread
            int hpSpread = 5;   //percent

            //Spawn
            int count = DataTableLevels.GetSpawnCount(age, level);  //TODO: add spread
            int countSpread = 10;   //percent

            int rate = DataTableLevels.GetSpawnRate(age, level);    //TODO: add spread
            int rateSpread = 15;    //percent

            //Enemies
            int speed = 3;                                          //TODO: add spread
            int speedSpread = 15;   //percent
            EnemyTypes[] enemies = DataTableLevels.GetEnemiesForLevel(age, level); 

            //Spawn points
            for (int i = 0; i < SpawnPoints.Length; i++)
            {
                SpawnPoints[i].OnDestroyedAllEnemiesFromSpawn += DestroyedAllEnemiesFromSpawn;
                SpawnPoints[i].Init(rate, count, hp, speed, enemies);
            }
        }

        public void StartSpawn()
        {
            for (int i = 0; i < SpawnPoints.Length; i++)
                SpawnPoints[i].StartSpawn();
        }


        void DestroyedAllEnemiesFromSpawn(EnemySpawnPoint spawnPoint)
        {
            spawnPoint.OnDestroyedAllEnemiesFromSpawn -= DestroyedAllEnemiesFromSpawn;

            m_SpawnpointsDestroyedEnemiesCount++;

            if (m_SpawnpointsDestroyedEnemiesCount >= SpawnPoints.Length)
                OnLevelFinished?.Invoke();
        }
    }
}
