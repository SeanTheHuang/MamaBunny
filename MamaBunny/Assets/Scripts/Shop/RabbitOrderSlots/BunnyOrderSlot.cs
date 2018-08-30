using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class BunnyOrderSlot : MonoBehaviour {

    //float m_startOfOrderTime;
    BunnyOrderController m_bunnyOrderController;

    public SaveInventory_Player m_playerInventory;

    public CustomerOrder m_customerOrder;
    public GameObject m_customerWaitLocation;
    public GameObject m_CustomerPrefab;

    public string m_interactText;

    private Customer m_Customer;

    public Image m_OrderImageUI;
    private BunnyOrderUI m_OrderUI;
    private TextMeshProUGUI m_OrderTextUI;
    private Image[] m_OrderIngredientsUI = new Image[3];
    private Transform m_bunny;

    private SpriteRenderer m_spriteRenderer;

    // Use this for initialization
    void Start () {
        m_bunnyOrderController = transform.GetComponentInParent<BunnyOrderController>();
        m_OrderUI = m_OrderImageUI.GetComponent<BunnyOrderUI>();

        m_OrderTextUI = m_OrderImageUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        m_OrderIngredientsUI[0] = m_OrderImageUI.transform.GetChild(1).GetComponent<Image>();
        m_OrderIngredientsUI[1] = m_OrderImageUI.transform.GetChild(2).GetComponent<Image>();
        m_OrderIngredientsUI[2] = m_OrderImageUI.transform.GetChild(3).GetComponent<Image>();
        //ShowUI(false);
        m_spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    void ShowUI(bool isEnabled)
    {
        m_OrderImageUI.enabled = isEnabled;
        m_OrderTextUI.enabled = isEnabled;
        m_OrderIngredientsUI[0].enabled = isEnabled;
        m_OrderIngredientsUI[1].enabled = isEnabled;
        m_OrderIngredientsUI[2].enabled = isEnabled;
        if(!isEnabled)
        {
            m_OrderUI.ResetSprites();
        }
    }

    public bool GetIsActive()
    {
        return m_customerOrder.m_isActive;
    }

    public void GenerateOrder(GameObject customer, RabboidColour[] possibleColours, RabboidBodyPart[] possibleBodyMods, RabboidBodyPart[] possibleHeadMods)
    {
        // Set Timer
        m_customerOrder.m_isActive = true;
        m_customerOrder.m_modelType = customer.GetComponent<Customer>().m_modelType;
        //m_startOfOrderTime = Time.time;
        //m_customerOrder.m_customerModel = customer.transform.Find("Model").gameObject;
        m_Customer = customer.GetComponent<Customer>();

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
                        m_customerOrder.m_size = RabboidCalculator.NORMAL_SIZE;
                    }
                }
                // Chose a colour
                else if (ingredientType == 1)
                {
                    int randomColor = Random.Range(0, possibleColours.Length);
                    m_customerOrder.m_colour = possibleColours[randomColor];
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

        ShowUI(true);
        m_spriteRenderer.enabled = true;
        m_OrderUI.SetRecipeSprites();

    }

    private void OnTriggerEnter(Collider other)
    {
        Transform parent = other.transform.parent;
        if (parent != null)
        {
            Rabboid rabboid = other.transform.parent.GetComponent<Rabboid>();
            if (rabboid != null)
            {
                if (m_customerOrder.m_isActive)
                {
                    RabboidResult RabboidStats = other.transform.parent.GetComponent<Rabboid>().RabboidStats;
                    // Check the order
                    int rabbitScore = 10; // Score is given based on how close the rabbit is to what the order wanted.
                    int negativePointModifier = 2;
                    int positivePointModifier = 2;

                    // Check Size
                    if (m_customerOrder.m_size == RabboidCalculator.NORMAL_SIZE)
                    {
                        if (RabboidStats.m_size <= RabboidCalculator.SMALL_SIZE)
                        {
                            rabbitScore -= negativePointModifier;
                        }
                        else if (RabboidStats.m_size >= RabboidCalculator.LARGE_SIZE)
                        {
                            rabbitScore -= negativePointModifier;
                        }
                        else
                        {
                            rabbitScore += positivePointModifier;
                        }
                    }
                    else if (m_customerOrder.m_size == RabboidCalculator.SMALL_SIZE)
                    {
                        if (RabboidStats.m_size <= RabboidCalculator.SMALL_SIZE)
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
                    else if (m_customerOrder.m_size == RabboidCalculator.LARGE_SIZE)
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
                    Color customerColor = Color.white;
                    if (m_customerOrder.m_colour != null)
                        customerColor = m_customerOrder.m_colour.m_color;
                    if (resultsColor == customerColor)
                    {
                        rabbitScore += positivePointModifier;
                    }
                    else
                    {
                        rabbitScore -= negativePointModifier;
                    }

                    // Check Body
                    if ((RabboidStats.m_backPart == null && m_customerOrder.m_backPart == null)
                    || (RabboidStats.m_backPart != null && m_customerOrder.m_backPart != null))
                    {
                        rabbitScore += positivePointModifier;
                    }
                    else
                    {
                        rabbitScore -= negativePointModifier;
                    }

                    // Check Head
                    if ((RabboidStats.m_mouthPart == null && m_customerOrder.m_mouthPart == null)
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
                    m_Customer.OrderComplete();
                    m_bunny = other.transform;
                    DestroyBunny();
                    m_spriteRenderer.enabled = false;
                }
            }
        }
    }

    void DestroyBunny()
    {
        foreach (Transform child in m_bunny)
        {
            Destroy(child.gameObject);
        }
        Destroy(m_bunny.gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (m_customerOrder.m_isActive)
        {
            GameObject model = Instantiate(CustomerCalculator.Instance.CalculateCustomerModel(m_customerOrder.m_modelType), m_customerWaitLocation.transform.position, transform.rotation);
            GameObject newCustomer = Instantiate(m_CustomerPrefab, m_customerWaitLocation.transform.position, transform.rotation);
            model.transform.parent = newCustomer.transform;
            m_Customer = newCustomer.GetComponent<Customer>();
            m_Customer.SetWaitingForOrder();
        }
    }
}
