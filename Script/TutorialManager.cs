using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
* 튜토리얼 매니저 스크립트
*/

public class TutorialManager : MonoBehaviour
{
    #region 변수
    // public GameObject[] Monster = new GameObject[4];         // 게임내 4개의 이미지 UI를 저장할 배열선언 및 초기회
    public GameObject[] Tuto = new GameObject[5];          // 캐릭터와 텍스트를 가진 이미지 파일을 저장할 배열선언 및 초기회 
    public int tutoNum;                                    // 튜토리얼 순서를 int형 변수로 저장
    #endregion

    public void Button()
    {
        tutoNum++;

        if (tutoNum == 2)
            GameObject.Find("SubCamera").gameObject.transform.Find("Canvas").
                gameObject.transform.Find("Tuto3_Area").gameObject.SetActive(true);
        if (tutoNum == 3)
            GameObject.Find("SubCamera").gameObject.transform.Find("Canvas").
                gameObject.transform.Find("Tuto3_Area").gameObject.SetActive(false);
        
        SoundManager.Instance.Btn_Click();                 // 버튼 클릭시 소리나게 함
        Tuto[tutoNum-1].SetActive(false);
        Tuto[tutoNum].SetActive(true);
    }
}