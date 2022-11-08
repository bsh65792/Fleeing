using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultPanel : MonoBehaviour
{
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
            transform.localScale = new Vector3(-deltaRate * (time - destTime) * (time - destTime) + 1, transform.localScale.y, transform.localScale.z);

            if (time >= destTime)
            {
                time = 0f;
                transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
                break;
            }

            yield return null;
        }

        yield return new WaitForSeconds(0.25f);
        
        while (true)
        {
            time += Time.unscaledDeltaTime;
            transform.localScale = new Vector3(transform.localScale.x, -deltaRate * (time - destTime) * (time - destTime) + 1, transform.localScale.z);

            if (time >= destTime)
            {
                time = 0f;
                transform.localScale = new Vector3(1f, 1f, transform.localScale.z);

                Time.timeScale = 0f;
                break;
            }

            yield return null;
        }
        
    }
    
    

    public void GoToMainScene()
    {
        GameManager.instance.LoadScene("MainScene");
    }
}
