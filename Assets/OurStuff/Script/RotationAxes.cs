using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RotationAxes : MonoBehaviour
{
    [SerializeField] float step;
    Axis currentAxis;
    Transform parentTransform;
    enum AXES {
        X,
        Y,
        Z
    };

    AXES activeAxis = AXES.Y;
    // Start is called before the first frame update
    void Start()
    {
        parentTransform = transform.parent.GetComponent<Transform>();
        ToggleOff();
    }

    public void RotateAroundAxis(int direction){
        float delta = direction * step;
        switch(activeAxis){
            case AXES.X:
                parentTransform.Rotate(new Vector3(delta, 0, 0));
            break;
            case AXES.Y:
                parentTransform.Rotate(new Vector3(0, delta, 0));
            break;
            case AXES.Z:
                parentTransform.Rotate(new Vector3(0, 0, delta));
            break;
        }
    }

    public void SetActiveAxis(int axis){
        if(currentAxis != transform.GetChild(axis).GetComponent<Axis>()){
            if(currentAxis != null)
                currentAxis.Unselect();
            currentAxis = transform.GetChild(axis).GetComponent<Axis>();
        }

        switch(axis){
            case 0:
                activeAxis = AXES.X;
            break;
            case 1:
                activeAxis = AXES.Y;
            break;
            case 2:
                activeAxis = AXES.Z;
            break;
        }
    }

    public void ToggleOn(){
        for(int i = 0; i < transform.childCount; i++){
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void ToggleOff(){
        for(int i = 0; i < transform.childCount; i++){
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
