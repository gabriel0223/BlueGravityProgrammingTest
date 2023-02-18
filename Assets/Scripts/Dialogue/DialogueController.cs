using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    
    [HideInInspector] public SO_Dialogue dialogue;
    [HideInInspector] public NPC npcInteracting;
    [HideInInspector] public Interactive objectInteracting;
    [Header("REFERENCES")]
    [SerializeField] private RawImage portrait;
    [SerializeField] private TextMeshProUGUI textDisplay;
    [SerializeField] private TextMeshProUGUI speakerName;
    [SerializeField] private GameObject dialogueCompleteIcon;
    [SerializeField] private TextMeshProUGUI noSpeakerText, speakerText;
    private Transform portraitCamera;
    private PlayerController player;
    private GrowerWindow textbox;

    [Space(10)]
    
    [Header("SETTINGS")]
    private String[] sentences;
    private int index;
    [SerializeField] private float typeSpeed = 0.02f;
    [Tooltip("How many letters will be printed between the voice sounds")]
    public int lettersBetweenVoice;
    private Coroutine typing;
    

    private void Awake()
    {
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player.LockInput(true);
        textbox = GetComponentInChildren<GrowerWindow>();

        if (npcInteracting != null) 
            npcInteracting.SetInteractionSign(false);
        
        List<String>sentencesList = new List<string>();

        foreach (var sentence in dialogue.sentences)
        {
            sentencesList.Add(sentence.text);
        }

        sentences = sentencesList.ToArray();
        
        UpdateDialogueFormat();

        textDisplay.text = "";
        typing = StartCoroutine(Type());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            NextSentence();
        }
    }

    private void OnDestroy()
    {
        if (npcInteracting != null) 
            npcInteracting.UpdateInteractionSign();
        
        player.LockInput(false);
        UIManager.instance.interactingWithUI = false;
        UIManager.instance.uiState = UIManager.UIStates.Idle;
    }

    IEnumerator Type()
    {
        int count = 0;
        dialogueCompleteIcon.SetActive(false);
        
        foreach (char letter in sentences[index])
        {
            textDisplay.text += letter;
            count++;

            if (char.IsLetterOrDigit(letter))
            {
                if (count % lettersBetweenVoice == 0) //if enough letters were printed, play voice sound
                {
                    var speaker = dialogue.sentences[index].speaker;
                    string speakerVoice = speaker == null || speaker.voiceAudio.Equals("") ? "DefaultVoice" : speaker.voiceAudio;

                    AudioManager.instance.Play(speakerVoice);
                }
            }
            else
            {
                if (count == sentences[index][sentences[index].Length - 1]) //if it's the last letter, no need to make a pause
                    continue;
                
                //PAUSE TYPING
                switch (letter)
                {
                    case '?':
                    case '.':
                    case '!':
                        yield return new WaitForSeconds(typeSpeed * 7.5f);
                        break;
                    case ',':
                        yield return new WaitForSeconds(typeSpeed * 2.5f);
                        break;
                }
            }
            yield return new WaitForSeconds(typeSpeed);
        }
        
        dialogueCompleteIcon.SetActive(true);
    }

    private void NextSentence()
    {
        if (textDisplay.text == sentences[index]) //if the text is 100% printed on screen
        {
            if (index < sentences.Length - 1) //and if there are still sentences to read
            {
                index++; //go to the next sentence
                UpdateDialogueFormat();
                textDisplay.text = "";
                typing = StartCoroutine(Type());
            }
            else
            {
                CloseDialogueBox();
            }
        }
        else
        {
            textDisplay.text = sentences[index];
            dialogueCompleteIcon.SetActive(true);
            StopCoroutine(typing);
        }
    }

    private void UpdateDialogueFormat()
    {
        if (dialogue.sentences[index].speaker == null) //if there's no speaker
        {
            SpeakerGUISetActive(false);
        }
        else
        {
            SpeakerGUISetActive(true);
            
            //set names, portraits and voices
            speakerName.SetText(dialogue.sentences[index].speaker.name);
            StartCoroutine(TakePortraitPhoto());
        }
    }

    private void SpeakerGUISetActive(bool active)
    {
        portrait.transform.parent.gameObject.SetActive(active);
        speakerName.transform.parent.gameObject.SetActive(active);
        speakerText.gameObject.SetActive(active);
        
        noSpeakerText.gameObject.SetActive(!active); //text formated to no speakers

        textDisplay = active ? speakerText : noSpeakerText; //text being autotyped is set to "Speaker" or "No Speaker"
    }

    IEnumerator TakePortraitPhoto()
    {
        //get portrait camera of the current speaker
        var portraitCams = FindObjectsOfType<Camera>(true).Where(cam => cam.gameObject.CompareTag("PortraitCamera"));

        var speakerPortraitCam =
            portraitCams.First(cam => cam.transform.parent.name.Contains(dialogue.sentences[index].speaker.name)).gameObject;
        
        speakerPortraitCam.SetActive(true);

        yield return null;
        
        speakerPortraitCam.SetActive(false);
    }

    private void CloseDialogueBox()
    {
        if (objectInteracting != null) //if it's the case, call interactive object event before closing
        {
            if (objectInteracting.triggerEvent) 
                objectInteracting.interactionEvent.Invoke();
        }
                
        textbox.Shrink(() => Destroy(gameObject)); //close dialogue box
    }
}
