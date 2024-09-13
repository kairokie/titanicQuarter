using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


public enum GameMode
{
    MAIL,
    TELEGRAPH,
    TYPEWRITER,
    MILITARY,
    NAUTIC
}

[RequireComponent(typeof(ScoreManager))]
public class GameManager : MonoBehaviour
{

    // Machines 
    [SerializeField]
    private Telegraph _telegraph;
    [SerializeField]
    private Military _military;
    [SerializeField]
    private Nautical _nautical;
    [SerializeField]
    private Mail _mail;

    // Managers
    private ScoreManager _scoreManager;
    [SerializeField]
    private FrustrationManager _frustrationManager;
    [SerializeField]
    private CameraManager _cameraManager;

    [SerializeField] // TODO : Currently unused and prefer to use the _currentMachine
    GameMode _gameMode;

    public Machine _currentMachine;

    static bool _paused = false;

    public static bool isPaused { get => _paused; }

    [SerializeField]
    public FMODUnity.StudioEventEmitter _musicSound;

    [SerializeField]
     FMODUnity.StudioEventEmitter _buttonSound;

    public void playUIButton()
    {
        if (!_buttonSound)
        {
            Debug.Log("No button sound");
            return;
        }
        _buttonSound?.Play();
    }

    // DEBUG regular mail spawn
    public int _mailSpawned = 0;

    //Pause menu
    [SerializeField]
    private GameObject menuManager;

    Coroutine _musicFade = null;

    IEnumerator MusicFade(bool pause)
    {
        float maxTime = 0.1f;
        float currentTime = 0.0f;
        Debug.Log("Music fade");
        while (currentTime < maxTime)
        {
            currentTime += Time.deltaTime;
            float p1 = 0;
            float p2 = 1;
            if (!pause)
            {
                p1 = 1;
                p2 = 0;
            }
            float t = Mathf.Lerp(p1, p2, currentTime / maxTime);
            Debug.Log("t = " + t);
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("PAUSE",t);
            yield return null;
        }
    }





    // Start is called before the first frame update
    void Start()
    {
        _scoreManager = GetComponent<ScoreManager>();

        _cameraManager = FindObjectOfType<CameraManager>();

        if (_telegraph)
        {
            //_telegraph.OnCorrectWord += _scoreManager.IncrementScore;
            //_telegraph.OnIncorrectWord += _frustrationManager.IncrementFrustration;
            //_telegraph.OnCorrectWord += _frustrationManager.DecrementFrustration;
        }
        _telegraph.gameObject.SetActive(false);
        _military.gameObject.SetActive(false);
        _nautical.gameObject.SetActive(false);
        _mail.gameObject.SetActive(false);

        //if (_currentMachine)
        //{
        //    ChangeGameMode(_currentMachine);
        //}
        //else
        //{
        //    ChangeGameMode(_mail);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        InputDetection();
    }

    public void ChangeGameMode(Machine nextMachine)
    {
        if (_currentMachine)
        {
            if (_currentMachine is Mail)
            {
                Mail mail = _currentMachine as Mail;
                mail.IsActivated = false;
            }
            else
            {
                _currentMachine.gameObject.SetActive(false);
            }
        }
        _currentMachine = nextMachine;
        _musicSound.SetParameter("MENU PAUSE", 0.0f);
        if (_cameraManager)
        {
            
            _cameraManager.SwitchCam(MachineToCameraMode());
        }


        if (_currentMachine is Mail)
        {
            Mail mail = _currentMachine as Mail;
            mail.IsActivated = true;
        }
        _currentMachine.gameObject.SetActive(true);
    }

    private void InputDetection()
    {
        if (!_paused && !_mail._pickedLetter)
        {
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                ChangeGameMode(_telegraph);
            }
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                ChangeGameMode(_military);
            }
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                ChangeGameMode(_nautical);
            }
            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                ChangeGameMode(_mail);
            }
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            GamePauseManager();
        }

    }

    private CameraMode MachineToCameraMode()
    {
        switch (_currentMachine)
        {
            case Telegraph telegraph:
                return CameraMode.MORSE;
            case Military military:
                return CameraMode.MILITAIRE;
            case Nautical nautical:
                return CameraMode.NAUTIQUE;
            case Mail mail:
                return CameraMode.GLOBAL;
            default:
                return CameraMode.MENU;
        }
    }


    void GamePauseManager()
    {
        if (_paused)
        {
            UnPause(); //Remove UI
            if (menuManager != null)
            {
                menuManager.GetComponent<menuManager>().Unpause();
            }
            else
            {
                print("Pause menu UI not found");
            }

        }
        else
        {
            Pause();
            if (menuManager != null) //Apply UI
            {
                menuManager.GetComponent<menuManager>().Pause();
            }
            else
            {
                print("Pause menu UI not found");
            }
        }
    }

    public void Pause()
    {
        _paused = true;
        Time.timeScale = 0;
        if (_musicSound)
        {
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("PAUSE", 1.0f);

            //if (_musicFade == null)
            //    _musicFade = StartCoroutine(MusicFade(true));
        }
        else
        {
            Debug.Log("No music sound");
        }
    }

    public void UnPause()
    {
        _paused = false;
        Time.timeScale = 1;
        if (_musicSound)
        {
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("PAUSE", 0.0f);

            //if (_musicFade == null)
            //    _musicFade = StartCoroutine(MusicFade(false));

        }
    }

    public void StartGame()
    {
        UnPause();
        ChangeGameMode(_mail);
    }
}
