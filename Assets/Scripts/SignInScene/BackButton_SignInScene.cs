using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton_SignInScene : MonoBehaviour
{
    public void GoToLogInScene()
    {
        GameManager.instance.LoadScene("LogInScene");
    }
}
