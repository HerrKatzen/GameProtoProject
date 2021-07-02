using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameProtoProject
{

    public abstract class Effect : MonoBehaviour
    {
        public float TTL;
        protected CharacterProperties owner;
        public abstract void ApplyEffect();

        public void AgeEffect(float value)
        {
            TTL -= value;
        }

        public void SetOwner(CharacterProperties cp)
        {
            owner = cp;
        }
    }
}