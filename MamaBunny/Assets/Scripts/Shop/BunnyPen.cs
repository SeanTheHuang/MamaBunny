using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyPen : MonoBehaviour {

    //rotate between 0, and 112 on Z axis
    Animation m_openAni;

    public BunnyPenData m_penData;
    public GameObject m_baseRabboidPrefab;

	void Start () {
        Debug.Log(gameObject.name);
        m_openAni = GetComponent<Animation>();
        if(m_penData.m_bunnyInside)
        {
            Close();
            GameObject rabboid = Instantiate(m_baseRabboidPrefab, transform.position, transform.rotation);
            RabboidResult RabboidStats = new RabboidResult();
            RabboidStats.m_resultColour = m_penData.m_bunnyColour;
            RabboidStats.m_size = m_penData.m_bunnySize;
            RabboidStats.m_mouthPart = m_penData.m_bunnyMouthPart;
            RabboidStats.m_backPart = m_penData.m_bunnyBackPart;
            rabboid.GetComponent<Rabboid>().Initialize(RabboidStats);
        }
        else
        {
            Open();
        }
	}

    public void Open()
    {
        m_openAni.PlayQueued("BunnyPenOpen");
    }

    public void Close()
    {
        m_openAni.PlayQueued("BunnyPenClose");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!m_penData.m_bunnyInside && other.transform.GetComponentInParent<Rabboid>())
        {
            Debug.Log("Called");
            RabboidResult RabboidStats = other.transform.parent.GetComponent<Rabboid>().RabboidStats;
            m_penData.m_bunnyInside = true;
            m_penData.m_bunnyColour = RabboidStats.m_resultColour;
            m_penData.m_bunnySize = RabboidStats.m_size;
            m_penData.m_bunnyMouthPart = RabboidStats.m_mouthPart;
            m_penData.m_bunnyBackPart = RabboidStats.m_backPart;
            Close();
        }
    }

}
