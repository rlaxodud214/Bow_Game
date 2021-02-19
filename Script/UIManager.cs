using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject ResultPanel;
    public GameObject PausePanel;
    public GameObject StagePanel;
    public GameObject DamagePanel;
    public GameObject Border_Life_Panel;
    public bool pause = false; // 일시정지시 몬스터 움직임을 없애기 위해서 선언

    // 싱글톤 패턴
    #region Singleton
    private static UIManager _Instance;    // 싱글톤 패턴을 사용하기 위한 인스턴스 변수, static으로 선언하여 어디서든 접근 가능
    
    // 인스턴스에 접근하기 위한 프로퍼티
    public static UIManager Instance
    {
        get { return _Instance; }  // UIManager 인스턴스 변수를 리턴
    }

    // 인스턴스 변수 초기화
    void Awake()
    {
        _Instance = GetComponent<UIManager>();  // _uiManager에 UIManager의 컴포넌트(자기 자신)에 대한 참조를 얻음
    }
    #endregion

    public void Player_damage_on()  // 데미지 입었을때
    {
        DamagePanel.SetActive(true);
        PausePanel.gameObject.SetActive(true);
        Border_Life_Panel.SetActive(false);
        Invoke("Player_damage_off", 0.15f);
    }
    public void Player_damage_off() // 데미지 입었을때
    {
        Border_Life_Panel.SetActive(true);
        DamagePanel.SetActive(false);
    }

    public void ui()
    {
        var Life = GameManager.Instance.Life;
        var life = GameManager.Instance.life;
        //틀린경우 라이프 감소
        life--;                          // 인덱스 1 감소
        if (life >= 0)
            Life[life+1].SetActive(false);     // 아까 life를 2로 초기화한 이유는 인덱스 값이 0 1 2 이므로 3번째 값을 호출할 때 2를 사용해서 2로 초기화 시킴.
        SoundManager.Instance.Player_damage();
        // Player_damage_on(); // 데미지 패널 추가 - 오류나서 안됌
        GameManager.Instance.life = life;
        //life == 0이 되면 게임오버
        //if (life < 0)
        //{
        //    DamagePanel.SetActive(false);
        //    GameOver();                  // 생명이 없으므로 게임 오버메소드 호출
        //}
    }

    public void GameOver() //게임오버+결과화면 함수
    {
        Time.timeScale = 0f;
        // 캔버스의 서브카메라가 결과 패널을 가려서 false 시킴
        GameObject.Find("SubCamera").gameObject.SetActive(false); 
        SoundManager.Instance.Gameover();
        ResultPanel.SetActive(true);
    }

    public void Menu() // 일시정지 버튼을 눌렀을때
    {
        SoundManager.Instance.Btn_Click();
        Time.timeScale = 0f;
        pause = true;
        PausePanel.SetActive(true);
    }

    public void Play() // 계속하기 버튼을 눌렀을때
    {
        SoundManager.Instance.Btn_Click();
        Time.timeScale = 1f;
        pause = false;
        PausePanel.SetActive(false);
    }

    public void StagePanel_on()
    {
        SoundManager.Instance.stageUp();

        StagePanel.SetActive(true);
        Invoke("StagePanel_off", 1.2f);
    }

    public void StagePanel_off()
    {
        StagePanel.SetActive(false);
    }
}

