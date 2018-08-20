using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RabboidResult
{
    public Color m_resultColour;
    public RabboidBodyPart m_mouthPart;
    public RabboidBodyPart m_backPart;
}

public class RabboidCalculator : MonoBehaviour {

    public static RabboidCalculator Instance
    { get; private set; }

    public RabboidColour[] m_possibleColours;
    public RabboidBodyPart[] m_mouthParts;
    public RabboidBodyPart[] m_backParts;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public RabboidResult CalculateRabboid(List<RabboidColour> _colourList, List<RabboidBodyPart> _mouthParts, List<RabboidBodyPart> _backParts)
    {
        RabboidResult result;
        result.m_resultColour = CalculateColor(_colourList);
        result.m_mouthPart = MostFrequentBodyPart(_mouthParts);
        result.m_backPart = MostFrequentBodyPart(_backParts);

        return result;
    }

    Color CalculateColor(List<RabboidColour> _colourList)
    {
        if (_colourList.Count < 1)
            return Color.white;

        // Calculate current color
        Color currentCol = Color.black;
        foreach (RabboidColour rc in _colourList)
            currentCol += rc.m_color;

        // Average things out
        currentCol /= _colourList.Count;

        float closestDiff = _colourList[0].ColourDifference(currentCol);
        RabboidColour currentColour = _colourList[0];

        for (int i = 1; i < _colourList.Count; i++)
        {
            float newDiff = _colourList[i].ColourDifference(currentCol);
            if (newDiff < closestDiff) // Found a colour closer to target!
            {
                closestDiff = newDiff;
                currentColour = _colourList[i];
            }
        }

        return currentColour.m_color;
    }

    RabboidBodyPart MostFrequentBodyPart(List<RabboidBodyPart> _bodyPartList)
    {
        if (_bodyPartList.Count < 1)
            return null;

        // Fill a dictionary
        Dictionary<RabboidBodyPart, uint> bodyDict = new Dictionary<RabboidBodyPart, uint>();
        foreach (RabboidBodyPart rbp in _bodyPartList)
        {
            if (!bodyDict.ContainsKey(rbp)) // New ID
                bodyDict.Add(rbp, 1);
            else
                bodyDict[rbp] = bodyDict[rbp] + 1; // Increment no. of element
        }

        // Find out which element comes up the most often
        uint currentFreq = 0;
        RabboidBodyPart currentPart = null;

        foreach (KeyValuePair<RabboidBodyPart, uint> entry in bodyDict)
        {
            if (entry.Value > currentFreq)
            {
                currentFreq = entry.Value;
                currentPart = entry.Key;
            }
        }

        return currentPart;
    }
}
