using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StartController : MonoBehaviour
{
    [SerializeField] Spawner spawner;
    RaycastHit hit;
    [SerializeField] Transform controller;
    [SerializeField] GameObject player;
    [SerializeField] GameObject teleportIndicator;
    bool isHeld;

    private void Start()
        {
            InputManager.inputActions.Start.spawn.performed += SpawnObject;
            InputManager.inputActions.Start.interact.performed += ToInteractMode;
            InputManager.inputActions.Start.teleport.started += TeleportStarted;
            InputManager.inputActions.Start.teleport.canceled += Teleport;
        }

    private void OnDisable()
    {
        InputManager.inputActions.Start.spawn.performed -= SpawnObject;
        InputManager.inputActions.Start.interact.performed -= ToInteractMode;
        InputManager.inputActions.Start.teleport.started -= TeleportStarted;
        InputManager.inputActions.Start.teleport.canceled -= Teleport;
    }

    private void Update()
    {
        if (teleportIndicator.activeSelf)
        {
            if(isHeld == true)
            {
                Vector3 fwd = controller.forward;
                Physics.Raycast(controller.position, fwd, out hit, 1000);
                if (hit.transform.gameObject.tag == "plane")
                {
                    teleportIndicator.transform.position = new Vector3(hit.point.x, teleportIndicator.transform.position.y, hit.point.z);

                }
            }
        }
        
    }


    public void SpawnObject(InputAction.CallbackContext obj)
    {
        Debug.Log("Spawning Object");
        spawner.SpawnObject();
        
    }

    public void ToInteractMode(InputAction.CallbackContext obj)
    {
        InputManager.ActionMapSwitch(InputManager.inputActions.Interact);
        Debug.Log("Switching to Interact Mode");
    }

    public void TeleportStarted(InputAction.CallbackContext obj)
    {
        Debug.Log("Button is being held");
        teleportIndicator.SetActive(true);
        isHeld = true;
    }

    public void Teleport(InputAction.CallbackContext obj)
    {
        Debug.Log("Teleporter clicked");
       Vector3 fwd = controller.forward;
        Debug.DrawRay(controller.position, fwd * 1000, UnityEngine.Color.yellow);
        if (Physics.Raycast(controller.position, fwd, out hit, 1000))
        {
            if(hit.transform.gameObject.tag == "plane")
            {
                Debug.Log("Teleporter hit");
                Vector3 position = controller.position;
                position = new Vector3(hit.point.x, 0, hit.point.z);
                player.transform.position = position;
                Debug.Log("Teleporter moved you to: " + position);
            }
            
        }
        teleportIndicator.SetActive(false);
        isHeld = false;
    }
}
