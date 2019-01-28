using UnityEngine;
using System.Collections.Generic;
using clicker.datatables;
using static clicker.datatables.DataTableEnemies;

namespace clicker.battle.level
{
    public class LevelController : MonoBehaviour
    {
        public EnemySpawnPoint[] SpawnPoints;

        public void Init(int age, int level)
        {
            int hp = DataTableLevels.GetHP(age, level);

            int count = DataTableLevels.GetSpawnCount(age, level);
            int rate = DataTableLevels.GetSpawnRate(age, level);

            int[] enemies = DataTableLevels.GetEnemiesForLevel(age, level);

            for (int i = 0; i < SpawnPoints.Length; i++)
                SpawnPoints[i].Init(rate, count, hp, enemies);
        }

        public void StartSpawn()
        {
            for (int i = 0; i < SpawnPoints.Length; i++)
                SpawnPoints[i].StartSpawn();
        }


        public enum SpawnPointTypes
        {
            SpawnPoint_Boss,
            SpawnPoint_1,
            SpawnPoint_2,
            SpawnPoint_3
        }

        public class Level
        {
            public float DelayBeforeStart;
            public float DelayBtwSpawn;

            public List<Spawn> SpawnList;

            public Level()
            {
                DelayBeforeStart = 2;
                DelayBtwSpawn = 1;

                SpawnList = new List<Spawn>();
                SpawnList.Add(new Spawn());
                SpawnList.Add(new Spawn());
                SpawnList.Add(new Spawn());
            }

            public class Spawn
            {
                public (int min, int max) EnemyPossibleHealth = (1, 2);
                public (int min, int max) EnemyPossibleSpeed = (1, 2);
                public EnemyTypes[] PossibleEnemies;

                public void SpawnEnemy()
                {
                    int health = Random.Range(EnemyPossibleHealth.min, EnemyPossibleHealth.max + 1);
                    int speed = Random.Range(EnemyPossibleSpeed.min, EnemyPossibleSpeed.max + 1);
                    EnemyTypes type = (EnemyTypes)Random.Range(0, PossibleEnemies.Length);

                    Debug.Log(string.Format("Spawn enemy {0}. Health: {1}. Speed {2}", type, health, speed));
                }
            }
        }
    }
}
