using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Rabboid/BodyPart")]
public class RabboidBodyPart : ScriptableObject {
    public string m_modName;
    public Transform m_bodyPrefab;
}
