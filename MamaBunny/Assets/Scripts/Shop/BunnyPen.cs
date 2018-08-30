using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyPen : MonoBehaviour {

    //rotate between 0, and 112 on Z axis
    Animation m_openAni;

    public BunnyPenData m_penData;
    public GameObject m_baseRabboidPrefab;

    void Start() {
        Debug.Log(gameObject.name);
        m_openAni = GetComponent<Animation>();
        if (m_penData.m_bunnyInside)
        {
            Close();
            SetRabboid();
        }
        else
        {
            Open();
            m_penData.m_bunnyInside = false;
        }
    }

    void SetRabboid()
    {
        GameObject rabboid = Instantiate(m_baseRabboidPrefab, transform.position, transform.rotation);
        RabboidResult RabboidStats = new RabboidResult();
        RabboidStats.m_resultColour = m_penData.m_bunnyColour;
        RabboidStats.m_size = m_penData.m_bunnySize;
        RabboidStats.m_mouthPart = m_penData.m_bunnyMouthPart;
        RabboidStats.m_backPart = m_penData.m_bunnyBackPart;
        rabboid.GetComponent<Rabboid>().Initialize(RabboidStats);
        m_penData.m_bunnyInside = true;
    }

    void Open()
    {
        m_openAni.PlayQueued("BunnyPenOpen");
    }

    void Close()
    {
        m_openAni.PlayQueued("BunnyPenClose");
    }

    public bool PlayerInteract(bool rabboidHeld, RabboidResult rabboidStats)
    {
        Debug.Log("here");
        // Put bunny in cage
        if (!m_penData.m_bunnyInside && rabboidHeld)
        {
            Debug.Log("close");
            m_penData.m_bunnyInside = true;
            m_penData.m_bunnyColour = rabboidStats.m_resultColour;
            m_penData.m_bunnySize = rabboidStats.m_size;
            m_penData.m_bunnyMouthPart = rabboidStats.m_mouthPart;
            m_penData.m_bunnyBackPart = rabboidStats.m_backPart;
            m_penData.ForceSerialization();
            SetRabboid();
            Close();
            return false;
        }
        // Take bunny out of cage
        else
        {
            Debug.Log("open");
            m_penData.m_bunnyInside = false;
            m_penData.m_bunnyColour = null;
            m_penData.m_bunnySize = 0;
            m_penData.m_bunnyMouthPart = null;
            m_penData.m_bunnyBackPart = null;
            m_penData.ForceSerialization();
            Open();
            return true;
        }
    }
}
