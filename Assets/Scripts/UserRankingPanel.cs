using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserRankingPanel : MonoBehaviour
{
    public TextMeshProUGUI rankTMPro;
    public TextMeshProUGUI infoTMPro;
    
    public void SetString(string rank, string info)
    {
        rankTMPro.text = rank;
        infoTMPro.text = info;
    }
}
