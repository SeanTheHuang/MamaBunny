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
    }
}
