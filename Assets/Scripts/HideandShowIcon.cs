using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideandShowIcon : MonoBehaviour
{
    public Unit RecupFire;
    public GameObject Icon ;

    // Start is called before the first frame update
    void Start()
    {
        //cache l'objet
        Icon.SetActive (false);
    }

    // Update is called once per frame
    void Update()
    {
        //Si le l'unité est en feu affiche l'icone
        if (RecupFire.onFire){
            Icon.SetActive(true);
        }
    }
}
