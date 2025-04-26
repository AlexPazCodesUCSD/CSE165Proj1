using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axis : Selectable
{
    [SerializeField] int axis;

    protected override void Update(){}

    public override void SetIsKinematic(bool val) {}

    public override void SelectObject(Vector3 point, int mode)
    {
        isSelected = true;
        transform.parent.GetComponent<RotationAxes>().SetActiveAxis(axis);
        // Tell Parent to unselect others
    }
    public override void ExitHover()
    {
        if(!isSelected){
            base.ExitHover();
        }
    }
    
    public override void Unselect(){
        //base.Unselect();
        isSelected = false;
        GetComponent<Renderer>().material.color = initialColor;
    }
}
