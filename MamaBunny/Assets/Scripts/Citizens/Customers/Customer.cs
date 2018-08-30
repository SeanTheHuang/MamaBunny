using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour {

    Animator m_anima;
    CustomerFace m_face;
    public float m_travelSpeed;
    public float m_wanderSpeed;

    private MeshRenderer[] m_meshRenderers;
    private List<Vector3> m_travelLocations = new List<Vector3>();

    public int m_travelDestinationIndex;
    private bool m_travelDestinationReached, m_leavingShop, m_despawning, m_wanderinginStore, m_waitingForOrder, m_respawnedCustomer;

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

    private GameObject m_orderWaitLocations;
    private Vector3 m_orderLocation;

    public ModelType m_modelType;

    public CitzenSpawner m_citzenSpawner;

    private void Start()
    {
        m_face = GetComponentInChildren<CustomerFace>();
        m_anima = GetComponentInChildren<Animator>();
        Transform model = transform.GetChild(0);
        model.transform.position = new Vector3(model.transform.position.x, model.transform.position.y - 0.5f, model.transform.position.z);
        m_agent = GetComponent<NavMeshAgent>();

        if (!m_respawnedCustomer)
        {
            m_anima.SetBool("isMoving", true);
            m_meshRenderers = model.GetComponentsInChildren<MeshRenderer>();
            m_travelSpeed += Random.Range(-m_travelSpeed / 4, m_travelSpeed / 4);

            m_orderWaitLocations = GameObject.Find("OrderWaitLocations");
            m_bunnyPens = GameObject.Find("BunnyPens");
        }
        else
        {
            m_agent.enabled = true;
        }
    }

    private void Update()
    {
        // The customer is a repawned customer and therefore cannot do this logic. (Bad system I know)
        if (!m_respawnedCustomer)
        {
            // Customer is walking to their desired location
            if (!m_travelDestinationReached)
            {
                moveTowardsDestination(m_travelLocations[m_travelDestinationIndex]);
                checkDestinationReached();
            }
            // Customer is inside the store
            else if (!m_leavingShop)
            {
                if (m_wanderinginStore)
                    WanderInsideNavMesh();
                else if (m_waitingForOrder)
                    MoveTowardsOrderLocation();
                else
                    MoveTowardsBunnyPen();
            }
            // Customer leaving shop
            else if (!m_despawning)
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
        else if (!m_waitingForOrder)
        {
            moveTowardsDestination(m_travelLocations[m_travelDestinationIndex]);
            checkDestinationReached();
        }

        if (m_agent == null
            || m_anima == null)
            return;
        if (!m_agent.enabled)
            return;

        if (m_agent.velocity == Vector3.zero)
        {//not moving
            if (m_anima.GetBool("isMoving") != false)
            {
                m_anima.SetBool("isMoving", false);
            }
        }
        else
        {//moving
            if (m_anima.GetBool("isMoving") != true)
            {
                m_anima.SetBool("isMoving", true);
            }
        }
    }

    void MoveTowardsOrderLocation()
    {
        m_agent.SetDestination(m_orderLocation);
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
                    //foreach (MeshRenderer m in m_meshRenderers)
                    //{
                    //    StartCoroutine(Lerp_MeshRenderer_Color(3, m.material, new Color(1, 1, 1, 0)));
                    //}
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
    public void EnteredShop()
    {
        Debug.Log(m_travelDestinationIndex);
        m_travelDestinationReached = true;
        MoveTowardsPen();
        m_agent.enabled = true;
        if (!m_DemandingCustomer)
        {
            Invoke("LeaveTheShop", Random.Range(m_timeInShop.minValue, m_timeInShop.maxValue));
        }
        else
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
        if (!m_leavingShop)
        {
            m_leavingShop = true;
            if(m_DemandingCustomer)
            {
                m_waitingForOrder = false;
                transform.parent = null;
            }
            if (m_respawnedCustomer)
            {
                m_agent.enabled = false;
                m_travelDestinationIndex = 0;
                m_travelLocations.Clear();
                m_travelLocations.Add(new Vector3(5.3f, 0.5f, 26.0f));
                m_anima.SetBool("isMoving", true);
            }
            else
            {
                --m_travelDestinationIndex;
            }
        }
    }

    void DestroyGameObject()
    {
        Destroy(gameObject);
    }

    // The customer makes a demand for a new order
    void MakeACustomerDemand()
    {
        m_bunnyOrderController = GameObject.Find("BunnyOrderController").GetComponent<BunnyOrderController>();
        m_bunnyOrderController.MakeANewOrder(gameObject);
    }

    // The customers order is complete
    public void OrderComplete(int _score)
    {//4 bad     //20 + good
        SoundEffectsPlayer.Instance.PlaySound("kaching");
        if(_score < 8)
        {
            if (Random.Range(0, 2) == 0)
            {
                m_face.SetFaceMaterial(CustomerFace.FACEEMOTION.SAD);
                m_anima.SetTrigger("beSad");
            }
            else
            {
                m_face.SetFaceMaterial(CustomerFace.FACEEMOTION.ANGRY);
                m_anima.SetTrigger("beAngry");
            }
            //play angry one here
            EventsController.Instance.SummonAngryMoney(transform.position);
        }
        else 
        {
            m_face.SetFaceMaterial(CustomerFace.FACEEMOTION.HAPPY);
            m_anima.SetTrigger("beHappy");
            
            EventsController.Instance.SummonMoney(transform.position);
        }

        //stop them from moving for a while
        Invoke("LeaveTheShop", 3);
    }

    public void SetOrderDestination(int orderIndex)
    {
        m_waitingForOrder = true;
        m_orderLocation = m_orderWaitLocations.transform.GetChild(orderIndex).position;
    }

    public void SetWaitingForOrder()
    {
        m_DemandingCustomer = true;
        m_respawnedCustomer = true;
        m_waitingForOrder = true;
    }

    public void RunInPanic()
    {
        m_travelSpeed = 4;
        if (m_agent != null && m_agent.enabled)
            m_agent.speed = 4;
        LeaveTheShop();
    }
}
