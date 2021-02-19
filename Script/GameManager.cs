﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
                     
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
    public Text RESULT_time;
    public Text RESULT_score;
    public Text RESULT_stage;
    public Text GAME_score;
    public Text STAGE_stage;
    public Text Count;
    public bool pass;

    // 활 스프라이트 변경
    // public Sprite[] sprites;
    // SpriteRenderer spriteRenderer;

    // 화살, 몬스터 생성관련 변수들
    public float time; // Time.deltatime값 누적하는 변수
    public float nextTime_Arrow = 0.0f;
    public float Timeplus_Arrow = 0.8f; // 화살 생성 주기 현재 : 1.0초
    public GameObject Arrow_Prefabs;  // 화살 복제시 사용할 프리팹
    public GameObject Arrow; // 화살 // bulletPos값을 저장하기 위해 초기 화살 오브젝트 변수 생성
    public Vector3 ArrowPos; // 화살 초기 위치(화살 생성 좌표값)
    public GameObject info;   // 화살 발사위치

    public float nextTime_Monster = 0.0f;
    public float Timeplus_Monster = 2.0f; // 몬스터 생성 주기 현재 : 1.5초

    // 몬스터 오브젝트
    public GameObject Monster_Prefabs;  // 몬스터 복제시 사용할 프리팹
    public List<GameObject> monster = new List<GameObject>();
    public Transform[] spawnPoints; // 몬스터 생성시 위치 배열
    public int monster_count = 0;

    // 오브젝트 이동속도
    public float MonsterSpeed; // 몬스터 이동속도
    public int ArrowSpeed = 5; // 화살 이동속도

    // 스테이지별로 변하는 값들 배열로 선언
    public int[] stage_up_Real = new int[5] { 40, 90, 150, 210, 280 }; // 실제
    public int[] stage_up = new int[5] { 4, 9, 15, 21, 28 };           // 테스트용

    public float[] Monster_Speed_Real;
    public float[] Monster_Speed_Test;
    public float[] Monster_Spawn;

    public int[] Arrow_Speed_Real; // 실제
    public int[] Arrow_Speed_Test; // 테스트용
    public float[] Arrow_Spawn;    // 실제
    
    List<List<int>> Row = 
        new List<List<int>>() {  new List<int> { 0, 0 }, new List<int> { 0, 0 },
                                 new List<int> { 0, 0 }, new List<int> { 2, 5 },  // 3라인
                                 new List<int> { 0, 0 }, new List<int> { 1, 6 },  // 5라인
                                 new List<int> { 0, 0 }, new List<int> { 0, 7 } }; // 7라인
    // 슬라이더 이벤트를 처리하는 2가지 방법
    // 1. 슬라이더의 값이 바뀔때, 함수를 호출하는 방법
    // 2. 스트립트에서 슬라이더에 접근해서 처리하는 방식 -> 사용

    // 슬라이더 관련
    public Slider slider; // 20 ~ 160 정수값 slider.value

    #endregion

    #region Singleton                                 // 싱글톤 패턴은 하나의 인스턴스에 전역적인 접근을 시키며 보통 호출될 때 인스턴스화 되므로 사용하지 않는다면 생성되지도 않습니다.

    private static GameManager _Instance;             // 싱글톤 객체 선언, 어디에서든지 접근할 수 있도록 하기위해 

    public static GameManager Instance                    // 객체에 접근하기 위한 속성으로 내부에 get set을 사용한다.
    {
        get { return _Instance; }                     // GameManager 객체 리턴
    }

    void Awake()                                      // 제일 처음 호출되는 함수
    {
        _Instance = GetComponent<GameManager>();      // _gManager라는 변수에 자신의 GameManager 컴포넌트를 참조하는 값을 저장, Game속성에 set코드를 짜면 다르게 대입가능
        ArrowPos = Arrow.gameObject.transform.position; // 화살의 현재 위치를 받아온다.

        Monster_Speed_Real = new float[5] { 0.02f, 0.025f, 0.03f, 0.035f, 0.04f }; // 실제
        Monster_Speed_Test = new float[5] { 0.2f, 0.5f, 0.8f, 1.1f, 2f }; // 테스트용
        Monster_Spawn = new float[5] { 2.0f, 1.9f, 1.8f, 1.65f, 1.5f }; // 실제
        MonsterSpeed = 0.01f; // 몬스터 이동속도 초기화
        
        Arrow_Speed_Real = new int[5] { 6, 7, 8, 9, 10 }; // 실제
        Arrow_Speed_Test = new int[5] { 7, 8, 9, 10, 11 }; // 테스트용
        Arrow_Spawn = new float[5] { 1.0f, 0.95f, 0.9f, 0.85f, 0.8f }; // 실제
        ArrowSpeed = 6;

        pass = true;
        slider.value = 90;
        for (int i = 0; i < 10; i++) {
            monster.Add(null);
        }
        RESULT_stage.text = "1";
        STAGE_stage.text = "1";
        //stage&life 변수 초기화
        life = 3;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        nextTime_Arrow += Timeplus_Arrow; // 제일 처음 화살과 겹쳐서 1초 이후에 부터 1초마다 생성시키기 위함
        // slider = GetComponent<Slider>();
    }
    // 화살은 충돌시 말고 1초마다 생성 - 1초는 테스트후 조정
    void Update()
    {
        time += Time.deltaTime;
        Create_Object();
        Stage_Check();
        ChangeColorAndRotation();
    }
    public void ChangeColorAndRotation()
    {
        Quaternion Direction = Quaternion.Euler(0, 0, (262 - bullet.Instance.slider_value)*1.2f);
        info.transform.rotation = Direction;
        Debug.Log("bullet.Instance.slider_value/10 : " + bullet.Instance.slider_value / 10);
        // 색상변경 영점 맞추기
        if (bullet.Instance.slider_value == 90)
            GameObject.Find("orignOne").GetComponent<Image>().color = new Color(255 / 255, 0 / 255, 48 / 255);
        else if ((int)bullet.Instance.slider_value/10 == 8 || (int)bullet.Instance.slider_value / 10 == 9)
            GameObject.Find("orignOne").GetComponent<Image>().color = new Color(255 / 255, 113 / 255, 0 / 255);
        else
            GameObject.Find("orignOne").GetComponent<Image>().color = new Color(0 / 255, 198 / 255, 255 / 255);
    }
    public void Stage_Check()
    {
        for (int i = stage_up.Length-1; i >= 0; i--)
        {
            // Debug.Log("i : " + i);
            if (count >= stage_up[i])
            {
                RESULT_stage.text = (i + 2).ToString();
                STAGE_stage.text = "스테이지 : " + (i + 2).ToString();
            }

            if (count == stage_up[i] && pass)
            {
                // Debug.LogError((i + 2) + "스테이지 진입");

                UIManager.Instance.StagePanel_on();
                pass = false;
                // MonsterSpeed = Monster_Speed_Test[i];
                MonsterSpeed = Monster_Speed_Real[i];
                ArrowSpeed = Arrow_Speed_Test[i];
                break;
            }
            if (count == stage_up[i] + 1) pass = true;
        }
        GAME_score.text = "점수 : " + (count * 10).ToString();
        RESULT_score.text = "점수 : " + (count * 10).ToString();
        RESULT_time.text = "게임시간 : " + time.ToString("N1") + "초";
        Count.text = "잡은 몬스터 수 : " + count.ToString();
        // Debug.Log("MonsterSpeed : " + MonsterSpeed);
        // Debug.Log("ArrowSpeed : " + ArrowSpeed);
    }
    public void countPlus() { count++; } // 몬스터 처치 횟수 증가

    void Create_Object() // 화살, 몬스터 생성코드
    {
        // 1초마다 실행 - 화살 생성
        if (time > nextTime_Arrow)
        {
            // Debug.Log("화살 생성 : " + time);
            Create_Bullet();
            nextTime_Arrow += Timeplus_Arrow; // Timeplus : 몬스터 생성주기

            //SpriteChange();
        }

        // 1.5초마다 실행 - 몬스터 생성
        if (time > nextTime_Monster)
        {
            // Debug.Log("몬스터 생성 : " + time);
            Create_Monster();
            nextTime_Monster += Timeplus_Monster; // Timeplus : 몬스터 생성주기
        }
    }

    public void Create_Bullet() // 새로운 화살 생성
    {
        // 초기 위치(bulletPos)에 화살을 생성시키는 코드
        // var t = Instantiate(Arrow_Prefabs, ArrowPos, Quaternion.identity, GameObject.Find("Canvas").transform.Find("GameObject").transform); // 새로운 화살 생성 Quaternion.identity : 회전값 지정 - 불필요   
        var t = Instantiate(Arrow_Prefabs, ArrowPos, Quaternion.identity); //, GameObject.Find("Canvas").transform); // 새로운 화살 생성 Quaternion.identity : 회전값 지정 - 불필요   
        t.transform.Rotate(new Vector3(0, 0, 90f));
    }

    public void Create_Monster()
    {
        // stage_up = new int[5] { 40, 90, 150, 210, 280 };
        // Monster_Spawn = new float[5] { 2.0f, 1.7f, 1.3f, 1.0f, 0.7f }; // 실제
        // Arrow_Spawn = new float[5] { 0.9f, 0.8f, 0.7f, 0.6f, 0.5f }; // 실제

        for(int i=0; i< stage_up.Length; i++)  {
            if (count <= stage_up[i]) {
                Timeplus_Monster = Monster_Spawn[i];
                Timeplus_Arrow = Arrow_Spawn[i];
                break;
            }
        }
        if (count > stage_up[4]) {
            Timeplus_Monster = 0.7f - count*0.001f;
            Timeplus_Arrow = 0.4f;
        }

        // monster_count = GameObject.FindGameObjectsWithTag("Monster").Length;
        for (int i = 0; i < monster.Count; i++)
        {
            if (monster[i] == null)
            {
                // int x = Random.Range(Row[3][0], Row[3][1]); // 디폴트는 3라인 - 테스트
                int x = 3;
                //if (Timeplus_Arrow == 0.6f)
                //    x = Random.Range(Row[5][0], Row[5][1]); // 스테이지3단계면 5라인
                //else if (Timeplus_Arrow == 0.4f)
                // int x = Random.Range(Row[7][0], Row[7][1]); // 스테이지3단계면 7라인
                int y = Random.Range(1, 8);
                //monster[i] = Instantiate(Monster_Prefabs, spawnPoints[x % 7].position + Vector3.up * (y % 7)
                //    , Quaternion.identity, GameObject.Find("Canvas").transform.Find("GameObject").transform); // 새로운 몬스터 생성 Quaternion.identity : 회전값 지정 - 불필요
                monster[i] = Instantiate(Monster_Prefabs, spawnPoints[x % 7].position + Vector3.up * (y % 7)
                    , Quaternion.identity); //, GameObject.Find("Canvas").transform); // 새로운 몬스터 생성 Quaternion.identity : 회전값 지정 - 불필요
                return;
            }
        }
    }
}
