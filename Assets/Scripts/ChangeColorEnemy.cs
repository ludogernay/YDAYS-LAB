using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorEnemy : MonoBehaviour
{
    public SO1 so;
    public Sprite spriteAttack,spriteDefaut,spriteTakeDamage;
    public GameObject enemy; 
    public void FixedUpdate(){
        SpriteRenderer spriteEnemy =enemy.GetComponent<SpriteRenderer>();
        if (so.playerattack){
            spriteEnemy.color = Color.red;
            spriteEnemy.sprite = spriteTakeDamage;
        }else if(so.enemyHeal){
            spriteEnemy.color = Color.green;
            spriteEnemy.sprite = spriteDefaut;
        }else if(so.enemyattack){
            spriteEnemy.sprite = spriteAttack;
        }else{
            spriteEnemy.color = Color.white;
            spriteEnemy.sprite = spriteDefaut;
        }
    }
}
