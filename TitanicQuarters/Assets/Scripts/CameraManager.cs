using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using static UnityEngine.Rendering.DebugUI;
using UnityEditor;


public enum CameraMode
{
    MENU,
    GLOBAL,
    MORSE,
    MILITAIRE,
    NAUTIQUE
}
public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject camMenu;
    [SerializeField] private GameObject camGlobal;
    [SerializeField] private GameObject camMorse;
    [SerializeField] private GameObject camMilitaire;
    [SerializeField] private GameObject camNautique;
    private Dictionary<CameraMode, GameObject> camList;

    [SerializeField] private CinemachineImpulseSource _impulseSource;


    private void Awake()
    {
        camList = new Dictionary<CameraMode, GameObject>()
        {
            { CameraMode.MENU, camMenu},
            { CameraMode.GLOBAL, camGlobal },
            { CameraMode.MORSE, camMorse },
            { CameraMode.MILITAIRE, camMilitaire },
            { CameraMode.NAUTIQUE, camNautique }
        };

        camList[CameraMode.MENU].SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ScreenShake()
    {
        _impulseSource.GenerateImpulse();
    }

    //public void SwitchCam(string camName)
    //{
    //    foreach (var cam in camList.Values)
    //    {
    //        cam.SetActive(false); //switch off the other cams
    //    }

    //    camList[camName].SetActive(true); //Activate right cam
    //}

    public void SwitchCam(CameraMode nextMode)
    {
        if (camList == null)
        {
            Debug.LogError("Camera list is null");  
        }
        foreach (var cam in camList.Values)
        {
            cam.SetActive(false); //switch off the other cams
        }

        camList[nextMode].SetActive(true); //Activate right cam
    }
}

