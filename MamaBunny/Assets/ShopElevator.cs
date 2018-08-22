using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopElevator : MonoBehaviour {

    public float m_moveSpeed;

    bool m_platformTriggered;
    bool m_movingDown;

    Vector3 m_topPosition = new Vector3(-0.36f, 0, 4.1f);
    Vector3 m_bottomPosition = new Vector3(-0.36f, -4.0f, 4.1f);

    private void Update()
    {
        if(m_platformTriggered)
        {
            if (m_movingDown)
            {
                moveToPosition(m_bottomPosition);
            }
            else
            {
                moveToPosition(m_topPosition);
            }
        }
    }

    void moveToPosition(Vector3 position)
    {
        float step = m_moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, position, step);
        if(transform.position == position)
        {
            m_platformTriggered = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!m_platformTriggered && other.gameObject.layer == LayerMask.NameToLayer("Citizen"))
        {
            m_platformTriggered = true;
            if (transform.position == m_topPosition)
                m_movingDown = true;
            else
                m_movingDown = false;
        }
    }
}
