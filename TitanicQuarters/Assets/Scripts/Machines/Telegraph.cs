using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;


public class Telegraph : WordMachine
{
    /*
     - Has a list of morse code strings to reproduce
    - Interprets the given inputs
    - Checks if the inputs are correct
    - Gives feedback to the player
     */
    [SerializeField]
    FMODUnity.StudioEventEmitter _telegraphSound;


    // Temporary text display
    public TextMeshProUGUI _textDisplay;
    public TextMeshProUGUI _questionTextDisplay;
    public TextMeshProUGUI _feedbackTextDisplay;

    [SerializeField]
    private Animator _animator;

    private float _errorDelay;

    [SerializeField]
    private float _errorDelayMax = 0.5f;

    [SerializeField]
    private GameObject _cam;



    protected override void Awake()
    {
        _doMatchToLatin = false;
        _machineLanguage = Alphabets.MORSE;
        //AddMail(CreateLetter("sst"));


        OnCorrectWord += CorrectWordDisplay;
        OnCorrectLetter += Correct;
        OnIncorrectWord += ErrorWordDisplay;
        OnIncorrectLetter += Error;

        base.Awake();

    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.isPaused)
        {
            InputDetection();
            if (_errorDelay > 0)
            {
                _errorDelay -= Time.deltaTime;
            }
        }
    }

    override protected void UpdateDisplay()
    {
        base.UpdateDisplay();
        if (_textDisplay != null)
        {
            _textDisplay.text = _currentText;
        }
        if (_questionTextDisplay != null)
        {
            _questionTextDisplay.text = CurrentLatinWord;
        }
    }


    public void ReadChar(char c)
    {
        if (_currentLetterIndex >= _currentMachineWord.Length)
        {
            Debug.Log("Word is complete");
            typingError();
            return;
        }
        else if (_currentMachineWord.ElementAt(_currentLetterIndex) != c)
        {
            //print typed letter and expected letter
            Debug.Log("Typed: " + c + " Expected: " + _currentMachineWord.ElementAt(_currentLetterIndex));
            typingError();
            return;
        }

        //Correct input
        if (_telegraphSound)
        {
            _telegraphSound.Play();
        }
        CorrectLetter();
        UpdateDisplay();

    }

    protected override void ClearDisplay()
    {
        if (_textDisplay != null)
        {
            _textDisplay.text = "";
        }
        if (_questionTextDisplay != null)
        {
            _questionTextDisplay.text = "";
        }
    }


    void CorrectLetter()
    {
        _currentText += _currentMachineWord.ElementAt(_currentLetterIndex);
        _currentLetterIndex++;
        // Check for spaces
        if (_currentLetterIndex < _currentMachineWord.Length && _currentMachineWord.ElementAt(_currentLetterIndex) == ' ')
        {
            _currentText += " ";
            _currentLetterIndex++;
        }
        OnCorrectLetter?.Invoke();
    }

    public void typingError()
    {
        OnIncorrectLetter?.Invoke();
        ErrorTimeout();
        
        _errorSound?.Play();
        
        while (_currentLetterIndex > 0)
        {
            if (_currentText.ElementAt(_currentLetterIndex - 1) == ' ')
            {
                break;
            }
            _currentLetterIndex--;
        }
        Debug.Log("old substring " + _currentText);

        _currentText = _currentText.Substring(0, _currentLetterIndex);
        UpdateDisplay();

        Debug.Log("Typing Error " + _currentLetterIndex + " new substring " + _currentText);
    }

    private void ErrorTimeout()
    {
        _errorDelay = _errorDelayMax;
    }

    protected override void InputDetection()
    {
        if (_currentMail && _errorDelay <=0 )
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                // we're not clicking on a UI object, so do your normal movement stuff here

                if (Input.GetMouseButtonDown(0))
                {
                    // "Dot " in morse code
                    ReadChar('•');
                    _animator.Play("Tic");
                }
                if (Input.GetMouseButtonDown(1))
                {
                    // "Dash" in morse code
                    ReadChar('-');
                    _animator.Play("Tic");
                }
            }

            // if enter key is pressed validate the word
            if (Input.GetKeyUp(KeyCode.Return))
            {
                SendWord();
            }
        }
        base.InputDetection();
    }

    public override void Error()
    {
        _textDisplay.color = Color.red;
        _feedbackTextDisplay.text = "";
        _cam.GetComponent<CameraManager>().ScreenShake();
    }

    public override void Correct()
    {
        _textDisplay.color = Color.black;
        _feedbackTextDisplay.text = "";
    }

    public override void CorrectWordDisplay()
    {
        _feedbackTextDisplay.text = "Correct!";
    }
    public override void ErrorWordDisplay()
    {
        _textDisplay.color = Color.red;
        _feedbackTextDisplay.text = "Incorrect!";
        _cam.GetComponent<CameraManager>().ScreenShake();
    }


}
