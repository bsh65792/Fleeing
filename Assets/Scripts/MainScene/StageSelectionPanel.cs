using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectionPanel : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine("Co_SetPanelAnimation");
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
            time += Time.deltaTime;
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
            time += Time.deltaTime;
            transform.localScale = new Vector3(transform.localScale.x, -deltaRate * (time - destTime) * (time - destTime) + 1, transform.localScale.z);

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
            time += Time.deltaTime;
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






    public void GoToSimulationScene()
    {
        GameManager.instance.LoadScene("SimulationScene");
    }

    public void GoToGameScene()
    {
        GameManager.instance.LoadScene("GameScene");
    }
}
