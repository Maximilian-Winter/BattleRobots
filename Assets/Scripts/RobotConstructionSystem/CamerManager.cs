using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTemplateProjects;

public class CamerManager : MonoBehaviour
{

    [SerializeField]
    private RobotConstructionController robotConstructionController;

    [SerializeField]
    private CameraFollow followCamera;

    [SerializeField]
    private SimpleCameraController freeCamera;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableFollowCamera()
    {
        if(robotConstructionController.RobotRootObject != null)
        {
            followCamera.Target = robotConstructionController.RobotRootObject.transform;
        }
        else
        {
            followCamera.Target = null;
        }
        
        freeCamera.enabled = false;
        followCamera.enabled = true;
    }

    public void EnableFreeCamera()
    {
        freeCamera.enabled = true;
        followCamera.enabled = false;
    }
}
