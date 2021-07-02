using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameProtoProject
{
    [System.Serializable]
    public class Phasing : UnityEvent<Vector3,Vector3>
    {
    }

    [System.Serializable]
    public class IsMoving : UnityEvent<bool>
    {
    }

    public class PhaserAI : AbstarctEnemy
    {
        public float speed = 600f;
        public Transform rotatingObj;
        public LayerMask whatIsGround;
        public LayerMask whatIsWall;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private Transform wallCheck;
        [SerializeField] private Transform groundForwardCheck;

        public LayerMask playerLayers;
        public Vector2 attackRange = new Vector2(6f, 0.2f);

        public UnityEvent phaserAttackGfx;
        public Phasing phasing;
        public UnityEvent aggro;
        public UnityEvent idle;
        public IsMoving isMoving;

        private float attackMaxValue = 4f;
        private float attackCooldown = 0f;

        private Transform target;
        private bool turnAround = false;
        private float turnTimer = 0f;
        private float maxTurnTimer = 1f;
        private float phaseTimer = 0f;
        private float maxPhaseTimer = 1f;
        private bool isAggro = false;
        private Vector3 targetPos = Vector3.zero;
        private bool phaseSetup = false;
        private bool isPhasing = false;
        private float groundedRadius = 0.2f;
        private bool grounded = false;
        private bool canWalkForward = false;
        private bool isThereWall = false;
        private bool facingRight = true;
        private bool wasMoving = false;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            target = GameObject.FindGameObjectWithTag("Player").transform;
            properties = GetComponent<CharacterProperties>();
            enemyType = "Phaser";
            if (phaserAttackGfx == null) phaserAttackGfx = new UnityEvent();
            if (aggro == null) aggro = new UnityEvent();
            if (idle == null) idle = new UnityEvent();
            if (phasing == null) phasing = new Phasing();
            if (isMoving == null) isMoving = new IsMoving();
        }

        protected override void UpdateAI()
        {
            if (attackCooldown > 0f)
            {
                attackCooldown -= Time.deltaTime;
            }

            grounded = false;
            canWalkForward = false;
            isThereWall = false;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    grounded = true;
                }
            }
            Collider2D[] colliders2 = Physics2D.OverlapCircleAll(wallCheck.position, groundedRadius, whatIsWall);
            for (int i = 0; i < colliders2.Length; i++)
            {
                if (colliders2[i].gameObject != gameObject)
                {
                    isThereWall = true;
                }
            }
            Collider2D[] colliders3 = Physics2D.OverlapCircleAll(groundForwardCheck.position, groundedRadius, whatIsGround);
            for (int i = 0; i < colliders3.Length; i++)
            {
                if (colliders3[i].gameObject != gameObject)
                {
                    canWalkForward = true;
                }
            }

            if (!isAggro && Vector3.Distance(target.transform.position, transform.position) < 10f && attackCooldown <= 0f)
            {
                isPhasing = true;
                phaseSetup = true;
                isAggro = true;
            }

            if ((!canWalkForward || isThereWall) && !isAggro && !turnAround)
            {
                IdleTurnAround();
            }

            if (isAggro)
            {
                aggro.Invoke();

                if (isPhasing)
                {
                    Phase();
                }
                else
                {
                    Attack();
                }
            }
            else
            {
                Idle();
            }

            rb.velocity = Vector2.ClampMagnitude(rb.velocity, 50f);
        }

        public void Phase()
        {
            if (phaseSetup)
            {
                rb.velocity = new Vector2(0f, 0f);
                targetPos = target.position;
                
                if (target.position.x - transform.position.x > 0)
                {
                    if (!facingRight) Flip();
                }
                else
                {
                    if (facingRight) Flip();
                }
                
                phaseTimer = maxPhaseTimer;

                phaseSetup = false;

                phasing.Invoke(transform.position, targetPos);
            }

            if (phaseTimer > 0f)
            {
                phaseTimer -= Time.deltaTime;
                return;
            }

            transform.position = targetPos;
            isPhasing = false;
        }

        public void Attack()
        {
            isAggro = false;
            if (attackCooldown > 0f) return;
            phaserAttackGfx.Invoke();

            Collider2D[] player = Physics2D.OverlapBoxAll(transform.position, attackRange, 0f, playerLayers);

            foreach (var c in player)
            {
                CharacterProperties cp = c.GetComponent<CharacterProperties>();
                if (cp != null)
                {
                    cp.DamageCharacter(properties.attackDMG);
                    attackCooldown = attackMaxValue;
                    return;
                }
            }
        }

        public void Idle()
        {
            idle.Invoke();

            if (!grounded)
            {
                Vector2 force2 = transform.right * speed * Time.deltaTime;
                rb.AddForce(force2);
                if (!wasMoving)
                {
                    wasMoving = true;
                    isMoving.Invoke(true);
                }
                return;
            }
            if (turnAround)
            {
                if (wasMoving)
                {
                    wasMoving = false;
                    isMoving.Invoke(false);
                }
                rb.velocity = Vector2.zero;
                if (turnTimer > 0f)
                {
                    turnTimer -= Time.deltaTime;
                    return;
                }
                Flip();
                turnAround = false;
            }

            if (!wasMoving)
            {
                wasMoving = true;
                isMoving.Invoke(true);
            }

            Vector2 force = transform.right * speed * Time.deltaTime;
            rb.AddForce(force);
        }

        public void IdleTurnAround()
        {
            turnTimer = maxTurnTimer;
            turnAround = true;
        }

        private void Flip()
        {
            facingRight = !facingRight;

            transform.Rotate(0f, 180f, 0f);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(transform.position, attackRange);
        }
    }
}