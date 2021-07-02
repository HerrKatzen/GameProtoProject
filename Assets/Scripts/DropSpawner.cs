using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameProtoProject
{

    public class DropSpawner : MonoBehaviour
    {
        public Vector2 spawnRange;
        public List<Effect> spawnableEffets;
        public LayerMask whatIsObstacle;

        private void Start()
        {
            StageHandler.Instance.waveCleared += SpawnDrop;
        }

        private void SpawnDrop()
        {
            foreach (var item in spawnableEffets)
            {
                bool shouldSpawn = (Random.value > 0.5f);
                if (shouldSpawn)
                {
                    Effect instance = Instantiate(
                        item,
                        new Vector3(
                            Random.Range(transform.position.x - (spawnRange.x / 2f), transform.position.x + (spawnRange.x / 2f)),
                            Random.Range(transform.position.y - (spawnRange.y / 2f), transform.position.y + (spawnRange.y / 2f)),
                            0f), 
                        Quaternion.identity);
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(instance.transform.position, 0.2f, whatIsObstacle);
                    if (colliders.Length > 0)
                    {
                        Destroy(instance.gameObject);
                    }
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(transform.position, spawnRange);
        }

        private void OnDestroy()
        {
            if (StageHandler.Instance != null)
                StageHandler.Instance.waveCleared -= SpawnDrop;
        }
    }
}
