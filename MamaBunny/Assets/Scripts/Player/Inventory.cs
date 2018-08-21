using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    List<PickUp> m_pickUpList;
    public uint m_capacity = 10; 

	void Start ()
    {
        m_pickUpList = new List<PickUp>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool AddToInventory(PickUp _pickup)
    {
        if (m_pickUpList.Count != m_capacity)
        {
            m_pickUpList.Add(_pickup);
            return true;
        }
        return false;
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(0, 0, 50, 50), m_pickUpList.Count.ToString());
    }
}
