using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeOutScenePanel : FadeInOutScenePanel
{
    private string nextSceneName;
    
    void OnEnable()
    {
        panelImage = GetComponent<Image>();
        StartCoroutine(Co_StartFadeOut());
    }

    public void SetNextSceneName(string nextSceneNameTemp)
    {
        nextSceneName = nextSceneNameTemp;
    }
    
    
    IEnumerator Co_StartFadeOut()
    {
        float time = 0f;

        while (true)
        {
            time += Time.deltaTime;
            panelImage.color = new Color(0f, 0f, 0f, time / fadeInOutTime);

            if (time >= fadeInOutTime)
            {
                time = 0f;
                SceneManager.LoadScene(nextSceneName);
            }

            yield return null;
        }
    }
}
