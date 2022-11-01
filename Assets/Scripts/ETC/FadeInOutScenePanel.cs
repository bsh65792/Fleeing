using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class FadeInOutScenePanel : MonoBehaviour
{
    protected float fadeInOutTime;
    
    protected Image panelImage;
    
    public FadeInOutScenePanel()
    {
        fadeInOutTime = 0.6f;
    }
    
    public void SetFadeOutTime(float fadeOutTime)
    {
        this.fadeInOutTime = fadeOutTime;
    }
}
