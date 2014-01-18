using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

    public int H_Force      = 80;
    public int Jump_Force   = 1000;
    public CharacterController CC;
     
    private Vector3 Frame_movement;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        Frame_movement.Set(0, 0, 0);
        gameObject.transform.Translate(new Vector3(0, -1 * Time.deltaTime, 0));
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Trigger");
        gameObject.transform.Translate(0, 2, 0);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision"); 
        gameObject.transform.Translate(0, 2, 0);
    }
}
