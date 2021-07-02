using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameProtoProject
{

    public class CharacterProperties : MonoBehaviour
    {
        public float maxHealth;
        [HideInInspector]
        public float health;
        public float speed;
        public float attackDMG;
        [HideInInspector]
        public bool isDead = false;
        public UnityEvent characterHit;
        public UnityEvent characterDied;

        private List<Effect> effects = new List<Effect>();

        protected void Start()
        {
            if (characterHit == null) characterHit = new UnityEvent();
            if (characterDied == null) characterDied = new UnityEvent();
            health = maxHealth;
        }

        private void Update()
        {
            if (isDead) return;

            for (int i = 0; i < effects.Count; i++)
            {
                effects[i].ApplyEffect();
                effects[i].AgeEffect(Time.deltaTime);
                if (effects[i].TTL <= 0)
                {
                    Effect e = effects[i];
                    effects.RemoveAt(i);
                    i--;
                    Destroy(e);
                }
            }
        }

        public void AddEffect(Effect e)
        {
            effects.Add(e);
        }

        public void RemoveAllEffects()
        {
            effects.Clear();
        }

        public void DamageCharacter(float hit)
        {
            if (isDead) return;

            health -= hit;
            if (health <= 0f)
            {
                isDead = true;
                characterDied.Invoke();
                RemoveAllEffects();
                return;
            }
            characterHit.Invoke();
        }
    }
}
