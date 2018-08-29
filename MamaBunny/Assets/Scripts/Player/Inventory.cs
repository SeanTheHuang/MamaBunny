using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public SaveInventory_Player m_inve;
    List<RabboidModBase> m_pickUpList;
    public uint m_capacity = 10;
    public bool m_displaying = false;

	void Awake ()
    {
        m_pickUpList = m_inve.m_inventory;
        m_capacity = m_inve.m_capacity;
        
        //m_pickUpList = new List<RabboidModBase>();
        //m_capacity = (uint)InventoryUI.Instance.m_capacityList.Count;
	}

    private void OnDisable()
    {
        m_inve.m_inventory = m_pickUpList;
        m_inve.m_capacity = m_capacity;
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

        EffectCanvas.Instance.InformText("Inventory is full!");
        return false;
    }

    public void TakeFromInventory(int _index)
    {
        SoundEffectsPlayer.Instance.PlaySound("Drop");

        //return the object for the player to spawn
        Transform camtr = Camera.main.transform;

        Transform tgo = Instantiate(m_pickUpList[_index].m_pickUpItemForm, camtr.position + camtr.forward * 0.5f, Quaternion.identity);
        tgo.GetComponent<Rigidbody>().AddForce(camtr.forward * 10 + camtr.up * 2f, ForceMode.Impulse);

        m_pickUpList.RemoveAt(_index);

        InventoryUI.Instance.Display(false, m_pickUpList);
        InventoryUI.Instance.m_linkedInventory = transform.GetComponent<Inventory>();
        InventoryUI.Instance.Display(true, m_pickUpList);

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
