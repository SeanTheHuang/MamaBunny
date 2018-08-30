using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Rabboid/Colour")]
public class RabboidColour : RabboidModBase
{
    public string m_colourName;
    public Color m_color = Color.white;
    public Color m_checkColor = Color.white;

    public float ColourDifference(Color _compareColor)
    {
        float m_colorValue = m_checkColor.r + m_checkColor.g * 100 + m_checkColor.b * 10000;
        float m_compareValue = _compareColor.r + _compareColor.g * 100 + _compareColor.b * 10000;

        return Mathf.Abs(m_compareValue - m_colorValue);
    }
}
