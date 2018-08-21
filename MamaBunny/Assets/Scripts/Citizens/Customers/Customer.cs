using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour {

    public float m_speed;

    private MeshRenderer m_meshRenderer;
    private List<Vector3> m_travelLocations;

    private int m_travelDestinnationIndex;
    private bool m_travelDestinationReached;

    private void Start()
    {
        m_meshRenderer = GetComponent<MeshRenderer>();
        StartCoroutine(Lerp_MeshRenderer_Color(3, m_meshRenderer.material.color, Color.white));
    }

    private void Update()
    {
        // Customer is walking to their desired location
        if (!m_travelDestinationReached)
        {
            moveTowardsDestination();
            checkDestinationReached();
        }
        // Customer is wandering

    }

    void wanderInsideConstraints()
    {

    }

    void moveTowardsDestination()
    {
        float step = m_speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, m_travelLocations[m_travelDestinnationIndex], step);
    }

    void checkDestinationReached()
    {
        // If the citizen is close enough to their target location, begin despawning
        if (!m_travelDestinationReached && Vector3.Distance(m_travelLocations[m_travelDestinnationIndex], transform.position) <= 0.2f)
        {
            ++m_travelDestinnationIndex;
            if(m_travelDestinnationIndex == m_travelLocations.Count)
                m_travelDestinationReached = true;
        }
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

    public void SetTravelLocations(List<Vector3> travelLocations)
    {
        m_travelLocations = travelLocations;
    }
}
