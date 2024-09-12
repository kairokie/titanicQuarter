using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuManager : MonoBehaviour
{
    [SerializeField] private GameObject cameraManager;
    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameObject creditsUI;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject loseUI;

    // Start is called before the first frame update
    void Start()
    {
        menuUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager)
        {
            gameManager.StartGame();
        }
        menuUI.SetActive(false);

    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void Pause()
    {
        pauseUI.SetActive(true);
    }

    public void Unpause()
    {
        pauseUI.SetActive(false);
    }

    public void EndGame()
    {
        loseUI.SetActive(true);
    }
}
