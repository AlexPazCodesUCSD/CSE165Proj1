using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Scalable : MonoBehaviour
{
    Transform tf;
    [SerializeField] float step;
    [SerializeField] float minScale;
    [SerializeField] float maxScale;
    float currentScale = 1;
    // Start is called before the first frame update
    void Start()
    {
        tf = GetComponent<Transform>();
    }

    public void ChangeScale(int direction){
        float delta = step * direction;
        currentScale = Mathf.Clamp(currentScale + delta, minScale, maxScale);

        tf.localScale = new Vector3(currentScale,currentScale,currentScale);
    }
}
