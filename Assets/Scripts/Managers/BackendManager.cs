using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public class BackendManager : MonoBehaviour
{
    public static BackendManager instance;

    //싱글턴
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(this);
        }
    }
    
    void Start()
    {
        var bro = Backend.Initialize(true);
        if (bro.IsSuccess())
        {
            // 초기화 성공 시 로직
            Debug.Log("Backend 초기화 성공");
        }
        else
        {
            // 초기화 실패 시 로직
            Debug.LogError("Backend 초기화 실패");
        }
    }
}
