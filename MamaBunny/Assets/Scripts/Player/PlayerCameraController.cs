using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour {

    private Transform m_camera;
    public float m_xSensitivity = 2.0f;
    public float m_ySensitivity = 2.0f;

    public bool m_cameraLocked = true;

    private Quaternion m_characterRot;
    private Quaternion m_cameraRot;

    private bool m_toPan;
    private bool m_lockedMode;
    // Where to stare at
    private Transform m_lockModeTarget;
    private Vector3 m_storedPosition;
    private Quaternion m_storedRotation;
    private Vector3 m_basePosition;
    private float m_screenShakeIntensity, m_screenShakeDurationLeft;

    void Start ()
    {
        m_camera = Camera.main.transform;
        m_lockedMode = false;
        UpdateCameraLock();

        //Base camera rotations off initial state of object/main camera
        m_characterRot = transform.localRotation;
        m_cameraRot = m_camera.localRotation;
        m_basePosition = m_camera.localPosition;
        m_screenShakeDurationLeft = 0;
	}
	
	void Update ()
    {
        if (m_lockedMode)
        {
            // Player has no control when game is focused
            FocusOnTarget();
            return;
        }

        if (m_cameraLocked)
        {
            UpdateLookRotaton(); //Only control camera when intended (i.e. not interacting with UI elements).
        }

        UpdateCameraLock();

        //TEST CODE. lock/unlock camera
        //if (Input.GetKeyDown(KeyCode.F1))
        //{
        //    m_cameraLocked = !m_cameraLocked;
        //}
	}

    private void LateUpdate()
    {
        ScreenShakeLogic();
    }

    public void StopShaking()
    {
        m_screenShakeDurationLeft = 0;
        m_camera.localPosition = m_basePosition;
    }

    void ScreenShakeLogic()
    {
        if (m_screenShakeDurationLeft > 0)
        {
            m_screenShakeDurationLeft -= Time.deltaTime;

            if (m_screenShakeDurationLeft > 0)
            {
                m_camera.localPosition = new Vector3(m_basePosition.x + Random.Range(-m_screenShakeIntensity, m_screenShakeIntensity),
                                                    m_basePosition.y + Random.Range(-m_screenShakeIntensity, m_screenShakeIntensity),
                                                    m_basePosition.z + Random.Range(-m_screenShakeIntensity, m_screenShakeIntensity));
            }
            else
            {
                m_camera.localPosition = m_basePosition;
            }
        }
    }

    public void ApplyScreenShake(float _duration, float _intensity)
    {
        m_screenShakeDurationLeft = _duration;
        m_screenShakeIntensity = _intensity;
    }

    void FocusOnTarget()
    {
        if (!m_toPan) // Nothing to look at..
            return;

        // Rotate body towards target
        Vector3 toTarget = m_lockModeTarget.position - transform.position;
        Vector3 dir = new Vector3(toTarget.x, 0, toTarget.z).normalized;
        Quaternion targetDir = Quaternion.LookRotation(dir);
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetDir, 60 * Time.deltaTime);

        // Rotate head to look slightly up
        targetDir = Quaternion.Euler(-10, 0, 0);
        m_camera.localRotation = Quaternion.RotateTowards(m_camera.localRotation, targetDir, 8 * Time.deltaTime);
    }

    void UpdateLookRotaton()
    {

        //Get input
        float yRot = Input.GetAxisRaw("Mouse X") * m_xSensitivity;
        float xRot = Input.GetAxisRaw("Mouse Y") * m_ySensitivity;
        
        //Apply rotation to current look rotation quaternion
        m_characterRot *= Quaternion.Euler(0f, yRot, 0f);
        m_cameraRot *= Quaternion.Euler(-xRot, 0f, 0f);

        //Limit x-axis rotation from 90 to -90
        LimitVerticalRotation();

        //Apply changes
        transform.localRotation = m_characterRot;
        m_camera.localRotation = m_cameraRot;
    }

    void LimitVerticalRotation()
    {
        //Limit x-axis rotation from 90 to -90
        m_cameraRot.x /= m_cameraRot.w;
        m_cameraRot.y /= m_cameraRot.w;
        m_cameraRot.z /= m_cameraRot.w;
        m_cameraRot.w = 1.0f;

        float angle = 2.0f * Mathf.Rad2Deg * Mathf.Atan(m_cameraRot.x);

        angle = Mathf.Clamp(angle, -90, 90);

        m_cameraRot.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angle);
    }

    void UpdateCameraLock()
    {
        //Have to do this every frame incase player tabs out.

        if (m_cameraLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    //Called by FirstPersonController class for raycasts
    public Transform GetCameraTransform()
    {
        return m_camera;
    }

    public void SetLockedMode(Transform _target, bool _panCamera = false)
    {
        m_toPan = _panCamera;
        m_lockedMode = true;
        m_lockModeTarget = _target;

        m_storedPosition = m_camera.localPosition;
        m_storedRotation = m_camera.localRotation;
    }

    public void SetUnlockedMode()
    {
        m_lockedMode = false;
        m_toPan = false;

        m_camera.localPosition = m_storedPosition;
        m_camera.localRotation = m_storedRotation;
    }
}
