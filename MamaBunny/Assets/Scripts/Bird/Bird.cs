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
    float m_targetYHeight;
    public float m_distTryGetAway = 10;

    [Header("Time stuff")]
    [MinMaxRange(0.05f, 1f)]
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

    public List<AudioClip> m_audioClips;
    float m_lastPlayedTime, m_timeBetweenSounds;
    AudioSource m_audioSource;

    private void Awake()
    {
        m_cc = GetComponent<CharacterController>();
        m_anim = GetComponentInChildren<Animator>();
        m_audioSource = GetComponent<AudioSource>(); 
    }

    private void Start()
    {
        m_targetYHeight = m_targetYHeightOffset + transform.position.y;
        m_stateChangeTime = Time.time + Random.Range(m_pauseDuration.minValue, m_pauseDuration.maxValue);
        m_currentState = BirdState.IDLE;
        m_lastPlayedTime = 0; m_timeBetweenSounds = 1.0f;
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

        m_cc.Move(m_currentVel * Time.deltaTime);

        RandomSounds();
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
            m_moveDir = Quaternion.Euler(0, Random.Range(45, 315), 0) * transform.forward;
            m_stateChangeTime = Time.time + Random.Range(m_walkDuration.minValue, m_walkDuration.maxValue);
            m_anim.SetBool("Moving", true);
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
            m_anim.SetBool("Moving", false);
            return;
        }

        Vector3 newMove = m_currentVel;
        newMove.y = 0;
        newMove = Vector3.Lerp(newMove, m_moveDir * m_walkSpeed, Time.deltaTime * 15);
        // Rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(newMove.normalized, transform.up), 25 * Time.deltaTime);
        newMove.y = m_currentVel.y;

        // Movement
        m_currentVel = newMove;
    }

    void ScaredLogic()
    {
        Vector3 newMove = m_currentVel;
        newMove.y = 0;
        newMove = Vector3.Lerp(newMove, m_moveDir * m_walkSpeed, Time.deltaTime * 20);
        // Rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(newMove.normalized, transform.up), 20 * Time.deltaTime);
        newMove.y = m_currentVel.y;

        // Fly 
        if (transform.position.y < m_targetYHeight)
            newMove.y += 2 * Time.deltaTime;
        else
            newMove.y = 0;

        // Movement
        m_currentVel = newMove;
    }


    public void NewRunAwayTarget(Transform _target = null)
    {
        m_anim.SetBool("Moving", true);
        m_currentState = BirdState.SCARED;

        if (_target)
        {
            m_citizen = _target;
            m_moveDir = transform.position - m_citizen.position;
            m_moveDir.y = 0;
            m_moveDir = Quaternion.Euler(new Vector3(0, Random.Range(-90, 90), 0)) * m_moveDir.normalized;
        }
        else
            m_moveDir = transform.forward;

        // Destroy after 10 seconds
        Destroy(gameObject, 10);
    }

    public override void TakeHit(float _damage)
    {
        m_health -= _damage;
        if (m_health < 1)
            OnDeath();

        NewRunAwayTarget();
    }

    void OnDeath()
    {
        m_dead = true;
        m_currentVel *= 0.2f;
        m_currentVel.y = 0;
    }

    void SpawnItemsAndDestroy()
    {
        Instantiate(m_spawnedItemPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    void RandomSounds()
    {
        if(Time.time - m_lastPlayedTime > m_timeBetweenSounds)
        {
            //time for new sound
            AudioClip ac = m_audioClips[Random.Range(0, m_audioClips.Count)];
            if (ac != m_audioSource.clip)
            {
                m_audioSource.clip = ac;
                m_audioSource.Play();

                m_lastPlayedTime = Time.time;
                m_timeBetweenSounds = ac.length + Random.Range(3.0f, 6.5f);
            }
            else
            {
                m_lastPlayedTime = Time.time;
                m_timeBetweenSounds = Random.Range(0.0f, 2.9f);
            }
        }
        //do nothing
    }
}
