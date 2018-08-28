using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShredderHitBox : MonoBehaviour {

    public Transform m_shredTargetPos;

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponentInParent<Rabboid>().GettingShredded(m_shredTargetPos.position);
    }
}
