using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameExitButton : MonoBehaviour
{
    public void GameExit()
    {
        GameManager.instance.ExitGame();
    }
}
