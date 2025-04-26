using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseController : MonoBehaviour
{
    const float MIN_TRIGGER_PRESS = .8f;
    const float MIN_GRIP_PRESS = .8f;
    [SerializeField] InputActionReference rightTriggerInput;
    [SerializeField] InputActionReference rightGripInput;
    [SerializeField] InputActionReference leftTriggerInput;
    [SerializeField] InputActionReference leftGripInput;

    //InputActionReference triggerInputActionRef;
    [SerializeField] TextMeshProUGUI modeDisplay;
    [SerializeField] GameObject lastHoveredObject;
    GameObject heldObject;

    Transform spawnPoint;
    Spawner sp;

    [SerializeField] LayerMask layerMask;

    [SerializeField] float rayReach;
    [SerializeField] float zStep;
    [SerializeField] float minReach;
    [SerializeField] float maxReach;

    bool holdingObject = false;
    bool rightTriggerPressed = false;
    bool leftTriggerPressed = false;
    bool rightGripPressed = false;
    bool leftGripPressed = false;

    enum CONTROLLER_MODE
    {
        MOVE = 0,
        ROTATE,
        SCALE,
        SPAWN
    }
    CONTROLLER_MODE currentMode = CONTROLLER_MODE.MOVE;
    RaycastHit hit;

    private void Start()
    {
        spawnPoint = transform.GetChild(0).GetComponent<Transform>();
        sp = GetComponent<Spawner>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        float _rightTriggerValue = rightTriggerInput.action.ReadValue<float>();
        float _rightGripValue = rightGripInput.action.ReadValue<float>();

        float _leftTriggerValue = leftTriggerInput.action.ReadValue<float>();
        float _leftGripValue = leftGripInput.action.ReadValue<float>();
        */


        HasHitSelectableObject();

        //Right Trigger
        if (Input.GetMouseButtonDown(0))
            RightTriggerPress();

        if (Input.GetMouseButtonUp(0))
            RightTriggerRelease();

        //Left Trigger
        if (Input.GetMouseButtonDown(1))
            LeftTriggerPress();

        if (Input.GetMouseButtonUp(1))
            LeftTriggerRelease();

        //Left Grip
        if (Input.GetKey(KeyCode.Z))
            LeftGripPress();
        else
            LeftGripRelease();

        //Right Grip
        if (Input.GetKey(KeyCode.X))
            RightGripPress();
        else
            RightGripRelease();

        if (holdingObject)
            DragObject();
    }
    
    #region Right Trigger
    void RightTriggerPress() {
        switch (currentMode) {
            case CONTROLLER_MODE.SPAWN:
                if(!rightGripPressed)
                    SelectSpawnedObject(sp.SpawnObject());
                break;
            default:
                if (HasHitSelectableObject())
                    if (!rightTriggerPressed)
                        SelectObject();
                break;
        }
        rightTriggerPressed = true;

    }

    void RightTriggerRelease() {

        if (rightTriggerPressed)
        {
            if (currentMode == CONTROLLER_MODE.SPAWN)
            {
                heldObject.GetComponent<Selectable>().SetIsKinematic(false);
            }
            UnselectObject();
        }
        rightTriggerPressed = false;
    }
    #endregion

    #region Left Trigger
    void LeftTriggerPress() {
        if (!leftTriggerPressed)
        {
            SwitchMode();
        }
        leftTriggerPressed = true;
    }

    void LeftTriggerRelease()
    {
        leftTriggerPressed = false;
    }
    #endregion

    void LeftGripPress()
    {
        switch (currentMode)
        {
            case CONTROLLER_MODE.MOVE:
                rayReach = Mathf.Clamp(rayReach + zStep, minReach, maxReach);
                break;
            case CONTROLLER_MODE.ROTATE:
                heldObject.GetComponent<Selectable>().Rotate(1);
                break;
            case CONTROLLER_MODE.SCALE:
                heldObject.GetComponent<Selectable>().ChangeScale(1);
                break;
            case CONTROLLER_MODE.SPAWN:
                if (holdingObject)
                {
                    heldObject.GetComponent<Selectable>().Rotate(1);
                }
                else {
                    if (!leftGripPressed)
                        sp.SwitchFurniture(0);
                }
                //heldObject.GetComponent<Selectable>().Rotate(1);
                break;
        }
        leftGripPressed = true;
    }

    void LeftGripRelease() { 
        leftGripPressed = false;
    }

    void RightGripPress()
    {
        switch (currentMode)
        {
            case CONTROLLER_MODE.MOVE:
                rayReach = Mathf.Clamp(rayReach - zStep, minReach, maxReach);
                break;
            case CONTROLLER_MODE.ROTATE:
                heldObject.GetComponent<Selectable>().Rotate(-1);
                break;
            case CONTROLLER_MODE.SCALE:
                heldObject.GetComponent<Selectable>().ChangeScale(-1);
                break;
            case CONTROLLER_MODE.SPAWN:
                if (holdingObject)
                {
                    heldObject.GetComponent<Selectable>().Rotate(-1);
                }
                else {
                    if (!rightGripPressed)
                        sp.SwitchFurniture(1);
                }
                //heldObject.GetComponent<Selectable>().Rotate(-1);
                break;
        }
        rightGripPressed = true;
    }

    void RightGripRelease() { 
        rightGripPressed = false;
    }

    bool HasHitSelectableObject()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position, RayCastEndPoint(), UnityEngine.Color.yellow);

        if (Physics.Raycast(transform.position, fwd, out hit, 100, layerMask))
        {   // If Raycast hits something
            hit.transform.GetComponent<Selectable>().EnterHover();
            if (lastHoveredObject != hit.transform.gameObject && lastHoveredObject != null)
            {
                lastHoveredObject.GetComponent<Selectable>().ExitHover();
            }
            lastHoveredObject = hit.transform.gameObject;
            return true;
        }
        else
        {
            if (!rightTriggerPressed)
            {
                if (lastHoveredObject != null)
                {
                    lastHoveredObject.GetComponent<Selectable>().ExitHover();
                }
            }
            return false;
        }
    }

    void SelectObject()
    {
        if (!holdingObject)
        {
            Selectable hitSelectable = hit.transform.GetComponent<Selectable>();
            if (hit.transform.gameObject.tag == "Holdable")
            {
                rayReach = (hit.point - transform.position).magnitude;
                holdingObject = true;
                SetHeldObject(hit.transform.gameObject);
            }

            hitSelectable.SelectObject(transform.position + RayCastEndPoint(), (int)currentMode);
        }
    }

    void UnselectObject()
    {
        if (holdingObject)
            heldObject.GetComponent<Selectable>().Unselect();
        holdingObject = false;
    }

    void SelectSpawnedObject(GameObject spawnedObject)
    {
        holdingObject = true;
        SetHeldObject(spawnedObject);
    }


    void SetHeldObject(GameObject newHeld) {
        if (heldObject == null)
        {
            heldObject = newHeld;
            heldObject.GetComponent<Selectable>().SetIsKinematic(true);
            return;
        }

        if (heldObject == newHeld)
        {
            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            if (rb == null)
                return;
            bool kinValue = rb.isKinematic ? false : true;
            heldObject.GetComponent<Selectable>().SetIsKinematic(kinValue);
        }
        else {
            Selectable sl = heldObject.GetComponent<Selectable>();
            sl.SetIsKinematic(false);
            sl.RotationToggleOff();
            heldObject = newHeld;
            heldObject.GetComponent<Selectable>().SetIsKinematic(true);
        }
    }

    void SwitchMode()
    {
        switch (currentMode)
        {
            case CONTROLLER_MODE.MOVE:
                currentMode = CONTROLLER_MODE.ROTATE;
                modeDisplay.text = "ROTATE";
                if (heldObject != null)
                    heldObject.GetComponent<Selectable>().RotationToggleOn();
                break;
            case CONTROLLER_MODE.ROTATE:
                currentMode = CONTROLLER_MODE.SCALE;
                modeDisplay.text = "SCALE";
                if (heldObject != null)
                    heldObject.GetComponent<Selectable>().RotationToggleOff();
                break;
            case CONTROLLER_MODE.SCALE:
                UnselectObject();
                currentMode = CONTROLLER_MODE.SPAWN;
                modeDisplay.text = "SPAWN";
                break;
            case CONTROLLER_MODE.SPAWN:
                currentMode = CONTROLLER_MODE.MOVE;
                modeDisplay.text = "MOVE";
                break;
        }
    }

    void DragObject()
    {
        if(currentMode == CONTROLLER_MODE.MOVE)
            heldObject.GetComponent<Selectable>().Drag(transform.position + RayCastEndPoint());
        else if(currentMode == CONTROLLER_MODE.SPAWN)
            heldObject.GetComponent<Selectable>().Drag(spawnPoint.position);

    }

    Vector3 RayCastEndPoint()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        return fwd * rayReach;
    }
}
