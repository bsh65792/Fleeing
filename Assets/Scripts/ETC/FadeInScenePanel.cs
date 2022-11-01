using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInScenePanel : FadeInOutScenePanel
{
    void OnEnable()
    {
        panelImage = GetComponent<Image>();
        StartCoroutine(Co_StartFadeIn());
    }

    
    IEnumerator Co_StartFadeIn()
    {
        float time = 0f;

        while (true)
        {
            time += Time.deltaTime;
            panelImage.color = new Color(0f, 0f, 0f, (fadeInOutTime - time) / fadeInOutTime);

            if (time >= fadeInOutTime)
            {
                time = 0f;
                Destroy(gameObject);
            }

            yield return null;
        }
    }
}
