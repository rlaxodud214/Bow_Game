﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
* 튜토리얼 매니저 스크립트
*/

public class TutorialManager : MonoBehaviour
{
    #region Singleton  
    // public GameObject[] Monster = new GameObject[4];         // 게임내 4개의 이미지 UI를 저장할 배열선언 및 초기회
    public GameObject[] Tuto = new GameObject[7];          // 캐릭터와 텍스트를 가진 이미지 파일을 저장할 배열선언 및 초기회 
    public GameObject CountDown;
    public GameObject backPanel;
    public int tutoNum;                                 // 튜토리얼 순서를 int형 변수로 저장
    public bool check;

    private static TutorialManager _Instance;          // 싱글톤 패턴을 사용하기 위한 인스턴스 변수, static 선언으로 어디서든 참조가 가능함
    public static TutorialManager Instance                    // 객체에 접근하기 위한 속성으로 내부에 get set을 사용한다.
    {
        get { return _Instance; }                         // _sceneManager이 변수값을 리턴받을 수 있음.
    }
    #endregion 
    
    public void Awake()
    {
        check = false;
    }

    public void Start()
    {
        if (check)
            StartCoroutine(StartCountDown());
        GameManager.Instance.isTuto = true;
    }
    public void Button()
    {
        tutoNum++;
        SoundManager.Instance.Btn_Click();                 // 버튼 클릭시 소리나게 함

        if (tutoNum < 4)
            Tuto[tutoNum - 1].SetActive(false);
        
        switch(tutoNum)
        {
            // 슬라이더 연습 안내
            case 5:
                Tuto[4].SetActive(true);
                break;

            // 슬라이더 5초 연습시작
            case 6:
                Tuto[4].SetActive(false);
                break;

            // 몬스터 4마리 스폰 안내
            case 7:
                Tuto[5].SetActive(true);
                break;

            // 몬스터 4마리 스폰
            case 8:
                Tuto[5].SetActive(false);
                break;

            // 잘하셨어요! 그럼 이제 게임을 시작해볼까요?
            case 9:
                Tuto[6].SetActive(true);
                break;
        }

        if (tutoNum == 10)
        {
            check = true;
            Start();
        }
        
        if (tutoNum < 4)
            Tuto[tutoNum].SetActive(true);
    }

    public IEnumerator StartCountDown()
    {
        Debug.Log("StartCountDown() 코루틴 호출 완료");
        if (check) {
            backPanel.SetActive(false);
            CountDown.SetActive(true);   // 카운트 다운하는 패널 true
            
            //GameObject.Find("CountDown_Text").GetComponent<Text>().text = "5";
            //yield return new WaitForSeconds(1.0f);
            //GameObject.Find("CountDown_Text").GetComponent<Text>().text = "4";
            //yield return new WaitForSeconds(1.0f);
            GameObject.Find("CountDown_Text").GetComponent<Text>().text = "3";
            yield return new WaitForSeconds(1.0f);
            GameObject.Find("CountDown_Text").GetComponent<Text>().text = "2";
            yield return new WaitForSeconds(1.0f);
            GameObject.Find("CountDown_Text").GetComponent<Text>().text = "1";
            yield return new WaitForSeconds(1.0f);
            GameObject.Find("CountDown_Text").GetComponent<Text>().text = "Start";
            yield return new WaitForSeconds(0.3f);
            SceneManager.LoadScene("Game");  // 게임시작함수
        }
    }
}
