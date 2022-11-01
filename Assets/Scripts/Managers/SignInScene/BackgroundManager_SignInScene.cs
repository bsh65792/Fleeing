using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager_SignInScene : MonoBehaviour
{
    public static BackgroundManager_SignInScene instance;
    
    
    public 

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Update()
    {
        
    }


    void CheckInputField()
    {
        
    }
}
