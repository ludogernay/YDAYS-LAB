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
}
