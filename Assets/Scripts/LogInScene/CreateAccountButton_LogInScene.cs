using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAccountButton_LogInScene : MonoBehaviour
{
    public void GoToSignInScene()
    {
        GameManager.instance.LoadScene("SignInScene");
    }
}
