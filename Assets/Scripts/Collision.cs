using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Collision : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Object;
    public GameObject Object2;
    public Sprite sprite;
    private SpriteRenderer spriteRenderer;
    public SO1 so;
    private bool beat = true;

    public void Start(){
        if (so.win){
            beat=false;
            //Lorsque le joueur gagne, on modifie le sprite du joueur
            Object2.GetComponent<SpriteRenderer>().sprite =sprite;
            //Lorsque le joueur gagne, on desactive le bouton
            Object.SetActive(false);
        }
        spriteRenderer = GetComponent<SpriteRenderer>(); 
    }
    public void OnCollisionEnter2D()
    {
        Object.SetActive(beat);
    }
    public void OnCollisionExit2D()
    {
        Object.SetActive(false);
    }
    
    public void Update() {
        if (so.win)
        {
            spriteRenderer.color = new Color (1, 0, 0, 1); 
        }
    }
}

    
