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
        }else if(so.enemyIsBurn){
            spriteEnemy.color = new Color(1f,0.5f,0f,1f);
            spriteEnemy.sprite = spriteTakeDamage;
        }else if(so.enemyIsPara){
            spriteEnemy.color = Color.yellow;
            spriteEnemy.sprite = spriteDefaut;
        }else{
            spriteEnemy.color = Color.white;
            spriteEnemy.sprite = spriteDefaut;
        }
    }
}
