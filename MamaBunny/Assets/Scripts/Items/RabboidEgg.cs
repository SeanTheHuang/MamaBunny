using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabboidEgg : MonoBehaviour {

    bool m_started = false;
    Transform[] m_pathToFollow;
    Rigidbody m_rb;
    RabboidMama m_mama;
    RabboidResult m_results;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_rb.isKinematic = true;
    }

    public void StartAnimation(Transform[] _path, RabboidMama _mama, RabboidResult _results)
    {
        if (m_started)
            return;

        m_started = true;
        m_pathToFollow = _path;
        m_mama = _mama;
        m_results = _results;
        StartCoroutine(SpawnAnimation());
    }

    IEnumerator SpawnAnimation()
    {
        // Animation will only play with 2 or more nodes
        if (m_pathToFollow.Length < 2)
            yield return null;

        transform.position = m_pathToFollow[0].position;

        // Move through pipe
        float moveSpeed = 3.5f;
        Vector3 rotateAxis = new Vector3(1, 0.6f, 0.3f);
        float rotateSpeed = 45f;
        Vector3 pushForce = (m_pathToFollow[m_pathToFollow.Length - 1].position - m_pathToFollow[m_pathToFollow.Length - 2].position).normalized * 5;

        for (int i = 1; i < m_pathToFollow.Length; i++)
        {
            bool done = false;
            while (!done)
            {
                // Move logic
                Vector3 newPos = Vector3.MoveTowards(transform.position, m_pathToFollow[i].position, moveSpeed * Time.deltaTime);
                if (newPos == m_pathToFollow[i].position)
                {
                    if (i + 1 < m_pathToFollow.Length - 1)
                        newPos = Vector3.MoveTowards(transform.position, m_pathToFollow[i + 1].position, moveSpeed * Time.deltaTime);
                    done = true;
                }
                transform.position = newPos;

                // Rotate logic
                transform.Rotate(rotateAxis * rotateSpeed * Time.deltaTime);
                yield return null;
            }
        }

        // Turn on physics and fire egg out
        // direction is based on last 2 path nodes
        m_rb.isKinematic = false;
        m_rb.AddForce(pushForce, ForceMode.VelocityChange);

        // Wait and then spawn rabboid
        yield return new WaitForSeconds(Random.Range(2.0f, 3.0f));
        m_mama.SpawnRabboid(transform.position, m_results);
        SoundEffectsPlayer.Instance.PlaySound("SpawnBunny");
        Destroy(gameObject);

        yield return null;
    }
}
