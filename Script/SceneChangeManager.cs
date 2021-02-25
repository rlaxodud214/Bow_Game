using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    public int time;
    public bool pass;
    public GameObject Main_Monster;

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
        Time.timeScale = 1f;
        SoundManager.Instance.Btn_Click();
        SceneManager.LoadScene("Game");
    }

    public void RePlay_inTuto() // 다시시작 버튼을 눌렀을때
    {
        Time.timeScale = 1f;
        SoundManager.Instance.Btn_Click();
        SceneManager.LoadScene("Tuto");
    }

    public void Home() // 홈 버튼을 눌렀을때
    {
        Time.timeScale = 1f;
        SoundManager.Instance.Btn_Click();
        SceneManager.LoadScene("Main");
    }

    public void Tutorial() // 튜토리얼 버튼을 눌렀을때
    {
        SoundManager.Instance.Btn_Click();
        SceneManager.LoadScene("Tuto");
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

    public void Main_Monster_Animation()
    {
        Debug.Log("Main_Monster_Animation() 호출 완료");
        Animator my_animator = Main_Monster.gameObject.GetComponent<Animator>();

        switch(Random.Range(0, 2))
        {
            case 0:
                my_animator.SetTrigger("attack");
                break;

            case 1:
                my_animator.SetTrigger("attack02");
                break;
        }
    }
}