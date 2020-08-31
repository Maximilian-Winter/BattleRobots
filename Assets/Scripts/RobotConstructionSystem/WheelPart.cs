﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelPart : MonoBehaviour
{
    [SerializeField]
    private BoxCollider constructionModeCollider;
    [SerializeField]
    private SimpleWheelController simpleWheelController;
    [SerializeField]
    private Rigidbody rigidbody;
    [SerializeField]
    private EasySuspension easySuspension;
    [SerializeField]
    private WheelCollider wheelCollider;
    [SerializeField]
    private Transform wheelDirectionIndicator;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(simpleWheelController.GetReverseSpinDirection())
        {
            wheelDirectionIndicator.localRotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
        }
        else
        {
            wheelDirectionIndicator.localRotation = Quaternion.Euler(0.0f, 270.0f, 0.0f);
        }
    }

    public void OnEnterTestingMode()
    {
        constructionModeCollider.enabled = false;
        wheelDirectionIndicator.gameObject.SetActive(false);
        simpleWheelController.enabled = true;
        rigidbody.isKinematic = false;
        easySuspension.enabled = true;
        wheelCollider.enabled = true;
    }

    public void OnIsPlaced()
    {
        constructionModeCollider.enabled = true;
    }

}