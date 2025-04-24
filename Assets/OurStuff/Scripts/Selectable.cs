using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
public class Selectable : MonoBehaviour
{
    protected Renderer renderer;
    [SerializeField] Color selectedColor;
    [SerializeField] float speed;
    [SerializeField] float zStep;
    protected Color initialColor;
    protected bool isSelected = false;
    Vector3 target;
    Vector3 offset;
    RotationAxes ra;
    Scalable sc;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        renderer = GetComponent<Renderer>();
        initialColor = renderer.material.color;
        ra = GetComponentInChildren<RotationAxes>();
        sc = GetComponent<Scalable>();
    }

    // Update is called once per frame
    protected virtual void Update() { }

    public void ChangeScale(int direction){
        sc.ChangeScale(direction);
    }

    public void RotationToggleOn(){
        ra.ToggleOn();
    }

    public void RotationToggleOff(){
        ra.ToggleOff();
    }
    public void Rotate(int direction){
        ra.RotateAroundAxis(direction);
    }
    public void EnterHover(){
        renderer.material.color = selectedColor;
    }

    public virtual void ExitHover(){
        renderer.material.color = initialColor;
    }

    public void Drag(Vector3 point){
        var step = speed * Time.deltaTime; // calculate distance to move

        target = point;
        if (Vector3.Distance(transform.position, target) >= 0.001f)
            transform.position = Vector3.MoveTowards(transform.position, target + offset, step);
    }

    public virtual void SelectObject(Vector3 point, int mode){
        switch(mode){
            case 0:
                offset = gameObject.transform.position - point;
                break;
            case 1:
                ra.ToggleOn();
                break;
        }
        isSelected = true;
    }

    public virtual void Unselect(){
        //ra.ToggleOff();
        isSelected = false;
    }
}
