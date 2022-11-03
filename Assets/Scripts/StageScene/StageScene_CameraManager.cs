using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageScene_CameraManager : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (StageManager.instance.isLearning == false)
        {
            transform.position = new Vector3
                (StageManager.instance.playerCarPrefab.transform.position.x,
                StageManager.instance.playerCarPrefab.transform.position.y + 2f,
                StageManager.instance.playerCarPrefab.transform.position.z - 1.3f);
        }
        
    }
}
