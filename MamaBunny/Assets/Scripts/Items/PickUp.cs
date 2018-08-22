using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickUp : MonoBehaviour {

    public float m_liveTime = 20;
    float m_startLiveTime;

    private void Awake()
    {
        m_startLiveTime = Time.time;
    }

    private void Update()
    {
        if (Time.time - m_startLiveTime >= m_liveTime)
            Destroy(gameObject);
    }

    public abstract void OnEatenByMamaRabbit(RabboidMama _mama);
}
