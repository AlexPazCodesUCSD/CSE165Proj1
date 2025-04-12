using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeManager : MonoBehaviour
{
    SpawnMode spawnMode;    // Reference to the SpawnMode script in the Player prefab
    Spawner spawner;
    enum Mode{              // Enum containing different modes that we could use
        SPAWN,
        TRAVEL,
        INTERACT,
        DEFAULT
    }

    Mode currMode = Mode.DEFAULT; // Represent which mode the player is currently in (SPAWN by default)
    // Start is called before the first frame update
    void Start()
    {
        /*
        * Here we are defining our references. GetComponent<SpawnMode> means to get the component "SpawnMode" that
        * is attached to the object that owns this script (in our case, the Player object). Same with GetComponent<TravelMode> 
        */
        spawnMode = GetComponent<SpawnMode>();
        spawner = GetComponent<Spawner>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currMode)
        { // Check which mode we're in
        case Mode.DEFAULT:
                if (Input.GetKeyDown(KeyCode.A))
                {
                    currMode = Mode.SPAWN;
                    print("Entered spawn mode");
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    //currMode = Mode.INTERACT;
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    //currMode = Mode.TRAVEL;
                }
                break;
            case Mode.SPAWN:    // If we're in spawn mode
                if (Input.GetKeyDown(KeyCode.A))
                {
                    currMode = Mode.DEFAULT;
                    print("Entered default mode");
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    spawner.SwitchFurniture();
                    print("toggled other furniture");
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    spawner.SpawnObject();
                    print("spawning object");
                }

                break;
        }
    }
}
