using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrigger : MonoBehaviour {

    public bool m_toHappy;
    ShopBGM m_bgm;

    private void Awake()
    {
        m_bgm = GetComponentInParent<ShopBGM>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (m_toHappy)
            m_bgm.ToHappyMusic();
        else
            m_bgm.ToSpoopyMusic();
    }
}
