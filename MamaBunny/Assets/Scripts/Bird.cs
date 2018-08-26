using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : GunTarget {
    enum BirdState
    {
        IDLE,
        WALK,
        SCARED
    }

    [Header("Stats")]
    public float m_walkSpeed = 10;
    public float m_flySpeed = 20;
    public float m_targetYHeightOffset = 15;
    public float m_distTryGetAway = 10;
    public float m_distTooClose = 5;

    [Header("Time stuff")]
    [MinMaxRange(0.3f, 1f)]
    public RangedFloat m_walkDuration;

    [MinMaxRange(1, 3)]
    public RangedFloat m_pauseDuration;

    bool m_dead = false;
    CharacterController m_cc;
    Animator m_anim;

    Transform m_citizen;
    Vector3 m_currentVel, m_moveDir;
    BirdState m_currentState;

    float m_stateChangeTime;

    private void Awake()
    {
        m_cc = GetComponent<CharacterController>();
        m_anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        m_stateChangeTime = Time.time + Random.Range(m_pauseDuration.minValue, m_pauseDuration.maxValue);
        m_currentState = BirdState.IDLE;
    }

    private void Update()
    {
        if (m_dead)
        {
            // Only fall
            if (!m_cc.isGrounded)
            {
                m_currentVel.y += -15 * Time.deltaTime;
                m_cc.SimpleMove(m_currentVel);
            }
            else
            {
                SpawnItemsAndDestroy();
            }
            return;
        }

        // Not dead
        switch (m_currentState)
        {
            case BirdState.IDLE:
                IdleLogic();
                break;
            case BirdState.WALK:
                WalkLogic();
                break;
            case BirdState.SCARED:
                ScaredLogic();
                break;
        }

        m_cc.SimpleMove(m_currentVel);
    }

    void IdleLogic()
    {
        // Gravity
        if (!m_cc.isGrounded)
            m_currentVel.y -= 15 * Time.deltaTime;
        else
            m_currentVel.y = 0;

        if (Time.time >= m_stateChangeTime)
        {
            m_currentState = BirdState.WALK;
            m_moveDir = Quaternion.Euler(0, Random.Range(0, 360), 0) * Vector3.forward;
            m_stateChangeTime = Time.time + Random.Range(m_walkDuration.minValue, m_walkDuration.maxValue);
        }
    }

    void WalkLogic()
    {
        // Gravity
        if (!m_cc.isGrounded)
            m_currentVel.y -= 15 * Time.deltaTime;
        else
            m_currentVel.y = 0;

        // See if time to change to idle
        if (Time.time >= m_stateChangeTime)
        {
            m_currentVel.x = m_currentVel.z = 0;
            m_stateChangeTime = Time.time + Random.Range(m_pauseDuration.minValue, m_pauseDuration.maxValue);
            m_currentState = BirdState.IDLE;
            return;
        }

        Vector3 newMove = m_currentVel;
        newMove.y = 0;
        newMove = Vector3.Lerp(newMove, m_moveDir * m_walkSpeed, Time.deltaTime * 8);
        // Rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(newMove.normalized), 10 * Time.deltaTime);
        newMove.y = m_currentVel.y;

        // Movement
        m_currentVel = newMove;
    }

    void ScaredLogic()
    {
        
    }
    

    public override void TakeHit(float _damage)
    {
        m_health -= _damage;
        if (m_health < 1)
            OnDeath();
    }

    void OnDeath()
    {
        m_dead = true;
        m_currentVel *= 0.2f;
        m_currentVel.y = 0;

        Debug.Log("Wackoon Died");
    }

    void SpawnItemsAndDestroy()
    {
        // TODO: Spawn
        Destroy(gameObject);
    }
}
