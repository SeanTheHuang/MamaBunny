using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggSpawnPath : MonoBehaviour {

    public Transform m_eggPrefab;
    public Transform[] m_movePath; // 1st node = start point

    private void OnDrawGizmos()
    {
        // Draw all nodes
        Gizmos.color = Color.blue;
        foreach (Transform t in m_movePath)
            Gizmos.DrawWireSphere(t.position, 0.03f);

        if (m_movePath.Length < 2)
            return;

        // Draw all paths
        Gizmos.color = Color.red;
        for (int i = 1; i < m_movePath.Length; i++)
            Gizmos.DrawLine(m_movePath[i - 1].position, m_movePath[i].position);

    }
}
