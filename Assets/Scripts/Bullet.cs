using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameProtoProject
{
    public class Bullet : MonoBehaviour
    {
        public float speed = 10f;
        public float damage = 10f;
        public float TTL = 3f;

        private Rigidbody2D rb;
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.velocity = transform.right * speed;
        }

        private void Update()
        {
            if (TTL <= 0f) Destroy(gameObject);
            TTL -= Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Portal") return;

            CharacterProperties cp = collision.GetComponent<CharacterProperties>();
            if (cp != null && collision.tag == "Player")
            {
                cp.DamageCharacter(damage);
            }
            Destroy(gameObject);
        }
    }
}