using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorePart : MonoBehaviour
{
    [SerializeField]
    private BoxCollider corePartCollider;
    [SerializeField]
    private Rigidbody corePartRigidbody;

    [SerializeField]
    private RobotAIMotionController robotAIMotionController;
    [SerializeField]
    private RobotAIAttackController robotAIAttackController;
    [SerializeField]
    private BoxCollider aiAttackTrigger;

    [SerializeField]
    private Seeker seeker;

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
        robotAIMotionController.enabled = true;
        robotAIAttackController.enabled = true;
        seeker.enabled = true;
        corePartRigidbody.isKinematic = false;
        aiAttackTrigger.enabled = true;
    }

    public void OnIsPlaced()
    {
        corePartCollider.enabled = true;
    }
}
