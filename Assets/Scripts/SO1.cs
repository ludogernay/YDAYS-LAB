using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "so", menuName = "so", order = 1)]
public class SO1 : ScriptableObject
{
    public bool win;
    public bool enemyattack;
    public bool playerattack;
    public bool playerHeal;
    public bool enemyHeal;
    public bool enemyIsPara;
    public bool playerIsPara;
    public bool enemyIsBurn;
    public bool playerIsBurn;
    public List<int> liste = new List<int>(){1,2,3,4};
}
