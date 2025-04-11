using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject object1;
    [SerializeField] GameObject object2;
    [SerializeField] Transform offset;
    int furniture = 0;
    bool isEnabled = true;

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
        switch(furniture){
            case 0:
                Instantiate(object1, offset.position, Quaternion.identity);
                break;
            case 1:
                Instantiate(object2, offset.position, Quaternion.identity);
                break;
        }
    }

    public void SwitchFurniture(){

        furniture = (furniture == 0) ? 1 : 0;
    }

    public void Disable(){
        isEnabled = false;
    }
}
