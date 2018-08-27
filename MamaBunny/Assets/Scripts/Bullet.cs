using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float m_intialSpeed = 60;
    public float m_damage = 10;

    public void Initialize(Vector3 _dir)
    {
        GetComponent<Rigidbody>().velocity = _dir * m_intialSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GunTarget gt = collision.transform.GetComponent<GunTarget>();
        if (gt)
            gt.TakeHit(m_damage);

        Destroy(gameObject);
    }
}
