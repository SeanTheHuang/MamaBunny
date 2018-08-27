using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour {

    [Header("Footstep Stuff")]
    public float m_footstepDelay = 0.3f;
    [MinMaxRange(0.5f, 1.5f)]
    public RangedFloat m_pitchChange;
    public float m_modWhenSprinting = 0.5f;
    float m_lastStepTime;
    AudioSource m_stepAudio;

    CharacterController m_chara;
    public float m_maxWalkSpeed = 5;
    public float m_maxRunSpeed = 10;
    float m_timeToReachMaxSpeed = 0.1f;
    float m_smoothMoveTime = 0.1f;
    float m_moveSpeed;
    float m_yVel;
    public float m_jumpForce;
    Vector3 m_currentVel, m_refVel;

    bool m_playerlocked = false, m_canRun = true, m_canJump = true;
    // Use this for initialization
    void Start()
    {
        m_chara = GetComponent<CharacterController>();
        m_stepAudio = GetComponent<AudioSource>();
        m_currentVel = Vector3.zero;
        m_yVel = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Running();
        MoveLogic();
        SoundLogic();
    }

    void SoundLogic()
    {
        if (!m_chara.isGrounded)
            return;

        float vAxis = Input.GetAxisRaw("Vertical");
        float hAxis = Input.GetAxisRaw("Horizontal");
        bool sprinting = Input.GetKeyDown(KeyCode.LeftShift);

        if (Mathf.Abs(vAxis) < 0.1f && Mathf.Abs(hAxis) < 0.1f) // Not moving
            return;

        float stepTime = sprinting ? m_footstepDelay * m_modWhenSprinting : m_footstepDelay;
        if (Time.time - m_lastStepTime > stepTime)
        {
            m_lastStepTime = Time.time + stepTime;
            m_stepAudio.pitch = Random.Range(m_pitchChange.minValue, m_pitchChange.maxValue);
            m_stepAudio.Play();
        }
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
        if(m_canRun && Input.GetKey(KeyCode.LeftShift))
        {
            m_moveSpeed = m_maxRunSpeed;
            m_timeToReachMaxSpeed = m_smoothMoveTime;
        }
        else
        {
            m_moveSpeed = m_maxWalkSpeed;
            m_timeToReachMaxSpeed = m_smoothMoveTime;
        }
    }

    float Jumping()
    {
        if (m_canJump && Input.GetKeyDown(KeyCode.Space))
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

    public void AimMode()
    {
        m_canRun = m_canJump = false;
        m_moveSpeed *= 0.5f;
    }

    public void NormalMode()
    {
        m_canRun = m_canJump = true;
        m_moveSpeed *= 2;
    }
}
