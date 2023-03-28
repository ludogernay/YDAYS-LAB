using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : MonoBehaviour
{
    public SO1 so;
    public void Start(){
        Debug.Log(so.win);
    }
    public void FixedUpdate()
    {
        if(so.win){
            Vector2 direction = new Vector2(0,1f);
            transform.Translate(direction * Time.fixedDeltaTime);
            if (transform.position.y>=6f){
                enabled = false;
            }
        }
    }
}
