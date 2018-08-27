using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float m_intialSpeed = 60;
    public float m_damage = 10;
    public float m_maxLife = 10;

    public void Initialize(Vector3 _dir)
    {
        GetComponent<Rigidbody>().velocity = _dir * m_intialSpeed;
        Destroy(gameObject, m_maxLife); // Destroy bullet if alive too long
    }

    private void OnTriggerEnter(Collider c)
    {
        GunTarget gt = c.transform.GetComponent<GunTarget>();
        if (gt)
            gt.TakeHit(m_damage);

        Destroy(gameObject);
    }
}
