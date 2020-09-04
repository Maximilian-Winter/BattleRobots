using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleHingeControllerAI : MonoBehaviour
{
    [SerializeField]
    private HingeJoint hinge;

    private JointMotor motor;

    // Start is called before the first frame update
    void Start()
    {
        motor = hinge.motor;
      
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent.gameObject.transform.parent.GetChild(0).gameObject.GetComponent<RobotAIAttackController>().CloseHinge)
        {
            CloseHinge();
        }
        else 
        {
            OpenHinge();
        }
    }

    private void OpenHinge()
    {
        motor.force = 10000;
        motor.targetVelocity = -1000;
        motor.freeSpin = false;
        hinge.motor = motor;
        hinge.useMotor = true;
    }

    private void CloseHinge()
    {
        motor.force = 10000;
        motor.targetVelocity = 1000;
        motor.freeSpin = false;
        hinge.motor = motor;
        hinge.useMotor = true;
    }
}
