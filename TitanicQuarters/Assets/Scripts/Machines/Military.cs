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

    // Start is called before the first frame update
    void Start()
    {
        _doMatchToLatin = true;
        _machineLanguage = Alphabets.MILITARY;
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

    void Update()
    {
        InputDetection();
        if (_textDisplay != null)
        {
            _textDisplay.text = _currentText;
        }
        if (_questionTextDisplay != null)
        {
            _questionTextDisplay.text = CurrentMachineWord;
        }
    }

    protected override void InputDetection()
    {
        base.InputDetection();

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
    }

    void CorrectLetter()
    {
        _currentText += CurrentLatinWord.ElementAt(_currentLetterIndex);
        _currentLetterIndex++;
        // Check for spaces
        
        OnCorrectLetter?.Invoke();
    }

    public void typingError()
    {
        OnIncorrectLetter?.Invoke();
        Debug.Log("Typing Error");
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
