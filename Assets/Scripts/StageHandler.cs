using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameProtoProject
{
    public delegate void WaveCleared();

    public class StageHandler : Singleton<StageHandler>
    {
        public Cinemachine.CinemachineVirtualCamera vCam;
        public Stage stage;
        public UnityEvent stageCleared;
        public List<Transform> spawnPoints = new List<Transform>();
        public WaveCleared waveCleared;

        public int waveCounter { get; private set; } = -1;

        private int totalKillCount = 0;

        public Dictionary<string, int> DeadMap { get; private set; } = new Dictionary<string, int>();

        private void SetDifficulty(string difficulty)
        {
            stage = transform.Find(string.Format("Stage - {0}", difficulty)).GetComponent<Stage>();
        }

        private void Start()
        {
            if (stageCleared == null) stageCleared = new UnityEvent();
            SetDifficulty(GlobalSettings.Instance.Difficulty);
            StartStage(); // debug only
        }

        public void StartStage()
        {
            stage.StartStage();
        }

        public void IncreaseDeadCount(string key)
        {
            int count = -1;
            DeadMap.TryGetValue(key, out count);
            if (count == -1)
            {
                DeadMap.Add(key, 1);
            }
            else
            {
                count++;
                DeadMap.Remove(key);
                DeadMap.Add(key, count);
            }

            totalKillCount++;
            if (totalKillCount == stage.totalEnemyCount)
            {
                stageCleared.Invoke();
            }
        }

        public void Spawn(SpawnEvent caller)
        {
            StartCoroutine(SpawnCO(caller));
        }

        private IEnumerator SpawnCO(SpawnEvent caller)
        {
            for (int i = 0; i < caller.numberOfEnemies; i++)
            {
                AbstarctEnemy enemy = Instantiate(caller.enemyType, spawnPoints[caller.spawnPointIndex].position, Quaternion.identity);
                enemy.GetComponent<CharacterProperties>().characterDied.AddListener(caller.IncreaseDeadCount);
                ParticleCollision pc = enemy.GetComponentInChildren<ParticleCollision>();
                if (pc != null) pc.cam = vCam;
                caller.spawnedEnemies.Add(enemy);
                yield return caller.internalSpawnDelay;
            }
            yield return null;
        }

        private void OnWaveCleared()
        {
            waveCounter++;
        }

        public void StartWaves()
        {
            waveCleared += OnWaveCleared;
            waveCounter = 1;
        }
    }
}