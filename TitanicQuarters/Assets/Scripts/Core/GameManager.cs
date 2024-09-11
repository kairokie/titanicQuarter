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
            //ChangeGameMode(_nautical);
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            ChangeGameMode(_mail);
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
                return CameraMode.MENU ;
        }
    }


    



}
