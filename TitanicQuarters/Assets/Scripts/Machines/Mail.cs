using System;
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
    GameObject _morseLetterPrefab;
    [SerializeField]
    GameObject _nauticLetterPrefab;
    [SerializeField]
    GameObject _militaryLetterPrefab;

    //Mail spawn variables
    [SerializeField]
    float timeBetweenLetters = 5.0f;
    [SerializeField]
    int _numberOfMails = 0;
    [SerializeField]
    int _numberOfMailsByWave = 5;

    [SerializeField]
    Vector3 _mailOffset = new Vector3(0.1f, 0, 1f);

    [SerializeField]
    float _pickedMailDistanceToCamera = 10.0f;

    // List of all possible wordsµ
    [SerializeField]
    List<Word> _AllWords = new List<Word>();
    public List<Word> Words { get => _AllWords; }


    // Input detection
    [SerializeField]
    bool _isActivated = true;
    public bool IsActivated { get => _isActivated; set => _isActivated = value; }

    // Letter picked
    public MailLetter _pickedLetter = null;

    // Mail Spawn transform
    [SerializeField] Transform _spawnCentre;
    [SerializeField] float _spawnAreaLength = 0f;
    [SerializeField] float _spawnAreaWidth = 0f;
    [SerializeField] float _spawnHeight = 2.0f;

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

    [SerializeField]
    protected FMODUnity.StudioEventEmitter _notificationSound;

    //Screen shake
    [SerializeField]
    private GameObject _cam;


    private Coroutine _mailWaveCoroutine;

    protected void Awake()
    {
        gameObject.SetActive(false);
    }


    // Start is called before the first frame update
    void Start()
    {
        ReadTextFile();
        _mailArrivalCoroutine = StartCoroutine(LetterArrivalCoroutine(timeBetweenLetters));
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.isPaused)
        {
            InputDetection();
        }
    }



    protected override void InputDetection()
    {
        base.InputDetection();
        if (_pickedLetter)
        {
            MoveMail();
        }
        if (_isActivated && Input.GetMouseButtonDown(0))
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
                        _pickedLetter.DropLetter();
                        _pickedLetter = null;
                    }
                }
            }
        }
    }


    IEnumerator LetterArrivalCoroutine(float timeBetween)
    {
        while (_isRunning)
        {
            _mailWaveCoroutine = StartCoroutine(MailWave());
            yield return new WaitForSeconds(timeBetween);
        }
    }

    void SpawnMail()
    {
        if (_morseLetterPrefab && _nauticLetterPrefab && _militaryLetterPrefab)
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            int machineIndex = UnityEngine.Random.Range(0, 3);
            Machine _machine = null;
            GameObject _prefab = null;

            if (machineIndex == 0)
            {
                _machine = FindObjectOfType<Telegraph>(true);
                _prefab = _morseLetterPrefab;
            }
            else if (machineIndex == 1)
            {
                _machine = FindObjectOfType<Military>(true);
                _prefab = _militaryLetterPrefab;

            }
            else if (machineIndex == 2)
            {
                _machine = FindObjectOfType<Nautical>(true);
                _prefab = _nauticLetterPrefab;
            }

            GameObject letter = Instantiate(_prefab);
            MailLetter mailLetter = letter.GetComponent<MailLetter>();
            if (mailLetter)
            {
                mailLetter.Word = _AllWords[UnityEngine.Random.Range(0, _AllWords.Count)];
                mailLetter.Machine = _machine;
            }
            float xOffset = UnityEngine.Random.Range(-_spawnAreaLength, _spawnAreaLength);
            float zOffset = UnityEngine.Random.Range(-_spawnAreaWidth, _spawnAreaWidth);
            float xPos = xOffset + _spawnCentre.position.x;
            float zPos = zOffset + _spawnCentre.position.z;
            float yPos = _spawnHeight;

            float xRot = UnityEngine.Random.Range(-60.0f, 60.0f);
            float zRot = UnityEngine.Random.Range(-60.0f, 60.0f);

            mailLetter.transform.position = new Vector3(xPos, yPos, zPos);
            mailLetter.transform.rotation = Quaternion.Euler(xRot, 90, zRot);
            _numberOfMails++;
        }
        else
        {
            Debug.Log("NO LETTER PREFABS");
        }
    }

    IEnumerator MailWave()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            if (gameManager._currentMachine != this)
            {
                _notificationSound?.Play();
            }
        }
        for (int i = 0; i < _numberOfMailsByWave; i++)
        {
            SpawnMail();
            yield return new WaitForSeconds(0.25f);
        }
    }


    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(_spawnCentre.position, new Vector3(_spawnAreaLength * 2, _spawnHeight, _spawnAreaWidth * 2));
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
        if (machine.TryPutMail(mail))
        {

            mail.DropLetter();
            mail.gameObject.SetActive(false);
            //Destroy(mail.gameObject);
            _pickedLetter = null;
        }
        else
        {
            _cam.GetComponent<CameraManager>().ScreenShake();
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
        string path = Application.dataPath + "/Resources/WordList.txt";
        Debug.Log("path = " + path);
        StreamReader reader = new StreamReader(path);
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            string[] subchains = line.Split(' ');
            if (subchains.Length != 2)
            {
                Debug.LogError("Error in WordList.txt");
                return;
            }
            string stringSub = subchains[0];
            string intSub = subchains[1];
            int importance = -1;
            int.TryParse(intSub, out importance);
            if (stringSub.All(char.IsLetter) && importance != -1 && importance >= 0 && importance < Enum.GetNames(typeof(WordSignificance)).Length)
            {
                _AllWords.Add(new Word(stringSub.ToLower(), (WordSignificance)importance));
            }
        }
    }
}
