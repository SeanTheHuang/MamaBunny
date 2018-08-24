using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AudioTester/TesterThing")]
public class AudioTester : ScriptableObject {

    public RangedFloat m_volume;
    [MinMaxRange(0,2)]
    public RangedFloat m_pitch;
    public AudioClip m_audioClip;

    public void Play(AudioSource _source)
    {
        _source.clip = m_audioClip;
        _source.volume = Random.Range(m_volume.minValue, m_volume.maxValue);
        _source.pitch = Random.Range(m_pitch.minValue, m_pitch.maxValue);
        _source.loop = false;
        _source.Play();
    }

    public void Reset()
    {
        m_volume.minValue = m_pitch.minValue = 0.98f;
        m_volume.maxValue = 1.0f;
        m_pitch.maxValue = 1.02f;
    }
}
