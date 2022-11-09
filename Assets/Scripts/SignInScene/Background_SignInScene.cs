using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Background_SignInScene : MonoBehaviour
{
    public static Background_SignInScene instance;

    public TMP_InputField idInputField;
    public TMP_InputField nicknameInfutField;
    public TMP_InputField passwordInputField;
    public TMP_InputField passwordConfirmInputField;


    void Awake()
    {
        instance = this;
    }
    
}
