using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyPen : MonoBehaviour {

    //rotate between 0, and 112 on Z axis
    Animation m_openAni;
    public uint m_rabbitsInside = 0;

    bool m_rotating = false;
	void Start () {
        m_openAni = GetComponent<Animation>();
        if(m_rabbitsInside != 0)
        {
            Close();
        }
        else
        {
            Open();
        }
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open()
    {
        m_openAni.PlayQueued("bunnyPen");
    }

    public void Close()
    {
        m_openAni.PlayQueued("bunnyPenClose");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.GetComponentInParent<Rabboid>())
        {
            Debug.Log("HONK HONK");
            //m_rabbitsInside++;
            Close();
        }
    }

}
