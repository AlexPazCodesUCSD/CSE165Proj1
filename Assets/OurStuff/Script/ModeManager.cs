using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeManager : MonoBehaviour
{
    SpawnMode spawnMode;
    TravelMode travelMode;
    enum Mode{
        SPAWN,
        TRAVEL
    }

    Mode currMode = Mode.SPAWN; 
    // Start is called before the first frame update
    void Start()
    {
        spawnMode = GetComponent<SpawnMode>();
        travelMode = GetComponent<TravelMode>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            Action();
        }
        if(Input.GetKeyDown(KeyCode.RightShift)){
            currMode = (currMode == Mode.SPAWN) ? Mode.TRAVEL : Mode.SPAWN;
            //GetComponent<Spawner>().enabled = false; 
        }
    }

    void Action(){
        switch(currMode){
            case Mode.SPAWN:
                spawnMode.Action();
                break;
            case Mode.TRAVEL:
                travelMode.Action();
                break;
        }
    }
}
