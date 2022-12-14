using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_MainScene : MonoBehaviour
{
    public GameObject stageSelectionPanel;
    public GameObject rankingPanel;
    public void SetStageSelectionPanel()
    {
        GameObject stageSelectionPanelPrefab = Instantiate(stageSelectionPanel);
        stageSelectionPanelPrefab.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
        stageSelectionPanelPrefab.transform.localPosition = Vector3.zero;
    }
    
    public void SetRankingPanel()
    {
        GameObject rankingPanelPrefab = Instantiate(rankingPanel);
        rankingPanelPrefab.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
        rankingPanelPrefab.transform.localPosition = Vector3.zero;
    }
}
