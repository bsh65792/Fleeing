using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using BackEnd;
using LitJson;

public class ResultPanel : MonoBehaviour
{
    public GameObject rankingPanel;
    
     void OnEnable()
    {
        StartCoroutine("Co_SetPanelAnimation");
    }

     public TextMeshProUGUI scoreTMPro;

     private int score;

     private void Start()
     {
         score = GameSceneManager.instance.nowScore;
         scoreTMPro.text = "your score : " + score.ToString();
         SetRanking();
     }
     
     public void SetRankingPanel()
     {
         GameObject rankingPanelPrefab = Instantiate(rankingPanel);
         rankingPanelPrefab.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
         rankingPanelPrefab.transform.localPosition = Vector3.zero;
     }

     bool IsHighestScore()
     {
         string userUuid = "1b2b2030-5fd4-11ed-b853-792fe3784dea";

         int limit = 100;

         List<RankItem> rankItemList = new List<RankItem>();

         BackendReturnObject bro = Backend.URank.User.GetMyRank(userUuid, limit);

         if (bro.IsSuccess())
         {
             JsonData rankListJson = bro.GetFlattenJSON();
             string extraName = string.Empty;
             for (int i = 0; i < rankListJson["rows"].Count; i++)
             {
                 RankItem rankItem = new RankItem();

                 rankItem.gamerInDate = rankListJson["rows"][i]["gamerInDate"].ToString();
                 rankItem.nickname = rankListJson["rows"][i]["nickname"].ToString();
                 rankItem.score = rankListJson["rows"][i]["score"].ToString();
                 rankItem.index = rankListJson["rows"][i]["index"].ToString();
                 rankItem.rank = rankListJson["rows"][i]["rank"].ToString();
                 rankItem.totalCount = rankListJson["totalCount"].ToString();

                 if (rankListJson["rows"][i].ContainsKey(rankItem.extraName))
                 {
                     rankItem.extraData = rankListJson["rows"][i][rankItem.extraName].ToString();
                 }

                 rankItemList.Add(rankItem);
                 Debug.Log(rankItem.ToString());
             }
         }

         for (int i = 0; i < rankItemList.Count; i++)
         {
             if (Int32.Parse(rankItemList[i].score) >= score)
             {
                 return false;
             }
         }

         return true;
     }

     void SetRanking()
     {
         string tableName = "Ranking";
         string rowIndate = string.Empty;
         string rankingUUID = "1b2b2030-5fd4-11ed-b853-792fe3784dea";

         /*bool isHighestScore = IsHighestScore();
         
         Debug.Log("IsHighestScore : " + isHighestScore);
         
         if (isHighestScore == false)
         {
             return;
         }*/

         Param param = new Param();
         param.Add("Score", score);

         var bro = Backend.GameData.Get("Ranking", new Where());

         if (bro.IsSuccess())
         {
             if (bro.FlattenRows().Count > 0)
             {
                 rowIndate = bro.FlattenRows()[0]["inDate"].ToString();
             }
             else
             {
                 var bro2 = Backend.GameData.Insert(tableName, param);

                 if (bro2.IsSuccess())
                 {
                     rowIndate = bro2.GetInDate();
                 }
                 else
                 {
                     return;
                 }

             }
         }
         else
         {
             return;
         }

         if (rowIndate == string.Empty)
         {
             return;
         }

         var rankBro = Backend.URank.User.UpdateUserScore(rankingUUID, tableName, rowIndate, param);
         if (rankBro.IsSuccess())
         {
             Debug.Log("랭킹 등록 성공");
         }
         else
         {
             Debug.Log("랭킹 등록 실패 : " + rankBro);
         }
     }


     IEnumerator Co_SetPanelAnimation()
    {
        float deltaRate = 50f;

        float time = 0f;
        float destTime = 0.15f;
        float firstScale = -deltaRate * destTime * destTime + 1;
        transform.localScale = new Vector3(firstScale, firstScale, 1f);
        
        
        while (true)
        {
            time += Time.unscaledDeltaTime;
            transform.localScale = new Vector3(1f, -deltaRate * (time - destTime) * (time - destTime) + 1, transform.localScale.z);

            if (time >= destTime)
            {
                time = 0f;
                transform.localScale = new Vector3(1f, 1f, transform.localScale.z);
                break;
            }

            yield return null;
        }

        Time.timeScale = 0f;

    }
    
    

    public void GoToMainScene()
    {
        GameManager.instance.LoadScene("MainScene");
    }
}
