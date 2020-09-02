using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformGizmoManager : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private RobotConstructionController robotConstructionController;

    [SerializeField]
    private Transform transformGizmoTransform;

    [SerializeField]
    private LayerMask gizmoLayerMask;

    private RaycastHit hitInfo;
    private GameObject target;
    private Vector3 screenSpace;
    private Vector3 offset;

    bool hit;

    [SerializeField]
    private bool isHoveringGizmo;

    [SerializeField]
    private bool isDraggingGizmo;

    public bool IsHoveringGizmo { get => isHoveringGizmo; set => isHoveringGizmo = value; }
    public bool IsDraggingGizmo { get => isDraggingGizmo; set => isDraggingGizmo = value; }

    // Use this for initialization
    void Start()
    {

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

        if (!IsDraggingGizmo)
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


                    target = GetClickedObject(out hitInfo);
                    if (target != null)
                    {
                        screenSpace = Camera.main.WorldToScreenPoint(target.transform.position);
                        offset = target.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
                    }
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
        }

        if (IsDraggingGizmo)
        {
            Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;

            if (hitInfo.transform.gameObject.tag == "Gizmo")
            {
                TransformGizmoHandleDirection transformGizmoHandleDirection = hitInfo.collider.gameObject.GetComponent<TransformGizmoHandle>().TransformGizmoHandleDirection;
                if (transformGizmoHandleDirection == TransformGizmoHandleDirection.XDir)
                {
                    if (robotConstructionController.SelectedRobotPartGameObject != null)
                    {
                        target.transform.position = new Vector3(curPosition.x, target.transform.position.y, target.transform.position.z);
                    }
                }
                if (transformGizmoHandleDirection == TransformGizmoHandleDirection.YDir)
                {
                    if (robotConstructionController.SelectedRobotPartGameObject != null)
                    {
                        target.transform.position = new Vector3(target.transform.position.x, curPosition.y, target.transform.position.z);
                    }
                }
                if (transformGizmoHandleDirection == TransformGizmoHandleDirection.ZDir)
                {
                    if (robotConstructionController.SelectedRobotPartGameObject != null)
                    {
                        target.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, curPosition.z);
                    }
                }

            }
        }

    }


    GameObject GetClickedObject(out RaycastHit hit)
    {
        GameObject target = null;
        if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, gizmoLayerMask))
        {
            target = robotConstructionController.SelectedRobotPartGameObject;
        }

        return target;
    }
}
