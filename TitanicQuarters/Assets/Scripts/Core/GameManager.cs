using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameMode
{
    MAIL,
    TELEGRAPH,
    TYPEWRITER,
    MILITARY,
    NAUTIC
}
public class GameManager : MonoBehaviour
{

    // Machines 
    [SerializeField]
    private Telegraph _telegraph;
    [SerializeField]
    private Mail _mail;

    // Managers
    [SerializeField]
    private InputListener _inputListener;
    [SerializeField]
    private ScoreManager _scoreManager;
    [SerializeField]
    private FrustrationManager _frustrationManager;

    [SerializeField] // TODO : Currently unused and prefer to use the _currentMachine
    GameMode _gameMode;

    public Machine _currentMachine;


  


    // Start is called before the first frame update
    void Start()
    {
        if (_telegraph)
        {
            //_telegraph.OnCorrectWord += _scoreManager.IncrementScore;
            //_telegraph.OnIncorrectWord += _frustrationManager.IncrementFrustration;
            //_telegraph.OnCorrectWord += _frustrationManager.DecrementFrustration;
        }
        ChangeGameMode(_telegraph);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeGameMode(Machine nextMachine)
    {
        if(_currentMachine)
        {
            _currentMachine.isActivated = false; 
        }
        // TODO : add the camera switch
        _currentMachine = nextMachine;
        _currentMachine.isActivated = true;
    }


}
