using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenTrigger : MonoBehaviour {

    Bird m_bird;

    private void Awake()
    {
        m_bird = GetComponentInParent<Bird>();   
    }

    private void OnTriggerEnter(Collider other)
    {
        m_bird.NewRunAwayTarget(other.transform);
    }

}
