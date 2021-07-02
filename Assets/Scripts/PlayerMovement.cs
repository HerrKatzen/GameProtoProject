using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameProtoProject
{

    [RequireComponent(typeof(CharacterProperties))]
    public class PlayerMovement : MonoBehaviour
    {
        public CharacterController playerController;

        public GameObject rotatingObject;

        float horizontalMove = 0f;
        bool jump = false;
        private CharacterProperties properties;
        private bool moving = false;

        private void Start()
        {
            properties = GetComponent<CharacterProperties>();
        }

        void Update()
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * properties.speed;

            if (horizontalMove != 0f)
            {
                if (moving == false)
                {
                    moving = true;
                    GfxEventHandler.Instance.walkingStateChanged.Invoke(moving);
                }
            }
            else
            {
                if (moving == true)
                {
                    moving = false;
                    GfxEventHandler.Instance.walkingStateChanged.Invoke(moving);
                }
            }

            if (Input.GetButtonDown("Jump"))
            {
                jump = true;
            }
        }

        private void FixedUpdate()
        {
            playerController.Move(horizontalMove * Time.fixedDeltaTime, jump);
            jump = false;
        }
    }

}