using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TransformGizmoHandleDirection
{
    XDir,
    YDir,
    ZDir
}

public class TransformGizmoHandle : MonoBehaviour
{
    [SerializeField]
    private TransformGizmoHandleDirection transformGizmoHandleDirection;

    public TransformGizmoHandleDirection TransformGizmoHandleDirection { get => transformGizmoHandleDirection; set => transformGizmoHandleDirection = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
