using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;

public class RankingPanel : MonoBehaviour
{
    public UserRankingPanel[] userRankingPanel;
    
    void OnEnable()
    {
        StartCoroutine("Co_SetPanelAnimation");
        LoadRanking();
    }



    void LoadRanking()
    {
        string userUuid = "1b2b2030-5fd4-11ed-b853-792fe3784dea";

        int limit = 100;

        List<RankItem> rankItemList = new List<RankItem>();

        BackendReturnObject bro = Backend.URank.User.GetRankList(userUuid, limit);

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


        for (int i = 0; i < 10; i++)
        {
            if (i < rankItemList.Count)
            {
                userRankingPanel[i].SetString("<" + (i + 1).ToString() + "등>", "닉네임 : " + rankItemList[i].nickname + "\n스코어 : " + rankItemList[i].score);
            }
            else
            {
                userRankingPanel[i].SetString("", "기록 없음");
            }
            
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
        
    }

    public void DestroyThisPanel()
    {
        StartCoroutine("Co_DestroyThisPanel");
    }

    IEnumerator Co_DestroyThisPanel()
    {
        float destTime = 0.25f;
        float time = 0f;
        float firstScaleY = transform.localScale.y;

        while (true)
        {
            time += Time.unscaledDeltaTime;
            transform.localScale = new Vector3(transform.localScale.x, firstScaleY * (1 - time * time * 4f), transform.localScale.z);

            if (time >= destTime)
            {
                time = 0f;
                transform.localScale = new Vector3(transform.localScale.x, 0f, transform.localScale.z);
                GameObject.Destroy(gameObject);
                break;
            }

            yield return null;
        }
    }
}



public class RankItem
{
    public string gamerInDate;
    public string nickname;
    public string score;
    public string index;
    public string rank;
    public string extraData = string.Empty;
    public string extraName = string.Empty;
    public string totalCount;

    public override string ToString()
    {
        string str = $"유저인데이트:{gamerInDate}\n닉네임:{nickname}\n점수:{score}\n정렬:{index}\n순위:{rank}\n총합:{totalCount}\n";
        if (extraName != string.Empty)
        {
            str += $"{extraName}:{extraData}\n";
        }
        return str;
    }
}

