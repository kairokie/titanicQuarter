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
    [SerializeField]
    private ScoreManager _scoreManager;
    [SerializeField]
    private FrustrationManager _frustrationManager;

    [SerializeField] // TODO : Currently unused and prefer to use the _currentMachine
    GameMode _gameMode;

    public Machine _currentMachine;
    Dictionary<Machine, Vector3> cameraPositions;




    // Start is called before the first frame update
    void Start()
    {
        _scoreManager = GetComponent<ScoreManager>();

         cameraPositions = new Dictionary<Machine, Vector3>()
    {
        {_telegraph,new Vector3(0,1,-310)},
        {_mail, new Vector3(155,1,-544)},
    };
        if (_telegraph)
        {
            //_telegraph.OnCorrectWord += _scoreManager.IncrementScore;
            //_telegraph.OnIncorrectWord += _frustrationManager.IncrementFrustration;
            //_telegraph.OnCorrectWord += _frustrationManager.DecrementFrustration;
        }
        _mail.gameObject.SetActive(false);
        _telegraph.gameObject.SetActive(false);
        _military.gameObject.SetActive(false);
        //_nautical.gameObject.SetActive(false);

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
        
    }

    public void ChangeGameMode(Machine nextMachine)
    {
        if(_currentMachine)
        {
            _currentMachine.gameObject.SetActive(false);
        }
        // TODO : add the camera switch
        _currentMachine = nextMachine;
        Vector3 position;  
        cameraPositions.TryGetValue(_currentMachine,out position);
        Debug.Log("New Vector position " + position);
        Camera.main.transform.position = position;
        _currentMachine.gameObject.SetActive(true);
    }


    



}
