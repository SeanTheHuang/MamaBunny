using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour {

    public float m_travelSpeed;
    public float m_wanderSpeed;

    private MeshRenderer[] m_meshRenderers;
    private List<Vector3> m_travelLocations = new List<Vector3>();

    private int m_travelDestinationIndex;
    private bool m_travelDestinationReached, m_leavingShop, m_despawning, m_wanderinginStore;

    [MinMaxRange(0.0f, 20.0f)]
    public RangedFloat m_wanderRadius;

    [MinMaxRange(0.0f, 20.0f)]
    public RangedFloat m_wanderTimer;

    private NavMeshAgent m_agent;
    private float m_timer;
    private float m_randomWandertime;

    [MinMaxRange(0.0f, 60.0f)]
    public RangedFloat m_timeInShop;

    GameObject m_bunnyPens;
    Vector3 m_targetBunnyPen;

    public bool m_DemandingCustomer;
    BunnyOrderController m_bunnyOrderController;

    private void Start()
    {
        Transform model = transform.GetChild(0);
        model.transform.position = new Vector3(model.transform.position.x, model.transform.position.y - 0.5f, model.transform.position.z);
        m_meshRenderers = model.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer m in m_meshRenderers)
        {
            StartCoroutine(Lerp_MeshRenderer_Color(3, m.material, Color.white));
        }

        m_agent = GetComponent<NavMeshAgent>();

        m_travelSpeed += Random.Range(-m_travelSpeed / 4, m_travelSpeed / 4);
        m_bunnyPens = GameObject.Find("BunnyPens");
    }

    private void Update()
    {
        // Customer is walking to their desired location
        if (!m_travelDestinationReached)
        {
            moveTowardsDestination(m_travelLocations[m_travelDestinationIndex]);
            checkDestinationReached();
        }
        // Customer is insdie the store
        else if(!m_leavingShop)
        {
            if (m_wanderinginStore)
                WanderInsideNavMesh();
            else
                MoveTowardsBunnyPen();
        }
        // Customer leaving shop
        else if(!m_despawning)
        {
            if (m_travelDestinationIndex == (m_travelLocations.Count - 1))
            {
                m_agent.SetDestination(m_travelLocations[m_travelDestinationIndex]);
            }
            else
            {
                m_agent.enabled = false;
                moveTowardsDestination(m_travelLocations[m_travelDestinationIndex]);
            }
            checkDestinationReached();
        }
    }

    void MoveTowardsBunnyPen()
    {
        m_agent.SetDestination(m_targetBunnyPen);
        if(Vector3.Distance(m_targetBunnyPen, transform.position) < 3.0f)
        {
            Invoke("StopLookingAtBunnyPen", Random.Range(3.0f, 8.0f));
        }
    }

    void StopLookingAtBunnyPen()
    {
        m_wanderinginStore = true;
        //Invoke("MoveTowardsPen", Random.Range(5.0f, 10.0f));
    }

    void MoveTowardsPen()
    {
        m_wanderinginStore = false;
        int randomPen = Random.Range(0, m_bunnyPens.transform.childCount);
        m_targetBunnyPen = m_bunnyPens.transform.GetChild(randomPen).position;
        m_targetBunnyPen.z += Random.Range(-1.0f, 1.0f);
        m_targetBunnyPen = getClosestPointOnNavMesh(m_targetBunnyPen);
    }

    void WanderInsideNavMesh()
    {
        m_timer += Time.deltaTime;

        if (m_timer >= m_randomWandertime)
        {
            Debug.Log("new wander Place");
            m_randomWandertime = Random.Range(m_wanderTimer.minValue, m_wanderTimer.maxValue);
            Vector3 newPos = RandomNavSphere(transform.position, Random.Range(m_wanderRadius.minValue, m_wanderRadius.maxValue), -1);
            m_agent.SetDestination(newPos);
            m_timer = 0;
        }
    }

    static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    Vector3 getClosestPointOnNavMesh(Vector3 point)
    {
        NavMeshHit navHit;
        NavMesh.SamplePosition(point, out navHit, 50.0f, NavMesh.AllAreas);
        return navHit.position;
    }

    void moveTowardsDestination(Vector3 destination)
    {
        float step = m_travelSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, destination, step);
        transform.LookAt(destination);
    }

    // Check if the destination the customer is trying to reach has been reached
    void checkDestinationReached()
    {
        // If the citizen is close enough to their target location, begin despawning
        if (Vector3.Distance(m_travelLocations[m_travelDestinationIndex], transform.position) <= 0.5f)
        {
            if (!m_leavingShop)
            {
                ++m_travelDestinationIndex;
                if (m_travelDestinationIndex == m_travelLocations.Count)
                {
                    EnteredShop();
                }
            }
            else
            {
                // Despawn if leaving and reached the final destination
                if (m_travelDestinationIndex == 0)
                {
                    foreach (MeshRenderer m in m_meshRenderers)
                    {
                        StartCoroutine(Lerp_MeshRenderer_Color(3, m.material, new Color(1, 1, 1, 0)));
                    }
                    Invoke("DestroyGameObject", 3);
                    m_despawning = true;
                }
                else
                    --m_travelDestinationIndex;
            }
        }
    }

    // Fade in/out the model
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

    // Called when the customer reaches the counter of the shop
    void EnteredShop()
    {
        Invoke("LeaveTheShop", Random.Range(m_timeInShop.minValue, m_timeInShop.maxValue));
        m_travelDestinationReached = true;
        MoveTowardsPen();
        m_agent.enabled = true;
        if (m_DemandingCustomer)
        {
            MakeACustomerDemand();
        }
    }

    // Called by the spawner to set where the customer should walk to
    public void SetTravelLocations(List<Vector3> travelLocations)
    {
        m_travelLocations.Add(transform.position);
        ++m_travelDestinationIndex;
        for (int i = 0; i < travelLocations.Count; ++i)
        {
            m_travelLocations.Add(travelLocations[i]);
        }
    }

    // Called once the customer leaves the shop
    void LeaveTheShop()
    {
        m_leavingShop = true;
        --m_travelDestinationIndex;
    }

    void DestroyGameObject()
    {
        Destroy(gameObject);
    }

    void MakeACustomerDemand()
    {
        m_bunnyOrderController = GameObject.Find("BunnyOrderController").GetComponent<BunnyOrderController>();
        m_bunnyOrderController.MakeANewOrder();
    }
}
