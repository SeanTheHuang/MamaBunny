using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour {

    public float m_spinVelocity = 360;
    float m_playDuration;

    public void Play(float _duration)
    {
        m_playDuration = _duration;
    }

    private void Update()
    {
        if (m_playDuration > 0)
        {
            transform.Rotate(new Vector3(0, 0, m_spinVelocity * Time.deltaTime));
            m_playDuration -= Time.deltaTime;
        }
    }
}
