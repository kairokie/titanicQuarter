using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class Telegraph : WordMachine
{
    /*
     - Has a list of morse code strings to reproduce
    - Interprets the given inputs
    - Checks if the inputs are correct
    - Gives feedback to the player
     */



    // Temporary text display
    public TextMeshProUGUI _textDisplay;
    public TextMeshProUGUI _questionTextDisplay;
    public TextMeshProUGUI _feedbackTextDisplay;

    // Start is called before the first frame update
    void Start()
    {
        _doMatchToLatin = false;
        _machineLanguage = Alphabets.MORSE;
        Words.Add(new Word("a"));
        Words.Add(new Word("test"));
        Words.Add(new Word("arbre"));
        _currentMachineWord = Words[_currentTestText].GetWord(_machineLanguage);
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
            _textDisplay.text = _currentText;
        }
        if (_questionTextDisplay != null  )
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
        CorrectLetter();
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
        Debug.Log("Typing Error");
    }

    protected override void InputDetection()
    {
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
