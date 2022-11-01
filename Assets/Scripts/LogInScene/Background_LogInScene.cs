using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Background_LogInScene : MonoBehaviour
{
    public static Background_LogInScene instance;

    public TMP_InputField idInputField;
    public TMP_InputField passwordInputField;
    

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
    
    
    
}
