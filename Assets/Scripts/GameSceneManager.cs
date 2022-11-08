using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager instance;
    public GameObject[] startPoints;

    public GameObject policeCarAgent;
    public GameObject playerCar;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        startPoints = GameObject.FindGameObjectsWithTag("StartPoint");
        StartCoroutine("Co_CreatePoliceCarWhenStart");
    }

    IEnumerator Co_CreatePoliceCarWhenStart()
    {
        for (int i = 0; i < 18; i++)
        {
            yield return new WaitForSeconds(1f);
            GameObject policeCarAgentPrefab = Instantiate(policeCarAgent);
            policeCarAgentPrefab.transform.position = startPoints[i % startPoints.Length].transform.position;
        }
    }

    public void ResetPoliceCarPosition(GameObject policeCarAgentPrefab)
    {
        int randomNumber = Random.Range(0, startPoints.Length);
        policeCarAgentPrefab.transform.position = startPoints[randomNumber].transform.position;
    }
    
    
    
    
}
