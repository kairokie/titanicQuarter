using System;
using System.Collections.Generic;
using System.Security.Cryptography;
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

    [SerializeField]
    protected MailLetter _currentMail;


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

    // First Add mail to the queue

    public void AddMail2nd(MailLetter mail)
    {
        _mails.Add(mail);
        if (_mailContainer)
        {
            _mailContainer.SetNumberOfMails(_mails.Count);
        }
        _wordCount = _mails.Count;
        _isEmpty = false;
    }
    // Then sometime pop the queue and stock it in the current mail
    void PopMail ()
    {
        if (_mails.Count > 0 && _currentMail == null)
        {
            _currentMail = _mails[0];
            _mails.RemoveAt(0);
            if (_mailContainer)
            {
                _mailContainer.SetNumberOfMails(_mails.Count);
            }
            _currentLatinWord = _currentMail.Word.GetWord(Alphabets.LATIN);
            _currentMachineWord = _currentMail.Word.GetWord(_machineLanguage);
            _wordCount = _mails.Count;
            _isEmpty = _mails.Count == 0;
            UpdateDisplay();
        }
    }
    // Whenever it's finished empty the current mail
    void EmptyMail()
    {
        if (_currentMail != null)
        {
            Destroy(_currentMail.gameObject);
            _currentMail = null;
            UpdateDisplay();
        }
    }
    // Sometime pop the next if possible
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

    void ResetWords()
    {
        _currentLatinWord = "";
        _currentMachineWord = "";
        _currentText = "";
        _currentLetterIndex = 0;
    }

    void CorrectWord()
    {
        _frustrationManager?.DecrementFrustrationWithWordSize(_currentLatinWord.Length);
        _scoreManager?.IncrementScore();

        //RemoveMail();
        EmptyMail();
        ResetWords();     
        ClearDisplay();

        //if (_mails.Count > 0)
        //{
        //    _currentMachineWord = _mails[0].Word.GetWord(_machineLanguage);
        //    _currentLatinWord = _mails[0].Word.GetWord(Alphabets.LATIN);
        //    UpdateDisplay();
        //}
        //Debug.Log("Correct");
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
            AddMail2nd(mail);
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

    override protected void InputDetection()
    {
        base.InputDetection();
        if (_currentMail == null && Input.GetMouseButtonDown(0))
        {
            Debug.Log("InputDetection WM");
            PopMail();
        }
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
