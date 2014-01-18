using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

    public int H_Force      = 80;
    public int Jump_Force   = 300;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        float H_Movement = Input.GetAxis("Horizontal");
        //float V_Movement = Input.GetAxis("Vertical");
        
        rigidbody2D.AddForce(new Vector2(H_Movement*H_Force,0));   
        //rigidbody2D.AddForce(new Vector2(0, Jump_Force));
    }
}
