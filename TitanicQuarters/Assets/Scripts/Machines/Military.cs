using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Military : WordMachine
{

    [SerializeField]
    FMODUnity.StudioEventEmitter _militarySound;

    // Temporary text display
    public TextMeshProUGUI _textDisplay;
    public TextMeshProUGUI _questionTextDisplay;
    public TextMeshProUGUI _feedbackTextDisplay;

    [SerializeField]
    List<Animator> _animators = new List<Animator>();

    //Errors
    private float _errorDelay;

    [SerializeField]
    private float _errorDelayMax = 0.5f;



    override protected void Awake()
    {
        _doMatchToLatin = true;
        _machineLanguage = Alphabets.MILITARY;


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

    void Update()
    {
        if (!GameManager.isPaused )
        {
            InputDetection();
            if (_errorDelay > 0)
            {
                _errorDelay -= Time.deltaTime;
            }
        }
    }

    protected override void InputDetection()
    {
        if (_currentMail)
        {
            for (KeyCode i = KeyCode.A; i <= KeyCode.Z; i++)
            {
                if (Input.GetKeyUp(i))
                {
                    _animators[(int)i - 97].Play("Touche");
                    ReadChar(Langages.intTocharacter((int)i - 97));
                }
            }

            // if enter key is pressed validate the word
            if (Input.GetKeyUp(KeyCode.Return))
            {
                _animators[_animators.Count - 1].Play("Touche");
                SendWord();
            }
        }

        base.InputDetection();
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

    override protected void UpdateDisplay()
    {
        base.UpdateDisplay();
        if (_textDisplay != null)
        {
            _textDisplay.text = _currentText;
        }
        if (_questionTextDisplay != null)
        {
            _questionTextDisplay.text = CurrentMachineWord;
        }
    }

    public void ReadChar(char c)
    {
        if (_currentLetterIndex >= _currentLatinWord.Length)
        {
            Debug.Log("Word is complete");
            typingError();
            return;
        }
        else if (_currentLatinWord.ElementAt(_currentLetterIndex) != c)
        {
            //print typed letter and expected letter
            Debug.Log("Typed: " + c + " Expected: " + _currentLatinWord.ElementAt(_currentLetterIndex));
            typingError();
            return;
        }

        //Correct input
        _militarySound?.Play();
        CorrectLetter();
        UpdateDisplay();
    }

    void CorrectLetter()
    {
        _currentText += CurrentLatinWord.ElementAt(_currentLetterIndex);
        _currentLetterIndex++;
        UpdateDisplay();

        OnCorrectLetter?.Invoke();
    }

    public void typingError()
    {
        OnIncorrectLetter?.Invoke();
        ErrorTimeout();
        _errorSound?.Play();
        Debug.Log("Typing Error");
        UpdateDisplay();
    }


    private void ErrorTimeout()
    {
        _errorDelay = _errorDelayMax;
    }

    public override void Error()
    {
        _textDisplay.color = Color.red;
        _feedbackTextDisplay.text = "";
        _cam.ScreenShake();
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
        _cam.ScreenShake();
    }
}
