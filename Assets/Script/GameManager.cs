using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;                       
/*
 * 게임 매니저 스크립트
 */

// GameManager : 게임 진행 규칙들을 관리하는 매니저이다. / 게임 오버 관리: 플레이어 사망 시 사망 상태를 적용하여 게임 오버를 관리한다. 
// 게임 오버UI 활성화: 플레이어가 사망 상태 시 UIManager에 접근하여 GameoverUI를 출력한다.
// 점수 획득: 플레이어가 적을 처치 시 해당 적의 점수를 획득한다. / 초기 점수 설정: 게임 시작 시 초기 점수(0점)을 설정한다.
public class GameManager : MonoBehaviour
{
    #region 변수

    public GameObject[] Life = new GameObject[4];     // 남은 목숨을 저장할 배열
    public int life;                                  // 남은 목숨, 하트 갯수
    public int count = 0;
    public Text monster_v;
    public Text score;
    public Text stage;

    // 몬스터 오브젝트
    public List<GameObject> monster = new List<GameObject>();
    public Vector3[] monster_location = new Vector3[7]; 
    public int monster_count = 0;

    #endregion

    #region Singleton                                 // 싱글톤 패턴은 하나의 인스턴스에 전역적인 접근을 시키며 보통 호출될 때 인스턴스화 되므로 사용하지 않는다면 생성되지도 않습니다.

    private static GameManager _gManager;             // 싱글톤 객체 선언, 어디에서든지 접근할 수 있도록 하기위해 

    public static GameManager Game                    // 객체에 접근하기 위한 속성으로 내부에 get set을 사용한다.
    {
        get { return _gManager; }                     // GameManager 객체 리턴
    }

    void Awake()                                      // 제일 처음 호출되는 함수
    {
        _gManager = GetComponent<GameManager>();      // _gManager라는 변수에 자신의 GameManager 컴포넌트를 참조하는 값을 저장, Game속성에 set코드를 짜면 다르게 대입가능
        for(int i=0; i<7; i++)
        {
            monster.Add(GameObject.FindGameObjectsWithTag("Monster")[i]);
            monster_location[i] = monster[i].transform.position;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // LevelText.text = SceneChangeManager.SCENE.Level + " Back"; //n Back 텍스트 설정
        stage.text = "1"; //n Back 텍스트 설정
        score.text = bulletMove.bullet.Score.ToString(); //n Back 텍스트 설정
        monster_v.text = "몬스터 속도 : 0.05f";
        //stage&life 변수 초기화
        life = 3;
    }

    void Update()
    {
        monster_count = GameObject.FindGameObjectsWithTag("Monster").Length;
        monster_v.text = "몬스터 속도 : " + Monster.state.MonsterSpeed.ToString() + "f";
        for (int i = 0; i < monster.Count; i++) {
            Debug.Log("monster_count : " + monster_count);
            if (monster[i] == null)
            {
                if (monster_count < 6) // 몬스터의 수가 6이하이면 랜덤 위치에 생성
                {
                    int num1 = Random.Range(1, 8);
                    int num2 = Random.Range(1, 5);
                    for (int j = 0; j < monster.Count; j++)
                    {
                        if (monster[j] != null)
                            monster.Add(Instantiate(monster[j], monster_location[num1 % 7] + Vector3.up * (num2 % 4), Quaternion.identity)); // 새로운 몬스터 생성 Quaternion.identity : 회전값 지정 - 불필요
                        break;
                    }
                }
            }
            
        }
    }

    public void countPlus() { count++; } // 적 처치 횟수 증가

    public void ui() { 
        //틀린경우 라이프 감소
        if(life > 0)
            Life[life].SetActive(false);     // 아까 life를 2로 초기화한 이유는 인덱스 값이 0 1 2 이므로 3번째 값을 호출할 때 2를 사용해서 2로 초기화 시킴.
        life--;                          // 인덱스 1 감소
        Debug.Log("life : " + life);
        //life == 0이 되면 게임오버
        if (life < 0)
        {
            GameOver();                  // 생명이 없으므로 게임 오버메소드 호출
        }
    }

    public void GameOver() //게임오버+결과화면 함수
    {
        Time.timeScale = 0f;

        if (count > 40)
            stage.text = "2";
        else if (count > 90)
            stage.text = "3";
        else if (count > 150)
            stage.text = "4";
        else if (count > 210)
            stage.text = "5";
        else if (count > 280)
            stage.text = "6";
        score.text = count.ToString();
        
        GameObject.Find("Canvas").transform.Find("ResultPanel").gameObject.SetActive(true); //결과화면 띄우기 // GameObject Canvas내부의 ResultPanel 패널을 찾아 띄우기.
    }

    public void ReGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");    
    }
}
