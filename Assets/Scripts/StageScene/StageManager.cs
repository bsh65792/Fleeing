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
    public GameObject playerCar;
    
    public GameObject policeCarAgent;
    public List<GameObject> firstPoliceCarPos = new List<GameObject>();
    
    private List<GameObject> policeCarAgents = new List<GameObject>();

    public bool isLearning = true;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (isLearning == true)
        {
            GameStart();
        }
        
        
    }

    void GameStart()
    {
        for (int i = 0; i < firstPoliceCarPos.Count; i++)
        {
            GameObject policeCarAgentPrefab = Instantiate(policeCarAgent);
            //policeCarAgentPrefab.transform.SetParent(GameObject.FindGameObjectWithTag("Map").transform);
            policeCarAgentPrefab.transform.position = firstPoliceCarPos[i].transform.position;
            policeCarAgentPrefab.transform.localRotation = firstPoliceCarPos[i].transform.localRotation;
            policeCarAgentPrefab.GetComponent<PoliceCarAgent>().idNumber = i;       //policeCarAgent 마다 ID값을 부여한다.
            policeCarAgents.Add(policeCarAgentPrefab);
            firstPoliceCarPos[i].SetActive(false);
        }
    }
    
    
}
