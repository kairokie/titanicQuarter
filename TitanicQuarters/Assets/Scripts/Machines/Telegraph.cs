using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class Telegraph : Machine
{
    /*
     - Has a list of morse code strings to reproduce
    - Interprets the given inputs
    - Checks if the inputs are correct
    - Gives feedback to the player
     
     
     */
    [SerializeField]
    public List<Word> Words = new List<Word>();
    string _currentMorseWord;

    public string CurrentMorseWord { get => _currentMorseWord; set => _currentMorseWord = value; }
    
    string _currentLatinWord;

    public string CurrentLatinWord { get => _currentLatinWord; set => _currentLatinWord = value; }

    // Reading indexes 

    int _currentTestText = 0;
    int _currentLetterIndex = 0;

    //Current text
    string _currentText = "";

    public string CurrentText { get => _currentText; set => _currentText = value; }

    // Action events
    public Action OnCorrectWord;
    public Action OnIncorrectWord;
    public Action OnCorrectLetter;
    public Action OnIncorrectLetter;

    // Temporary text display
    public TextMeshProUGUI _textDisplay;
    public TextMeshProUGUI _questionTextDisplay;
    public TextMeshProUGUI _feedbackTextDisplay;

    // Start is called before the first frame update
    void Start()
    {
        Words.Add(new Word("a"));
        Words.Add(new Word("test"));
        Words.Add(new Word("arbre"));
        _currentMorseWord = Words[_currentTestText].GetWord(Alphabets.MORSE);
        _currentLatinWord = Words[_currentTestText].GetWord(Alphabets.LATIN);

        OnCorrectWord += CorrectWordDisplay;
        OnCorrectLetter += Correct;
        OnIncorrectWord += ErrorWordDisplay;
        OnIncorrectLetter += Error;
    }

    // Update is called once per frame
    void Update()
    {
        InputDetection();
        if (_textDisplay != null  )
        {
            _textDisplay.text = CurrentText;
        }
        if (_questionTextDisplay != null  )
        {
            _questionTextDisplay.text = CurrentLatinWord;
        }
    }


    public void ReadChar(char c)
    {
        if (_currentLetterIndex >= _currentMorseWord.Length)
        {
            Debug.Log("Word is complete");
            typingError();
            return;
        }
        else if (_currentMorseWord.ElementAt(_currentLetterIndex) != c)
        {
            //print typed letter and expected letter
            Debug.Log("Typed: " + c + " Expected: " + _currentMorseWord.ElementAt(_currentLetterIndex));
            typingError();
            return;
        }

        //Correct input
        CorrectLetter();
    }

    public void SendWord()
    {
        Debug.Log("Word: " + _currentText);
        if (_currentText == _currentMorseWord)
        {
            CorrectWord();
        }
        else
        {
            IncorrectWord();
        }
        ResetWord();
    }

    void CorrectWord()
    {
        _currentTestText++;
        _currentTestText %= Words.Count;
        _currentMorseWord = Words[_currentTestText].GetWord(Alphabets.MORSE);
        _currentLatinWord = Words[_currentTestText].GetWord(Alphabets.LATIN);
        Debug.Log("Correct");
        OnCorrectWord?.Invoke();
    }

    void IncorrectWord()
    {
        Debug.Log("Incorrect");
        OnIncorrectWord?.Invoke();
    }

    void ResetWord()
    {
        _currentText = "";
        _currentLetterIndex = 0;
    }

    void CorrectLetter()
    {
        _currentText += _currentMorseWord.ElementAt(_currentLetterIndex);
        _currentLetterIndex++;
        // Check for spaces
        if (_currentLetterIndex < _currentMorseWord.Length && _currentMorseWord.ElementAt(_currentLetterIndex) == ' ')
        {
            _currentText += " ";
            _currentLetterIndex++;
        }
        OnCorrectLetter?.Invoke();
    }

    public void typingError()
    {
        OnIncorrectLetter?.Invoke();
        Debug.Log("Typing Error");
    }

    protected override void InputDetection()
    {
        if (!_isActivated) return;
        base.InputDetection();
        if (Input.GetMouseButtonDown(0))
        {
            // "Dot " in morse code
            ReadChar('•');
        }
        if (Input.GetMouseButtonDown(1))
        {
            // "Dash" in morse code
            ReadChar('-');
        }

        // if enter key is pressed validate the word
        if (Input.GetKeyUp(KeyCode.Return))
        {
            SendWord();
        }
    }

    void Error()
    {
        _textDisplay.color = Color.red;
        _feedbackTextDisplay.text = "";
    }

    void Correct()
    {
        _textDisplay.color = Color.black;
        _feedbackTextDisplay.text = "";
    }

    void CorrectWordDisplay()
    {
        _feedbackTextDisplay.text = "Correct!";
    }
    void ErrorWordDisplay()
    {
        _feedbackTextDisplay.text = "Incorrect!";
    }


}
