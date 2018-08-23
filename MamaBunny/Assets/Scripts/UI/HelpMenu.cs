using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpMenu : MonoBehaviour {

    public Image m_helpBookImage;

    [Header("Different page sprites")]
    public Sprite m_recipeSprite;
    public Sprite m_goalSprite;
    public Sprite m_controlsSprite;
    public Sprite m_itemsBackgroundSprite;

    public PlayerControl m_player;

    private void Start()
    {
        // Start off with game object turned off
        m_helpBookImage.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            // Flip toggle state
            if (m_helpBookImage.sprite == m_recipeSprite)
            {
                m_helpBookImage.gameObject.SetActive(!m_helpBookImage.gameObject.activeInHierarchy);
                if (m_player)
                    m_player.LockPlayer(m_helpBookImage.gameObject.activeInHierarchy);
                ToRecipePage();
            }
            else
            {
                m_helpBookImage.gameObject.SetActive(true);
                if (m_player)
                    m_player.LockPlayer(true);
                ToRecipePage();
            }
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            // Flip toggle state
            if (!m_helpBookImage.gameObject.activeInHierarchy)
            {
                ToItemsPage();
                m_helpBookImage.gameObject.SetActive(true);
                if (m_player)
                    m_player.LockPlayer(true);
            }
            else if (m_helpBookImage.sprite == m_itemsBackgroundSprite)
            {
                m_helpBookImage.gameObject.SetActive(false);
                if (m_player)
                    m_player.LockPlayer(false);
            }
            else
                ToItemsPage();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Flip toggle state
            m_helpBookImage.gameObject.SetActive(false);
            if (m_player)
                m_player.LockPlayer(false);
        }
    }

    public void ToRecipePage()
    {
        HideInventory();
        m_helpBookImage.sprite = m_recipeSprite;
    }

    public void ToGoalPage()
    {
        HideInventory();
        m_helpBookImage.sprite = m_goalSprite;
    }

    public void ToControlPage()
    {
        HideInventory();
        m_helpBookImage.sprite = m_controlsSprite;
    }

    public void ToItemsPage()
    {
        ShowInventory();
        m_helpBookImage.sprite = m_itemsBackgroundSprite;
    }

    // === FILL THESE FUNCTIONS HUGO
    void HideInventory()
    {

    }

    void ShowInventory()
    {

    }
}
