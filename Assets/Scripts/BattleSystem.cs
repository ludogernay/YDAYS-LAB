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
    public SOEnnemi Ennemi;

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

        GameObject enemyGO = Instantiate(Ennemi.Ennemi, enemyBattleStation);
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

    IEnumerator takeRed(GameObject unit) {
        Transform spriteTransform = unit.transform.Find("sprite");
        SpriteRenderer spriteSpriteRenderer = spriteTransform.GetComponent<SpriteRenderer>();

        Debug.Log(spriteSpriteRenderer);
        Debug.Log("red");

        yield return new WaitForSeconds(2f);
        spriteSpriteRenderer.color = new Color(1f, 0f, 0f, 1f);
        yield return new WaitForSeconds(2f);
        spriteSpriteRenderer.color = Color.white;
    }

    IEnumerator PlayerAttack()
    {
        Debug.Log("Debut du tour Joueur");

        StartCoroutine(takeRed(playerPrefab));

        string dialogue = "";

        attackfail = Random.Range(0, 100);//initialise une valeur entre 0 et 100 
        if (attackfail <= 10){ // fait échouer l'attaque
            dialogue = "Vous avez raté !";
            isDead = enemyUnit.TakeDamage(playerUnit.damage, 6, Tour, playerUnit, enemyUnit, state);
        }

        else if (attackfail > 10 && attackfail < 90){ // fait réussir l'attaque normalement
            
            if (capacity == 4){//permet de mettre a jour l'hud du joueur quand il se soigne
                dialogue = "Vous vous êtes soigné.";
            }else if (capacity == 3) {
                dialogue = "Vous avez brulé l'ennemi";
            }else{
                dialogue = "Vous avez attaqué !";
            }
            isDead = enemyUnit.TakeDamage(playerUnit.damage, capacity, Tour, playerUnit, enemyUnit, state);
        }

        else if (attackfail >= 90){ // fait réussir l'attaque de manière critique (double les dégats)
            if (capacity == 4){//permet de mettre a jour l'hud du joueur quand il se soigne
                dialogue = "Vous vous êtes grandement soigné.";
            }else if (capacity == 3) {
                dialogue = "Vous avez grandement brulé l'ennemi";
            }else{
                dialogue = "Coup critique !";
            }
            isDead = enemyUnit.TakeDamage((playerUnit.damage * 2), capacity, Tour, playerUnit, enemyUnit, state);
        }
        state = BattleState.TRAITEMENT;

        enemyHUD.SetHP(enemyUnit.currentHP, enemyUnit.armor, enemyUnit,enemyUnit.onFire);//met a jour l'hud de l'ennemi
        playerHUD.SetHP(playerUnit.currentHP, playerUnit.armor, playerUnit,playerUnit.onFire);

        if (playerUnit.Paralysis == true){ // vérifie si l'ennemi est paralysé
            if (playerUnit.attack == false)// vérifie si l'ennemi a pu attaqué
                dialogueText.text = "Vous êtes paralysée !";
            else{
                dialogueText.text = "Vous êtes paralysée mais vous avez réussi à attaqué !";
                yield return new WaitForSeconds(1f);
                dialogueText.text = dialogue;
            }
        }else{
            dialogueText.text = dialogue;
        }

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
            enemyUnit.resetParalysis(Tour+1);
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
        Debug.Log("---Debut du tour de l'ennemi---");

        // StartCoroutine(takeRed(enemyPrefab));

        string dialogue = "";
        int RandomCapacity = Random.Range(1, 6);
        Debug.Log("CapacityRandom ENEMY : "+RandomCapacity);

        attackfail = Random.Range(0, 100);//initialise une valeur entre 0 et 100 
        if (attackfail <= 10){ // fait échouer l'attaque
            dialogue = enemyUnit.unitName + " a raté son attaque !";
            isDead = playerUnit.TakeDamage(enemyUnit.damage,6, Tour, playerUnit, enemyUnit, state);
        }
        else if (attackfail > 10 && attackfail < 90){// fait réussir l'attaque normalement
            dialogue = enemyUnit.unitName + " attaque !";

            if (RandomCapacity == 4){
            dialogue = enemyUnit.unitName + " s'est soigné.";
            }

            isDead = playerUnit.TakeDamage(enemyUnit.damage,RandomCapacity, Tour, playerUnit, enemyUnit, state);
        }
        else if (attackfail >= 90){// fait réussir l'attaque de manière critique (double les dégats)
            dialogue = enemyUnit.unitName + " a fait un coup critique !";

            isDead = playerUnit.TakeDamage((enemyUnit.damage * 2),RandomCapacity, Tour, playerUnit, enemyUnit, state);
        }

        if (enemyUnit.Paralysis == true){ // vérifie si l'ennemi est paralysé
            if (enemyUnit.attack == false)// vérifie si l'ennemi a pu attaqué
                dialogueText.text = enemyUnit.unitName + " est paralysée !";
            else{
                dialogueText.text = enemyUnit.unitName + " est paralysée mais a réussi à attaqué !";
                yield return new WaitForSeconds(1f);
                dialogueText.text = dialogue;
            }
        }else{
            dialogueText.text = dialogue;
        }

        enemyHUD.SetHP(enemyUnit.currentHP, enemyUnit.armor, enemyUnit,enemyUnit.onFire);//met a jour l'hud de l'ennemi
        playerHUD.SetHP(playerUnit.currentHP, playerUnit.armor, playerUnit,playerUnit.onFire);

        yield return new WaitForSeconds(1f);

        bool isDeadFire = false;
        if (enemyUnit.onFire == true && isDead == false) { //Mettre les ticks de l'attaque puis du feu
            yield return new WaitForSeconds(1f);
            isDeadFire = enemyUnit.IsOnFire(Tour);
            enemyHUD.SetHP(enemyUnit.currentHP, enemyUnit.armor, enemyUnit,enemyUnit.onFire);
            dialogueText.text = enemyUnit.unitName + " est brulé";
        }

        yield return new WaitForSeconds(1f);
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
        if(state == BattleState.WON)
        {
            dialogueText.text = "Vous avez gagné la bataille !";
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

