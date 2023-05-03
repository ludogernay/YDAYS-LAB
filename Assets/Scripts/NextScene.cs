using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public SO1 so;
    public Animator transition; //récupère l'animator choisi
    public SOEnnemi Ennemi;
    

    public float transitionTime = 1f; // variable du temps de transition
    
    public void NxtScene(){ // Charge la scène suivante dans l'index avec les transitions

        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex+1));
    }
    public void NxtSceneNG(){ // charge la scène "Move"
        so.win = false;
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex+1));;
    }


IEnumerator LoadLevel (int levelIndex)
{
    transition.SetTrigger("Start"); // trigger la transition

    yield return new WaitForSeconds(transitionTime); // attends avant de charger la scène
    
    SceneManager.LoadScene(levelIndex);

}

}
