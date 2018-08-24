using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour{

    public Image m_block;
    Image m_Background;
    public Image m_selectSquare;

    public Inventory m_linkedInventory;

    uint m_capacity = 0;
    List<Image> m_capacityList;

    public uint m_rightMax = 5;
    public uint m_downMax = 4;
    public Vector2 m_startPos;
    public uint m_distance = 100;
    bool m_displaying = false;

    Color m_blockColor;

    List<RabboidModBase> m_rmb;

    public static InventoryUI Instance
    { get; private set; }

	void Awake () {
        Instance = this;//singleton
        m_capacityList = new List<Image> { };
        m_Background = GetComponent<Image>();
        m_blockColor = m_block.color;
        m_rmb = new List<RabboidModBase> { };
        Display(false, null);
	}
	
	void Update ()
    {
        if(m_displaying == true)
        {
            Manage();
        }
	}

    public void Display(bool _display, List<RabboidModBase> _rmb)
    {
        m_Background.gameObject.SetActive(_display);
        foreach(Image rt in m_capacityList)
        {
            rt.gameObject.SetActive(_display);
        }

        if(_display == true)
        {
            DisplayInventory(_rmb);
        }
        else
        {
            HideInventory();
        }
    }

    public void SetCapacity(uint _capacity)
    {
        if(m_capacity == _capacity)
        {//no need to change
            return;
        }
        
        foreach(Image im in m_capacityList)
        {
            Destroy(im.gameObject);
        }

        m_capacityList.Clear();
        m_capacity = _capacity;

        int count = 0;
        for (uint i = 0; i < m_downMax; i++) 
        {
            for (uint j = 0; j < m_rightMax; j++) 
            {
                if(count == m_capacity)
                {
                    i = m_downMax;
                    break;//leave the double for loop
                }

                GameObject go = Instantiate(m_block.gameObject);
                go.transform.SetParent(transform, false);
                go.GetComponent<RectTransform>().anchoredPosition = new Vector2(m_startPos.x + (j * m_distance), m_startPos.y - (i * m_distance));

                m_capacityList.Add(go.transform.GetComponent<Image>());

                count++;
            }
        }

    }

    void DisplayInventory(List<RabboidModBase> _rmb)
    {
        m_rmb = _rmb;
        m_displaying = true;
        //show their sprites
        //set the sprites too

        for (int i = 0; i < _rmb.Count; i++) 
        {
            m_capacityList[i].transform.GetChild(0).gameObject.SetActive(true);//turn on
            m_capacityList[i].transform.GetChild(0).GetComponent<Image>().sprite = _rmb[i].m_itemSprite;// set sprite
            m_capacityList[i].GetComponent<InventoryBlock>().m_listIndex = i;//set index
        }
    }

    void HideInventory()
    {
        m_linkedInventory = null;
        m_rmb = null;
        m_displaying = false;
        foreach(Image im in m_capacityList)
        {
            //set to defualt
            im.transform.GetChild(0).gameObject.SetActive(false);
            //remove sprite
        }

        for (int i = 0; i < m_capacity; i++)
        {//stop them from appearing bad when they pop up straight away
            m_capacityList[i].GetComponent<InventoryBlock>().ResetColor();
        }
    }

    void Manage()
    {

    }

    public void BlockClicked(int _index)
    {
        if(m_displaying == false)
        {
            return;
        }
        if(_index < m_rmb.Count)
        {//there is an object there
            //dopr it
            m_linkedInventory.TakeFromInventory(_index);
        }
    }
}
