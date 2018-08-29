using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopElevator : MonoBehaviour {

    public float m_moveSpeed;

    bool m_platformTriggered;
    bool m_movingDown;

    Vector3 m_topPosition;
    Vector3 m_bottomPosition;

    private void Start()
    {
        m_topPosition = transform.position;
        m_bottomPosition = m_topPosition;
        m_bottomPosition.y -= 4.0f;
    }

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

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        MoveLiftDown();
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

    public void MoveLiftUp()
    {
        m_platformTriggered = true;
        m_movingDown = false;
    }

    public void MoveLiftDown()
    {
        m_platformTriggered = true;
        m_movingDown = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!m_platformTriggered && other.gameObject.layer == LayerMask.NameToLayer("Citizen"))
        {
            other.transform.parent = transform;
            if (transform.position == m_topPosition)
                MoveLiftDown();
            else
                MoveLiftUp();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.parent = null;
    }
}
