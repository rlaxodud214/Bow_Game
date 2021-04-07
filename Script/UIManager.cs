using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject ResultPanel;
    public GameObject PausePanel;
    public GameObject StagePanel;
    public GameObject Border_Life_Panel;
    public Text Monster_Spawn_Slot_Up;
    public bool pause = false; // 일시정지시 몬스터 움직임을 없애기 위해서 선언
    public bool check = false;
    public GameObject origin;

    public Text MaxScore_Text; // 최고점수
    public Text test;          // 디버깅용 - 게임씬 - 화면 상단에 text오브젝트 두고 상태나 오류확인
    public Text test_Result;   // 빌드시 디버깅용 - 게임씬 - 결과패널에 GAMEOVER 텍스트 오브젝트 유니티에서 넣기 - 상태나 오류 확인
    public int maxScore;
    public string date;
    public string userID;
    public string gameID;
    public string sql;
    public Dictionary<string, float> Dic = new Dictionary<string, float>();
    public List<string> series1Data2;

    // 싱글톤 패턴
    #region Singleton
    private static UIManager _Instance;    // 싱글톤 패턴을 사용하기 위한 인스턴스 변수, static으로 선언하여 어디서든 접근 가능
    
    // 인스턴스에 접근하기 위한 프로퍼티
    public static UIManager Instance
    {
        get { return _Instance; }          // UIManager 인스턴스 변수를 리턴
    }

    // 인스턴스 변수 초기화
    void Awake()
    {
        _Instance = this;  // _uiManager에 UIManager의 컴포넌트(자기 자신)에 대한 참조를 얻음

        // 오늘날짜 + 현재시간
        date = DateTime.Now.ToString("yyyy년 MM월 dd일 HH시 mm분 ss초");
        date = SqlFormat(date);

        userID = SqlFormat("유저아이디2"); // 나중에 로그인 구현시 Column 클래스로 만들어서 그때 객체화하기
        gameID = SqlFormat("21arrow");     // 각자 게임 이름에 맞게 변경
        maxScore = 0;
        MaxScore_Text.text = "최고점수 : " + maxScore;

        DBManager.DbConnectionCHek();

        // gameScore를 기준으로 내림차순 정렬후 제일 첫 레코드의 값을 가져오기
        DBManager.DataBaseRead(string.Format("SELECT * FROM Game WHERE userID = {0} ORDER BY gameScore DESC ", userID));
        while (DBManager.dataReader.Read())                            // 쿼리로 돌아온 레코드 읽기
        {
            // Debug.Log(DBManager.dataReader.GetInt32(5));               // 5번 점수 필드 읽기
            int maxScore = DBManager.dataReader.GetInt32(5);
            MaxScore_Text = UIManager.Instance.MaxScore_Text;
            MaxScore_Text.text = "최고점수 : " + maxScore;
            break; // 내림차순 정렬이므로 처음에 한 번만 레코드값을 가져오면 된다.
        }
        DBManager.DBClose();

        DBManager.DataBaseRead(string.Format("SELECT * FROM Game WHERE userID = {0}", userID));
        while (DBManager.dataReader.Read())                            // 쿼리로 돌아온 레코드 읽기
        {
            string date = DBManager.dataReader.GetString(0).Substring(0, 13);
            float maxRotation = (float)DBManager.dataReader.GetDouble(3);

            if (Dic.Keys.Count == 0)
                Dic.Add(date, maxRotation);

            if (Dic.ContainsKey(date).Equals(true))
            {   // date값이 이미 키값으로 들어가있다면 Add 하지 않고 뒤에 벨류값만 비교함.
                if (Dic[date] < maxRotation) { Dic[date] = maxRotation; }
            }
            else
            {
                Dic.Add(date, maxRotation);
            }

            // 디비의 모든 데이터를 출력해주는 코드
            // Debug.Log("date : " + date + ", Dic[date] : " + Dic[date]);
        }

        // 2021년 03월 04일 16시 47분 46초 -> 3/4로 변경하는 코드
        foreach (KeyValuePair<string, float> item in Dic)
        {
            // Debug.Log(item.Key + " : " + item.Value);
            if (item.Value != 0)
            {
                if (item.Key.Substring(10, 1) == "0") // 0~9일과 10일~30일 구분해서 처리하기
                    series1Data2.Add(item.Key.Substring(7, 1) + "/" + item.Key.Substring(11, 1) + ", " + item.Value.ToString("N0")); // ex) 3/3
                else
                    series1Data2.Add(item.Key.Substring(7, 1) + "/" + item.Key.Substring(10, 2) + ", " + item.Value.ToString("N0")); // ex) 3/15
            }
        }
        // 디비에서 뽑아온 데이터 확인하는 코드
        //for (int i = 0; i < series1Data2.Count; i++)
        //{
        //    Debug.Log("i : " + i + "  series1Data2[i]" + series1Data2[i]);
        //}

        DBManager.DBClose();
    }
    public string SqlFormat(string sql)
    {
        return string.Format("\"{0}\"", sql);
    }
    #endregion

    public void Player_damage_on()  // 데미지 입었을때
    {
        Border_Life_Panel.GetComponent<Image>().color = GameManager.Instance.HexToColor("EF2D2378");
        Invoke("Player_damage_off", 0.3f);
    }
    public void Player_damage_off() // 원상태로 되돌리기
    {
        Border_Life_Panel.GetComponent<Image>().color = GameManager.Instance.HexToColor("5E5E0C31");
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
        Player_damage_on(); // 데미지 패널 추가 - 오류나서 안됌
        GameManager.Instance.life = life;
        //life == 0이 되면 게임오버
        if (life < 0)
        {
            GameOver();                  // 생명이 없으므로 게임 오버메소드 호출
        }
    }
    
    public void GameOver() //게임오버+결과화면 함수
    {
        float num1 = (float)((GameManager.Instance.SliderMaxValue - GameManager.Instance.SliderMinValue) * ((Double)1 / (Double)140));
        origin.transform.GetComponent<Image>().fillAmount = num1;

        float Rotate = (float)((90 - GameManager.Instance.SliderMinValue) * ((Double)18 / (Double)7));
        origin.transform.rotation = Quaternion.Euler(0, 0, Mathf.Abs(Rotate)); // Mathf.Abs : 절대값

        Time.timeScale = 0f;
        // 캔버스의 서브카메라가 결과 패널을 가려서 false 시킴
        SoundManager.Instance.Gameover();
        int score = GameManager.Instance.count * 10;
        string playtime = GameManager.Instance.time.ToString("N2") + "초";
        float playtime_REAL = (float)(Math.Truncate(GameManager.Instance.time*100)/100); // 소수점 2째자리까지 나타내주는 코드 - 이하 버림
        GameManager.Instance.RESULT_score.text = "점수 : " + score.ToString();
        GameManager.Instance.RESULT_time.text = "게임시간 : " + playtime;
        ResultPanel.SetActive(true);

        // 디비 연동 코드
        string sql = string.Format("Insert into Game(date, userID, gameID, angleOfRotation, gamePlayTime, gameScore) " +
        "VALUES( {0}, {1}, {2}, {3}, {4}, {5} )", date, userID, gameID, num1*180, playtime_REAL, score);
        Debug.Log("date : " + date);
        Debug.Log("userID : " + userID);
        Debug.Log("gameID : " + gameID);
        DBManager.DatabaseSQLAdd(sql);
    }

    public void Menu() // 일시정지 버튼을 눌렀을때
    {
        Time.timeScale = 0f;
        SoundManager.Instance.Btn_Click();
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

    public void StagePanel_on(int i)
    {
        SoundManager.Instance.stageUp();
        StagePanel.SetActive(true);

        if (i == 1 || i == 3) // 3스테이지 or 5스테이지
        {
            Monster_Spawn_Slot_Up.gameObject.SetActive(true);
            check = true;
            Invoke("StagePanel_off", 1.5f);
        }
        else
        {
            Invoke("StagePanel_off", 1.2f);
        }
        UIManager.Instance.pause = true;
    }

    public void StagePanel_off()
    {
        UIManager.Instance.pause = false;
        if (check)
            Monster_Spawn_Slot_Up.gameObject.SetActive(false);
        
        StagePanel.SetActive(false);
    }

 
}