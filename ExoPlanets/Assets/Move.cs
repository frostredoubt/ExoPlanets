using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

    public int H_Force = 10;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        float H_Movement = Input.GetAxis("Horizontal");
        rigidbody2D.AddForce(new Vector2(H_Movement*H_Force,0));
    }
}
