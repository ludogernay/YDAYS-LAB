using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int unitLevel;

    public int damage;
    public bool onFire;

    public bool Paralysis ;
    
    public int getturnp=0;// pour récuperer les tours depuis la dernière utilisation de l'attaque de paralisie
    public bool attack = true ;
    public int maxHP;
    public int currentHP;

    public int armor;
    public int getturnf = 0;

    public bool TakeDamage(int dmg, int capacity, int Tour, Unit playerUnit, Unit enemyUnit , BattleState state )
    {
        int fail = 0;
        if (playerUnit.Paralysis == true || enemyUnit.Paralysis){
            if (state == BattleState.PLAYERTURN && playerUnit.Paralysis){
                    if (playerUnit.getturnp+5 >= Tour){
                        fail = Random.Range(0, 100);
                        Debug.Log("Test PARALYSIE YOU: " + fail );
                        if (fail > 25){
                            playerUnit.attack = false;
                            return false;
                        }
                        else playerUnit.attack = true;
                    }
                    else if (playerUnit.getturnp+5 <= Tour)
                        playerUnit.Paralysis = false;
            }

            if (state == BattleState.ENEMYTURN && enemyUnit.Paralysis){
                    if (enemyUnit.getturnp+5 >= Tour){
                        fail = Random.Range(0, 100);
                        Debug.Log("Test PARALYSIE ENEMY: " + fail );
                        if (fail > 25){
                            enemyUnit.attack = false;
                            return false;
                        }
                        else enemyUnit.attack = true;
                    }
                    else if (enemyUnit.getturnp+5 <= Tour)
                        enemyUnit.Paralysis = false;
            }
        }

        if (capacity == 1){
            if (dmg>armor)
                currentHP -= dmg-armor;
        }
        if (capacity == 2){
            dmg = dmg / 3;
            TakeArmorDamage(10,dmg);
        }
        if (capacity == 3){
            if (state == BattleState.PLAYERTURN){
                Debug.Log("Fuego -> ENEMY");
                enemyUnit.getturnf = Tour;
                enemyUnit.onFire = true;
                if (5>armor){
                    enemyUnit.currentHP -= 5-armor;
                }
            }
            if (state == BattleState.ENEMYTURN){
                Debug.Log("Fuego -> YOU");
                playerUnit.getturnf = Tour;
                playerUnit.onFire = true;
                if (5>armor){
                    playerUnit.currentHP -= 5-armor;
                }
            }
        }

        if (capacity == 4)
            if (state == BattleState.PLAYERTURN){
                Debug.Log("Heal YOU");
                playerUnit.Heal(5);
            }
            else {
                Debug.Log("Heal ENEMY");
                enemyUnit.Heal(5);
            }
        
        if (capacity == 5){
            if (state == BattleState.PLAYERTURN){
                enemyUnit.getturnp = Tour;
                enemyUnit.Paralysis = true;
                Debug.Log("Paralyse -> ENEMY");
            }else if (state == BattleState.ENEMYTURN){
                playerUnit.getturnp = Tour;
                playerUnit.Paralysis = true;
                Debug.Log("Paralyse -> YOU");
            }
                currentHP -= 3;
        }

        if (capacity == 6){}// Capacité pour les attaques ratés (augmenter la valeur quand on ajoutes des compétances)

        if (currentHP <= 0)
        {
            currentHP=0;
            return true;
        }
        else 
            return false;
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP)
            currentHP = maxHP;
    }

    public bool TakeArmorDamage(int admg, int dmg)
    {
        if (armor<=admg){
            admg = admg-armor;
            armor = 0;
            currentHP -= 2*dmg;
        }else {
            armor -= admg;
            currentHP -= dmg;
        }   
        if (currentHP <= 0)
        {
            currentHP=0;
            return true;
        }
        else 
            return false;
    }

    public bool IsOnFire(int Tour)//calcule les dÃ©gats de feu
    {   
        if (onFire){
            currentHP -= 3;
            if (currentHP <= 0)
            {   
                currentHP=0;
                return true;
            }
        }

        if (getturnf+5 < Tour){
            onFire = false;
        }
        return false;
    }
    public void resetParalysis(int Tour){
        if (getturnp+5 <= Tour)
            Paralysis = false;
    }
}
