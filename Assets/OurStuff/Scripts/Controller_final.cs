using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller_final : MonoBehaviour
{
    const float MIN_TRIGGER_PRESS = .8f;
    const float MIN_GRIP_PRESS = .8f;
    [SerializeField] InputActionReference rightTriggerInput;
    [SerializeField] InputActionReference rightGripInput;
    [SerializeField] InputActionReference leftTriggerInput;
    [SerializeField] InputActionReference leftGripInput;

    //InputActionReference triggerInputActionRef;
    [SerializeField] TextMeshPro modeDisplay;
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
    bool usingGaze = false;

    enum CONTROLLER_MODE
    {
        MOVE = 0,
        ROTATE,
        SCALE,
        SPAWN,
        TELEPORT
    }
    CONTROLLER_MODE currentMode = CONTROLLER_MODE.MOVE;
    RaycastHit hit;
    RaycastHit teleportHit;

    [SerializeField] GameObject player;
    [SerializeField] GameObject teleportIndicator;
    [SerializeField] Transform controller;
    [SerializeField] Transform gazeHoldPoint;
    [SerializeField] Transform gazeOrigin;
    bool teleportIsHeld;

    private void Start()
    {
        spawnPoint = transform.GetChild(0).GetComponent<Transform>();
        sp = GetComponent<Spawner>();
    }

    // Update is called once per frame
    void Update()
    {
        
        float _rightTriggerValue = rightTriggerInput.action.ReadValue<float>();
        float _rightGripValue = rightGripInput.action.ReadValue<float>();

        float _leftTriggerValue = leftTriggerInput.action.ReadValue<float>();
        float _leftGripValue = leftGripInput.action.ReadValue<float>();

        if (HasHitSelectableObject())
        {
            usingGaze = false;
            
        }
        else {
            Gaze_HasHitSelectableObject();
            if (!rightTriggerPressed)
                usingGaze = true;
        }

        //Right Trigger
        if (_rightTriggerValue > MIN_TRIGGER_PRESS)
            RightTriggerPress();
        else
            RightTriggerRelease();

        //Left Trigger
        if (_leftTriggerValue > MIN_TRIGGER_PRESS)
            LeftTriggerPress();
        else
            LeftTriggerRelease();

        //Left Grip
        if (_leftGripValue > MIN_GRIP_PRESS)
            LeftGripPress();
        else
            LeftGripRelease();

        //Right Grip
        if (_rightGripValue > MIN_GRIP_PRESS)
            RightGripPress();
        else
            RightGripRelease();

        if (holdingObject)
            DragObject();

        if (teleportIndicator.activeSelf)
        {
            if (teleportIsHeld == true)
            {
                Vector3 fwd = controller.forward;
                Physics.Raycast(controller.position, fwd, out teleportHit, 1000);
                if (teleportHit.transform.gameObject.tag == "plane")
                {
                    teleportIndicator.transform.position = new Vector3(teleportHit.point.x, teleportIndicator.transform.position.y, teleportHit.point.z);

                }
            }
        }
    }

    #region Right Trigger
    void RightTriggerPress()
    {
        switch (currentMode)
        {
            case CONTROLLER_MODE.SPAWN:
                if (!rightTriggerPressed)
                    SelectSpawnedObject(sp.SpawnObject());
                break;
            case CONTROLLER_MODE.TELEPORT:
                if (!rightTriggerPressed)
                {
                    Debug.Log("Button is being held");
                    teleportIndicator.SetActive(true);
                    teleportIsHeld = true;
                    teleportIndicator.transform.rotation = player.transform.rotation;
                }
                break;
            default:
                if (HasHitSelectableObject() || Gaze_HasHitSelectableObject())
                    if (!rightTriggerPressed)
                        SelectObject();
                break;
        }
        rightTriggerPressed = true;

    }

    void RightTriggerRelease()
    {

        if (rightTriggerPressed)
        {
            if (currentMode == CONTROLLER_MODE.SPAWN)
            {
                heldObject.GetComponent<Selectable>().SetIsKinematic(false);
            }
            UnselectObject();
        }
        if (teleportIsHeld)
        {
            Debug.Log("Teleporter clicked");
            Vector3 fwd = controller.forward;
            Debug.DrawRay(controller.position, fwd * 1000, UnityEngine.Color.yellow);
            if (Physics.Raycast(controller.position, fwd, out teleportHit, 1000))
            {
                if (teleportHit.transform.gameObject.tag == "plane")
                {
                    Debug.Log("Teleporter hit");
                    Vector3 position = controller.position;
                    position = new Vector3(teleportHit.point.x, 0, teleportHit.point.z);
                    player.transform.position = position;
                    player.transform.rotation = teleportIndicator.transform.rotation;
                    Debug.Log("Teleporter moved you to: " + position);
                }
            }
            teleportIsHeld = false;
            teleportIndicator.SetActive(false);
            
        }
        rightTriggerPressed = false;
    }
    #endregion

    #region Left Trigger
    void LeftTriggerPress()
    {
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
                else
                {
                    if (!leftGripPressed)
                        sp.SwitchFurniture(0);
                }
                //heldObject.GetComponent<Selectable>().Rotate(1);
                break;
            case CONTROLLER_MODE.TELEPORT:
                if (teleportIsHeld && !leftGripPressed)
                {
                    teleportIndicator.transform.Rotate(0f, 90f, 0f);
                }
                break;
        }
        leftGripPressed = true;
    }

    void LeftGripRelease()
    {
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
                else
                {
                    if (!rightGripPressed)
                        sp.SwitchFurniture(1);
                }
                //heldObject.GetComponent<Selectable>().Rotate(-1);
                break;
        }
        rightGripPressed = true;
    }

    void RightGripRelease()
    {
        rightGripPressed = false;
    }

    bool HasHitSelectableObject()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position, RayCastEndPoint(transform), UnityEngine.Color.yellow);

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

    bool Gaze_HasHitSelectableObject()
    {
        
        Vector3 fwd = gazeOrigin.TransformDirection(Vector3.forward);
        Debug.DrawRay(gazeOrigin.position, RayCastEndPoint(gazeOrigin), UnityEngine.Color.yellow);

        if (Physics.Raycast(gazeOrigin.position, fwd, out hit, 100, layerMask))
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

            hitSelectable.SelectObject(transform.position + RayCastEndPoint(transform), (int)currentMode);
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


    void SetHeldObject(GameObject newHeld)
    {
        if (heldObject == null)
        {
            heldObject = newHeld;
            heldObject.GetComponent<Selectable>().SetIsKinematic(true);
            return;
        }



        if (heldObject == newHeld)
        {
            if (usingGaze)
            {
                heldObject.GetComponent<Selectable>().SetIsKinematic(false);
            }
            else
            {
                heldObject.GetComponent<Selectable>().SetIsKinematic(true);
            }
        }
        else
        {
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
                currentMode = CONTROLLER_MODE.TELEPORT;
                modeDisplay.text = "TELEPORT";
                break;
            case CONTROLLER_MODE.TELEPORT:
                currentMode = CONTROLLER_MODE.MOVE;
                modeDisplay.text = "MOVE";
                break;
        }
    }

    void DragObject()
    {
        Selectable targetSelectable = heldObject.GetComponent<Selectable>();

        if (currentMode == CONTROLLER_MODE.MOVE)
        {
            if (!usingGaze)
                targetSelectable.Drag(transform.position + RayCastEndPoint(transform));
            else {
                targetSelectable.Drag(gazeHoldPoint.position, false);
            }
        }
        else if (currentMode == CONTROLLER_MODE.SPAWN) {
            targetSelectable.Drag(spawnPoint.position);
        }

    }

    Vector3 RayCastEndPoint(Transform transf)
    {
        Vector3 fwd = transf.TransformDirection(Vector3.forward);
        return fwd * rayReach;
    }
}
