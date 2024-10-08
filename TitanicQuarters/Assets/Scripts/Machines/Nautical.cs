using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Nautical : WordMachine
{


    public TextMeshProUGUI _questionTextDisplay;
    public TextMeshProUGUI _feedbackTextDisplay;
    public RectTransform _NauticalSpotCenterPosition;
    [SerializeField]
    private float _distanceBetweenSpots = 25;

    private List<NauticalSpot> _nauticalSpots = new List<NauticalSpot>();
    private List<NauticalFlag> _nauticalFlags = new List<NauticalFlag>();

    [SerializeField]
    private GameObject _nauticalSpotPrefab;
    [SerializeField]
    private GameObject _nauticalFlagPrefab;

    [SerializeField]
    private List<Sprite> _nauticalAlphabet = new List<Sprite>(26);

    private bool _holdingFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        _doMatchToLatin = true;
        _machineLanguage = Alphabets.NAUTIC;
        _mails.Add(CreateLetter("a"));
        _mails.Add(CreateLetter("test"));
        _mails.Add(CreateLetter("arbre"));
        _currentMachineWord = _mails[_currentTestText].Word.GetWord(_machineLanguage);
        _currentLatinWord = _mails[_currentTestText].Word.GetWord(Alphabets.LATIN);

        OnCorrectWord += CorrectWordDisplay;
        OnCorrectWord += ResetNauticalSpots;
        OnCorrectLetter += Correct;
        OnIncorrectWord += ErrorWordDisplay;
        OnIncorrectLetter += Error;

        ResetNauticalSpots();
    }

    // Update is called once per frame
    void Update()
    {
        InputDetection();

        if (_questionTextDisplay != null)
        {
            _questionTextDisplay.text = CurrentLatinWord;
        }
    }

    void ResetNauticalSpots()
    {
        foreach (NauticalSpot nauticalSpot in _nauticalSpots)
        {
            Destroy(nauticalSpot);
        }
        foreach (NauticalFlag nauticalFlag in _nauticalFlags)
        {
            Destroy(nauticalFlag);
        }



        float dist = _nauticalSpotPrefab.GetComponent<RectTransform>().sizeDelta.x + _distanceBetweenSpots;
        Vector3 halfSizeX = new Vector3(dist / 2f, 0, 0) * Mathf.Max(_currentLatinWord.Length - 1, 0);
        Vector3 leftPos = _NauticalSpotCenterPosition.position - halfSizeX;


        for (int i = 0; i < _currentLatinWord.Length; i++)
        {
            Vector3 offset = new Vector3(dist, 0, 0) * i; 
            Instantiate(_nauticalSpotPrefab, leftPos + offset ,Quaternion.identity, _NauticalSpotCenterPosition);
            GameObject flag = Instantiate(_nauticalFlagPrefab, leftPos + offset, Quaternion.identity, _NauticalSpotCenterPosition);
            flag.GetComponent<NauticalFlag>()._flagId = Langages.characterToInt(_currentLatinWord[i]);
            flag.GetComponent<Image>().sprite = _nauticalAlphabet[i];
        }
    }

    protected override void InputDetection()
    {
        base.InputDetection();

        // if enter key is pressed validate the word
        if (Input.GetKeyUp(KeyCode.Return))
        {
            for (int i = 0; i < _nauticalSpots.Count; i++)
            {
                if (_nauticalSpots[i]._attachedFlag)
                {
                    _currentText += Langages.intTocharacter(_nauticalSpots[i]._attachedFlag._flagId);
                }
                else
                {
                    _currentText += " ";
                }
            }

            SendWord();
        }

        /*
        if (_holdingFlag)
        {
            Vector3 proj = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)); // + Camera.main.transform.forward * 2;
            ////Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Camera.main.transform.forward * 2;
            //Debug.Log(proj + " | " + Input.mousePosition);
            //_holdingFlag.transform.position = proj;

            if (proj != _nextMailPosition)
            {
                _nextMailPosition = proj;
                _oldMailPosition = _holdingFlag.transform.position;
                if (_mailMovementCoroutine != null)
                {
                    StopCoroutine(_mailMovementCoroutine);
                    _mailMovementCoroutine = null;
                }
                _mailMovementCoroutine = StartCoroutine(mailMovementCoroutine());
            }

            if (Input.GetMouseButtonDown(0))
            {
                _holdingFlag = null;
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
                            _holdingFlag = mailClicked;
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
        */
    }


    public override void Error()
    {
        _feedbackTextDisplay.text = "";
    }

    public override void Correct()
    {
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
