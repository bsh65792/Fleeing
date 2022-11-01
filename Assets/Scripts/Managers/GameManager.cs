using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public GameObject alarm;

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





    public void SetAlarm(string alarmText, float alarmTime = 2f)
    {
        GameObject alarmPrefab = Instantiate(alarm);
        alarmPrefab.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
        alarmPrefab.transform.localPosition = new Vector3(0f, 0f, 0f);
        alarmPrefab.transform.localScale = Vector3.zero;

    }
}
