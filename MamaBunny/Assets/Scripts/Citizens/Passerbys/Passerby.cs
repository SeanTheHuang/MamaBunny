using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passerby : MonoBehaviour
{

    public float speed;

    private Vector3 m_TravelLocation;
    private bool m_destinationReached;

    private MeshRenderer[] m_meshRenderers;

    private void Start()
    {
        Transform model = transform.GetChild(0);
        model.transform.position = new Vector3(model.transform.position.x, model.transform.position.y - 0.5f, model.transform.position.z);
        m_meshRenderers = model.GetComponentsInChildren<MeshRenderer>();
        
        foreach(MeshRenderer m in m_meshRenderers)
        {
            StartCoroutine(Lerp_MeshRenderer_Color(3, m.material, Color.white));
        }
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
            foreach (MeshRenderer m in m_meshRenderers)
            {
                StartCoroutine(Lerp_MeshRenderer_Color(3, m.material, new Color(1, 1, 1, 0)));
            }
            Invoke("DestroyGameObject", 3);
        }
    }

    private IEnumerator Lerp_MeshRenderer_Color(float lerpDuration, Material material, Color targetLerp)
    {
        float lerpStart_Time = Time.time;
        float lerpProgress;
        bool lerping = true;
        while (lerping)
        {
            yield return new WaitForEndOfFrame();
            lerpProgress = Time.time - lerpStart_Time;
            if (material != null)
            {
                material.color = Color.Lerp(material.color, targetLerp, lerpProgress / lerpDuration);
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
