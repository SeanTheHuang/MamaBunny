using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Rabboid/Colour")]
public class RabboidColour : ScriptableObject {
    public string m_colourName;
    public Color m_color = Color.white;

    public float ColourDifference(Color _compareColor)
    {
        float result = Mathf.Abs(m_color.r - _compareColor.r) +
                        Mathf.Abs(m_color.g - _compareColor.g) +
                        Mathf.Abs(m_color.b - _compareColor.b);

        return result;
    }
}
