using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationButton : GunTarget {

    public RabboidMama m_mama;
    public Animation m_anim;

    float m_cooldown, m_timeForWhenReady;
    bool m_ready;

    private void Awake()
    {
        m_anim = GetComponentInParent<Animation>();
        m_cooldown = m_anim.clip.length * 1.1f;
    }

    public override void TakeHit(float _damage)
    {
        if (!m_ready)
            return;

        // Just press button when hit
        m_timeForWhenReady = Time.time + m_cooldown;
        m_ready = false;
        m_anim.Play();

        if (m_mama)
            m_mama.SpawnRabboidEgg();
    }

    private void Update()
    {
        if (!m_ready)
            if (Time.time >= m_timeForWhenReady)
                m_ready = true;
    }

}
