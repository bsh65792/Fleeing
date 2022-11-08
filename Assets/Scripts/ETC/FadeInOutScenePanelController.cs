using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOutScenePanelController : MonoBehaviour
{
    public GameObject fadeInScenePanel;

    void Awake()
    {
        GameObject fadeInScenePanelPrefab = Instantiate(fadeInScenePanel);
        fadeInScenePanelPrefab.transform.SetParent(gameObject.transform);
        fadeInScenePanelPrefab.transform.localPosition = Vector3.zero;
        fadeInScenePanelPrefab.transform.localScale = Vector3.one;
        Time.timeScale = 1f;
    }
}
