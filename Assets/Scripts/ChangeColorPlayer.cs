using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorPlayer : MonoBehaviour
{
    public SO1 so;
    public Sprite spriteDefaut,spriteTakeDamage;
    public GameObject player; 
    public void FixedUpdate(){
        SpriteRenderer spritePlayer =player.GetComponent<SpriteRenderer>();
        if (so.enemyattack){
            spritePlayer.color = Color.red;
            spritePlayer.sprite = spriteTakeDamage;
        }else if(so.playerHeal){
            spritePlayer.color = Color.green;
            spritePlayer.sprite = spriteDefaut;
        }else{
            spritePlayer.color = Color.white;
            spritePlayer.sprite = spriteDefaut;
        }
    }
}
