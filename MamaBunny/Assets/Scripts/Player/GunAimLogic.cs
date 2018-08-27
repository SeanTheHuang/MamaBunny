using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAimLogic : MonoBehaviour {

    public Vector3 m_aimOffset;
    public float m_aimFOV = 75;
    Vector3 m_startPos, m_aimPos;
    float m_normalFOV;
    Camera m_camera;
    PlayerControl m_player;
    bool m_aiming = false;

    private void Awake()
    {
        m_camera = Camera.main;
        m_normalFOV = m_camera.fieldOfView;
        m_startPos = transform.localPosition;
        m_aimPos = m_startPos + m_aimOffset;
        m_player = m_camera.transform.parent.GetComponent<PlayerControl>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, m_aimPos, 20 * Time.deltaTime);
            m_camera.fieldOfView = Mathf.Lerp(m_camera.fieldOfView, m_aimFOV, 20 * Time.deltaTime);

            if (!m_aiming)
            {
                m_aiming = true;
                m_player.AimMode();
            }
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, m_startPos, 20 * Time.deltaTime);
            m_camera.fieldOfView = Mathf.Lerp(m_camera.fieldOfView, m_normalFOV, 20 * Time.deltaTime);

            if (m_aiming)
            {
                m_aiming = false;
                m_player.NormalMode();
            }
        }
    }


}
