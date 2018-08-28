using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTable : MonoBehaviour {

    public Transform GunChild;
    bool m_gotWeapon = false;

    void Awake()
    {
        GunChild.gameObject.SetActive(m_gotWeapon);
    }

    void Update () {
		
	}

    public void PlaceGun()
    {
        GunChild.gameObject.SetActive(true);
        m_gotWeapon = true;
    }

    public bool TakeGun()
    {
        if (m_gotWeapon)
        {
            GunChild.gameObject.SetActive(false);
            m_gotWeapon = false;
            return true;
        }
        return false;
    }
}
