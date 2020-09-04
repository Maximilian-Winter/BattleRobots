using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingePart : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rigidbody;

    [SerializeField]
    private Rigidbody hingeRigidbody;

    [SerializeField]
    private RigidbodyIdentifier rigidbodyIdentifier;

    [SerializeField]
    private RigidbodyIdentifier hingeRigidbodyIdentifier;

    [SerializeField]
    private SimpleHingeController simpleHingeController;

    [SerializeField]
    private SimpleHingeControllerAI simpleHingeControllerAI;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEnterTestingMode()
    {
        simpleHingeController.enabled = true;
        rigidbody.isKinematic = false;
        hingeRigidbody.isKinematic = false;
    }

    public void OnEnterTestingModeAI()
    {
        rigidbody.isKinematic = false;
        hingeRigidbody.isKinematic = false;
        simpleHingeControllerAI.enabled = true;
    }

    public void OnIsPlaced(ref int rigidbodyCount)
    {
        rigidbodyIdentifier.Identifier = rigidbodyCount;
        rigidbodyCount++;
        hingeRigidbodyIdentifier.Identifier = rigidbodyCount;
        rigidbodyCount++;
    }
}
