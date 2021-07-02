using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private bool                m_walking = false;
    private bool                m_grounded = true;
    private bool                m_rolling = false;
    private int                 m_facingDirection = 1;
    private int                 m_currentAttack = 0;
    private float               m_timeSinceAttack = 0.0f;
    private float               m_delayToIdle = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
    }

    public void walkingState(bool run) {
        if(run)
            m_animator.SetInteger("AnimState", 1);
        else
            m_animator.SetInteger("AnimState", 0);
    }

    public void roll() {
        Debug.Log("roll");
        m_animator.SetTrigger("Roll");
    }

    public void attack() {
        m_animator.SetInteger("AttackType", Random.Range(0,3));
        m_animator.SetTrigger("Attack");
    }
    

    public void jump() {
        m_animator.SetTrigger("Jump");
        m_animator.ResetTrigger("Jump");
    }

    public void die() {
        m_animator.SetBool("isDead", true);
    }

    public void groundedStateChange(bool s) {
        m_animator.SetBool("isGrounded", s);
        if(s)
            m_animator.SetTrigger("Ground");
        else
            m_animator.SetTrigger("Fall");
    }

    // Update is called once per frame
    void Update()
    {
            // time -= Time.deltaTime;
            // if(m_grounded)
            //     if(m_walking) {
            //             m_animator.SetInteger("AnimState", 1);
            //         }
            //     else {
            //             m_animator.SetInteger("AnimState", 0);
            //         }
            // else {
            //     if(time <= 0f)
            //     {
            //         m_animator.SetInteger("AnimState", 3);
            //     }
            // }
        
    }
}
