using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftButton : MonoBehaviour {

    public GameObject m_elevator;
    private ShopElevator m_elevatorScript;

    private void Start()
    {
        m_elevatorScript = m_elevator.GetComponent<ShopElevator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        m_elevatorScript.MoveLiftDown();
    }
}
