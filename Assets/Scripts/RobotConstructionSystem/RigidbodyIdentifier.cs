using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyIdentifier : MonoBehaviour
{
    [SerializeField]
    int identifier;

    public int Identifier { get => identifier; set => identifier = value; }
}
