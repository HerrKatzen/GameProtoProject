using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameProtoProject
{

    public class Portal : MonoBehaviour
    {
        public Portal Pair;
        private delegate void RemoveColliders();
        private HashSet<Collider2D> colliders = new HashSet<Collider2D>();
        public float offsetX = 0f;
        public bool verticalPortal = false;

        private float offsetY = 0f;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (Pair == null || collision.tag == "Target") return;
            if (!colliders.Contains(collision))
            {
                collision.transform.parent = transform;
                Pair.Teleport(collision);
                return;
            }
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (Pair == null || collision.tag == "Target") return;
            if (!colliders.Contains(collision))
            {
                
                Pair.Teleport(collision);
                return;
            }
        }

        IEnumerator TeleportCooldown(RemoveColliders rc)
        {
            yield return new WaitForSeconds(0.25f);
            rc.Invoke();
        }

        public void Teleport(Collider2D entity)
        {
            if (colliders.Contains(entity))
            {
                return;
            }
            offsetY = 0f;
            foreach (Collider2D c in entity.GetComponents<Collider2D>())
            {
                colliders.Add(c);
            }
            if (verticalPortal)
            {
                offsetX = entity.transform.localPosition.x;
            }
            else
            {
                offsetY = entity.transform.localPosition.y;
            }
            entity.transform.parent = transform;
            entity.transform.localPosition = new Vector3(offsetX, offsetY, 0f);
            entity.transform.parent = null;

            Debug.Log("Teleported to " + gameObject.name + " with positon of " + entity.transform.position);

            StartCoroutine(TeleportCooldown(() =>
                {
                    foreach (Collider2D c in entity.GetComponents<Collider2D>())
                    {
                        colliders.Remove(c);
                    }
                })
            );
        }
    }
}