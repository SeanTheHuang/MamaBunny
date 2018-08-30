using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {

    public float m_movespeed = 2.0f;
	// Use this for initialization
	void Start () {


		
	}
	
	// Update is called once per frame
	void Update () {

        transform.position += transform.right * Time.deltaTime * m_movespeed;
	}

    void OnTriggerEnter(Collider coll)
    {
        if(coll.transform.CompareTag("CarTurn"))
        {
            transform.eulerAngles += (Vector3.up * 180);
        }
    }
}
