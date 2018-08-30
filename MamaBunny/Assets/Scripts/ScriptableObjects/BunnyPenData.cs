using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BunnyPen/PenData")]
public class BunnyPenData : ScriptableObject
{

    public bool m_bunnyInside;
    public float m_bunnySize;
    public RabboidColour m_bunnyColour;
    public RabboidBodyPart m_bunnyMouthPart;
    public RabboidBodyPart m_bunnyBackPart;

    private void OnEnable()
    {
        ResetVariables();
    }

    public void ForceSerialization()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }

    public void ResetVariables()
    {
        m_bunnyInside = false;
        m_bunnySize = 0;
        m_bunnyColour = null;
        m_bunnyMouthPart = null;
        m_bunnyBackPart = null;
    }
}
