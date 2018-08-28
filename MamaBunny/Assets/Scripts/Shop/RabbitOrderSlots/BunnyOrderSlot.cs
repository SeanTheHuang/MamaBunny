using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BunnyOrderSlot : MonoBehaviour {

    //float m_startOfOrderTime;
    BunnyOrderController m_bunnyOrderController;

    public SaveInventory_Player m_playerInventory;

    public CustomerOrder m_customerOrder;

    public Image m_OrderUI;
    private TextMeshProUGUI m_OrderTextUI;
    private Image[] m_OrderIngredientsUI = new Image[3];

    // Use this for initialization
    void Start () {
        m_bunnyOrderController = transform.GetComponentInParent<BunnyOrderController>();

        m_OrderTextUI = m_OrderUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        m_OrderIngredientsUI[0] = m_OrderUI.transform.GetChild(1).GetComponent<Image>();
        m_OrderIngredientsUI[1] = m_OrderUI.transform.GetChild(2).GetComponent<Image>();
        m_OrderIngredientsUI[2] = m_OrderUI.transform.GetChild(3).GetComponent<Image>();

        //ShowUI(false);
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

    private void Awake()
    {
        //if(m_customerOrder.m_isActive)
        //{
        //    GameObject customer = Instantiate(m_customerOrder.m_customer, transform.position, transform.rotation);
        //    customer.GetComponent<Customer>().EnteredShop();
        //    customer.GetComponent<Customer>().m_travelDestinationIndex = 3;
        //}
    }

    void ShowUI(bool isEnabled)
    {
        m_OrderUI.enabled = isEnabled;
        m_OrderTextUI.enabled = isEnabled;
        m_OrderIngredientsUI[0].enabled = isEnabled;
        m_OrderIngredientsUI[1].enabled = isEnabled;
        m_OrderIngredientsUI[2].enabled = isEnabled;
    }

    public bool GetIsActive()
    {
        return m_customerOrder.m_isActive;
    }

    public void GenerateOrder(GameObject customer, RabboidColour[] possibleColours, RabboidBodyPart[] possibleBodyMods, RabboidBodyPart[] possibleHeadMods)
    {
        // Set Timer
        m_customerOrder.m_isActive = true;
        ShowUI(true);
        //m_startOfOrderTime = Time.time;
        m_customerOrder.m_customer = customer;

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
                        m_customerOrder.m_size = RabboidCalculator.LARGE_SIZE;
                    }
                    else
                    {
                        m_customerOrder.m_size = RabboidCalculator.SMALL_SIZE;
                    }
                }
                // Chose a colour
                else if (ingredientType == 1)
                {
                    int randomColor = Random.Range(0, possibleColours.Length);
                    m_customerOrder.m_colour = possibleColours[randomColor].m_color;
                }
                // Chose a mouthpart
                else if (ingredientType == 2)
                {
                    int randomMod = Random.Range(0, possibleHeadMods.Length);
                    m_customerOrder.m_mouthPart = possibleHeadMods[randomMod];
                }
                // Chose a backpart
                else if (ingredientType == 3)
                {
                    int randomMod = Random.Range(0, possibleBodyMods.Length);
                    m_customerOrder.m_backPart = possibleBodyMods[randomMod];
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
        if (m_customerOrder.m_isActive)
        {
            RabboidResult RabboidStats = other.transform.parent.GetComponent<Rabboid>().RabboidStats;
            // Check the order
            int rabbitScore = 20; // Score is given based on how close the rabbit is to what the order wanted.
            int negativePointModifier = 1;
            int positivePointModifier = 2;

            // Check Size
            if (m_customerOrder.m_size == 0)
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
            if(m_customerOrder.m_size == RabboidCalculator.SMALL_SIZE)
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
            if (m_customerOrder.m_size == RabboidCalculator.LARGE_SIZE)
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
            if(resultsColor == m_customerOrder.m_colour)
            {
                rabbitScore += positivePointModifier;
            }
            else
            {
                rabbitScore -= negativePointModifier;
            }

            // Check Body
            if((RabboidStats.m_backPart == null && m_customerOrder.m_backPart == null)
            || (RabboidStats.m_backPart != null && m_customerOrder.m_backPart != null))
            {
                rabbitScore += positivePointModifier;
            }
            else
            {
                rabbitScore -= negativePointModifier;
            }

            // Check Head
            if((RabboidStats.m_mouthPart == null && m_customerOrder.m_mouthPart == null)
            || (RabboidStats.m_mouthPart != null && m_customerOrder.m_mouthPart != null))
            {
                rabbitScore += positivePointModifier;
            }
            else
            {
                rabbitScore -= negativePointModifier;
            }

            // Add coins based on performance
            if (rabbitScore < 0)
                rabbitScore = 0;
            m_playerInventory.m_money += (uint)rabbitScore;
            m_bunnyOrderController.UpdateCoinCounter(m_playerInventory.m_money);

            // Tidy up
            //m_customerOrder.m_customer.GetComponent<Customer>().OrderComplete();
            ShowUI(false);
            m_customerOrder.m_isActive = false;
            m_customerOrder.ResetVariables();
            Destroy(other.gameObject);
        }
    }
}
