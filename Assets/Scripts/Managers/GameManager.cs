using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public GameObject alarmPanel;
    public GameObject fadeOutScenePanel;

    private GameObject nowAlarmPanel;

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



    public void LoadScene(string sceneName, float fadeOutTime = 0.6f)
    {
        GameObject fadeOutScenePanelPrefab = Instantiate(fadeOutScenePanel);
        fadeOutScenePanelPrefab.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
        fadeOutScenePanelPrefab.transform.localPosition = Vector3.zero;
        fadeOutScenePanelPrefab.transform.localScale = Vector3.one;
        
        fadeOutScenePanelPrefab.GetComponent<FadeOutScenePanel>().SetNextSceneName(sceneName);
        fadeOutScenePanelPrefab.GetComponent<FadeOutScenePanel>().SetFadeOutTime(fadeOutTime);
    }


    public void SetAlarm(string alarmText, float alarmTime = 2f)
    {
        if (nowAlarmPanel != null)
        {
            Destroy(nowAlarmPanel);
        }
        
        GameObject alarmPanelPrefab = Instantiate(alarmPanel);
        alarmPanelPrefab.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
        alarmPanelPrefab.transform.localPosition = new Vector3(0f, 148f, 0f);
        alarmPanelPrefab.transform.localScale = Vector3.one;
        alarmPanelPrefab.GetComponent<AlarmPanel>().alarmText = alarmText;
        alarmPanelPrefab.GetComponent<AlarmPanel>().alarmTime = alarmTime;

        nowAlarmPanel = alarmPanelPrefab;

    }


    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
        
        if (!Application.isEditor)
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        //Application.Quit은 앱크래시나는 버그가 있어 이렇게 처리
    }
}
