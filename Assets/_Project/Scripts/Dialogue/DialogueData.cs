using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class DialogueData : ScriptableObject
{
    [Serializable]
    public class Sentence
    {
        public SpeakerData speakerData;

        [TextArea(4, 20)] 
        public string text;
    }

    public Sentence[] sentences;
}



