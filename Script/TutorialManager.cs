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
    public GameObject[] Monster = new GameObject[4];         // 게임내 4개의 이미지 UI를 저장할 배열선언 및 초기회
    public GameObject[] Tuto = new GameObject[5];          // 캐릭터와 텍스트를 가진 이미지 파일을 저장할 배열선언 및 초기회 
    public Button Okay;
    public bool clear = false;
    public int tutoNum;                                    // 튜토리얼 순서를 int형 변수로 저장
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;                               // 확인 버튼을 눌러야 시간이 흐를텐데 초기값을 1f로 한 이유 아래에서 찾기
        for (int i = 0; i < Monster.Length; i++)
            Monster[i].SetActive(false);
    }
    private void Update()
    {
        if(Monster[0] == null && Monster[1] == null && Monster[2] == null && Monster[3] == null && !clear)
        {
            clear = true;
            tutoNum = 5;
            OkayButton();
        }
    }

    public void OkayButton()
    {
        SoundManager.Instance.Btn_Click();                 // 버튼 클릭시 소리나게 함

        // 게임진행
        // 0-> 1-> 2 -> 3 -> 4
        switch (tutoNum)
        {
            case 0:
                Tuto[tutoNum].SetActive(false);
                tutoNum++;
                Okay.gameObject.SetActive(false);
                Invoke("OkayButton", 5f);
                break;

            case 1:
                Tuto[tutoNum].SetActive(true);
                Okay.gameObject.SetActive(true);
                tutoNum++;
                break;

            case 2:
                Tuto[tutoNum - 1].SetActive(false);
                Tuto[tutoNum].SetActive(true);
                tutoNum++;
                break;

            case 3: 
                Tuto[tutoNum - 1].SetActive(false);

                // 몬스터 활성화!
                for (int i = 0; i < Monster.Length; i++)
                    Monster[i].SetActive(true);

                Tuto[tutoNum].SetActive(true);
                tutoNum++;
                break;

            case 4:
                Okay.gameObject.SetActive(false);
                Tuto[tutoNum-1].SetActive(false);
                break;

            case 5: // 몬스터 다 잡으면 활성화
                Tuto[tutoNum-1].SetActive(true);
                Okay.gameObject.SetActive(true);
                tutoNum++;
                break;

            case 6:
                Tuto[tutoNum-2].SetActive(false);
                Okay.gameObject.SetActive(false);
                SceneChangeManager.Instance.Home();
                break;
        }
    }
}