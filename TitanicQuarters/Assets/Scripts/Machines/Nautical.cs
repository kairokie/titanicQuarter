using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Nautical : WordMachine
{
    [SerializeField]
    public FMODUnity.StudioEventEmitter _nauticDragSound;
    [SerializeField]
    public FMODUnity.StudioEventEmitter _nauticDropSound;

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

    override protected void Awake()
    {
        _doMatchToLatin = true;
        _machineLanguage = Alphabets.NAUTIC;
        //AddMail(CreateLetter("test"));
        OnCorrectWord += CorrectWordDisplay;
        OnCorrectLetter += Correct;
        OnIncorrectWord += ErrorWordDisplay;
        OnIncorrectLetter += Error;
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {


        //ResetNauticalSpots();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.isPaused )
        {
            InputDetection();
        }
    }

    void ResetNauticalSpots()
    {
        foreach (NauticalSpot nauticalSpot in _nauticalSpots)
        {
            Destroy(nauticalSpot.gameObject);
        }
        _nauticalSpots.Clear();
        foreach (NauticalFlag nauticalFlag in _nauticalFlags)
        {
            Destroy(nauticalFlag.gameObject);
        }
        _nauticalFlags.Clear();

        for (int i = 0; i < _currentLatinWord.Length; i++)
        {
            NauticalSpot spot = Instantiate(_nauticalSpotPrefab, _NauticalSpotCenterPosition).GetComponent<NauticalSpot>();

            spot._distanceBetweenSpots = _distanceBetweenSpots;
            spot._previewLetter.text = _currentLatinWord[i].ToString();
            _nauticalSpots.Add(spot);
        }

        for (int i = 0; i < _currentLatinWord.Length; i++)
        {
            NauticalFlag flag = Instantiate(_nauticalFlagPrefab, _NauticalSpotCenterPosition).GetComponent<NauticalFlag>();
            flag._flagId = Langages.characterToInt(_currentLatinWord[i]);
            flag.GetComponent<Image>().sprite = _nauticalAlphabet[flag._flagId];
            _nauticalFlags.Add(flag);
        }

        _nauticalFlags.Shuffle();

        for (int i = 0; i < _currentLatinWord.Length; i++)
        {
            _nauticalFlags[i].transform.SetParent(_nauticalSpots[i].transform, true);
            _nauticalFlags[i]._attachedSpot = _nauticalSpots[i];
            _nauticalFlags[i]._attachedSpot._attachedFlag = _nauticalFlags[i];
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

    override protected void UpdateDisplay()
    {
        base.UpdateDisplay();
        _questionTextDisplay.text = CurrentLatinWord;
        ResetNauticalSpots();
    }

    override protected void ClearDisplay()
    {
        foreach (NauticalSpot nauticalSpot in _nauticalSpots)
        {
            Destroy(nauticalSpot.gameObject);
        }
        _nauticalSpots.Clear();
        foreach (NauticalFlag nauticalFlag in _nauticalFlags)
        {
            Destroy(nauticalFlag.gameObject);
        }
        _nauticalFlags.Clear();

        _questionTextDisplay.text = "";
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
