using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static OVRInput;

public class ModeController : MonoBehaviour
{

    //SpawnMode spawnMode;    // Reference to the SpawnMode script in the Player prefab
    Spawner spawner;
    [SerializeField] GameObject cube;
    [SerializeField] Transform tf;
    enum Mode
    {              // Enum containing different modes that we could use
        START,
        INTERACT,
        MOVE,
        SCALE,
        ROTATE
    }

    private Mode mode;

    private void Start()
    {
        spawner = GetComponent<Spawner>();
        mode = Mode.START;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(cube, tf.position, Quaternion.identity);
        }
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.GetActiveController()))
        {
            Instantiate(cube, tf.position, Quaternion.identity);
        }
        CheckMode();
    }

    // Start is called before the first frame update
    void CheckMode()
    {
        switch (mode) {
            case Mode.START:
                StartMapping();
                break;
            case Mode.INTERACT:
                InteractMapping();
                break;
        }


    }

    void StartMapping()
    {
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.GetActiveController()))
        {
            mode = Mode.INTERACT;
            Debug.Log("Button One pressed — switch mode to interact");
        }
        else if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.GetActiveController()))
        {
            spawner.SpawnObject();
            Debug.Log("Button Two pressed — spawning object");
        }
    }

    void InteractMapping()
    {
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.GetActiveController()))
        {
            mode = Mode.START;
            Debug.Log("Button One pressed — switch mode to start");
        }
    }
}
