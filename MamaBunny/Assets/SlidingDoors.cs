using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoors : MonoBehaviour {

    public float m_SensorRadius;
    public float m_moveSpeed;

    float m_step;

    GameObject m_leftDoor;
    GameObject m_rightDoor;

    Vector3 m_leftDoorClosedPosition;
    Vector3 m_rightDoorClosedPosition;

    Vector3 m_leftDoorOpenPosition;
    Vector3 m_rightDoorOpenPosition;
    
    private void Start()
    {
        m_leftDoor = transform.Find("LeftDoor").gameObject;
        m_rightDoor = transform.Find("RightDoor").gameObject;

        m_leftDoorClosedPosition = m_leftDoor.transform.position;
        m_rightDoorClosedPosition = m_rightDoor.transform.position;

        m_leftDoorOpenPosition = m_leftDoorClosedPosition;
        m_leftDoorOpenPosition.x -= 1.3f;

        m_rightDoorOpenPosition = m_rightDoorClosedPosition;
        m_rightDoorOpenPosition.x += 1.3f;
    }

    private void Update()
    {
        m_step = m_moveSpeed * Time.deltaTime;
        if (CheckIfActivatingButton())
        {
            m_leftDoor.transform.position = Vector3.MoveTowards(m_leftDoor.transform.position, m_leftDoorOpenPosition, m_step);
            m_rightDoor.transform.position = Vector3.MoveTowards(m_rightDoor.transform.position, m_rightDoorOpenPosition, m_step);
        }
        else
        {
            m_leftDoor.transform.position = Vector3.MoveTowards(m_leftDoor.transform.position, m_leftDoorClosedPosition, m_step);
            m_rightDoor.transform.position = Vector3.MoveTowards(m_rightDoor.transform.position, m_rightDoorClosedPosition, m_step);
        }
    }

    // Checks if the rock is close enough to activate a rock button
    bool CheckIfActivatingButton()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, m_SensorRadius, 1 << 12);

        // no one is near the door
        if (hitColliders.Length == 0)
            return false;

        // someone is near the door
        return true;

    }
}
