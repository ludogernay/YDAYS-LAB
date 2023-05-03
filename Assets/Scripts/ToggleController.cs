using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{
    public Toggle[] toggles;
    public SO1 so;
    int i;

    private int maxSelected = 4;
    private int selectedCount = 0;

    private void Start()
    {
        for (int i = 0;i<99;i++){
            so.liste.Remove(i);
        }
        // Ajoute les 4 premiers toggles à la liste "liste"
        for (int i = 0; i < toggles.Length && selectedCount < maxSelected; i++)
        {
            if (toggles[i].isOn)
            {
                so.liste.Add(i + 1);
                selectedCount++;
            }
        }
    }

    public void OnToggleSelected(int num)
    {  

        Debug.Log(selectedCount);
        if (toggles[num-1].isOn)
        {
            so.liste.Add(num);
            selectedCount++;
            
            if (selectedCount > maxSelected)
            {
                // Si on a sélectionné plus de toogles que le maximum autorisé, on déselectionne le dernier toogle sélectionné
                Toggle lastSelectedToggle = GetLastSelectedToggle();
                lastSelectedToggle.isOn = false;

                // On affiche un message d'erreur
                Debug.LogError("Vous ne pouvez sélectionner que " + maxSelected + " toogles maximum !");
            }
        }
        else
        {   
            so.liste.Remove(num);
            selectedCount--;
        }
    }

    private Toggle GetLastSelectedToggle()
    {
        // On récupère la liste des toogles sélectionnés
        List<Toggle> selectedToggles = new List<Toggle>();
        foreach (Toggle toggle in toggles)
        {
            if (toggle.isOn)
            {
                selectedToggles.Add(toggle);
            }
        }

        // On renvoie le dernier toogle sélectionné
        return selectedToggles[selectedToggles.Count - 1];
    }
}
