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

    public List<MailLetter> _mails = new List<MailLetter>();
    [SerializeField]
    private float _maxWords = 50.0f;
    public float MaxMails { get => _maxWords; set => _maxWords = value; }
    protected string _currentMachineWord;

    protected Alphabets _machineLanguage = Alphabets.NONE;
    protected bool _doMatchToLatin = false;

    public string CurrentMachineWord { get => _currentMachineWord; set => _currentMachineWord = value; }

    protected string _currentLatinWord;

    public string CurrentLatinWord { get => _currentLatinWord; set => _currentLatinWord = value; }


    // Reading indexes 

    protected int _currentLetterIndex = 0;
    protected bool _isEmpty = true;


    //Current text
    protected string _currentText = "";

    // Action events
    public Action OnCorrectWord;
    public Action OnIncorrectWord;
    public Action OnCorrectLetter;
    public Action OnIncorrectLetter;

    [SerializeField]
    private MailContainer _mailContainer;

    //TEST SERIALIZED FIELDS
    [SerializeField]
    float _wordCount = 0;


    protected virtual void Awake()
    {
        ClearDisplay();
        gameObject.SetActive(false);
    }
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
        _frustrationManager?.DecrementFrustrationWithWordSize(_currentLatinWord.Length);
        _scoreManager?.IncrementScore();

        RemoveMail();
        _currentLatinWord = "";
        _currentMachineWord = "";
        _currentText = "";
        _currentLetterIndex = 0;
        ClearDisplay();

        if (_mails.Count > 0)
        {
            _currentMachineWord = _mails[0].Word.GetWord(_machineLanguage);
            _currentLatinWord = _mails[0].Word.GetWord(Alphabets.LATIN);
            UpdateDisplay();
        }
        Debug.Log("Correct");
        OnCorrectWord?.Invoke();
    }

    protected virtual void UpdateDisplay()
    {

    }

    protected virtual void ClearDisplay()
    {

    }

    void RemoveMail()
    {
        _mails.RemoveAt(0);
        _wordCount = _mails.Count;
        if (_mails.Count == 0)
        {
            _isEmpty = true;
        }
        if (_mailContainer)
        {
            _mailContainer.SetNumberOfMails(_mails.Count);
        }
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

    public void AddMail(MailLetter mail)
    {
        _mails.Add(mail);
        if (_mails.Count == 1)
        {
            _currentMachineWord = _mails[0].Word.GetWord(_machineLanguage);
            _currentLatinWord = _mails[0].Word.GetWord(Alphabets.LATIN);
            UpdateDisplay();
        }
        if (_mailContainer)
        {
            _mailContainer.SetNumberOfMails(_mails.Count);
        }
        _wordCount = _mails.Count;
        _isEmpty = false;
    }

    public bool TryPutMail(MailLetter mail)
    {
        if (mail.Machine == this && MaxMails >= _mails.Count)
        {
            AddMail(mail);
            return true;
        }
        else
        {
            Debug.Log("mail.Machine: " + mail.Machine + " | this =  " + this + " MaxWords = " + MaxMails);
        }
        return false;
    }

    protected MailLetter CreateLetter(string word)
    {
        MailLetter mail = gameObject.AddComponent<MailLetter>();
        mail.Word = new Word(word);
        return mail;
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
