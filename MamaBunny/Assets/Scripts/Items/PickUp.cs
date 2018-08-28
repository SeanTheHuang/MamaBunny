using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickUp : MonoBehaviour {

    public string m_name = "NO_NAME_INPUTTED";
    public float m_liveTime = 20;
    float m_startLiveTime;
    bool m_beingEaten = false;

    private void Awake()
    {
        m_startLiveTime = Time.time;
    }

    private void Update()
    {
        if (m_beingEaten)
            return;

        if (Time.time - m_startLiveTime >= m_liveTime)
            Destroy(gameObject);
    }

    public abstract void OnEatenByMamaRabbit(RabboidMama _mama);

    public void OnBeingEaten(Transform[] _pathToFollow)
    {
        if (m_beingEaten)
            return;

        SoundEffectsPlayer.Instance.PlaySound("Succ");
        m_beingEaten = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = true;

        StartCoroutine(FollowPipePath(_pathToFollow));
    }

    IEnumerator FollowPipePath(Transform[] _pathToFollow)
    {
        float moveSpeed = 3.5f;
        Vector3 rotateAxis = new Vector3(1, 0.6f, 0.3f);
        float rotateSpeed = 45f;

        for (int i = 0; i < _pathToFollow.Length; i++)
        {
            bool done = false;
            while (!done)
            {
                // Move logic
                Vector3 newPos = Vector3.MoveTowards(transform.position, _pathToFollow[i].position, moveSpeed * Time.deltaTime);
                if (newPos == _pathToFollow[i].position)
                {
                    if (i + 1 < _pathToFollow.Length - 1)
                        newPos = Vector3.MoveTowards(transform.position, _pathToFollow[i + 1].position, moveSpeed * Time.deltaTime);
                    done = true;
                }
                transform.position = newPos;

                // Rotate logic
                transform.Rotate(rotateAxis * rotateSpeed * Time.deltaTime);
                yield return null;
            }
        }

        // Start moving again
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = false;

        yield return null;
    }
}
