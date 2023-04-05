using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, TRAITEMENT, ENEMYTURN, WON, LOST } //définis les états de la bataille

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    Unit playerUnit;
    Unit enemyUnit;

    public TextMeshProUGUI dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public BattleState state;
    public int Tour = 0;
    public int capacity = 0;
    public int attackfail = 0;
    public bool isDead = false;
    
    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation); // instantie le joueur
        playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation); // instantie l'ennemi
        enemyUnit = enemyGO.GetComponent<Unit>();

        dialogueText.text = "Une " + enemyUnit.unitName + " sauvage approche...";

        playerHUD.SetHUD(playerUnit); //initialise les hud
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);
        Tour = 0;
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void PlayerTurn()
    {
        dialogueText.text = "Choisissez une action :";
        Tour++;
    }

    public void OnAttackButton() //permet de faire une attaque normale
    {
        if (state != BattleState.PLAYERTURN){
            return;
        }
        capacity = 1;
        StartCoroutine(PlayerAttack());
    }

    public void OnAPAttackButton()// permet de faire une attaque perce armure
    {
        if (state != BattleState.PLAYERTURN){
            return;
        }
        capacity = 2;
        StartCoroutine(PlayerAttack());
    }

    public void OnFireAttackButton() // fait une attaque de feu
    {
        if (state != BattleState.PLAYERTURN){
            return;
        }
        capacity = 3;
        StartCoroutine(PlayerAttack());
    }

    public void OnHealButton() // permet de se soigner
    {
        if (state != BattleState.PLAYERTURN)
            return;
        capacity = 4;
        StartCoroutine(PlayerAttack()); 
    }
    public void OnParalysisButton() // permet de paralyser
    {
        if (state != BattleState.PLAYERTURN)
            return;
        capacity = 5;
        StartCoroutine(PlayerAttack()); 
    }
    public void OnDebuffButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        capacity = 6;
        StartCoroutine(PlayerAttack()); 
    }
    public void OnBuffButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        capacity = 7;
        StartCoroutine(PlayerAttack()); 
    }
    public void OnShieldButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        capacity = 8;
        StartCoroutine(PlayerAttack()); 
    }




    IEnumerator PlayerAttack()
    {
        attackfail = Random.Range(0, 100);//initialise une valeur entre 0 et 100 
        if (attackfail <= 10){ // fait échouer l'attaque
            dialogueText.text = "Vous avez raté !";
            isDead = enemyUnit.TakeDamage(playerUnit.damage, 9, Tour, playerUnit, enemyUnit, state);
        }
        else if (attackfail > 10 && attackfail < 90){ // fait réussir l'attaque normalement
            isDead = enemyUnit.TakeDamage(playerUnit.damage, capacity, Tour, playerUnit, enemyUnit, state);
            
            if (playerUnit.Paralysis == true){ // vérifie si le joueur est paralysé
                if (playerUnit.attack == false) // vérifie si le joueur a pu attaqué
                    dialogueText.text = "Vous êtes paralysée !";
                else 
                    dialogueText.text = "Vous êtes paralysée mais avez réussi a attaqué !";
            }

            enemyHUD.SetHP(enemyUnit.currentHP, enemyUnit.armor, enemyUnit);//met a jour l'hud de l'enemie
            if (capacity == 4 || capacity == 7){//permet de mettre a jour l'hud du joueur quand il se soigne
                playerHUD.SetHP(playerUnit.currentHP, playerUnit.armor, playerUnit);
                dialogueText.text = "Vous vous êtes soigné.";
            }else{
                dialogueText.text = "L'attaque a réussi !";
            }
        }
        else if (attackfail >= 90){ // fait réussir l'attaque de manière critique (double les dégats)
            isDead = enemyUnit.TakeDamage((playerUnit.damage * 2), capacity, Tour, playerUnit, enemyUnit, state);
        
            if (playerUnit.Paralysis == true){// vérifie si le joueur est paralysé
                if (playerUnit.attack == false)// vérifie si le joueur a pu attaqué
                    dialogueText.text = "Vous êtes paralysée !";
                else 
                    dialogueText.text = "Vous êtes paralysée mais avez réussi a faire une attaque critique !";
            }

            enemyHUD.SetHP(enemyUnit.currentHP, enemyUnit.armor, enemyUnit);//met a jour l'hud de l'ennemi
            if (capacity == 4 || capacity == 7){//permet de mettre a jour l'hud du joueur quand il se soigne
                playerHUD.SetHP(playerUnit.currentHP, playerUnit.armor, playerUnit);
                dialogueText.text = "Vous vous êtes grandement soigné.";
            }else{
                dialogueText.text = "Coup critique !";
            }
        }  
        state = BattleState.TRAITEMENT;//sert a empecher le joueur d'utiliser les boutons

        yield return new WaitForSeconds(2f);//permet d'attendre 2sec

        if(isDead)// permet de tester si l'ennemi est mort
        {
            state = BattleState.WON;
            StartCoroutine(EndBattle());
        }else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        attackfail = Random.Range(0, 100);//initialise une valeur entre 0 et 100 
        if (attackfail <= 10){ // fait échouer l'attaque
            dialogueText.text = enemyUnit.unitName + " a raté son attaque !";
            isDead = playerUnit.TakeDamage(enemyUnit.damage,9, Tour, playerUnit, enemyUnit, state);
        }
        else if (attackfail > 10 && attackfail < 90){// fait réussir l'attaque normalement
            dialogueText.text = enemyUnit.unitName + " attaque !";

            if (enemyUnit.Paralysis == true){ // vérifie si l'ennemi est paralysé
                if (enemyUnit.attack == false)// vérifie si l'ennemi a pu attaqué
                    dialogueText.text = enemyUnit.unitName + " est paralysée !";
                else 
                    dialogueText.text = enemyUnit.unitName + " est paralysée mais avez réussi a attaqué !";
            }

            yield return new WaitForSeconds(1f);

            isDead = playerUnit.TakeDamage(enemyUnit.damage,1, Tour, playerUnit, enemyUnit, state);

            playerHUD.SetHP(playerUnit.currentHP, playerUnit.armor, playerUnit);//met a jour l'hud du joueur
        }
        else if (attackfail >= 90){// fait réussir l'attaque de manière critique (double les dégats)
                dialogueText.text = enemyUnit.unitName + " a fait un coup critique !";

            if (enemyUnit.Paralysis == true){ // vérifie si l'ennemi est paralysé
                if (enemyUnit.attack == false)// vérifie si l'ennemi a pu attaqué
                    dialogueText.text = enemyUnit.unitName + " est paralysée !";
                else 
                    dialogueText.text = enemyUnit.unitName + " est paralysée mais avez réussi a faire un coup critique !";
            }

            yield return new WaitForSeconds(1f);

            isDead = playerUnit.TakeDamage((enemyUnit.damage * 2),1, Tour, playerUnit, enemyUnit, state);

            playerHUD.SetHP(playerUnit.currentHP, playerUnit.armor, playerUnit);//met a jour l'hud du joueur
        }
        yield return new WaitForSeconds(1f);
        Tour++;
        if(isDead)// permet de tester si le joueur est mort
        {
            state = BattleState.LOST;
            StartCoroutine(EndBattle());
        }else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    IEnumerator EndBattle()
    {
        if(state == BattleState.WON)//si le joueur gagne
        {
            dialogueText.text = "Vous avez gagné la bataille !";
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene("Move");
            
        } else if (state == BattleState.LOST)// si l'ennemi gagne
        {
            dialogueText.text = "Vous avez perdu.";
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene("Move");
        }
    }
}
