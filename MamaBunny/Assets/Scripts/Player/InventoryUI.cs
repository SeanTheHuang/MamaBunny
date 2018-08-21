using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {

    public Image blockbacker;
    Image m_Background;

    public Inventory m_linkedInventory;

    uint m_capacity = 0;
    List<Image> m_capacityList;

    public uint m_rightMax = 5;
    public uint m_downMax = 4;
    public Vector2 m_startPos;
    public uint m_distance = 100;

    public static InventoryUI Instance
    { get; private set; }

	void Awake () {
        Instance = this;//singleton
        m_capacityList = new List<Image> { };
        m_Background = GetComponent<Image>();
        Display(false, null);
	}
	
	void Update ()
    {
		//if(Input.GetKeyDown(KeyCode.P))
  //      {
  //          SetCapacity((uint)Random.Range(1, 20));
  //      }
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

                GameObject go = Instantiate(blockbacker.gameObject);
                go.transform.SetParent(transform, false);
                go.GetComponent<RectTransform>().anchoredPosition = new Vector2(m_startPos.x + (j * m_distance), m_startPos.y - (i * m_distance));

                m_capacityList.Add(go.transform.GetComponent<Image>());

                count++;
            }
        }

    }

    void DisplayInventory(List<RabboidModBase> _rmb)
    {
        //show their sprites
        for (int i = 0; i < _rmb.Count; i++) 
        {
            m_capacityList[i].transform.GetChild(0).gameObject.SetActive(true);
            /*
            GameObject go = new GameObject("itemtext");
            go.transform.parent = m_capacityList[i].transform;
            go.AddComponent<Text>();
            go.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            go.GetComponent<Text>().text = _rmb[i].name;
            go.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);*/
        }
    }

    void HideInventory()
    {
        foreach(Image im in m_capacityList)
        {
            //set to defualt
            im.transform.GetChild(0).gameObject.SetActive(false);
            //remove sprite
        }
    }
}
