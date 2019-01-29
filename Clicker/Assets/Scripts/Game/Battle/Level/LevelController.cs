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
            //HP
            int hp = DataTableLevels.GetHP(age, level);             //TODO: add spread
            
            //Spawn
            int count = DataTableLevels.GetSpawnCount(age, level);  //TODO: add spread
            int rate = DataTableLevels.GetSpawnRate(age, level);    //TODO: add spread

            //Enemies
            int speed = 3;                                          //TODO: add spread
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
