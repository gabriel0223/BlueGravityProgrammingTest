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

    [Space]

    [SerializeField] private float _timeBetweenLetters;
    [Tooltip("Intervals between periods, interrogation and exclamation points")]
    [SerializeField] private float _shortPunctuationPause;
    [Tooltip("Intervals between commas")]
    [SerializeField] private float _longPunctuationPause;
    [Tooltip("How many letters will be printed between the voice sounds")]
    [SerializeField] public int _lettersBetweenSounds;

    private InputManager _inputManager;
    private Transform _portraitCamera;
    private DialogueData _dialogueData;
    private String[] _sentences;
    private string _currentSentence;
    private int _sentenceIndex;
    private Coroutine _typingCoroutine;
    private Action _onComplete;

    void Start()
    {
        Initialize();
    }

    private void OnDestroy()
    {
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

    private void Initialize()
    {
        _sentences = _dialogueData.sentences.Select(sentence => sentence.text).ToArray();
        _currentSentence = _sentences[0];
        
        UpdateDialogueFormat();

        _textBeingDisplayed.text = "";
        _typingCoroutine = StartCoroutine(TypeSentence(_currentSentence));
    }

    private void HandleConfirm()
    {
        bool isSentenceFullyPrinted = _textBeingDisplayed.text == _currentSentence;
        bool isLastSentence = _sentenceIndex >= _sentences.Length - 1;

        if (isSentenceFullyPrinted)
        {
            if (!isLastSentence)
            {
                GoToNextSentence();
            }
            else
            {
                CloseDialogueBox();
            }
        }
        else
        {
            SkipTextType();
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        int lettersTypedCount = 0;

        SpeakerData speaker = _dialogueData.sentences[_sentenceIndex].speakerData;
        string speakerVoice = speaker == null || speaker.VoiceAudio.Equals("")? "DefaultVoice" : speaker.VoiceAudio;

        _dialogueCompleteIcon.SetActive(false);
        
        foreach (char letter in sentence)
        {
            _textBeingDisplayed.text += letter;
            lettersTypedCount++;

            if (char.IsLetterOrDigit(letter))
            {
                if (lettersTypedCount % _lettersBetweenSounds == 0) //if enough letters were printed, play voice sound
                {
                    AudioManager.instance.Play(speakerVoice);
                }
            }
            else
            {
                bool isLastLetter = lettersTypedCount == sentence.Length - 1;

                if (isLastLetter)
                {
                    continue;
                }

                switch (letter)
                {
                    case '?':
                    case '.':
                    case '!':
                        yield return new WaitForSeconds(_timeBetweenLetters * _longPunctuationPause);
                        break;
                    case ',':
                        yield return new WaitForSeconds(_timeBetweenLetters * _shortPunctuationPause);
                        break;
                }
            }
            yield return new WaitForSeconds(_timeBetweenLetters);
        }
        
        _dialogueCompleteIcon.SetActive(true);
    }

    private void GoToNextSentence()
    {
        _sentenceIndex++;

        _currentSentence = _sentences[_sentenceIndex];
        UpdateDialogueFormat();
        _textBeingDisplayed.text = "";
        _typingCoroutine = StartCoroutine(TypeSentence(_currentSentence));
    }

    private void SkipTextType()
    {
        _textBeingDisplayed.text = _currentSentence;
        _dialogueCompleteIcon.SetActive(true);
        StopCoroutine(_typingCoroutine);
    }

    private void UpdateDialogueFormat()
    {
        DialogueData.Sentence currentSentenceData = _dialogueData.sentences[_sentenceIndex];
        bool isThereASpeaker = currentSentenceData.speakerData != null;

        if (isThereASpeaker)
        {
            string speakerName = currentSentenceData.speakerData.Name;
            _speakerNameText.SetText(speakerName);

            SetSpeakerGUIActive(true);
            StartCoroutine(TakePortraitPhoto());
        }
        else
        {
            SetSpeakerGUIActive(false);
        }
    }

    private void SetSpeakerGUIActive(bool value)
    {
        _portrait.transform.parent.gameObject.SetActive(value);
        _speakerNameText.transform.parent.gameObject.SetActive(value);

        _speakerText.gameObject.SetActive(value);
        _noSpeakerText.gameObject.SetActive(!value);

        _textBeingDisplayed = value? _speakerText : _noSpeakerText;
    }

    IEnumerator TakePortraitPhoto()
    {
        SpeakerData currentSpeaker = _dialogueData.sentences[_sentenceIndex].speakerData;

        PortraitCamera portraitCamera = FindObjectsOfType<PortraitCamera>(true)
            .First(cam => cam.SpeakerData.Equals(currentSpeaker));

        portraitCamera.gameObject.SetActive(true);

        yield return null;
        
        portraitCamera.gameObject.SetActive(false);
    }

    private void CloseDialogueBox()
    {
        _textbox.Shrink(() => Destroy(gameObject));
    }
}
