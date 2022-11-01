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
            DontDestroyOnLoad(gameObject);
            
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
        else
        {
            Destroy(this);
        }
    }

    void Update()
    {
        //비동기 처리를 위함
        Backend.AsyncPoll();
    }
    


    public void TryLogIn()
    {
        string id = Background_LogInScene.instance.idInputField.text;
        string password = Background_LogInScene.instance.passwordInputField.text;

        if (id == "")
        {
            GameManager.instance.SetAlarm("id를 입력하세요");
            return;
        }
        
        if (password == "")
        {
            GameManager.instance.SetAlarm("비밀번호를 입력하세요");
            return;
        }
        
        
        BackendReturnObject bro = Backend.BMember.CustomLogin ( id , password );
        
        if(bro.IsSuccess())
        {
            GameManager.instance.SetAlarm("로그인에 성공하였습니다.");
            GameManager.instance.LoadScene("MainScene");
        }
        else
        {
            if (bro.GetStatusCode() == "400")
            {
                GameManager.instance.SetAlarm("device_unique_id를 확인할 수 없습니다.");
            }
            else if (bro.GetStatusCode() == "401")
            {
                GameManager.instance.SetAlarm("아이디 혹은 패스워드가 틀렸습니다.");
            }
            else
            {
                GameManager.instance.SetAlarm("기타 오류가 발생하였습니다.");
                Debug.Log("Error Code : " + bro.GetErrorCode());
            }
        }
    }

    public void TryCreateAccount()
    {
        string id = Background_SignInScene.instance.idInputField.text;
        string password = Background_SignInScene.instance.passwordInputField.text;
        string passwordConfirm = Background_SignInScene.instance.passwordConfirmInputField.text;

        if (id == "")
        {
            GameManager.instance.SetAlarm("아이디를 입력하세요.");
            return;
        }
        if (password == "")
        {
            GameManager.instance.SetAlarm("비밀번호를 입력하세요.");
            return;
        }
        
        if (passwordConfirm == "")
        {
            GameManager.instance.SetAlarm("확인용 비밀번호를 입력하세요.");
            return;
        }
        
        if (password != passwordConfirm)
        {
            GameManager.instance.SetAlarm("패스워드가 일치하지 않습니다.");
            return;
        }
        
        BackendReturnObject bro = Backend.BMember.CustomSignUp(id, password);
        if(bro.IsSuccess())
        {
            GameManager.instance.SetAlarm("회원가입에 성공했습니다.");
        }
        else
        {
            if (bro.GetStatusCode() == "409")
            {
                GameManager.instance.SetAlarm("이미 존재하는 ID입니다.");
            }
            else if (bro.GetStatusCode() == "401")
            {
                GameManager.instance.SetAlarm("서버 점검중입니다.");
            }
            else if (bro.GetStatusCode() == "400")
            {
                GameManager.instance.SetAlarm("디바이스 정보가 없습니다.");
            }
            else if (bro.GetStatusCode() == "403")
            {
                GameManager.instance.SetAlarm("차단당한 Device 이거나 서버 과부하 상태입니다.");
            }
            
        }
    }
}
