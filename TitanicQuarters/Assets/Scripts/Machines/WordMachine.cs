using System;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class WordMachine : Machine
{
    /*
    - Has a list of Machine code strings to reproduce
    - Interprets the given inputs
    - Checks if the inputs are correct
    - Gives feedback to the player
     */

    [SerializeField]
    FrustrationManager _frustrationManager;

    [SerializeField]
    ScoreManager _scoreManager;
    
    public List<Word> Words = new List<Word>();
    protected string _currentMachineWord;

    protected Alphabets _machineLanguage = Alphabets.NONE;
    protected bool _doMatchToLatin = false;

    public string CurrentMachineWord { get => _currentMachineWord; set => _currentMachineWord = value; }

    protected string _currentLatinWord;

    public string CurrentLatinWord { get => _currentLatinWord; set => _currentLatinWord = value; }


    // Reading indexes 

    protected int _currentTestText = 0;
    protected int _currentLetterIndex = 0;


    //Current text
    protected string _currentText = "";

    // Action events
    public Action OnCorrectWord;
    public Action OnIncorrectWord;
    public Action OnCorrectLetter;
    public Action OnIncorrectLetter;

    public void SendWord()
    {
        Debug.Log("Word: " + _currentText);
        Debug.Log("Expected Word: " + (_doMatchToLatin ? _currentLatinWord : _currentMachineWord));
        if (_currentText == (_doMatchToLatin ? _currentLatinWord : _currentMachineWord))
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
        _frustrationManager.DecrementFrustrationWithWordSize(_currentLatinWord.Length);
        _scoreManager.IncrementScoreWithWordSize(_currentLatinWord.Length);
        print(CurrentLatinWord);
        print(CurrentLatinWord.Length);

        _currentTestText++;
        _currentTestText %= Words.Count;
        _currentMachineWord = Words[_currentTestText].GetWord(_machineLanguage);
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

    public virtual void Error()
    {
        
    }

    public virtual void Correct()
    {
        
    }

    public virtual void CorrectWordDisplay()
    {
        
    }
    public virtual void ErrorWordDisplay()
    {
        
    }
}
