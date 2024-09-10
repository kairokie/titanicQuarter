using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour
{

    // Activated 
    protected bool _isActivated = false;
    public bool isActivated { get => _isActivated; set => _isActivated = value; }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void WinMiniGame()
    {

    }

    public void MinigameError()
    {

    }

    protected virtual void InputDetection()
    {
        if (Input.GetKeyUp(KeyCode.Backspace) && !(this is Mail))
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            Mail mail = FindObjectOfType<Mail>();
            gameManager.ChangeGameMode(mail);
        }
        
    }
}
