using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorePart : MonoBehaviour
{
    [SerializeField]
    private BoxCollider corePartCollider;
    [SerializeField]
    private Rigidbody corePartRigidbody;

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
        corePartRigidbody.isKinematic = false;
    }

    public void OnEnterTestingModeAI()
    {
        corePartRigidbody.isKinematic = false;
    }

    public void OnIsPlaced()
    {
        corePartCollider.enabled = true;
    }
}
