using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShredderButton : GunTarget {

    public float m_playDuration = 4;
    Spin[] m_spinners;
    bool m_shredding = false;
    float m_timeTillStop;
    Animation m_anim;

    private void Start()
    {
        m_spinners = transform.parent.GetComponentsInChildren<Spin>();
        m_anim = GetComponent<Animation>();
    }

    private void Update()
    {
        if (m_shredding)
            if (Time.time >= m_timeTillStop)
                StopShredding();
    }

    void StopShredding()
    {
        m_shredding = false;
        // TODO, Turn off shred collider + sound
    }

    public override void TakeHit(float _damage)
    {
        m_timeTillStop = Time.time + m_playDuration;
        m_anim.Stop();
        m_anim.Play();
        // TODO: Turn shredder collider + sound on

        foreach (Spin s in m_spinners)
            s.Play(m_playDuration);
    }
}
