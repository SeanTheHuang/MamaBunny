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
        public Color m_colour;
        public RabboidBodyPart m_mouthPart;
        public RabboidBodyPart m_backPart;
    }

    RabboidOrder m_rabboidOrder = new RabboidOrder();

    // Use this for initialization
    void Start () {
        m_bunnyOrderController = transform.GetComponentInParent<BunnyOrderController>();
        m_rabboidOrder.m_colour = Color.white;

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

    public void GenerateOrder(GameObject customer, RabboidColour[] possibleColours, RabboidBodyPart[] possibleBodyMods, RabboidBodyPart[] possibleHeadMods)
    {
        // Set Timer
        m_isActive = true;
        m_startOfOrderTime = Time.time;
        m_customer = customer;

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
                        m_rabboidOrder.m_size = RabboidCalculator.LARGE_SIZE;
                    }
                    else
                    {
                        m_rabboidOrder.m_size = RabboidCalculator.SMALL_SIZE;
                    }
                }
                // Chose a colour
                else if (ingredientType == 1)
                {
                    int randomColor = Random.Range(0, possibleColours.Length);
                    m_rabboidOrder.m_colour = possibleColours[randomColor].m_color;
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
            RabboidResult RabboidStats = other.transform.parent.GetComponent<Rabboid>().RabboidStats;
            // Check the order
            int rabbitScore = 20; // Score is given based on how close the rabbit is to what the order wanted.
            int negativePointModifier = 5;
            int positivePointModifier = 10;

            // Check Size
            if (m_rabboidOrder.m_size == 0)
            {
                if (RabboidStats.m_size <= RabboidCalculator.SMALL_SIZE || RabboidStats.m_size >= RabboidCalculator.LARGE_SIZE)
                {
                    rabbitScore -= negativePointModifier;
                }
                else
                {
                    rabbitScore += positivePointModifier;
                }
            }
            if(m_rabboidOrder.m_size == RabboidCalculator.SMALL_SIZE)
            {
                if(RabboidStats.m_size <= RabboidCalculator.SMALL_SIZE)
                {
                    rabbitScore += positivePointModifier;
                }
                else if (RabboidStats.m_size >= RabboidCalculator.LARGE_SIZE)
                {
                    rabbitScore -= negativePointModifier * 2;
                }
                else
                {
                    rabbitScore -= negativePointModifier;
                }
            }
            if (m_rabboidOrder.m_size == RabboidCalculator.LARGE_SIZE)
            {
                if (RabboidStats.m_size >= RabboidCalculator.LARGE_SIZE)
                {
                    rabbitScore += positivePointModifier;
                }
                else if (RabboidStats.m_size <= RabboidCalculator.SMALL_SIZE)
                {
                    rabbitScore -= negativePointModifier * 2;
                }
                else
                {
                    rabbitScore -= negativePointModifier;
                }
            }

            // Check Color
            Color resultsColor = Color.white;
            if (RabboidStats.m_resultColour != null)
                resultsColor = RabboidStats.m_resultColour.m_color;
            if(resultsColor == m_rabboidOrder.m_colour)
            {
                rabbitScore += positivePointModifier;
            }
            else
            {
                rabbitScore -= negativePointModifier;
            }

            // Check Body
            if((RabboidStats.m_backPart == null && m_rabboidOrder.m_backPart == null)
            || (RabboidStats.m_backPart != null && m_rabboidOrder.m_backPart != null))
            {
                rabbitScore += positivePointModifier;
            }
            else
            {
                rabbitScore -= negativePointModifier;
            }

            // Check Head
            if((RabboidStats.m_mouthPart == null && m_rabboidOrder.m_mouthPart == null)
            || (RabboidStats.m_mouthPart != null && m_rabboidOrder.m_mouthPart != null))
            {
                rabbitScore += positivePointModifier;
            }
            else
            {
                rabbitScore -= negativePointModifier;
            }

            // Add coins based on performance
            Debug.Log("New Coin Total: " + rabbitScore);

            // Tidy up
            Destroy(other.gameObject);
            m_isActive = false;
        }
    }
}
