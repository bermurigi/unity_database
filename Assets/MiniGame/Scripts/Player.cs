using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private Rigidbody2D rigidbody;

    private Animator animator;

    private SpriteRenderer renderer;

    private float speed = 9;

    private float horizontal;

	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        horizontal = Input.GetAxis("Horizontal");

        if (GameManager.Instance.stopTrigger)
        {
            animator.SetTrigger("start");
            PlayerMove();
        }
        

        if (!GameManager.Instance.stopTrigger)
        {
            animator.SetTrigger("dead");
        }

        ScreenChk();
    }

    private void PlayerMove()
    {
        animator.SetFloat("speed",Mathf.Abs(horizontal));
        if(horizontal < 0)
        {
            renderer.flipX = true;
        }
        else if(horizontal >0)
        {
            renderer.flipX = false;
        }
        rigidbody.velocity = new Vector2(horizontal * speed, rigidbody.velocity.y);        
    }

    private void ScreenChk()
    {
        Vector3 worlpos = Camera.main.WorldToViewportPoint(this.transform.position);
        if (worlpos.x < 0.05f) worlpos.x = 0.05f;
        if (worlpos.x > 0.95f) worlpos.x = 0.95f;
        this.transform.position = Camera.main.ViewportToWorldPoint(worlpos);
    }
}
