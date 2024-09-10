using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum GameMode
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

    // Managers
    [SerializeField]
    private InputListener _inputListener;
    [SerializeField]
    private ScoreManager _scoreManager;
    [SerializeField]
    private FrustrationManager _frustrationManager;

    [SerializeField]
    GameMode _gameMode;


  


    // Start is called before the first frame update
    void Start()
    {
        if (_telegraph)
        {
            _telegraph.OnCorrectWord += _scoreManager.IncrementScore;
            _telegraph.OnIncorrectWord += _frustrationManager.IncrementFrustration;
            _telegraph.OnCorrectWord += _frustrationManager.DecrementFrustration;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
