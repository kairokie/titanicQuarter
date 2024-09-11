using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour
{

    private void Awake()
    {
        //gameObject.SetActive(false);
    }
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
            Mail mail = FindObjectOfType<Mail>(true);
            gameManager.ChangeGameMode(mail);
        }
        
    }
}
