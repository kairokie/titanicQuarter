using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mail : Machine
{

    // TODO : mail box

    // Letter picked

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InputDetection();
    }

    protected override void InputDetection() 
    {
        if (!_isActivated) return;
       base.InputDetection();
       if (Input.GetMouseButtonDown(0))
        {
            if (false /* case if mail is picked*/)
            {

            }
            else
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray,out hit)) {

                    if (hit.collider != null)
                    {

                        // Check if mail clicked

                        // else check if machine clicked
                        Machine machineClicked = hit.collider.gameObject.GetComponent<Machine>();
                        if (machineClicked)
                        {
                            // TODO : send mail to machine
                            if (false)
                            {

                            }
                            else {
                                GameManager gameManager = FindObjectOfType<GameManager>();
                                gameManager.ChangeGameMode(machineClicked);
                            }

                           
                        }
                    }
                }

            }
        }
    }
}
