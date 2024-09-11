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


    List<Word> Words = new List<Word>();



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
        if (_pickedLetter)
        {
            Vector3 proj = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _pickedMailDistanceToCamera)) /*+ Camera.main.transform.forward * 2*/;
            ////Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Camera.main.transform.forward * 2;
            //Debug.Log(proj + " | " + Input.mousePosition);
            //_pickedLetter.transform.position = proj;

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

            if (Input.GetMouseButtonDown(0))
            {
                _pickedLetter = null;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider != null)
                    {
                        // Check if mail clicked
                        MailLetter mailClicked = hit.collider.gameObject.GetComponent<MailLetter>();
                        if (mailClicked)
                        {
                            _pickedLetter = mailClicked;
                            // TODO : send mail to machine
                            return;
                        }
                        // else check if machine clicked
                        Machine machineClicked = hit.collider.gameObject.GetComponent<Machine>();
                        if (machineClicked)
                        {
                            // TODO : send mail to machine
                            if (false)
                            {

                            }
                            else
                            {
                                GameManager gameManager = FindObjectOfType<GameManager>();
                                gameManager.ChangeGameMode(machineClicked);
                            }
                        }

                    }
                }
            }
        }

    }


    IEnumerator LetterArrivalCoroutine(float timeBetween)
    {
        //if (!_isActivated) ;
        while (_isRunning)
        {
            if (_letterPrefab1)
            {
                GameObject letter = Instantiate(_letterPrefab1);
                MailLetter mailLetter = letter.GetComponent<MailLetter>();
                if (mailLetter)
                {
                    mailLetter.Word = Words[Random.Range(0, Words.Count)];
                }
                mailLetter.transform.position = transform.position + _mailOffset * _numberOfMails;
                mailLetter.transform.rotation = Quaternion.Euler(-45f, 0, 0);
                _numberOfMails++;
            }
            yield return new WaitForSeconds(timeBetween);
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
                Words.Add(new Word(line.ToLower()));
            }
        }
    }
}
