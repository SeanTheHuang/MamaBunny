using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour {

    CharacterController m_chara;
    public float m_maxWalkSpeed = 5;
    public float m_maxRunSpeed = 10;
    float m_timeToReachMaxSpeed = 0.1f;
    float m_timeToWalkSpeed = 0.1f, m_timeToRunSpeed = 0.5f;
    float m_moveSpeed;
    float m_yVel;
    public float m_jumpForce;
    Vector3 m_currentVel, m_refVel;

     bool m_playerlocked = false;
	// Use this for initialization
	void Start ()
    {
        m_chara = GetComponent<CharacterController>();
        m_currentVel = Vector3.zero;
        m_yVel = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Running();
        MoveLogic();
	}

    void MoveLogic()
    {
        float vAxis = 0, hAxis = 0;
        if (m_playerlocked == false)
        {
            vAxis = Input.GetAxisRaw("Vertical");
            hAxis = Input.GetAxisRaw("Horizontal");
            if (m_chara.isGrounded)
            {
                m_yVel = 0 + Jumping();
            }
        }

        hAxis *= 0.75f;//slows down left and right
        if(vAxis < 0)//slows down backwards
        {
            vAxis *= 0.75f;
        }

        Vector3 velocity = transform.rotation * new Vector3(hAxis, 0, vAxis).normalized;
        velocity *= m_moveSpeed;
        m_currentVel = Vector3.SmoothDamp(m_currentVel, velocity, ref m_refVel, m_timeToReachMaxSpeed);

        //gravity
        

        m_yVel += Physics.gravity.y * Time.deltaTime;
        m_currentVel.y = m_yVel;

        m_chara.Move(m_currentVel  * Time.deltaTime);

    }

    void Running()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            m_moveSpeed = m_maxRunSpeed;
            m_timeToReachMaxSpeed = m_timeToRunSpeed;
        }
        else
        {
            m_moveSpeed = m_maxWalkSpeed;
            m_timeToReachMaxSpeed = m_timeToWalkSpeed;
        }
    }

    float  Jumping()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
           // Debug.Log("jump");
            return m_jumpForce;
        }
        return 0.0f;
    }

    public void LockPlayer(bool _lock)
    {
        m_playerlocked = _lock;
        GetComponent<PlayerCameraController>().m_cameraLocked = !_lock;
        GetComponentInChildren<Gun>().LockGun(_lock);
        GetComponentInChildren<GrabHand>().LockHand(_lock);
    }
}
