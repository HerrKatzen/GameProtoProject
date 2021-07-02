using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBirdAnimation : MonoBehaviour
{

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;

    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
    }

    public void die() {
        m_animator.SetTrigger("Die");
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
