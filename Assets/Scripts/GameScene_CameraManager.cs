using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene_CameraManager : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3
        (GameSceneManager.instance.playerCar.transform.position.x,
            GameSceneManager.instance.playerCar.transform.position.y + 2f,
            GameSceneManager.instance.playerCar.transform.position.z - 1.3f);
    }
}
