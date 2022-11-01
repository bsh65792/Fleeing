using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogInButton : MonoBehaviour
{
    public void TryLogIn()
    {
        BackendManager.instance.TryLogIn();
    }
}
