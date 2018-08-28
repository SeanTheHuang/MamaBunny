using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour {

    public float m_spinVelocity = 360;
    bool m_play = false;

    public void Play(bool _play)
    {
        m_play = _play;
    }

    private void Update()
    {
        if (m_play)
        {
            transform.Rotate(new Vector3(0, 0, m_spinVelocity * Time.deltaTime));
        }
    }
}
