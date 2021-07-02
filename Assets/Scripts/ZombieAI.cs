using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameProtoProject
{
    public class ZombieAI : AbstarctEnemy
    {
        public float jumpForce = 12f;
        public Transform enemyGFX;

        public LayerMask playerLayers;
        public Transform attackPoint;
        public float attackRange = 0.3f;

        public UnityEvent zombieAttack;

        private float attackMaxValue = 0.8f;
        private float attackCooldown = 0f;
        private Transform target;
        private bool runOnce = false;
        private bool facingRight = true;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            properties = GetComponent<CharacterProperties>();
            target = GameObject.FindGameObjectWithTag("Player").transform;
            enemyType = "Zombie";
            if (zombieAttack == null) zombieAttack = new UnityEvent();
        }
        protected override void UpdateAI()
        {

            if (target.position.x - rb.position.x > 0f)
            {
                if (!facingRight)
                {
                    Flip();
                }
            }
            else
            {
                if (facingRight)
                {
                    Flip();
                }
            }

            Vector2 force = transform.right * properties.speed * Time.deltaTime;
            rb.AddForce(force);

            rb.velocity = Vector2.ClampMagnitude(rb.velocity, 20f);

            if (Vector2.Distance(target.position, transform.position) < 2f)
            {
                if (attackCooldown > 0f) 
                {
                    attackCooldown -= Time.deltaTime;
                    return;
                }
                Attack();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (properties.isDead) return;
            if (other.tag != "Jump") return;

            runOnce = false;
            if (((target.position.y - rb.position.y) * 2f) > Mathf.Abs(target.position.x - rb.position.x))
            {
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (properties.isDead) return;
            if (collision.tag != "Jump") return;

            if (runOnce) return;
            if (Mathf.Abs(rb.velocity.x) < 0.2f && Vector2.Distance(target.transform.position, rb.position) > 1.5f)
            {
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                runOnce = true;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (properties.isDead) return;
            if (collision.tag != "Jump") return;
            runOnce = false;
        }

        public void Attack()
        {
            Collider2D[] player = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);

            foreach (var c in player)
            {
                CharacterProperties cp = c.GetComponent<CharacterProperties>();
                if (cp != null) 
                { 
                    cp.DamageCharacter(properties.attackDMG);
                    attackCooldown = attackMaxValue;
                    zombieAttack.Invoke();
                    return;
                }
            }
        }

        private void Flip()
        {
            facingRight = !facingRight;

            transform.Rotate(0f, 180f, 0f);
        }

        private void OnDrawGizmosSelected()
        {
            if (attackPoint == null) return;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}