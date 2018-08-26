using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    List<RabboidModBase> m_pickUpList;
    public uint m_capacity = 10;
    public bool m_displaying = false;

	void Start ()
    {
        m_pickUpList = new List<RabboidModBase>();
        m_capacity = (uint)InventoryUI.Instance.m_capacityList.Count;
	}
	
     //Update is called once per frame
    void Update ()
    {
    	/*if(Input.GetKeyDown(KeyCode.I))
        {
            OpenInventory();
        }*/
    }

    public bool AddToInventory(PickUp _pickup)
    {
        if (m_pickUpList.Count != m_capacity)
        {
            m_pickUpList.Add(PickUptoBase(_pickup));
            return true;
        }

        return false;
    }

    public void TakeFromInventory(int _index)
    {
        //return the object for the player to spawn
        Instantiate(m_pickUpList[_index].m_pickUpItemForm, transform.position + transform.forward, Quaternion.identity);
        m_pickUpList.RemoveAt(_index);
        InventoryUI.Instance.Display(false, m_pickUpList);
        InventoryUI.Instance.m_linkedInventory = transform.GetComponent<Inventory>();
        InventoryUI.Instance.Display(true, m_pickUpList);

    }

    private void OnGUI()
    {
        GUI.Box(new Rect(0, 0, 50, 50), m_pickUpList.Count.ToString());
    }

    RabboidModBase PickUptoBase(PickUp _pickUp)
    {
        if(_pickUp.GetComponent<ColorModPickUp>())
        {
            return _pickUp.GetComponent<ColorModPickUp>().m_colourMod;
        }
        else if (_pickUp.GetComponent<BackModPickUp>())
        {
            return _pickUp.GetComponent<BackModPickUp>().m_bodyPart;
        }
        else if (_pickUp.GetComponent<MouthModPickUp>())
        {
            return _pickUp.GetComponent<MouthModPickUp>().m_mouthMod;
        }
        else if (_pickUp.GetComponent<SizeModPickUp>())
        {
            return _pickUp.GetComponent<SizeModPickUp>().m_sizeMod;
        }
        else
        {
            Debug.Log("SCREAMING");
            return null;
        }
    }

    PickUp BasetoPickUp(RabboidModBase _rmb)
    {
        return _rmb.m_pickUpItemForm.GetComponent<PickUp>();
    }

    void OpenInventory()
    {
        InventoryUI.Instance.SetCapacity(m_capacity);

        GetComponent<PlayerControl>().LockPlayer(!m_displaying);
        InventoryUI.Instance.m_linkedInventory = transform.GetComponent<Inventory>();
        if (m_displaying)
        {
            InventoryUI.Instance.m_linkedInventory = transform.GetComponent<Inventory>();
            InventoryUI.Instance.Display(false, null);

        }
        else
        {
            InventoryUI.Instance.Display(true, m_pickUpList);
        }
        m_displaying = !m_displaying;
    }

    public void ShowInventory()
    {
        InventoryUI.Instance.SetCapacity(m_capacity);
        InventoryUI.Instance.m_linkedInventory = transform.GetComponent<Inventory>();
        InventoryUI.Instance.Display(true, m_pickUpList);
        m_displaying = true;
    }

    public void HideInventory()
    {
        //Debug.Log("hiding");
        InventoryUI.Instance.Display(false, null);
        m_displaying = false;
    }
}
