using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RabboidResult
{
    public string m_name;
    public float m_size;
    public RabboidColour m_resultColour;
    public RabboidBodyPart m_mouthPart;
    public RabboidBodyPart m_backPart;
}

public class RabboidCalculator : MonoBehaviour {

    public static RabboidCalculator Instance
    { get; private set; }

    public static float LARGE_SIZE = 1.3f;
    public static float SMALL_SIZE = 0.7f;

    public RabboidColour[] m_possibleColours;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public RabboidResult CalculateRabboid(List<RabboidColour> _colourList, List<RabboidBodyPart> _mouthParts, List<RabboidBodyPart> _backParts, List<RabboidSizeMod> _sizeMods)
    {
        RabboidResult result;
        result.m_resultColour = CalculateColor(_colourList);
        result.m_mouthPart = MostFrequentBodyPart(_mouthParts);
        result.m_backPart = MostFrequentBodyPart(_backParts);
        result.m_size = CalculateSize(_sizeMods);
        result.m_name = CalculateName(result.m_resultColour, result.m_size, result.m_mouthPart, result.m_backPart);

        return result;
    }

    float CalculateSize(List<RabboidSizeMod> _sizeMods)
    {
        float currentSize = 1;

        foreach (RabboidSizeMod rsm in _sizeMods)
            currentSize += rsm.m_sizeMod;

        return currentSize;
    }

    string CalculateName(RabboidColour _color, float _size, RabboidBodyPart _mouthPart, RabboidBodyPart _backPart)
    {
        // Format = [SIZE] [BACK_MOD] & [MOUTH_MOD] [COLOR]-Rabboid
        string name = "";

        if (_size >= LARGE_SIZE)
            name += "Large ";
        else if (_size <= SMALL_SIZE)
            name += "Small ";

        if (_mouthPart && _backPart)
            name += _backPart.m_modName + " & " + _mouthPart.m_modName + " ";
        else
        {
            if (_mouthPart)
                name += _mouthPart.m_modName + " ";
            if (_backPart)
                name += _backPart.m_modName + " ";
        }

        if (_color)
            name += _color.m_colourName + "-";

        name += "Rabboid";

        return name;
    }

    RabboidColour CalculateColor(List<RabboidColour> _colourList)
    {
        if (_colourList.Count < 1)
            return null;

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

        return currentColour;
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
