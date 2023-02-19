using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class DialogueView : MonoBehaviour
{
    [SerializeField] private GrowerWindowView _textbox;
    [SerializeField] private RawImage _portrait;
    [SerializeField] private TMP_Text _textBeingDisplayed;
    [SerializeField] private TMP_Text _speakerNameText;
    [SerializeField] private GameObject _dialogueCompleteIcon;
    [SerializeField] private TMP_Text _speakerText;
    [SerializeField] private TMP_Text _noSpeakerText;
    [SerializeField] private float _timeBetweenLetters;
    [Tooltip("How many letters will be printed between the voice sounds")]
    [SerializeField] public int _lettersBetweenSounds;

    private InputManager _inputManager;
    private Transform _portraitCamera;
    private DialogueData _dialogueData;
    private String[] _sentences;
    private int _index;
    private Coroutine _typingCoroutine;
    private Action _onComplete;

    // Start is called before the first frame update
    void Start()
    {
        List<String>sentencesList = new List<string>();

        foreach (var sentence in _dialogueData.sentences)
        {
            sentencesList.Add(sentence.text);
        }

        _sentences = sentencesList.ToArray();
        
        UpdateDialogueFormat();

        _textBeingDisplayed.text = "";
        _typingCoroutine = StartCoroutine(Type());
    }

    private void OnDestroy()
    {
        UIManager.instance.interactingWithUI = false;
        UIManager.instance.uiState = UIManager.UIStates.Idle;

        _inputManager.OnConfirm -= HandleConfirm;

        _onComplete?.Invoke();
    }

    public void SetDialogueData(DialogueData dialogueData)
    {
        _dialogueData = dialogueData;
    }

    public void SetOnComplete(Action callback)
    {
        _onComplete = callback;
    }

    public void SetInputManager(InputManager inputManager)
    {
        _inputManager = inputManager;

        _inputManager.OnConfirm += HandleConfirm;
    }

    private void HandleConfirm()
    {
        Debug.Log("confirmei");
        NextSentence();
    }

    IEnumerator Type()
    {
        int count = 0;
        _dialogueCompleteIcon.SetActive(false);
        
        foreach (char letter in _sentences[_index])
        {
            _textBeingDisplayed.text += letter;
            count++;

            if (char.IsLetterOrDigit(letter))
            {
                if (count % _lettersBetweenSounds == 0) //if enough letters were printed, play voice sound
                {
                    var speaker = _dialogueData.sentences[_index].speakerData;
                    string speakerVoice = speaker == null || speaker.VoiceAudio.Equals("") ? "DefaultVoice" : speaker.VoiceAudio;

                    AudioManager.instance.Play(speakerVoice);
                }
            }
            else
            {
                if (count == _sentences[_index][_sentences[_index].Length - 1]) //if it's the last letter, no need to make a pause
                    continue;
                
                //PAUSE TYPING
                switch (letter)
                {
                    case '?':
                    case '.':
                    case '!':
                        yield return new WaitForSeconds(_timeBetweenLetters * 7.5f);
                        break;
                    case ',':
                        yield return new WaitForSeconds(_timeBetweenLetters * 2.5f);
                        break;
                }
            }
            yield return new WaitForSeconds(_timeBetweenLetters);
        }
        
        _dialogueCompleteIcon.SetActive(true);
    }

    private void NextSentence()
    {
        if (_textBeingDisplayed.text == _sentences[_index]) //if the text is 100% printed on screen
        {
            if (_index < _sentences.Length - 1) //and if there are still sentences to read
            {
                _index++; //go to the next sentence
                UpdateDialogueFormat();
                _textBeingDisplayed.text = "";
                _typingCoroutine = StartCoroutine(Type());
            }
            else
            {
                CloseDialogueBox();
            }
        }
        else
        {
            _textBeingDisplayed.text = _sentences[_index];
            _dialogueCompleteIcon.SetActive(true);
            StopCoroutine(_typingCoroutine);
        }
    }

    private void UpdateDialogueFormat()
    {
        if (_dialogueData.sentences[_index].speakerData == null) //if there's no speaker
        {
            SpeakerGUISetActive(false);
        }
        else
        {
            SpeakerGUISetActive(true);
            
            //set names, portraits and voices
            _speakerNameText.SetText(_dialogueData.sentences[_index].speakerData.Name);
            StartCoroutine(TakePortraitPhoto());
        }
    }

    private void SpeakerGUISetActive(bool active)
    {
        _portrait.transform.parent.gameObject.SetActive(active);
        _speakerNameText.transform.parent.gameObject.SetActive(active);
        _speakerText.gameObject.SetActive(active);
        
        _noSpeakerText.gameObject.SetActive(!active); //text formated to no speakers

        _textBeingDisplayed = active ? _speakerText : _noSpeakerText; //text being autotyped is set to "Speaker" or "No Speaker"
    }

    IEnumerator TakePortraitPhoto()
    {
        //get portrait camera of the current speaker
        var portraitCams = FindObjectsOfType<Camera>(true).Where(cam => cam.gameObject.CompareTag("PortraitCamera"));

        var speakerPortraitCam =
            portraitCams.First(cam => cam.transform.parent.name.Contains(_dialogueData.sentences[_index].speakerData.Name)).gameObject;
        
        speakerPortraitCam.SetActive(true);

        yield return null;
        
        speakerPortraitCam.SetActive(false);
    }

    private void CloseDialogueBox()
    {
        _textbox.Shrink(() => Destroy(gameObject)); //close dialogue box
    }
}
