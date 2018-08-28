using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShredderHitBox : MonoBehaviour {

    public Transform m_shredTargetPos;
    PlayerCameraController m_cameraController;

    private void Awake()
    {
        m_cameraController = Camera.main.GetComponentInParent<PlayerCameraController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponentInParent<Rabboid>().GettingShredded(m_shredTargetPos.position);
        m_cameraController.ApplyScreenShake(2.7f, 0.02f);
    }
}
