using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager instance;
    public GameObject[] startPoints;

    public GameObject policeCarAgent;
    public GameObject playerCar;


    public GameObject GameSceneMenuPanel;


    public TextMeshProUGUI timeTMPro;
    public TextMeshProUGUI scoreTMPro;

    public GameObject resultPanel;

    private int remainTime;
    public int nowScore;

    private void Awake()
    {
        instance = this;
        remainTime = 60;
    }

    private void Start()
    {
        startPoints = GameObject.FindGameObjectsWithTag("StartPoint");
        StartCoroutine("Co_CreatePoliceCarWhenStart");
        SetScore();
        StartCoroutine("Co_SetScore");
    }

    IEnumerator Co_SetScore()
    {
        Debug.Log("Co_SetScore 호출됨ㅋ");
        while (remainTime >= 0)
        {
            timeTMPro.text = "Time : " + remainTime.ToString();
            
            remainTime--;
            yield return new WaitForSeconds(1f);
        }

        GameObject resultPanelPrefab = Instantiate(resultPanel);
        
        resultPanelPrefab.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);

        resultPanelPrefab.transform.localPosition = Vector3.zero;
        resultPanelPrefab.transform.localScale = Vector3.one;
        
    }

    public void AddScore()
    {
        nowScore++;
        SetScore();
    }

    private void SetScore()
    {
        scoreTMPro.text = "Score : " + nowScore.ToString();
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
    
    
    public void GoToMainScene()
    {
        GameManager.instance.LoadScene("MainScene");
    }

    public void SetMenu()
    {
        Time.timeScale = 0f;

        GameObject GameSceneMenuPanelPrefab = Instantiate(GameSceneMenuPanel);
        
        GameSceneMenuPanelPrefab.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);

        GameSceneMenuPanelPrefab.transform.localPosition = Vector3.zero;
        GameSceneMenuPanelPrefab.transform.localScale = Vector3.one;
    }
    
    
    
    
}
