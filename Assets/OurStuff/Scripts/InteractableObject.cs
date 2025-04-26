using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{

    [SerializeField] GameObject rotationAxis;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "Holdable";
        gameObject.layer = LayerMask.NameToLayer("Selectable");
        GameObject childRotationAxis = Instantiate(rotationAxis, transform.position, transform.rotation);
        childRotationAxis.transform.parent = transform;
        gameObject.AddComponent<Selectable>();
        gameObject.AddComponent<Scalable>();
    }
}
