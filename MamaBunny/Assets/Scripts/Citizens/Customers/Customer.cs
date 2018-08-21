using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour {

    public float m_travelSpeed;
    public float m_wanderSpeed;

    private MeshRenderer m_meshRenderer;
    private List<Vector3> m_travelLocations = new List<Vector3>();

    private int m_travelDestinationIndex;
    private bool m_travelDestinationReached, m_leavingShop, m_despawning;

    public float m_wanderRadius;
    public float m_wanderTimer;

    private NavMeshAgent m_agent;
    private float m_timer;

    private void Start()
    {
        m_meshRenderer = GetComponent<MeshRenderer>();
        StartCoroutine(Lerp_MeshRenderer_Color(3, m_meshRenderer.material.color, Color.white));
        m_timer = m_wanderTimer;
        m_travelSpeed += Random.Range(-m_travelSpeed / 4, m_travelSpeed / 4);
    }

    private void Update()
    {
        // Customer is walking to their desired location
        if (!m_travelDestinationReached)
        {
            moveTowardsDestination(m_travelLocations[m_travelDestinationIndex]);
            checkDestinationReached();
        }
        // Customer is wandering
        else if(!m_leavingShop)
        {
            wanderInsideNavMesh();
        }
        // Customer leaving shop
        else if(!m_despawning)
        {
            moveTowardsDestination(m_travelLocations[m_travelDestinationIndex]);
            checkDestinationReached();
        }
    }

    void wanderInsideNavMesh()
    {
        if (m_agent == null)
        {
            m_agent = gameObject.AddComponent<NavMeshAgent>();
            m_agent.speed = m_wanderSpeed;
        }

        m_timer += Time.deltaTime;

        if (m_timer >= m_wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, m_wanderRadius, -1);
            m_agent.SetDestination(newPos);
            m_timer = 0;
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    void moveTowardsDestination(Vector3 destination)
    {
        float step = m_travelSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, destination, step);
    }

    void checkDestinationReached()
    {
        // If the citizen is close enough to their target location, begin despawning
        if (Vector3.Distance(m_travelLocations[m_travelDestinationIndex], transform.position) <= 0.2f)
        {
            if (!m_leavingShop)
            {
                ++m_travelDestinationIndex;
                if (m_travelDestinationIndex == m_travelLocations.Count)
                    m_travelDestinationReached = true;
            }
            else
            {
                // Despawn if leaving and reached the final destination
                if (m_travelDestinationIndex == 0)
                {
                    StartCoroutine(Lerp_MeshRenderer_Color(3, m_meshRenderer.material.color, new Color(1, 1, 1, 0)));
                    Invoke("DestroyGameObject", 3);
                    m_despawning = true;
                }
                else
                    --m_travelDestinationIndex;
            }
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

    void DestroyGameObject()
    {
        Destroy(gameObject);
    }

    public void SetTravelLocations(List<Vector3> travelLocations)
    {
        m_travelLocations.Add(transform.position);
        ++m_travelDestinationIndex;
        for (int i = 0; i < travelLocations.Count; ++i)
        {
            m_travelLocations.Add(travelLocations[i]);
        }
    }

    public void SetLeavingShop(bool leavingShop)
    {
        m_leavingShop = leavingShop;
        --m_travelDestinationIndex;
        m_agent.enabled = false;
    }
}
