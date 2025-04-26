using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
public class Selectable : MonoBehaviour
{
    Renderer renderer;
    [SerializeField] Color selectedColor = new Color(.12f,.87f,.80f);
    const float DRAG_SPEED = 9.0f;
    protected Color initialColor;
    protected bool isSelected = false;
    Vector3 target;
    Vector3 offset;
    RotationAxes ra;
    Scalable sc;
    Rigidbody rb;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        renderer = GetComponent<Renderer>();
        initialColor = renderer.material.color;
        rb = GetComponent<Rigidbody>();
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

    public virtual void SetIsKinematic(bool val) {
        if (rb == null)
            return;
        rb.isKinematic = val;
    }

    public void Drag(Vector3 point){
        var step = DRAG_SPEED * Time.deltaTime; // calculate distance to move

        target = point;
        if (Vector3.Distance(transform.position, target) >= 0.001f)
            transform.position = Vector3.MoveTowards(transform.position, target + offset, step);
    }

    public void Drag(Vector3 point, bool useOffset)
    {
        var step = DRAG_SPEED * Time.deltaTime; // calculate distance to move

        target = point;
        if (!useOffset) {
            if (Vector3.Distance(transform.position, target) >= 0.001f)
                transform.position = Vector3.MoveTowards(transform.position, target, step);
        }
        else{
            if (Vector3.Distance(transform.position, target) >= 0.001f)
                transform.position = Vector3.MoveTowards(transform.position, target + offset, step);
        }
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
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if(rb != null)
        {
            rb.useGravity = false;
        }
        isSelected = true;
    }

    public virtual void Unselect(){
        //ra.ToggleOff();
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if(rb != null)
        {
            rb.useGravity = true;
        }
        isSelected = false;
    }
}
