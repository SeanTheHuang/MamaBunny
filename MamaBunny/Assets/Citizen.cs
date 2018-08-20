using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Citizen : MonoBehaviour {

    private Vector3 m_TravelTarget;

    private MeshRenderer m_meshRenderer;

    private void Start()
    {
        m_meshRenderer = GetComponent<MeshRenderer>();
        StartCoroutine(Lerp_MeshRenderer_Color(3, new Color(0, 0, 0, 1), new Color(1, 1, 1, 1)));
    }

    // Update is called once per frame
    void Update () {
		
	}

    private IEnumerator Lerp_MeshRenderer_Color(float lerpDuration, Color startLerp, Color targetLerp)
    {
        float lerpStart_Time = Time.time;
        float lerpProgress;
        bool lerping = true;
        while (lerping)
        {
            yield return new WaitForEndOfFrame();
            lerpProgress = Time.time - lerpStart_Time;
            if (m_meshRenderer != null)
            {
                m_meshRenderer.material.color = Color.Lerp(startLerp, targetLerp, lerpProgress / lerpDuration);
            }
            else
            {
                lerping = false;
            }


            if (lerpProgress >= lerpDuration)
            {
                lerping = false;
            }
        }
        yield break;
    }
}
