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
    Dictionary<Machine, Vector3> cameraPositions;




    // Start is called before the first frame update
    void Start()
    {

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
        Vector3 position;  
        cameraPositions.TryGetValue(_currentMachine,out position);
        Debug.Log("New Vector position " + position);
        Camera.main.transform.position = position;
        _currentMachine.isActivated = true;
    }


    public void readTextFile()
    {
        string path = "Assets/Resources/WordList.txt";
        StreamReader reader = new StreamReader(path);
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            if (line.All(char.IsLetter))
            {
                Debug.Log(line);
                //_textFile.Add(line);
                //Words.Add(new Word(line, Alphabets.LATIN));
            }
        }
    }



}
