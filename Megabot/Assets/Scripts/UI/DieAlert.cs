using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieAlert : MonoBehaviour
{
    public static DieAlert instance;

    private void Awake()
    {
        gameObject.SetActive(true);
        instance = this;
        
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }
}
