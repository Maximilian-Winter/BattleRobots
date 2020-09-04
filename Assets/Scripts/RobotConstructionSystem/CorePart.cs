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
    private RigidbodyIdentifier rigidbodyIdentifier;


    [SerializeField]
    private RobotAIMotionController robotAIMotionController;
    [SerializeField]
    private RobotAIAttackController robotAIAttackController;
    [SerializeField]
    private BoxCollider aiAttackTrigger;

    [SerializeField]
    private Seeker seeker;

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

    public void OnIsPlaced(ref int rigidbodyCount)
    {
        corePartCollider.enabled = true;

        rigidbodyIdentifier.Identifier = rigidbodyCount;
        rigidbodyCount++;
    }
}
