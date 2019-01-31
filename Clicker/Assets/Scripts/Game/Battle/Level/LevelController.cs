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
            int hp = DataTableLevels.GetHP(age, level);             
            int hpSpread = DataTableLevels.GetHPSpread(age, level);

            //Spawn
            int spawnCount = DataTableLevels.GetSpawnCount(age, level);  
            int spawnCountSpread = 10;   

            int rate = DataTableLevels.GetSpawnRate(age, level);    
            int rateSpread = 15;    

            //Enemies
            int speed = DataTableLevels.GetSpeed(age, level);
            int speedSpread = 15;  

            EnemyTypes[] enemies = DataTableLevels.GetEnemiesForLevel(age, level);

            Debug.LogWarning(string.Format("LevelController: Start level. Age: {0}. Level: {1}", age, level));           
            Debug.Log(string.Format(" HP {0}. HPSpread {1} \n SpawnCount: {2}. SpawnCountSpread: {3} \n SpawnRate: {4}. SpawnRateSpread: {5} \n Speed: {6}. SpeedSpread: {7}",
                                    hp, hpSpread,
                                    spawnCount, spawnCountSpread,
                                    rate, rateSpread,
                                    speed, speedSpread));

            Debug.Log("Enemies:\n");
            for (int i = 0; i < enemies.Length; i++)
                Debug.Log(string.Format("{0}\n", enemies[i]));

            //Spawn points
            for (int i = 0; i < SpawnPoints.Length; i++)
            {
                SpawnPoints[i].OnDestroyedAllEnemiesFromSpawn += DestroyedAllEnemiesFromSpawn;
                SpawnPoints[i].Init(hp,             hpSpread,                   //HP
                                    spawnCount,     spawnCountSpread,           //Spawn
                                    rate,           rateSpread, 
                                    speed,          speedSpread,                //Enemies
                                    enemies);
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
