using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlarmPanel : MonoBehaviour
{
    public TextMeshProUGUI alarmTMPro;
    public string alarmText;
    public float alarmTime;

    private Image image;
    void Start()
    {
        image = GetComponent<Image>();
        StartCoroutine("Co_DestroyAlarm");
        alarmTMPro.text = alarmText;
    }

    IEnumerator Co_DestroyAlarm()
    {
        yield return new WaitForSeconds(alarmTime);
        float time = 0f;
        float a = image.color.a;
        
        while (true)
        {
            time += Time.deltaTime;
            image.color = new Color(1f, 1f, 1f, a * (1 - time));
            
            if (time >= 1f)
            {
                Destroy(gameObject);
                break;
            }

            yield return null;
        }
    }
    
    
    
}
