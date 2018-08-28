using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Rabboid : MonoBehaviour {

    [Header("Spawn Body part points")]
    public Transform m_backPoint;
    public Transform m_mouthPoint;

    [Header("Default body parts")]
    public Transform m_defaultBack;
    public Transform m_defaultMouth;

    public MeshRenderer[] m_mainBodyRenderers;

    public RabboidResult RabboidStats
    {
        get; private set;
    }

    public void Initialize(RabboidResult _result)
    {
        RabboidStats = _result;
        Transform newBack = null, newMouth = null;

        // Set back
        if (_result.m_backPart)
            newBack = Instantiate(_result.m_backPart.m_bodyPrefab, m_backPoint.position, m_backPoint.rotation, m_backPoint.parent);
        else if (m_defaultBack)
            newBack = Instantiate(m_defaultBack, m_backPoint.position, m_backPoint.rotation, m_backPoint.parent);

        // Set mouth
        if (_result.m_mouthPart)
            newMouth = Instantiate(_result.m_mouthPart.m_bodyPrefab, m_mouthPoint.position, m_mouthPoint.rotation, m_mouthPoint.parent);
        else if (m_defaultMouth)
            newMouth = Instantiate(m_defaultMouth, m_mouthPoint.position, m_mouthPoint.rotation, m_mouthPoint.parent);

        // Set colour
        if (_result.m_resultColour)
            SetColours(_result.m_resultColour, newBack, newMouth);

        // Set size
        transform.localScale = transform.localScale * _result.m_size;


        // Output name
        Debug.Log("New Rabboid: " + _result.m_name);
    }

    void SetColours(RabboidColour _colours, Transform _newBack, Transform _newMouth)
    {
        Material newBodyMat = null;

        // Set colours - extra features
        foreach (MeshRenderer r in m_mainBodyRenderers)
        {
            newBodyMat = new Material(r.sharedMaterial);
            newBodyMat.color = _colours.m_color;
            r.sharedMaterial = newBodyMat;
        }

        //if (_newBack)
        //{
        //    Renderer[] rendList = _newBack.GetComponentsInChildren<Renderer>();
        //    foreach (Renderer r in rendList)
        //    {
        //        newBodyMat = new Material(r.sharedMaterial);
        //        newBodyMat.color = _colours.m_color;
        //        r.sharedMaterial = newBodyMat;
        //    }
        //}

        //if (_newMouth)
        //{
        //    Renderer[] rendList = _newMouth.GetComponentsInChildren<Renderer>();
        //    foreach (Renderer r in rendList)
        //    {
        //        newBodyMat = new Material(r.sharedMaterial);
        //        newBodyMat.color = _colours.m_color;
        //        r.sharedMaterial = newBodyMat;
        //    }
        //}
    }

    public void GettingShredded(Vector3 _shredderCenter)
    {
        gameObject.layer = 2; // Cannot interact
        GetComponent<Rigidbody>().isKinematic = true;
        StartCoroutine(ShredAnimation(_shredderCenter));
    }

    IEnumerator ShredAnimation(Vector3 _shredderCenter)
    {
        float animationTime = 4;
        Vector3 startPos = transform.position;
        Vector3 endPos = _shredderCenter;

        float timer = 0;

        while (timer < animationTime)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, timer / animationTime);
            transform.rotation = Random.rotation;
            transform.localScale *= (1 - Time.deltaTime/animationTime);
            yield return null;
        }

        yield return null;
    }

}
