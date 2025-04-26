using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Scalable : MonoBehaviour
{
    Transform tf;
    const float STEP = .1f;
    const float MIN_SCALE = .1f;
    const float MAX_SCALE = 10.0f;
    float currentScale = 1;
    // Start is called before the first frame update
    void Start()
    {
        tf = GetComponent<Transform>();
    }

    public void ChangeScale(int direction){
        float delta = STEP * direction;
        currentScale = Mathf.Clamp(currentScale + delta, MIN_SCALE, MAX_SCALE);

        tf.localScale = new Vector3(currentScale,currentScale,currentScale);
    }
}
