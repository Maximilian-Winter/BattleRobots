using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformGizmoManager : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private Canvas uiCanvas;


    [SerializeField]
    private RobotConstructionController robotConstructionController;

    [SerializeField]
    private Transform transformGizmoTransform;

    [SerializeField]
    private LayerMask gizmoLayerMask;

    [SerializeField]
    private bool isHoveringGizmo;

    [SerializeField]
    private bool isDraggingGizmo;

    private Vector3 startPos;
    private Vector3 lastPos;
    private Vector3 delta;

    RaycastHit hitInfo;
    bool hit;

    public bool IsHoveringGizmo { get => isHoveringGizmo; set => isHoveringGizmo = value; }
    public bool IsDraggingGizmo { get => isDraggingGizmo; set => isDraggingGizmo = value; }

    // Start is called before the first frame update
    void Start()
    {
        hitInfo = new RaycastHit();
        hit = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (robotConstructionController.SelectedRobotPartGameObject != null)
        {
            transformGizmoTransform.gameObject.SetActive(true);
            transformGizmoTransform.position = robotConstructionController.SelectedRobotPartGameObject.transform.position;
            transformGizmoTransform.rotation = robotConstructionController.SelectedRobotPartGameObject.transform.rotation;
        }
        else
        {
            transformGizmoTransform.gameObject.SetActive(false);
        }
        
        if(!IsDraggingGizmo)
        {
            hitInfo = new RaycastHit();
            hit = Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hitInfo, 1000.0f, gizmoLayerMask);
            if (hit)
            {
                IsHoveringGizmo = true;
            }
            else
            {
                IsHoveringGizmo = false;
            }

            if (IsHoveringGizmo)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    startPos = Input.mousePosition;
                    lastPos = Input.mousePosition;
                }
                else if (Input.GetMouseButton(0))
                {
                    IsDraggingGizmo = true;
                }
            }
        }
       


        if (Input.GetMouseButtonUp(0))
        {
            IsDraggingGizmo = false;
            //lastPos = Input.mousePosition;
        }

        if(IsDraggingGizmo)
        {
            delta = Input.mousePosition - lastPos;
            Vector3 direction = (Input.mousePosition - startPos).normalized;
            Debug.Log("Direction: " + direction);
            float distanceMoved = Vector3.Dot(delta, direction);

            //float distanceMoved = delta.magnitude;

            //Vector3 differenceBetweenCameraAndTransformGizmo = mainCamera.transform.position - transformGizmoTransform.position;
            //float scaleFactor = ConvertRange(0.0f, 10.0f, 0.0f, 1.0f, differenceBetweenCameraAndTransformGizmo.magnitude);
           
            

            if (hit)
            {
                if (hitInfo.transform.gameObject.tag == "Gizmo")
                {
                    TransformGizmoHandleDirection transformGizmoHandleDirection = hitInfo.collider.gameObject.GetComponent<TransformGizmoHandle>().TransformGizmoHandleDirection;
                    if (transformGizmoHandleDirection == TransformGizmoHandleDirection.XDir)
                    {
                        if (robotConstructionController.SelectedRobotPartGameObject != null)
                        {
                            robotConstructionController.SelectedRobotPartGameObject.transform.position += new Vector3(distanceMoved * Time.deltaTime, 0.0f, 0.0f);
                        }
                    }
                    if (transformGizmoHandleDirection == TransformGizmoHandleDirection.YDir)
                    {
                        if (robotConstructionController.SelectedRobotPartGameObject != null)
                        {
                            robotConstructionController.SelectedRobotPartGameObject.transform.position += new Vector3(0.0f, distanceMoved * Time.deltaTime, 0.0f);
                        }
                    }
                    if (transformGizmoHandleDirection == TransformGizmoHandleDirection.ZDir)
                    {
                        if (robotConstructionController.SelectedRobotPartGameObject != null)
                        {
                            robotConstructionController.SelectedRobotPartGameObject.transform.position += new Vector3(0.0f, 0.0f, distanceMoved * Time.deltaTime);
                        }
                    }

                }


            }
            lastPos = Input.mousePosition;
        }
    }

    public static float ConvertRange(
    float originalStart, float originalEnd, // original range
    float newStart, float newEnd, // desired range
    float value) // value to convert
    {
        double scale = (double)(newEnd - newStart) / (originalEnd - originalStart);
        return (float)(newStart + ((value - originalStart) * scale));
    }
}
