using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAIMotionController : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float nextWaypointDistance = 3f;
    [SerializeField]
    private float stopDistance = 1f;

    private Vector3 relativeWaypointPosition = new Vector3(0f, 0f, 0f);
    private float inputSteer = 0.0f;
    private float inputTorque = 0.0f;

    private Seeker seeker;
    private Path path;
    private int currentWaypoint = 0;

    private bool doCrazyDriving = false;

    [SerializeField]
    private bool followTarget;

    public float InputSteer { get => inputSteer; set => inputSteer = value; }
    public float InputTorque { get => inputTorque; set => inputTorque = value; }


    // Start is called before the first frame update
    void Start()
    {
        GameObject targetObject = GameObject.FindGameObjectWithTag("PlayerTarget");
        target = targetObject.transform;
        if (target != null)
        {
            seeker = GetComponent<Seeker>();
            followTarget = true;
            InvokeRepeating("UpdatePath", 0f, 0.5f);
        }
        
    }

    private void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(transform.position, target.position, OnPathComplete);
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void Update()
    {

        InputSteer = 0.0f;
        InputTorque = 0.0f;

        if (!followTarget)
            return;

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget < stopDistance)
            return;

        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        UpdateSteeringAndTorque();

        float distance = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    public void SetFollowTargetOn()
    {
        followTarget = true;
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    public void SetFollowTargetOff()
    {
        followTarget = false;
        CancelInvoke();
    }

    void UpdateSteeringAndTorque()
    {
        if (path.vectorPath.Count > 0)
        {
            relativeWaypointPosition = transform.InverseTransformPoint(path.vectorPath[currentWaypoint]);
        }
        InputSteer = (relativeWaypointPosition.x / relativeWaypointPosition.magnitude);
        
        InputTorque = (relativeWaypointPosition.z / relativeWaypointPosition.magnitude);

        if(InputSteer > 0.99f)
        {
            doCrazyDriving = true;
            Invoke("StopCrazyDriving", 2.0f);
        }

        if (InputSteer < -0.99f)
        {
            doCrazyDriving = true;
            Invoke("StopCrazyDriving", 2.0f);
        }

        if (doCrazyDriving)
        {
            InputSteer *= -1.0f;
            InputTorque = -1.0f;
        }
    }

    void StopCrazyDriving()
    {
        doCrazyDriving = false;
    }
}
