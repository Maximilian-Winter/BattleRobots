using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleHingeController : MonoBehaviour
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
        if (Input.GetKey(KeyCode.Keypad1))
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
        motor.force = 100;
        motor.targetVelocity = -100;
        motor.freeSpin = false;
        hinge.motor = motor;
        hinge.useMotor = true;
    }

    private void CloseHinge()
    {
        motor.force = 100;
        motor.targetVelocity = 100;
        motor.freeSpin = false;
        hinge.motor = motor;
        hinge.useMotor = true;
    }
}
