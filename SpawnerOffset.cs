using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerOffset : MonoBehaviour
{

    public static Transform SpawnOffset { get; private set; }
    public float CameraDistance = 5f;
    public static Transform offsetChange;

    public Transform cameraObject;
    // Start is called before the first frame update
    void Awake()
    {
        GameObject offsetGameObject = new GameObject("SpawnerOffset");
        offsetChange = offsetGameObject.transform;
        SpawnOffset = offsetChange;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (cameraObject == null || offsetChange == null) return;

        Vector3 newPosition = cameraObject.position + cameraObject.forward * CameraDistance;
        offsetChange.position = newPosition;
        offsetChange.rotation = Quaternion.LookRotation(cameraObject.forward, Vector3.up);
    }
}
