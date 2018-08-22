using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryBlock : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public int m_listIndex;
    Color m_startColor;
    Image m_image;
    InventoryUI m_invUI;

    public void OnPointerClick(PointerEventData eventData)
    {
        //report to inventory to drop;
        m_invUI.BlockClicked(m_listIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_image.color = Color.white;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_image.color = m_startColor;
    }

    // Use this for initialization
    void Start () {
        m_image = GetComponent<Image>();
        m_startColor = m_image.color;
        m_invUI = transform.parent.GetComponent<InventoryUI>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
