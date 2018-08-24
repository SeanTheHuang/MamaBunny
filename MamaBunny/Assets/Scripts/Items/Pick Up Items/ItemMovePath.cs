using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMovePath : MonoBehaviour {

    public Transform[] m_movePath;

    // SHOULD ONLY COLLIDE WITH ITEMS.
    // ONLY PICKUPS SHOULD BE ON THE PICKUP LAYER
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGERED OwO");
        other.GetComponent<PickUp>().OnBeingEaten(m_movePath);
    }

    private void OnDrawGizmos()
    {
        // Draw all nodes
        Gizmos.color = Color.red;
        foreach (Transform t in m_movePath)
            Gizmos.DrawWireSphere(t.position, 0.03f);

        if (m_movePath.Length < 2)
            return;

        // Draw all paths
        Gizmos.color = Color.green;
        for (int i = 1; i < m_movePath.Length; i++)
            Gizmos.DrawLine(m_movePath[i - 1].position, m_movePath[i].position);

    }
}
