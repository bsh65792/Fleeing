using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;




/*
 * PlayerCar 생성 및 좌표 설정(Stage에 따라서!)
 * PoliceCarAgent 생성 및 좌표 설정(Stage에 따라서!)
 * Time 및 GameOver 관련 설정
 * Stage 
 */




public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    public GameObject[] maps;
    
    public GameObject playerCar;

    public GameObject[] policeCarAgent;
    public GameObject policeCarAgentDummy;
    public List<GameObject> firstPoliceCarPos = new List<GameObject>();
    public GameObject firstPlayerCarPos;
    
    private List<GameObject> policeCarAgents = new List<GameObject>();

    //강화학습 시 켜줘야 함
    public bool isLearning;


    private GameObject policeCarAgentPrefab;
    public GameObject playerCarPrefab;

    private int nowLevel;
    private int nextLevel;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (isLearning == true)
        {
            //playerCarPrefab = Instantiate(playerCar);
           // policeCarAgentPrefab = Instantiate(policeCarAgent);
        
            nowLevel = 1;
            SetLevelMap(1);
            //SetAllCarPosition();
        }
        else
        {
            nowLevel = 1;
            SetLevelMap(1);
        }
        
    }

    public void CreatePoliceCarAgent(int level)
    {
        GameObject[] startPoints = GameObject.FindGameObjectsWithTag("StartPoint");

        int randomNumber = Random.Range(0, startPoints.Length);

        GameObject policeCarAgentPref = Instantiate(policeCarAgent[level]);
        
        policeCarAgentPref.transform.position = new Vector3(startPoints[randomNumber].transform.position.x, 0.06f, startPoints[randomNumber].transform.position.z);
        policeCarAgentPref.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
    }

    public void ResetPoliceCarAgentPosition(GameObject policeCarTemp)
    {
        GameObject[] startPoints = GameObject.FindGameObjectsWithTag("StartPoint");

        int randomNumber = Random.Range(0, startPoints.Length);
        
        policeCarTemp.transform.position = new Vector3(startPoints[randomNumber].transform.position.x, 0.06f, startPoints[randomNumber].transform.position.z);
        policeCarTemp.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
    }

    //PoliceCarAgent에서 다음 에피소드 불러올 때 사용
    public void PrepareNextEpisode()
    {
        if (isLearning == true)
        {
            CheckLevel();
            SetAllCarPosition();
        }
    }

    public void CheckLevel()
    {
        if (nowLevel != nextLevel)
        {
            SetLevelMap(nextLevel);
            nowLevel = nextLevel;
        }
    }

    public void SetNextLevel(int nextLevel)
    {
        if (isLearning == true)
        {
            this.nextLevel = nextLevel;
        }
        else
        {
            GameObject[] policeCars = GameObject.FindGameObjectsWithTag("PoliceCarAgent");
            for (int i = 0; i < policeCars.Length; i++)
            {
                Destroy(policeCars[i]);
            }
            
            for (int i = 0; i < maps.Length; i++)
            {
                maps[i].SetActive(false);
            }
        
            maps[nextLevel].SetActive(true);
        }
    }


    public void ResetAll()
    {
        GameObject[] policeCars = GameObject.FindGameObjectsWithTag("PoliceCarAgent");
        for (int i = 0; i < policeCars.Length; i++)
        {
            Destroy(policeCars[i]);
        }
    }

    public void SetLevelMap(int level)
    {
        for (int i = 0; i < maps.Length; i++)
        {
            maps[i].SetActive(false);
        }
        
        maps[level].SetActive(true);

        if (isLearning == true)
        {
            SetAllCarPosition();
        }
        
        
        GameObject[] startPoints = GameObject.FindGameObjectsWithTag("StartPoint");
        for (int i = 0; i < startPoints.Length; i++)
        {
            startPoints[i].transform.localPosition = new Vector3(startPoints[i].transform.localPosition.x,
                0.3f, startPoints[i].transform.localPosition.z);
        }
    }
    

    public void SetAllCarPosition()
    {

        GameObject[] startPoints = GameObject.FindGameObjectsWithTag("StartPoint");

        List<Vector3> startPointList = new List<Vector3>();

        for (int i = 0; i < startPoints.Length; i++)
        {
            startPointList.Add(new Vector3(startPoints[i].transform.position.x, startPoints[i].transform.position.y, startPoints[i].transform.position.z));
        }

        int randomNumber = Random.Range(0, startPointList.Count);
        
        policeCarAgentPrefab.transform.position = new Vector3(startPointList[randomNumber].x, 0.06f, startPointList[randomNumber].z);
        startPointList.RemoveAt(randomNumber);
        policeCarAgentPrefab.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

        
        randomNumber = Random.Range(0, startPointList.Count);
        
        playerCarPrefab.transform.position = new Vector3(startPointList[randomNumber].x, 0.06f, startPointList[randomNumber].z);
        startPointList.RemoveAt(randomNumber);
        playerCarPrefab.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

    }


    public void GoToMainScene()
    {
        GameManager.instance.LoadScene("MainScene");
    }


    public void SetGameSpeed(int speed)
    {
        Time.timeScale = speed;
    }


}
