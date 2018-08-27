using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float m_damage = 10;
    public float m_initialSpeed = 715;

    public void Initialize(Vector3 _direction)
    {
        GetComponent<Rigidbody>().velocity = _direction * m_initialSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GunTarget target = collision.transform.GetComponent<GunTarget>();

        if (target)
            target.TakeHit(m_damage);

        Destroy(gameObject);
    }
}
