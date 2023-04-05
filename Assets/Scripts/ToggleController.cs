using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{
    public Toggle[] toggles;

    private int maxSelected = 4;
    private int selectedCount = 0;

    public void OnToggleSelected(bool isSelected)
    {
        if (isSelected)
        {
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
