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

    public GameObject policeCarAgent;
    public List<GameObject> firstPoliceCarPos = new List<GameObject>();
    public GameObject firstPlayerCarPos;
    
    private List<GameObject> policeCarAgents = new List<GameObject>();

    //강화학습 시 켜줘야 함
    public bool isLearning = true;


    private GameObject policeCarAgentPrefab;
    private GameObject playerCarPrefab;

    private int nowLevel;
    private int nextLevel;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        policeCarAgentPrefab = Instantiate(policeCarAgent);
        playerCarPrefab = Instantiate(playerCar);
        nowLevel = 1;
        SetLevelMap(1);
        SetAllCarPosition();
    }

    //PoliceCarAgent에서 다음 에피소드 불러올 때 사용
    public void PrepareNextEpisode()
    {
        CheckLevel();
        SetAllCarPosition();
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
        this.nextLevel = nextLevel;
    }

    public void SetLevelMap(int level)
    {
        for (int i = 0; i < maps.Length; i++)
        {
            maps[i].SetActive(false);
        }
        
        maps[level].SetActive(true);
        SetAllCarPosition();
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
        
        policeCarAgentPrefab.transform.position = startPointList[randomNumber];
        startPointList.RemoveAt(randomNumber);
        policeCarAgentPrefab.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

        
        randomNumber = Random.Range(0, startPointList.Count);
        
        playerCarPrefab.transform.position = startPointList[randomNumber];
        startPointList.RemoveAt(randomNumber);
        playerCarPrefab.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

    }
    
    
}
