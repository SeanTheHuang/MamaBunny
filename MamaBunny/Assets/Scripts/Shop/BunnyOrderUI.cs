using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BunnyOrderUI : MonoBehaviour {

    private Image m_BgImageUI;
    private TextMeshProUGUI m_OrderTextUI;
    private Image[] m_OrderIngredientsUI = new Image[3];

    public CustomerOrder m_customerOrder;

    public Sprite m_normalSize;
    public Sprite m_largeSize;


    private void Awake()
    {
        m_BgImageUI = GetComponent<Image>();
        m_OrderTextUI = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        m_OrderIngredientsUI[0] = transform.GetChild(1).GetComponent<Image>();
        m_OrderIngredientsUI[1] = transform.GetChild(2).GetComponent<Image>();
        m_OrderIngredientsUI[2] = transform.GetChild(3).GetComponent<Image>();
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
        if (!m_customerOrder.m_isActive)
        {
            m_BgImageUI.enabled = false;
            m_OrderTextUI.enabled = false;
            m_OrderIngredientsUI[0].enabled = false;
            m_OrderIngredientsUI[1].enabled = false;
            m_OrderIngredientsUI[2].enabled = false;
        }
        SetRecipeSprites();
    }

    public void ResetSprites()
    {
        m_BgImageUI.sprite = null;
        m_OrderIngredientsUI[0].sprite = null;
        m_OrderIngredientsUI[1].sprite = null;
        m_OrderIngredientsUI[2].sprite = null;
    }

    public void SetRecipeSprites()
    {
        bool colorSet = false;
        bool bodySet = false;
        bool headSet = false;
        bool sizeSet = false;

        for (int i = 0; i < m_OrderIngredientsUI.Length; ++i)
        {
            if(!colorSet && m_customerOrder.m_colour != null)
            {
                colorSet = true;
                m_OrderIngredientsUI[i].sprite = m_customerOrder.m_colour.m_recipeSprite;
                m_OrderIngredientsUI[i].preserveAspect = true;
            }
            else if(!bodySet && m_customerOrder.m_backPart != null)
            {
                bodySet = true;
                m_OrderIngredientsUI[i].sprite = m_customerOrder.m_backPart.m_recipeSprite;
                m_OrderIngredientsUI[i].preserveAspect = true;
            }
            else if (!headSet && m_customerOrder.m_mouthPart != null)
            {
                headSet = true;
                m_OrderIngredientsUI[i].sprite = m_customerOrder.m_mouthPart.m_recipeSprite;
                m_OrderIngredientsUI[i].preserveAspect = true;
            }
            else if (!sizeSet && m_customerOrder.m_size != 0)
            {
                sizeSet = true;
                if (m_customerOrder.m_size == RabboidCalculator.NORMAL_SIZE)
                    m_OrderIngredientsUI[i].sprite = m_normalSize;
                else if (m_customerOrder.m_size == RabboidCalculator.LARGE_SIZE)
                    m_OrderIngredientsUI[i].sprite = m_largeSize;
                m_OrderIngredientsUI[i].preserveAspect = true;
            }
        }
    }
}
