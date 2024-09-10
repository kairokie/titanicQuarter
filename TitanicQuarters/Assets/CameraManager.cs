using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using static UnityEngine.Rendering.DebugUI;
using UnityEditor;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject camMenu;
    [SerializeField] private GameObject camGlobal;
    [SerializeField] private GameObject camMorse;
    [SerializeField] private GameObject camMilitaire;
    [SerializeField] private GameObject camNautique;
    private Dictionary<string, GameObject> camList;



    // Start is called before the first frame update
    void Start()
    {
        camList = new Dictionary<string, GameObject>()
        {
            { "menu", camMenu},
            { "global", camGlobal },
            { "morse", camMorse },
            { "militaire", camMilitaire },
            { "nautique", camNautique }
        };

        camList["menu"].SetActive(true); //Global cam 
    }

    public void SwitchCam(string camName)
    {
        foreach (var cam in camList.Values)
        {
            cam.SetActive(false); //switch off the other cams
        }

        camList[camName].SetActive(true); //Activate right cam
    }
}

