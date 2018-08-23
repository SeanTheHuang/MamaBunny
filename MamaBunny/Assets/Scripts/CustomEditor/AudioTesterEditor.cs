using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AudioTester), true)]
public class AudioTesterEditor : Editor
{
    [SerializeField]
    private AudioSource m_previewer;

    public void OnEnable()
    {
        m_previewer = EditorUtility.CreateGameObjectWithHideFlags("Audio preview", HideFlags.HideAndDontSave, typeof(AudioSource)).GetComponent<AudioSource>();
    }

    public void OnDisable()
    {
        DestroyImmediate(m_previewer.gameObject);
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);
        if (GUILayout.Button("Play Audio"))
        {
            ((AudioTester)target).Play(m_previewer);
        }
        if (GUILayout.Button("Reset values"))
        {
            ((AudioTester)target).Reset();
        }
        EditorGUI.EndDisabledGroup();
    }
}
