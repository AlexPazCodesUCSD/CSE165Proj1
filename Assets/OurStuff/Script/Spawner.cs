using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject object1;    // 1st type of furniture we wanna spawn
    [SerializeField] GameObject object2;    // 2nd type of furniture we wanna spawn
    [SerializeField] Transform offset;      // The position where the furniture will be spawned
    int furniture = 0;                      // Represents which is the current type of furniture to spawn
    bool isEnabled = true;                  // Set this to false if you don't want any Spawner functionality

    // Update is called once per frame
    void Update()
    {
        if(!isEnabled)
            return;
        if(Input.GetMouseButtonDown(0)){
            SpawnObject(furniture);
        }
        if(Input.GetMouseButtonDown(1)){
            SwitchFurniture();
        }
    }

    public void SpawnObject(int furniture){
        switch(furniture){  // Check which type of furniture was passed down
            case 0: //If it's the first type
                // Create an instance of the first furniture type, at the offset's position, maintaining it's original rotation
                Instantiate(object1, offset.position, Quaternion.identity);
                break;
            case 1: // If it's the second type
                // Do the same, but create an instance of the second furniture type instead.
                Instantiate(object2, offset.position, Quaternion.identity);
                break;
        }
    }

    public void SwitchFurniture(){  // Switch which type of furniture we'll spawn
        furniture = (furniture == 0) ? 1 : 0;
    }

    public void Disable(){  //  Disables Spawner functionality.
        isEnabled = false;
    }
}
