using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameProtoProject
{
    [RequireComponent(typeof(Seeker))]
    public class FlyingAI : AbstarctEnemy
    {
        public float nextWaypointDistance = 3f;
        public Transform enemyGFX;
        public Transform firePoint;
        public float attackRange = 10f;
        public LayerMask playerLayers;
        public GameObject bulletPrefab;

        public UnityEvent shootGfx;

        Path path;
        int currentWaypoint = 0;
        bool reachedEndOfPath = false;
        private Transform target;
        private float attackCooldown = 0f;
        private float attackMaxValue = 1.5f;
        private bool facingRight = true;

        Seeker seeker;

        void Start()
        {
            seeker = GetComponent<Seeker>();
            rb = GetComponent<Rigidbody2D>();
            target = GameObject.FindGameObjectWithTag("Player").transform;
            properties = GetComponent<CharacterProperties>();
            enemyType = "Flying";

            InvokeRepeating("UpdatePath", 0f, 0.5f);
            seeker.StartPath(rb.position, target.position, OnPathComplete);
            if (shootGfx == null) shootGfx = new UnityEvent();
        }

        protected override void UpdateAI()
        {

            if (Vector2.Distance(target.position, transform.position) <= attackRange)
            {
                if (attackCooldown > 0f)
                {
                    attackCooldown -= Time.deltaTime;
                }
                else
                {
                    Attack();
                }
            }

            if (path == null) return;

            if (currentWaypoint > path.vectorPath.Count)
            {
                reachedEndOfPath = true;
                return;
            }
            else
            {
                reachedEndOfPath = false;
            }
            if (currentWaypoint >= path.vectorPath.Count) return;
            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * properties.speed * Time.deltaTime;

            rb.AddForce(force);

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            if (distance < nextWaypointDistance && !reachedEndOfPath)
            {
                currentWaypoint++;
            }

            if (rb.velocity.x > 0.01f)
            {
                if (!facingRight)
                {
                    Flip();
                }
            }
            if (rb.velocity.x < -0.01f)
            {
                if (facingRight)
                {
                    Flip();
                }
            }
        }

        void UpdatePath()
        {
            if (seeker.IsDone())
            {
                seeker.StartPath(rb.position, target.position, OnPathComplete);
            }
        }

        void OnPathComplete(Path p)
        {
            if (!p.error)
            {
                path = p;
                currentWaypoint = 0;
            }
        }


        public void Attack()
        {
            Collider2D[] player = Physics2D.OverlapCircleAll(transform.position, attackRange, playerLayers);

            foreach (var c in player)
            {
                CharacterProperties cp = c.GetComponent<CharacterProperties>();
                if (cp != null)
                {
                    Shoot();
                    attackCooldown = attackMaxValue;
                    shootGfx.Invoke();
                    return;
                }
            }
        }

        private void Shoot()
        {
            GameObject b = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            b.transform.right = (target.transform.position - transform.position);
            b.GetComponent<Bullet>().damage = properties.attackDMG;
        }
        private void Flip()
        {
            Debug.Log("flipped right " + facingRight);
            facingRight = !facingRight;

            transform.Rotate(0f, 180f, 0f);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}