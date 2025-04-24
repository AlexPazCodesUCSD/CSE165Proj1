using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeManager : MonoBehaviour
{
    SpawnMode spawnMode;    // Reference to the SpawnMode script in the Player prefab
    TravelMode travelMode;  // Reference to the TravelMode script in the Player prefab
    enum Mode{              // Enum containing different modes that we could use
        SPAWN,
        TRAVEL
    }

    Mode currMode = Mode.SPAWN; // Represent which mode the player is currently in (SPAWN by default)
    // Start is called before the first frame update
    void Start()
    {
        /*
        * Here we are defining our references. GetComponent<SpawnMode> means to get the component "SpawnMode" that
        * is attached to the object that owns this script (in our case, the Player object). Same with GetComponent<TravelMode> 
        */
        spawnMode = GetComponent<SpawnMode>();
        travelMode = GetComponent<TravelMode>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){    // This is how you check for key presses. Consult the Unity documentation to see which keycodes correspond to which keys
            Action();
        }
        if(Input.GetKeyDown(KeyCode.RightShift)){
            currMode = (currMode == Mode.SPAWN) ? Mode.TRAVEL : Mode.SPAWN; // If you press RightShift, we swtich the current mode
            //GetComponent<Spawner>().enabled = false;  // This is how you disable scripts and components
        }
    }

    void Action(){
        switch(currMode){ // Check which mode we're in
            case Mode.SPAWN:    // If we're in spawn mode
                spawnMode.Action();     // Call the Action method from the SpawnMode script
                break;
            case Mode.TRAVEL:   // If we're in travel mode
                travelMode.Action();    // Call the Action method from the TravelMode script
                break;
        }
    }
}
