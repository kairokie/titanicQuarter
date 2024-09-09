using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


public class Telegraph : MonoBehaviour
{
    /*
     - Has a list of morse code strings to reproduce
    - Interprets the given inputs
    - Checks if the inputs are correct
    - Gives feedback to the player
     
     
     */
    [SerializeField]
    public List<Word> Words = new List<Word>();
    public string currentMorseWord;

    // Reading indexes 

    public int _currentTestText = 0;
    public int _currentLetterIndex = 0;

    //Current text
    public string _currentText = "";

    // Action events
    public Action OnCorrectWord;
    public Action OnIncorrectWord;
    public Action OnCorrectLetter;
    public Action OnIncorrectLetter;

    //UI test display
    public TextMeshProUGUI _feedbackTextDisplay;
    // Start is called before the first frame update
    void Start()
    {
        Words.Add(new Word("t", Alphabets.LATIN));
        Words.Add(new Word("test", Alphabets.LATIN));
        Words.Add(new Word("arbre", Alphabets.LATIN));
        currentMorseWord = Words[_currentTestText].GetWord(Alphabets.MORSE);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public string GetCurrentText()
    {
        return _currentText;
    }

    public string GetCurrentMorseWord()
    {
        return currentMorseWord;
    }

    public void ReadChar(char c)
    {
        if (_currentLetterIndex >= currentMorseWord.Length)
        {
            Debug.Log("Word is complete");
            typingError();
            return;
        }
        else if (currentMorseWord.ElementAt(_currentLetterIndex) != c)
        {
            //print typed letter and expected letter
            Debug.Log("Typed: " + c + " Expected: " + currentMorseWord.ElementAt(_currentLetterIndex));
            typingError();
            return;
        }

        //Correct input
        CorrectLetter();
    }

    public void SendWord()
    {
        Debug.Log("Word: " + _currentText);
        if (_currentText == currentMorseWord)
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
        currentMorseWord = Words[_currentTestText].GetWord(Alphabets.MORSE);
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
        _currentText += currentMorseWord.ElementAt(_currentLetterIndex);
        _currentLetterIndex++;
        OnCorrectLetter?.Invoke();
    }

    public void typingError()
    {
        OnIncorrectLetter?.Invoke();
        Debug.Log("Typing Error");
    }


}
