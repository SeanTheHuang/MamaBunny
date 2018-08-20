using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour {

    private MeshRenderer m_meshRenderer;

    private void Start()
    {
        m_meshRenderer = GetComponent<MeshRenderer>();
        StartCoroutine(Lerp_MeshRenderer_Color(3, m_meshRenderer.material.color, Color.white));
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
