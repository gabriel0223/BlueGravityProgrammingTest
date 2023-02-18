using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class SO_Dialogue : ScriptableObject
{

    [Serializable]
    public class Sentence
    {
        public SO_Speaker speaker;

        [TextArea(4, 20)] 
        public string text;
    }

    public Sentence[] sentences; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}



