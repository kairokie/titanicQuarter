using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Mail : Machine
{

    // TODO : mail box
    [SerializeField]
    GameObject _letterPrefab1;

    //Mail spawn variables
    [SerializeField]
    float timeBetweenLetters = 5.0f;
    [SerializeField]
    int _numberOfMails = 0;
    [SerializeField]
    Vector3 _mailOffset = new Vector3(0.1f, 0, 1f);

    [SerializeField]
    float _pickedMailDistanceToCamera = 10.0f;

    // List of all possible words
    List<Word> _AllWords = new List<Word>();
    public List<Word> Words { get => _AllWords; }


    // Input detection
    [SerializeField]
    bool _isActivated = true;
    public bool IsActivated { get => _isActivated; set => _isActivated = value; }

    // Letter picked
    public MailLetter _pickedLetter = null;

    // Coroutines
    Coroutine _mailArrivalCoroutine = null;
    Coroutine _mailMovementCoroutine = null;
    [SerializeField]
    float _timeToFollowMouse = 0.1f;
    [SerializeField]
    private Vector3 _nextMailPosition;
    [SerializeField]
    private Vector3 _oldMailPosition;
    [SerializeField]
    AnimationCurve _mailOvermovementCurve;
    bool _isRunning = true;




    // Start is called before the first frame update
    void Start()
    {
        ReadTextFile();
        _mailArrivalCoroutine = StartCoroutine(LetterArrivalCoroutine(timeBetweenLetters));
    }

    // Update is called once per frame
    void Update()
    {
        InputDetection();
    }

    

    protected override void InputDetection()
    {
        base.InputDetection();
        if(!_isActivated) return;
        if (_pickedLetter)
        {
            MoveMail();
        }
        if (Input.GetMouseButtonDown(0))
        {
            MouseRaycast();
        }
    }

    public void MouseRaycast()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                MailLetter mailClicked = hit.collider.gameObject.GetComponent<MailLetter>();
                WordMachine machineClicked = hit.collider.gameObject.GetComponentInChildren<WordMachine>(true);
                // Check if mail clicked
                if (!_pickedLetter)
                {
                    if (mailClicked)
                    {
                        _pickedLetter = mailClicked;
                        _pickedLetter.PickLetter();
                        return;
                    }
                    //else check if machine clicked and change game mode
                    else if (machineClicked)
                    {
                        GameManager gameManager = FindObjectOfType<GameManager>();
                        gameManager.ChangeGameMode(machineClicked);
                    }
                }
                else
                {
                    // else check if machine clicked
                    if (machineClicked)
                    {
                        TryPutMail(machineClicked, _pickedLetter);
                    }
                    else if (true /* table test */)
                    {
                        Debug.Log("Mail dropped on table");
                        _pickedLetter.DropLetter();
                        _pickedLetter = null;
                    }
                }
            }
        }
    }


    IEnumerator LetterArrivalCoroutine(float timeBetween)
    {
        //if (!_isActivated) ;
        Debug.Log("LetterArrivalCoroutine started");
        while (_isRunning)
        {
            if (_letterPrefab1)
            {
                GameObject letter = Instantiate(_letterPrefab1);
                MailLetter mailLetter = letter.GetComponent<MailLetter>();
                if (mailLetter)
                {
                    mailLetter.Word = _AllWords[Random.Range(0, _AllWords.Count)];
                }
                mailLetter.transform.position = transform.position + _mailOffset * _numberOfMails;
                mailLetter.transform.rotation = Quaternion.Euler(-45f, -90, 0);
                _numberOfMails++;
            }
            else
            {
                Debug.Log("NO LETTER PREFABS");
            }
            yield return new WaitForSeconds(timeBetween);
        }

    }

    private void MoveMail()
    {
        Vector3 proj = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _pickedMailDistanceToCamera)) /*+ Camera.main.transform.forward * 2*/;

        if (proj != _nextMailPosition)
        {
            _nextMailPosition = proj;
            _oldMailPosition = _pickedLetter.transform.position;
            if (_mailMovementCoroutine != null)
            {
                StopCoroutine(_mailMovementCoroutine);
                _mailMovementCoroutine = null;
            }
            _mailMovementCoroutine = StartCoroutine(mailMovementCoroutine());
        }
    }

    void TryPutMail(WordMachine machine, MailLetter mail)
    {
        Debug.Log("TryPutMail in Mail");
        if (machine.TryPutMail(mail))
        {
            
            mail.DropLetter();
            Destroy(mail.gameObject);
            _pickedLetter = null;
        }
    }

    IEnumerator mailMovementCoroutine()
    {
        float time = 0.0f;
        while (time < _timeToFollowMouse)
        {
            time += Time.deltaTime;
            if (_pickedLetter)
            {
                float t = time / _timeToFollowMouse;
                Vector3 movement = Vector3.Lerp(_oldMailPosition, _nextMailPosition, t);
                _pickedLetter.transform.position = movement;
            }
            yield return null;
        }
        StopCoroutine(_mailMovementCoroutine);
        _mailMovementCoroutine = null;
    }

    public static float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    void ReadTextFile()
    {
        string path = "Assets/Resources/WordList.txt";
        StreamReader reader = new StreamReader(path);
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            if (line.All(char.IsLetter))
            {
                _AllWords.Add(new Word(line.ToLower()));
            }
        }
    }
}
