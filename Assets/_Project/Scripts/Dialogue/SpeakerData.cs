using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Speaker", menuName = "Speaker")]
public class SpeakerData : ScriptableObject
{
    public String Name;
    public string VoiceAudio;
}
