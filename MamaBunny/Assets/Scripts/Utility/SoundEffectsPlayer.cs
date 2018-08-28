using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AudioGroup
{
    public string m_name;
    public RangedFloat m_volume;
    [MinMaxRange(0, 2)]
    public RangedFloat m_pitch; 
    public AudioClip[] m_audioClips; // Chooses one out of these
}

public class SoundEffectsPlayer : MonoBehaviour {

    public AudioGroup[] m_initalClips;
    Dictionary<string, AudioGroup> m_audioDict;

    public static SoundEffectsPlayer Instance
    { get; private set; }

    AudioSource m_source;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        // Shitty singleton baby
        Instance = this;
        m_source = GetComponent<AudioSource>();

        // Initialize soundDict
        m_audioDict = new Dictionary<string, AudioGroup>();
        foreach (AudioGroup sc in m_initalClips)
        {
            m_audioDict.Add(sc.m_name, sc);
        }
    }

    public void PlaySound(string _name, AudioSource _source = null)
    {
        AudioSource usedSource = _source ? _source : m_source;

        // Get correct audio group
        AudioGroup group = m_audioDict[_name];

        // Randomize clip, volume, and pitch
        int clipIndex = Random.Range(0, group.m_audioClips.Length);
        usedSource.volume = Random.Range(group.m_volume.minValue, group.m_volume.maxValue);
        usedSource.pitch = Random.Range(group.m_volume.minValue, group.m_volume.maxValue);

        // Play
        m_source.PlayOneShot(group.m_audioClips[clipIndex]);
    }
}
