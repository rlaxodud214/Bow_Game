using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
* 튜토리얼 매니저 스크립트
*/

public class TutorialManager : MonoBehaviour
{
    #region Singleton  
    // public GameObject[] Monster = new GameObject[4];         // 게임내 4개의 이미지 UI를 저장할 배열선언 및 초기회
    public GameObject[] Tuto = new GameObject[5];          // 캐릭터와 텍스트를 가진 이미지 파일을 저장할 배열선언 및 초기회 
    public int tutoNum;                                    // 튜토리얼 순서를 int형 변수로 저장

    private static TutorialManager _Instance;          // 싱글톤 패턴을 사용하기 위한 인스턴스 변수, static 선언으로 어디서든 참조가 가능함
    public static TutorialManager Instance                    // 객체에 접근하기 위한 속성으로 내부에 get set을 사용한다.
    {
        get { return _Instance; }                         // _sceneManager이 변수값을 리턴받을 수 있음.
    }
    #endregion 

    public void Button()
    {
        tutoNum++;

        SoundManager.Instance.Btn_Click();                 // 버튼 클릭시 소리나게 함
        Tuto[tutoNum - 1].SetActive(false);

        if (tutoNum == 2)
            GameObject.Find("SubCamera").gameObject.transform.Find("Canvas").
                gameObject.transform.Find("Tuto3_Area").gameObject.SetActive(true);
        if (tutoNum == 3)
            GameObject.Find("SubCamera").gameObject.transform.Find("Canvas").
                gameObject.transform.Find("Tuto3_Area").gameObject.SetActive(false);
        if (tutoNum == 5)
            SceneChangeManager.Instance.Tutorial_Go_Game();
        else
            Tuto[tutoNum].SetActive(true);
        
        
        


    }
}