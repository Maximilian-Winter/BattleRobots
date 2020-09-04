using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyIdentifier : MonoBehaviour
{
    [SerializeField]
    private int identifier;

    [SerializeField]
    private Transform partTransform;


    public int Identifier { get => identifier; set => identifier = value; }
    public Transform PartTransform { get => partTransform; set => partTransform = value; }
}
