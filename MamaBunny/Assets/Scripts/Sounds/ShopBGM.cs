using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopBGM : MonoBehaviour {

    public AudioSource m_happyMusic;
    public AudioSource m_spoopyMusic;
    public float m_transitionTime = 2;
    bool m_transitioning, m_toHappy;

    float m_happyVolume, m_spoopyVolume;

    private void Awake()
    {
        m_transitioning = true;
        m_toHappy = false;
        m_happyVolume = m_happyMusic.volume;
        m_spoopyVolume = m_spoopyMusic.volume;
    }

    private void Update()
    {
        if (!m_transitioning)
            return;

        if (m_toHappy)
        {
            m_happyMusic.volume = Mathf.Clamp01(m_happyMusic.volume + (Time.deltaTime / m_transitionTime));
            m_spoopyMusic.volume = Mathf.Clamp01(m_spoopyMusic.volume - (Time.deltaTime / m_transitionTime));

            if (m_happyMusic.volume >= m_happyVolume)
            {
                m_transitioning = false;
                m_happyMusic.volume = m_happyVolume;
                m_spoopyMusic.volume = 0;
            }
        }
        else
        {
            m_happyMusic.volume = Mathf.Clamp01(m_happyMusic.volume - (Time.deltaTime / m_transitionTime));
            m_spoopyMusic.volume = Mathf.Clamp01(m_spoopyMusic.volume + (Time.deltaTime / m_transitionTime));

            if (m_spoopyMusic.volume >= m_spoopyVolume)
            {
                m_transitioning = false;
                m_happyMusic.volume = 0;
                m_spoopyMusic.volume = m_spoopyVolume;
            }
        }
    }

    public void ToHappyMusic()
    {
        m_transitioning = m_toHappy = true;    
    }

    public void ToSpoopyMusic()
    {
        m_transitioning = true;
        m_toHappy = false;
    }
}
