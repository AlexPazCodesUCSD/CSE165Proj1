using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Controller : MonoBehaviour
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

    [SerializeField] LayerMask layerMask;

    [SerializeField] float rayReach;
    [SerializeField] float zStep;
    [SerializeField] float minReach;
    [SerializeField] float maxReach;
    bool holdingObject = false;
    bool rightTriggerPressed = false;
    bool leftTriggerPressed = false;

    enum CONTROLLER_MODE{
        MOVE = 0,
        ROTATE,
        SCALE
    }
    CONTROLLER_MODE currentMode = CONTROLLER_MODE.MOVE;
    RaycastHit hit;

    // Update is called once per frame
    void Update()
    {
        float _rightTriggerValue = rightTriggerInput.action.ReadValue<float>();
        float _rightGripValue = rightGripInput.action.ReadValue<float>();

        float _leftTriggerValue = leftTriggerInput.action.ReadValue<float>();
        float _leftGripValue = leftGripInput.action.ReadValue<float>();

        if(HasHitSelectableObject()){
            if(_rightTriggerValue > MIN_TRIGGER_PRESS){
                if(!rightTriggerPressed)
                    SelectObject();
            }
        }
        if(_rightTriggerValue <= MIN_TRIGGER_PRESS){
            if(rightTriggerPressed)
                UnselectObject();
        }

        if(_leftTriggerValue > MIN_TRIGGER_PRESS){
            if(!leftTriggerPressed){
                SwitchMode();
                leftTriggerPressed = true;
            }
        }else{
            leftTriggerPressed = false;
        }

        if(_rightGripValue > MIN_GRIP_PRESS){
            IncHoldTrigger();
        }

        if(_leftGripValue > MIN_GRIP_PRESS){
            DecHoldTrigger();
        }

        if(holdingObject){
            DragObject();
        }
    }

    bool HasHitSelectableObject(){
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position, RayCastEndPoint(), UnityEngine.Color.yellow);

        if(Physics.Raycast(transform.position, fwd, out hit, 100,layerMask)){   // If Raycast hits something
            hit.transform.GetComponent<Selectable>().EnterHover();
            if(lastHoveredObject != hit.transform.gameObject && lastHoveredObject != null){
                lastHoveredObject.GetComponent<Selectable>().ExitHover();
            }
            lastHoveredObject = hit.transform.gameObject;
            return true;
        }else{
            if(!rightTriggerPressed){
                if(lastHoveredObject != null){
                    lastHoveredObject.GetComponent<Selectable>().ExitHover();
                }
            }
            return false;
        }
    }

    void SelectObject(){
        rightTriggerPressed = true;
        if(!holdingObject){
            Selectable hitSelectable = hit.transform.GetComponent<Selectable>();
            if(hit.transform.gameObject.tag == "Holdable"){
                rayReach = (hit.point - transform.position).magnitude;
                holdingObject = true;
                heldObject = hit.transform.gameObject;
            }
            hitSelectable.SelectObject(transform.position + RayCastEndPoint(), (int)currentMode);
        }
    }

    void UnselectObject(){
        rightTriggerPressed = false;
        if(holdingObject){
            heldObject.GetComponent<Selectable>().Unselect();
        }
        holdingObject = false;
    }

    void SwitchMode(){
        switch(currentMode){
            case CONTROLLER_MODE.MOVE:
                currentMode = CONTROLLER_MODE.ROTATE;
                modeDisplay.text = "ROTATE";
                if(heldObject != null)
                    heldObject.GetComponent<Selectable>().RotationToggleOn();
                break;
            case CONTROLLER_MODE.ROTATE:
                currentMode = CONTROLLER_MODE.SCALE;
                modeDisplay.text = "SCALE";
                if(heldObject != null)
                    heldObject.GetComponent<Selectable>().RotationToggleOff();
                break;
            case CONTROLLER_MODE.SCALE:
                currentMode = CONTROLLER_MODE.MOVE;
                modeDisplay.text = "MOVE";
                break;
        }
    }

    void IncHoldTrigger(){
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
        }
    }

    void DecHoldTrigger(){
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
        }
    }

    void DragObject(){
        if(currentMode != CONTROLLER_MODE.MOVE)
            return;

        heldObject.GetComponent<Selectable>().Drag(transform.position + RayCastEndPoint());
    }

    Vector3 RayCastEndPoint(){
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        return fwd * rayReach;
    }
}
