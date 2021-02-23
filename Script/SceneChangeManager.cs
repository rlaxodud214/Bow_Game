using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    public int time;
    #region Singleton
    private static SceneChangeManager _Instance;    // 싱글톤 패턴을 사용하기 위한 인스턴스 변수, static으로 선언하여 어디서든 접근 가능

    // 인스턴스에 접근하기 위한 프로퍼티
    public static SceneChangeManager Instance
    {
        get { return _Instance; }   // SceneChangeManager 인스턴스 변수를 리턴
    }

    // 인스턴스 변수 초기화
    void Awake()
    {
        _Instance = GetComponent<SceneChangeManager>();    // _sceneManager에 SceneChangeManager의 컴포넌트(자기 자신)에 대한 참조를 얻음
    }
    #endregion

    private void Start()
    {
        time = 0;
    }
    public void RePlay_inGame() // 다시시작 버튼을 눌렀을때
    {
        SoundManager.Instance.Btn_Click();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }

    public void RePlay_inTuto() // 다시시작 버튼을 눌렀을때
    {
        SoundManager.Instance.Btn_Click();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Tuto");
    }

    public void Home() // 홈 버튼을 눌렀을때
    {
        SoundManager.Instance.Btn_Click();
        SceneManager.LoadScene("Main");
    }

    public void Tutorial() // 홈 버튼을 눌렀을때
    {
        SoundManager.Instance.Btn_Click();
        SceneManager.LoadScene("Tuto");
    }

    public void Tutorial_Go_Game() // 홈 버튼을 눌렀을때
    {
        Debug.Log("Tutorial_Go_Game() 호출완료1");
        bool pass = true;
        
        if (time == 4)
        {
            pass = false;
            SceneManager.LoadScene("Game");
        }
        Debug.Log("Tutorial_Go_Game() 호출완료2");
        if (time == 0) // 초기 설정
        {
            GameObject.Find("SubCamera").SetActive(false);  // 서브카메라 오브젝트 false
            // 카운트 다운하는 패널 true
            GameObject.Find("Canvas").gameObject.transform.Find("Tuto")
                .gameObject.transform.Find("CountDown").gameObject.SetActive(true);
        }
        Debug.Log("Tutorial_Go_Game() 호출완료3");
        // 바로 시작하지 말고 3 2 1 start를 출력후 게임씬으로 전환하기
        if (time == 3)
            GameObject.Find("CountDown_Text").GetComponent<Text>().text = "Start";

        else if (time != 4)
            GameObject.Find("CountDown_Text").GetComponent<Text>().text = (3 - time).ToString();
        Debug.Log("Tutorial_Go_Game() 호출완료4");
        if (pass)
        {
            if (time == 3)
                Invoke("Tutorial_Go_Game", 0.3f); // start 출력후 0.3초뒤 게임 시작
            else
                Invoke("Tutorial_Go_Game", 1f);   // 3 2 1 숫자는 1초씩 딜레이
        }
            

        time++;
    }

    public void GameExit() // 종료하기 버튼을 눌렀을때
    {
        SoundManager.Instance.Btn_Click();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                        Application.Quit(); // 어플리케이션 종료
#endif
    }
}
