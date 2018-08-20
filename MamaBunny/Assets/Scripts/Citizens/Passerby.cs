using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passerby : MonoBehaviour
{

    public float speed;

    private Vector3 m_TravelLocation;
    private bool m_destinationReached;

    private MeshRenderer m_meshRenderer;

    private void Start()
    {
        m_meshRenderer = GetComponent<MeshRenderer>();
        StartCoroutine(Lerp_MeshRenderer_Color(3, m_meshRenderer.material.color, Color.white));
    }

    void Update()
    {

        moveTowardsDestination();
        checkDestinationReached();
    }

    void moveTowardsDestination()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, m_TravelLocation, step);
    }

    void checkDestinationReached()
    {
        // If the citizen is close enough to their target location, begin despawning
        if (!m_destinationReached && Vector3.Distance(m_TravelLocation, transform.position) <= 0.2f)
        {
            m_destinationReached = true;
            StartCoroutine(Lerp_MeshRenderer_Color(3, m_meshRenderer.material.color, new Color(1, 1, 1, 0)));
            Invoke("DestroyGameObject", 3);
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

    public void SetTargetLocation(Vector3 TravelLocation)
    {
        m_TravelLocation = TravelLocation;
    }

    void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
