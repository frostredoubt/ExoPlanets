using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

    public int H_Force      = 80;
    public int Jump_Force   = 1000;
    
    private bool jumping = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    bool ground_intersect = Physics2D.Linecast(transform.position, transform.Find("Ground_collider").position, 1 << LayerMask.NameToLayer("Ground") );
        if( ground_intersect && Input.GetButtonDown("Jump") )
            jumping = true;
	}

    void FixedUpdate()
    {
        float H_Movement = Input.GetAxis("Horizontal");

        if (H_Movement < 0.1)
        {
            if (rigidbody2D.velocity.y == 0)
            {
                rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
            }
        }

        rigidbody2D.AddForce(new Vector2(H_Movement * H_Force, 0));

        if (jumping)
        {
            rigidbody2D.AddForce(new Vector2(0, Jump_Force));
            jumping = false;
        }
    }
}
