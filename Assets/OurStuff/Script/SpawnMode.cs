using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.enabled == false)
        {
            return;
        }
    }

    public void Action(Enum button){
        if (this.enabled == false)
        {
            return;
        }
        print("SPAWN!!!");
    }
}
