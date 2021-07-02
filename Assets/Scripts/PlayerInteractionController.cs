using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameProtoProject
{
    [RequireComponent(typeof(CharacterProperties))]
    [RequireComponent(typeof(CharacterController))]
    public class PlayerInteractionController : MonoBehaviour
    {
        public LayerMask enemyLayers;
        public Transform attackPoint;
        public float attackRange = 0.5f;
        public float attackStrenght = 5f;

        private float attackMaxValue = 0.5f;
        private float attackCooldown = 0f;
        private bool attacking = false;
        private CharacterProperties properties;
        private CharacterController controller;

        private void Start()
        {
            properties = GetComponent<CharacterProperties>();
            controller = GetComponent<CharacterController>();
        }

        void Update()
        {
            if (properties.isDead) return;

            if (attackCooldown > 0f)
            {
                attackCooldown -= Time.deltaTime;
                return;
            }
            attacking = Input.GetButtonDown("Fire1");

            if (attacking)
            {
                attacking = false;
                attackCooldown = attackMaxValue;
                Attack();
            }
        }

        private void Attack()
        {
            GfxEventHandler.Instance.playerAttack.Invoke();

            Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            foreach (var c in enemies)
            {
                CharacterProperties cp = c.GetComponent<CharacterProperties>();
                if (cp != null)
                {
                    cp.DamageCharacter(properties.attackDMG);  
                    float direction = 1f;
                    if (!controller.FacingRight) direction = -1f;
                    c.attachedRigidbody.AddForce(new Vector2(direction * attackStrenght, 0f), ForceMode2D.Impulse);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (attackPoint == null) return;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}