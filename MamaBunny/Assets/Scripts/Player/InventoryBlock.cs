using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryBlock : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,  IPointerUpHandler , IPointerClickHandler
{
    public int m_listIndex;
    Color m_defualtColor;
    Color m_selectColor;
    Image m_image;
    InventoryUI m_invUI;



    // Use this for initialization
    void Start () {
        m_image = GetComponent<Image>();
        m_selectColor = m_image.color;
        m_invUI = transform.parent.GetComponent<InventoryUI>();
        m_defualtColor = Color.clear;

        ResetColor();
	}
	

    public void ResetColor()
    {
        m_image.color = m_defualtColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //report to inventory to drop;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        m_invUI.BlockClicked(m_listIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_image.color = m_selectColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ResetColor();
    }
}
