using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {

	// Use this for initialization
	void Start () {


		
	}
	
	// Update is called once per frame
	void Update () {

        transform.position += transform.forward * Time.deltatime * 2.0f;
	}

    void OnTriggerEnter(Collider coll)
    {
        if(coll.transform.compareTag)
    }
}
