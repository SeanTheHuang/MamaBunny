using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Customer/Counter")]
public class CustomerCounter : ScriptableObject
{
    public int m_cusomterCounter;

    private void OnEnable()
    {
        m_cusomterCounter = 4;
    }
}