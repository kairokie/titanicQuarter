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
    FMODUnity.StudioEventEmitter _musicSound;

    // DEBUG regular mail spawn
    public int _mailSpawned = 0;

    //Pause menu
    [SerializeField]
    private GameObject menuManager;





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

        if (_currentMachine)
        {
            ChangeGameMode(_currentMachine);
        }
        else
        {
            ChangeGameMode(_mail);
        }
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
        // TODO : add the camera switch
        _currentMachine = nextMachine;
        if (_cameraManager)
        {
            _cameraManager.SwitchCam(MachineToCameraMode());
        }
        if (_currentMachine is Mail)
        {
            Mail mail = _currentMachine as Mail;
            mail.IsActivated = true;
        }
        else
        {
            _currentMachine.gameObject.SetActive(true);
        }
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

    void Pause()
    {
        _paused = true;
        Time.timeScale = 0;
        if(_musicSound)
        {
            _musicSound.SetParameter("MENU SWITCH",1.0f);
        }
        else
        {
            Debug.Log("No music sound");
        }
    }

    void UnPause()
    {
        _paused = false;
        Time.timeScale = 1;
        if (_musicSound)
        {
            _musicSound.SetParameter("MENU SWITCH", 0.0f);
        }
    }






}
