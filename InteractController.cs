using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractController : MonoBehaviour
{
    private void Start()
    {
        InputManager.inputActions.Interact.exit.performed += ExitInteract;
    }

    private void OnDisable()
    {
        InputManager.inputActions.Interact.exit.performed -= ExitInteract;
    }

    public void ExitInteract(InputAction.CallbackContext context)
    {
        InputManager.ActionMapSwitch(InputManager.inputActions.Start);
        Debug.Log("Switching to Start");
    }
}
