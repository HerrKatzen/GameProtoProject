using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameProtoProject
{
    [System.Serializable]
    public class WalkingStateChanged : UnityEvent<bool>
    {
    }

    [System.Serializable]
    public class GroundedStateChanged : UnityEvent<bool>
    {
    }

    public class GfxEventHandler : Singleton<GfxEventHandler>
    {

        [Header("Events")]

        public WalkingStateChanged walkingStateChanged;
        public UnityEvent playerTookDMG;
        public UnityEvent playerDied;
        public GroundedStateChanged groundedStateChanged;
        public UnityEvent OnWallJump;
        public UnityEvent OnJump;
        public UnityEvent playerAttack;

        private void Start()
        {
            if (playerDied == null) playerDied = new UnityEvent();
            if (playerTookDMG == null) playerTookDMG = new UnityEvent();
            if (walkingStateChanged == null) walkingStateChanged = new WalkingStateChanged();
            if (groundedStateChanged == null) groundedStateChanged = new GroundedStateChanged();
            if (OnWallJump == null) OnWallJump = new UnityEvent();
            if (OnJump == null) OnJump = new UnityEvent();
            if (playerAttack == null) playerAttack = new UnityEvent();
        }

        public void PlayerHit()
        {
            playerTookDMG.Invoke();
        }

        public void PlayerDied()
        {
            playerDied.Invoke();
        }
    }
}