using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideText : MonoBehaviour
{
    public GameObject Object;

    void Start()
    {
        Object.SetActive(false);
    }

    public void OnCollisionEnter2D()
    {
        Object.SetActive(true);
    }
    public void OnCollisionExit2D()
    {
        Object.SetActive(false);
    }

}
