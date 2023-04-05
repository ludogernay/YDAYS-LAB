using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, TRAITEMENT, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public SO1 so;
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
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();

        dialogueText.text = "Une " + enemyUnit.unitName + " sauvage approche...";

        playerHUD.SetHUD(playerUnit);
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

        public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN){
            return;
        }
        capacity = 1;
        StartCoroutine(PlayerAttack());
        
    }

    public void OnAPAttackButton()
    {
        if (state != BattleState.PLAYERTURN){
            return;
        }
        capacity = 2;
        StartCoroutine(PlayerAttack());
        
    }

    public void OnFireAttackButton()
    {
        if (state != BattleState.PLAYERTURN){
            return;
        }
        capacity = 3;
        StartCoroutine(PlayerAttack());
        
    }

    public void OnHealButton()
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

    IEnumerator PlayerAttack()
    {
        GameObject obj = Instantiate(enemyPrefab);
        Transform objTransform = obj.transform.Find("Dwayne");
        objTransform.GetComponent<SpriteRenderer>().material.color = Color.red;
        enemyHUD.SetHP(enemyUnit.currentHP, enemyUnit.armor, enemyUnit, enemyUnit.onFire);
        Debug.Log("Debut du tour Joueur");
        attackfail = Random.Range(0, 100);//initialise une valeur entre 0 et 100 
        if (attackfail <= 10){ // fait échouer l'attaque
            dialogueText.text = "Vous avez raté !";
            isDead = enemyUnit.TakeDamage(playerUnit.damage, 6, Tour, playerUnit, enemyUnit, state);
        }
        else if (attackfail > 10 && attackfail < 90){ // fait réussir l'attaque normalement
            isDead = enemyUnit.TakeDamage(playerUnit.damage, capacity, Tour, playerUnit, enemyUnit, state);
            
            if (playerUnit.Paralysis){ // vérifie si le joueur est paralysé
                if (playerUnit.attack == false) // vérifie si le joueur a pu attaqué
                    dialogueText.text = "Vous êtes paralysée !";
                else{
                    enemyHUD.SetHP(enemyUnit.currentHP, enemyUnit.armor, enemyUnit,enemyUnit.onFire);//met a jour l'hud de l'ennemi
                    if (capacity == 4){//permet de mettre a jour l'hud du joueur quand il se soigne
                        playerHUD.SetHP(playerUnit.currentHP, playerUnit.armor, playerUnit,playerUnit.onFire);
                        dialogueText.text = "Vous vous êtes soigné.";
                    }else if (capacity == 3) {
                        dialogueText.text = "Vous avez brulé l'ennemi";
                    }else{
                        dialogueText.text = "Vous avez attaqué !";
                    }
                }
            }else{
                enemyHUD.SetHP(enemyUnit.currentHP, enemyUnit.armor, enemyUnit,enemyUnit.onFire);//met a jour l'hud de l'ennemi
                if (capacity == 4){//permet de mettre a jour l'hud du joueur quand il se soigne
                    playerHUD.SetHP(playerUnit.currentHP, playerUnit.armor, playerUnit,playerUnit.onFire);
                    dialogueText.text = "Vous vous êtes  soigné.";
                }else if (capacity == 3) {
                    dialogueText.text = "Vous avez  brulé l'ennemi";
                }else{
                    dialogueText.text = "Vous avez attaqué !";
            }
        }

    }

    else if (attackfail >= 90){ // fait réussir l'attaque de manière critique (double les dégats)
        isDead = enemyUnit.TakeDamage((playerUnit.damage * 2), capacity, Tour, playerUnit, enemyUnit, state);
        
        if (playerUnit.Paralysis){// vérifie si le joueur est paralysé
            if (playerUnit.attack == false)// vérifie si le joueur a pu attaqué
                dialogueText.text = "Vous êtes paralysée !";
            else{
                enemyHUD.SetHP(enemyUnit.currentHP, enemyUnit.armor, enemyUnit,enemyUnit.onFire);//met a jour l'hud de l'ennemi
                if (capacity == 4){//permet de mettre a jour l'hud du joueur quand il se soigne
                    playerHUD.SetHP(playerUnit.currentHP, playerUnit.armor, playerUnit,playerUnit.onFire);
                dialogueText.text = "Vous vous êtes grandement soigné.";
                }else if (capacity == 3) {
                    dialogueText.text = "Vous avez grandement brulé l'ennemi";
                }else{
                    dialogueText.text = "Coup critique !";
                }
        }
        }else{
            enemyHUD.SetHP(enemyUnit.currentHP, enemyUnit.armor, enemyUnit,enemyUnit.onFire);//met a jour l'hud de l'ennemi
                if (capacity == 4){//permet de mettre a jour l'hud du joueur quand il se soigne
                    playerHUD.SetHP(playerUnit.currentHP, playerUnit.armor, playerUnit,playerUnit.onFire);
                dialogueText.text = "Vous vous êtes grandement soigné.";
                }else if (capacity == 3) {
                    dialogueText.text = "Vous avez grandement brulé l'ennemi";
                }else{
                    dialogueText.text = "Coup critique !";
            }
        }
    }
        state = BattleState.TRAITEMENT;
        yield return new WaitForSeconds(1f);

        bool isDeadFire = false;
        if (playerUnit.onFire == true && isDead == false) { //Mettre les ticks de l'attaque puis du feu
            yield return new WaitForSeconds(1f);
            isDeadFire = playerUnit.IsOnFire(Tour);
            playerHUD.SetHP(playerUnit.currentHP, playerUnit.armor, playerUnit, playerUnit.onFire);
            dialogueText.text = "Vous êtes brulé !";
        }
        

        Debug.Log("Tour" + Tour);

        if (enemyUnit.Paralysis == true){
            enemyUnit.resetParalysis(Tour);
            enemyHUD.SetHP(enemyUnit.currentHP, enemyUnit.armor, enemyUnit,enemyUnit.onFire);
        }

        yield return new WaitForSeconds(2f);


        if(isDead)
        {
            state = BattleState.WON;
            StartCoroutine(EndBattle());
        }
        else if (isDeadFire){
            state = BattleState.LOST;
            StartCoroutine(EndBattle());
        
        }else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        playerHUD.SetHP(playerUnit.currentHP, playerUnit.armor, playerUnit, playerUnit.onFire);
        Debug.Log("Debut du tour de l'ennemi ");
        int RandomCapacity = Random.Range(1, 6);
        Debug.Log("CapacityRandom ENEMY : "+RandomCapacity);
        attackfail = Random.Range(0, 100);//initialise une valeur entre 0 et 100 
        if (attackfail <= 10){ // fait échouer l'attaque
            dialogueText.text = enemyUnit.unitName + " a raté son attaque !";
            isDead = playerUnit.TakeDamage(enemyUnit.damage,6, Tour, playerUnit, enemyUnit, state);
        }
        else if (attackfail > 10 && attackfail < 90){// fait réussir l'attaque normalement
            dialogueText.text = enemyUnit.unitName + " attaque !";

            if (enemyUnit.Paralysis){ // vérifie si l'ennemi est paralysé
                if (enemyUnit.attack == false)// vérifie si l'ennemi a pu attaqué
                    dialogueText.text = enemyUnit.unitName + " est paralysée !";
                else 
                    dialogueText.text = enemyUnit.unitName + " est paralysée mais avez réussi a attaqué !";
            }
            else if (RandomCapacity == 4){
            dialogueText.text = enemyUnit.unitName + " s'est soigné.";
        }

            yield return new WaitForSeconds(1f);

            isDead = playerUnit.TakeDamage(enemyUnit.damage,RandomCapacity, Tour, playerUnit, enemyUnit, state);

            playerHUD.SetHP(playerUnit.currentHP, playerUnit.armor, playerUnit, playerUnit.onFire);//met a jour l'hud du joueur
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

            isDead = playerUnit.TakeDamage((enemyUnit.damage * 2),RandomCapacity, Tour, playerUnit, enemyUnit, state);

            playerHUD.SetHP(playerUnit.currentHP, playerUnit.armor, playerUnit, playerUnit.onFire);//met a jour l'hud du joueur
        }

        yield return new WaitForSeconds(1f);

        bool isDeadFire = false;
        if (enemyUnit.onFire == true && isDead == false) { //Mettre les ticks de l'attaque puis du feu
            yield return new WaitForSeconds(1f);
            isDeadFire = enemyUnit.IsOnFire(Tour);
            enemyHUD.SetHP(enemyUnit.currentHP, enemyUnit.armor, enemyUnit,enemyUnit.onFire);
            dialogueText.text = enemyUnit.unitName + " est brulé";
        }

        
        Tour++;
        
        if (playerUnit.Paralysis == true){
            playerUnit.resetParalysis(Tour);
            playerHUD.SetHP(playerUnit.currentHP, playerUnit.armor, playerUnit, playerUnit.onFire);
        }
        

        if(isDead)
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
        Transform childTransform =enemyPrefab.transform.Find("Dwayne");
        SpriteRenderer spriteRenderer = childTransform.GetComponent<SpriteRenderer>();
        // change la couleur de spriterenderer
        spriteRenderer.color = new Color(1f, 0f, 0f, 1f);

        if(state == BattleState.WON)
        {
            dialogueText.text = "Vous avez gagné la bataille !";
            so.win=true;
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene("Move");
            
        } else if (state == BattleState.LOST)
        {
            dialogueText.text = "Vous avez perdu.";
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene("Move");
        }
    }
}

