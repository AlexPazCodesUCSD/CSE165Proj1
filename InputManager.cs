using System.Collections;
using System.Collections.Generic;
using OVR.OpenVR;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static Project1InputActions inputActions;
    //public static event Action<InputActionMap> actionMapEvent;

    void Awake()
    {
        if(inputActions == null)
        {
            inputActions = new Project1InputActions();
        }
    }

    private void Start()
    {
        ActionMapSwitch(inputActions.Start);
    }

    public static void ActionMapSwitch(InputActionMap actionMap)
    {
        if (actionMap.enabled)
        {
            return;
        }

        inputActions.Disable();
        //actionMapEvent?.Invoke(actionMap);
        actionMap.Enable();
    }
}
