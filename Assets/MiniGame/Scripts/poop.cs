using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class poop : MonoBehaviour {

    [SerializeField]
    private Animator animator;

    private Rigidbody2D rigidbody;
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            GameManager.Instance.Score();
            animator.SetTrigger("poop");
        }

        if (collision.gameObject.tag == "Player")
        {
            GameManager.Instance.GameOver();
            animator.SetTrigger("poop");
        }
    }
}
