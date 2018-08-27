using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyOrderSlot : MonoBehaviour {

    public float m_orderTime; // How long the player has before the order is cancelled

    bool m_isActive;
    float m_startOfOrderTime;
    BunnyOrderController m_bunnyOrderController;
    GameObject m_customer;

    public struct RabboidOrder
    {
        public float m_size;
        public RabboidColour m_colour;
        public RabboidBodyPart m_mouthPart;
        public RabboidBodyPart m_backPart;
    }

    RabboidOrder m_rabboidOrder = new RabboidOrder();

    // Use this for initialization
    void Start () {
        m_bunnyOrderController = transform.GetComponentInParent<BunnyOrderController>();
	}
	
	// Update is called once per frame
	void Update () {
        //if (m_isActive)
        //    UpdateOrder();
    }

    //private void UpdateOrder()
    //{
    //    // Check the player has not run out of time on the order
    //    if(m_startOfOrderTime < Time.time)
    //    {
    //        EndOrder();
    //    }
    //}

    void EndOrder()
    {
        m_isActive = false;
    }

    public bool GetIsActive()
    {
        return m_isActive;
    }

    public void GenerateOrder(RabboidColour[] possibleColours, RabboidBodyPart[] possibleBodyMods, RabboidBodyPart[] possibleHeadMods)
    {
        // Set Timer
        m_isActive = true;
        m_startOfOrderTime = Time.time;

        // Generate a random set of ingredients for the rabbit
        int numOfIngredients = Random.Range(1, 4);
        bool[] ingredientChosen = new bool[] { false, false, false, false };
        for (int i = 0; i < numOfIngredients; ++i)
        {
            int ingredientType = Random.Range(0, 4);
            if (!ingredientChosen[ingredientType])
            {
                ingredientChosen[ingredientType] = true;
                // Chose a size 50/50 big small
                if (ingredientType == 0)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        m_rabboidOrder.m_size = 1.3f;
                    }
                    else
                    {
                        m_rabboidOrder.m_size = 0.7f;
                    }
                }
                // Chose a colour
                else if (ingredientType == 1)
                {
                    int randomColor = Random.Range(0, possibleColours.Length);
                    m_rabboidOrder.m_colour = possibleColours[randomColor];
                }
                // Chose a mouthpart
                else if (ingredientType == 2)
                {
                    int randomMod = Random.Range(0, possibleHeadMods.Length);
                    m_rabboidOrder.m_mouthPart = possibleHeadMods[randomMod];
                }
                // Chose a backpart
                else if (ingredientType == 3)
                {
                    int randomMod = Random.Range(0, possibleBodyMods.Length);
                    m_rabboidOrder.m_backPart = possibleBodyMods[randomMod];
                }
            }
            else
            {
                --i;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_isActive)
        {
            // Check the order

            // Add coins based on performance

            // Tidy up
            Destroy(other.gameObject);
            m_isActive = false;
            Debug.Log("RabbitSold!");
        }
    }
}
