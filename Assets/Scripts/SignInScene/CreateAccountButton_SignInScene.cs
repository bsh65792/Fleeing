using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAccountButton_SignInScene : MonoBehaviour
{
    public void TryCreateAccount()
    {
        BackendManager.instance.TryCreateAccount();
    }
}
