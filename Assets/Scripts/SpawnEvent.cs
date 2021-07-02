using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameProtoProject
{
    [System.Serializable]
    public class SpawnEvent
    {
        public AbstarctEnemy enemyType;
        public int numberOfEnemies = 1;
        public int spawnPointIndex = 0;
        public SpawnType spawnType;
        public float spawnDelay = 0f;

        public bool AllCharacterDead { get; private set; } = false;
        public List<AbstarctEnemy> spawnedEnemies { get; private set; } = new List<AbstarctEnemy>();

        public WaitForSeconds internalSpawnDelay { get; private set; } = new WaitForSeconds(1f);
        private int deadCount = 0;

        public void Spawn()
        {
            if (spawnPointIndex >= StageHandler.Instance.spawnPoints.Count) return;
            StageHandler.Instance.Spawn(this);
        }
        public void IncreaseDeadCount()
        {
            deadCount++;

            if (deadCount == spawnedEnemies.Count)
            {
                AllCharacterDead = true;
                StageHandler.Instance.StartCoroutine(DestoryAllSpawned());
            }
        }

        IEnumerator DestoryAllSpawned()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForFixedUpdate();
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < spawnedEnemies.Count; i++)
            {
                AbstarctEnemy enemy = spawnedEnemies[i];
                spawnedEnemies.RemoveAt(i);
                MonoBehaviour.Destroy(enemy.gameObject);
                i--;
            }
            yield return null;
        }

        [System.Serializable]
        public enum SpawnType
        {
            DelayNext,
            WaitUntilAllDead
        }
    }
}