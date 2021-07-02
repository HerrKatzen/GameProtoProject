using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameProtoProject
{
    public class Potion : Effect
    {
        private void Start()
        {
            TTL = 0f;
        }

        public override void ApplyEffect()
        {
            owner.health = Mathf.Clamp(owner.health + 50f, owner.health, owner.maxHealth);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag != "Player") return;
            CharacterProperties cp = collision.GetComponent<CharacterProperties>();
            if (cp == null) return;
            SetOwner(cp);
            cp.AddEffect(this);
            gameObject.SetActive(false);
        }
    }
}