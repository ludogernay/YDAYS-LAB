using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject InventaireUI;
    public GameObject sonUI;
    public GameObject ParametreUI;
    public GameObject LanguageUI;
    public TextMeshProUGUI textMeshPro;

    public GameObject Cube_O;
    public GameObject Cube_E;
    public GameObject Cube_C;

    public SO1 so;
    

    public static bool gamepause = false;
    public static bool inv = false;
    public static bool param = false;
    public static bool son = true;
    public AudioSource audioSource;


    public static PauseMenu instance;

    void Update()
    {
       if(Input.GetKeyDown(KeyCode.Escape))
       {
           if (inv || param)
            {
                Masquer_Inventaire();
                Masquer_Parametre();
            }else{

                if (gamepause)
                {
                    Resume();
                    gamepause = false;
                }
                else
                {
                    Paused();
                    gamepause = true;
                }
            }

       }
    }

    public void Paused()
    {
        PlayerMovement.instance.enabled = false;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        gamepause = true;  
    }

    public void Resume()
    {
        PlayerMovement.instance.enabled = true;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        gamepause = false;  
    }

    public void QuitReturnToMenu()
    {
        PlayerMovement.instance.enabled = true;
        gamepause = false; 
        SceneManager.LoadScene("Menu");
    }

    public void Aff_Inventaire()
    {
        InventaireUI.SetActive(true);
        inv = true;
    }

    public IEnumerator AffSec()
    {
        textMeshPro.gameObject.SetActive(true);
        float pauseStartTime = Time.realtimeSinceStartup;
        bool hasWaitedForTwoSeconds = false;
        while (!hasWaitedForTwoSeconds)
        {
            if (Time.timeScale == 0)
            {
                if (Time.realtimeSinceStartup - pauseStartTime >= 2f)
                {
                    hasWaitedForTwoSeconds = true;
                }
            }
            else
            {
                hasWaitedForTwoSeconds = true;
            }
            yield return null;
        }
        textMeshPro.gameObject.SetActive(false);
    }


    public void Masquer_Inventaire()
    {
        if(so.liste.Count < 4){
            InventaireUI.SetActive(true);
            StartCoroutine(AffSec());
        }else{
            InventaireUI.SetActive(false);
            inv = false;
        }   
    }

    public void Aff_Parametre()
    {
        ParametreUI.SetActive(true);
        param = true;
    }

    public void Masquer_Parametre()
    {
        ParametreUI.SetActive(false);
        param = false;
    }

    public void Aff_Language()
    {
        LanguageUI.SetActive(true);
    }

    public void Masquer_Language()
    {
        LanguageUI.SetActive(false);
    }
    public void sons()
    {
       if (son == true){
            audioSource.Pause();
            sonUI.SetActive(false);
            son = false;
        }else{
            audioSource.Play();
            sonUI.SetActive(true);
            son = true;
        }
    }



    public void GererInv(GameObject obj)
    {
        if (obj != null){

            Debug.Log(Cube_E);
            Debug.Log(Cube_O);
            Debug.Log(Cube_C);

            if (Cube_C != null && Cube_E != null && Cube_O != null){
                Debug.Log(obj);

                Cube_E.SetActive(false);
                Cube_C.SetActive(false);
                Cube_O.SetActive(false);

                obj.SetActive(true);
            }
        }
    }
}