using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class turn
{
    public int getturnp;

    public void setturnp(int Tour){
        getturnp = Tour;
    }
}*/

public class Unit : MonoBehaviour
{
    public string unitName;
    public int unitLevel;

    public int damage;
    public bool onFire;
    public bool Paralysis;
    public bool Debuffatk;
    public bool boostatk;

    public int maxHP;
    public int currentHP;

    public int armor;
    int getturnf = 0;// pour récuperer les tours depuis la dernière utilisation de l'attaque de feu
    public int getturnp;// pour récuperer les tours depuis la dernière utilisation de l'attaque de paralisie
    public int getturnd;
    public int getturnb;
    public int damagetemp;// pour
    public int damagetemp2;
    public bool attack = false;
    
    
    public bool TakeDamage(int dmg, int capacity, int Tour, Unit playerUnit, Unit enemyUnit, BattleState state)
    {
        int fail = 0;
        if (playerUnit.Paralysis == true || enemyUnit.Paralysis == true){ //permet de faire la fonction de paralisie que si quelqu'un l'est
            if (state == BattleState.PLAYERTURN){ // test si c'est le tour du joueur
                    if (playerUnit.getturnp+3 > Tour){
                        fail = Random.Range(0, 100);// génere une valeur aléatoire entre 0 et 100
                        if (fail > 25) // 75% de chance de rater le coup
                            return false;
                        else attack = true;
                    }
                    else if (playerUnit.getturnp+3 < Tour)// permet de retirer la paralisie si elle s'épuise
                        playerUnit.Paralysis = false;
            }
            if (state == BattleState.ENEMYTURN){ // test si c'est le tour de l'ennemi
                    if (enemyUnit.getturnp+3 > Tour){
                        fail = Random.Range(0, 100);// génere une valeur aléatoire entre 0 et 100
                        if (fail > 25)// 75% de chance de rater le coup
                            return false;
                        else attack = true;
                    }
                    else if (enemyUnit.getturnp+3 < Tour)// permet de retirer la paralisie si elle s'épuise
                        enemyUnit.Paralysis = false;
                }
        }
        if (capacity == 1){ //calcule les dégats de l'attaque normale
            if (dmg>armor)
                currentHP -= dmg-armor;
        }
        if (capacity == 2){ //calcule les dégats de l'attaque perce armure
            dmg = dmg / 3;
            TakeArmorDamage(10,dmg);
        }
        if (capacity == 3){ //calcule les dégats de l'attaque de feu
            getturnf = Tour;// pour récuperer les tours depuis la dernière utilisation de l'attaque de feu
            if (5>armor){
                currentHP -= 5-armor;
                onFire = true;
            }else{
                onFire = true;
            }
        }
        if (capacity == 4) //Lance la fonction de soin
            playerUnit.Heal(5);
        
        if (capacity == 5){
            if (state == BattleState.PLAYERTURN){
            enemyUnit.getturnp = Tour;// pour récuperer les tours pour l'ennemi depuis la dernière utilisation de l'attaque de paralisie
            }else if (state == BattleState.ENEMYTURN){
            playerUnit.getturnp = Tour;// pour récuperer les tours pour le joueur depuis la dernière utilisation de l'attaque de paralisie
            }
            Paralysis = true;
            currentHP -= 3;
        }
        if (capacity == 6){
            Debuffatk = true;
            getturnd = Tour;
            damagetemp = damage;
        }
        if (capacity == 7){
            boostatk = true;
            playerUnit.getturnb = Tour;
            damagetemp2 = damage;
        }
        if (capacity == 8){
            playerUnit.Shield();
        }

        if (capacity == 9){}// Capacité pour les attaques ratés (augmenter la valeur quand on ajoutes des compétances)
        
        if (getturnd+4 > Tour){
            IsDebuffatk(Debuffatk);
        }else{
            Debuffatk = false;
            IsDebuffatk(Debuffatk);
        }
        Debug.Log("tour :" + Tour);
        Debug.Log("tourb :" + playerUnit.getturnb);
        if (playerUnit.getturnb+4 > Tour){
            Boostatk(boostatk);
            Debug.Log("actif :" + damage);
        }else{
            boostatk = false;
            Boostatk(boostatk);
            Debug.Log("desac :" + damage);
        }
        if (getturnf+5 > Tour) //inflige les dégats de feu pour 3 tours
            IsOnFire(onFire);

        if (currentHP <= 0)//Vérifie si l'unit sur laquelle la fonction s'éxécute est morte
        {
            currentHP=0;
            return true;
        }
        else 
            return false;
    }
    public void Heal(int amount)//calcule les soins
    {
        currentHP += amount;
        if (currentHP > maxHP)
            currentHP = maxHP;
    }

    public bool TakeArmorDamage(int admg, int dmg)//calcule les dégats de l'attaque perce armure
    {
        if (armor<=admg){
            admg = admg-armor;
            armor = 0;
            currentHP -= admg+dmg;
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

    public bool IsOnFire(bool onFire)//calcule les dégats de feu
    {
        
        if (onFire)
            currentHP -= 3;

        if (currentHP <= 0)
        {   
            currentHP=0;
            return true;
        }
        else 
            return false;
    }

    public void IsDebuffatk(bool Debuffatk)
    {
        if (Debuffatk){
            if (damagetemp <= 10)
                damage = 0;
            else
                damage -= 10;
        }else
            damage = damagetemp;
    }

    public void Boostatk(bool boostatk)
    {
        if (boostatk)
           damage += 5;
        else
            damage = damagetemp2;
    }

    public void Shield()
    {
        armor += 5;
    }
}

