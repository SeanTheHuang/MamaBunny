using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShredderButton : GunTarget {

    public Collider m_shredderHitBox;
    float m_timeTillCanHit;
    Spin[] m_spinners;
    bool m_shredding;
    Animation m_anim;
    private void Start()
    {
        m_spinners = transform.parent.GetComponentsInChildren<Spin>();
        m_anim = GetComponent<Animation>();
        m_shredderHitBox.enabled = m_shredding = false;
    }

    public override void TakeHit(float _damage)
    {
        if (Time.time < m_timeTillCanHit)
            return;

        m_timeTillCanHit = Time.time + m_anim.clip.length;
        m_anim.Play();
        m_shredding = !m_shredding;
        m_shredderHitBox.enabled = m_shredding;
        // TODO: Turn shredder collider + sound on

        foreach (Spin s in m_spinners)
            s.Play(m_shredding);
    }
}
