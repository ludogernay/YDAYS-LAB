using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{
    public GameObject IconP ;
    public GameObject IconF ;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI ArmorText;
    public TextMeshProUGUI PVText;
    public Slider hpSlider;

    public void SetHUD(Unit unit)
    {
        nameText.text = unit.unitName;
        levelText.text = "Lvl " + unit.unitLevel;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;
        ArmorText.text = "DEF: " + unit.armor;
        PVText.text = "PV: " + unit.currentHP;
    }

    public void SetHP(int hp, int armor, Unit unit , bool fire)
    {
        hpSlider.value = hp;
        ArmorText.text = "DEF: " + unit.armor;
        PVText.text = "PV: " + unit.currentHP;
        if(unit.onFire){ 
            IconF.SetActive(true);
        }else if (unit.onFire == false){
            IconF.SetActive(false);
        }
        if(unit.Paralysis){
            IconP.SetActive(true);
        }else if (unit.Paralysis==false){
            IconP.SetActive(false);
        }
    }
}
