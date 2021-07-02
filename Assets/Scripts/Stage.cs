using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameProtoProject
{
    public class Stage : MonoBehaviour
    {
        public StageType stageType;
        [Space]
        public List<SpawnEvent> spawnEvents;

        public List<AbstarctEnemy> AllEnemyTypes;
        public int totalEnemyCount { get; private set; } = 0;

        public void StartStage()
        {
            if(stageType == StageType.staged)
            {
                foreach (var e in spawnEvents)
                {
                    totalEnemyCount += e.numberOfEnemies;
                }

                StartCoroutine(StartStageCO());
            }
            if (stageType == StageType.endless)
            {
                StartCoroutine(StartEndless());
            }
        }

        private IEnumerator StartEndless()
        {
            StageHandler.Instance.StartWaves();
            int randCount;
            List<SpawnEvent> events = new List<SpawnEvent>();
            while (enabled)
            {
                randCount = Random.Range(1, StageHandler.Instance.waveCounter);
                for (int i = 0; i < randCount; i++)
                {
                    SpawnEvent se = new SpawnEvent();
                    se.enemyType = AllEnemyTypes[Random.Range(0, AllEnemyTypes.Count)];
                    se.spawnPointIndex = Random.Range(0, StageHandler.Instance.spawnPoints.Count);
                    se.numberOfEnemies = 1;
                    events.Add(se);
                    se.Spawn();
                }
                foreach (var e in events)
                {
                    yield return new WaitUntil(() => e.AllCharacterDead);
                }
                StageHandler.Instance.waveCleared.Invoke();
                yield return new WaitForSeconds(1f);
            }
        }

        private IEnumerator StartStageCO()
        {
            for (int i = 0; i < spawnEvents.Count; i++)
            {
                spawnEvents[i].Spawn();
                if(spawnEvents[i].spawnType == SpawnEvent.SpawnType.DelayNext)
                {
                    yield return new WaitForSeconds(spawnEvents[i].spawnDelay);
                }
                else
                {
                    for (int j = 0; j <= i; j++)
                    {
                        yield return new WaitUntil(() => spawnEvents[j].AllCharacterDead);
                    }
                }
                StageHandler.Instance.waveCleared.Invoke();
            }
            yield return null;
        }

    }

    [System.Serializable]
    public enum StageType
    {
        staged,
        endless
    }
}