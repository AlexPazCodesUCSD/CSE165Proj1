using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TriggerTest : MonoBehaviour
{
    [SerializeField] float MIN_PRESS;
    public InputActionReference triggerInputActionRef;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float _triggerValue = triggerInputActionRef.action.ReadValue<float>();
        if(_triggerValue > MIN_PRESS){
            print("TRIGGER PRESSED");
            GetComponent<MeshRenderer>().enabled = false;
        }else{
            GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
