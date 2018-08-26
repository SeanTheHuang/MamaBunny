using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour {

    public int m_sceneIndexToGo;
    public float m_timeToTeleport = 2;
    float m_currentTimer;
    bool m_channeling = false;
    bool m_teleporting = false;

    private void Update()
    {
        if (!m_channeling || m_teleporting)
            return;

        m_currentTimer += Time.deltaTime;
        if (m_currentTimer >= m_timeToTeleport)
            ChangeScene();
    }

    void ChangeScene()
    {
        m_teleporting = true;
        SceneManager.LoadScene(m_sceneIndexToGo);
        Debug.Log("Change scene!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered teleporter");
            m_channeling = true;
            m_currentTimer = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited teleporter");
            m_channeling = false;
            m_currentTimer = 0;
        }
    }


}
