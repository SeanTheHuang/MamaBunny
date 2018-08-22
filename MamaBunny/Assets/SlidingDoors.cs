using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoors : MonoBehaviour {

    public float m_SensorRadius;

    private void Update()
    {
        CheckIfActivatingButton();
    }

    // Checks if the rock is close enough to activate a rock button
    void CheckIfActivatingButton()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, m_SensorRadius, 1 << 12);

        // no one is near the door
        if (hitColliders.Length == 0)
        {
            return;
        }

        // someone is near the door
        else
        {

        }
    }
}
