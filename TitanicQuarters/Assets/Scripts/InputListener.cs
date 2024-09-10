using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class InputListener : MonoBehaviour
{


    

    // Current GameMode
    private GameMode _gameMode;
    public GameMode currentGameMode { get => _gameMode; set => _gameMode = value; }
    // Telegraph
    public Telegraph _telegraph;
    // Start is called before the first frame update
    void Start()
    {
        if (_telegraph != null)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        MouseInputDetection();
       
    }

    void MouseInputDetection()
    {
        if (_gameMode == GameMode.TELEGRAPH)
        {

        }
       
    }

    void TelegraphInput()
    {
    }

    

}






