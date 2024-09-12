using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Military : WordMachine
{
    // Temporary text display
    public TextMeshProUGUI _textDisplay;
    public TextMeshProUGUI _questionTextDisplay;
    public TextMeshProUGUI _feedbackTextDisplay;

    //Errors
    private float _errorDelay;

    [SerializeField]
    private float _errorDelayMax = 0.5f;

    public string s;
    public string s2;


    override protected void Awake()
    {
        _doMatchToLatin = true;
        _machineLanguage = Alphabets.MILITARY;
        //AddMail(CreateLetter("urban"));


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
        InputDetection();
        if (_errorDelay > 0)
        {
            _errorDelay -= Time.deltaTime;
        }
        s = _currentMachineWord;
        s2 = _currentLatinWord;
    }

    protected override void InputDetection()
    {
        base.InputDetection();
        if ( _mails.Count == 0)
        {
            return;
        }

        for (KeyCode i = KeyCode.A; i <= KeyCode.Z; i++)
        {
            if (Input.GetKeyUp(i))
            {
                //print(i);
                //print((int)i);
                //print(Langages.intTocharacter((int)i));
                ReadChar(Langages.intTocharacter((int)i-97));
            }
        }

        // if enter key is pressed validate the word
        if (Input.GetKeyUp(KeyCode.Return))
        {
            SendWord();
        }
    }

    protected override void ClearDisplay()
    {
        Debug.Log("Clear display");
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
        _feedbackTextDisplay.text = "Incorrect!";
    }
}
