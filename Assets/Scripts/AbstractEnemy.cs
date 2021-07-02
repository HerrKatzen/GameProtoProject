using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameProtoProject
{
    [RequireComponent(typeof(CharacterProperties))]
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class AbstarctEnemy : MonoBehaviour
    {
        protected string enemyType;
        protected CharacterProperties properties;
        protected Rigidbody2D rb;
        private bool died = false;
        private float dieTimer = 1f;
        protected abstract void UpdateAI();

        protected void Update()
        {
            if (properties.isDead)
            {
                if (!died)
                {
                    if (dieTimer == 1f)
                    {
                        dieTimer -= 0.05f;
                        StageHandler.Instance.IncreaseDeadCount(enemyType);
                        return;
                    }
                    if (dieTimer > 0f)
                    {
                        dieTimer -= Time.deltaTime;
                        return;
                    }
                    rb.isKinematic = true;
                    rb.velocity = Vector2.zero;
                    Collider2D c = GetComponent<Collider2D>();
                    if (c != null) c.enabled = false;
                    died = true;
                }
                return;
            }

            UpdateAI();
        }
    }
}
