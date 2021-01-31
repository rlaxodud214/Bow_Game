using System.Collections;
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
    public Text score;
    public Text stage;

    // 활 스프라이트 변경
    public Sprite[] sprites;
    SpriteRenderer spriteRenderer;

    // 화살, 몬스터 생성관련 변수들
    public float time; // Time.deltatime값 누적하는 변수
    public float nextTime_Arrow = 0.0f;
    public float Timeplus_Arrow = 0.8f; // 화살 생성 주기 현재 : 1.0초
    public GameObject Arrow_Prefabs;  // 화살 복제시 사용할 프리팹
    public GameObject Arrow; // 화살 // bulletPos값을 저장하기 위해 초기 화살 오브젝트 변수 생성
    public Vector3 ArrowPos; // 화살 초기 위치(화살 생성 좌표값)

    public float nextTime_Monster = 0.0f;
    public float Timeplus_Monster = 2.0f; // 몬스터 생성 주기 현재 : 1.5초
    // 몬스터 오브젝트
    public GameObject Monster_Prefabs;  // 몬스터 복제시 사용할 프리팹
    public List<GameObject> monster = new List<GameObject>();
    public Transform[] spawnPoints; // 몬스터 생성시 위치 배열
    public int monster_count = 0;

    // 스테이지 클리어 조건 몬스터 수 배열
    // public int[] stage_up = new int[5] { 40, 90, 150, 210, 280 }; // 실제
    public int[] stage_up = new int[5] { 4, 9, 15, 21, 28 }; // 테스트용
    public float[] Monster_Spawn = new float[5] { 2.0f, 1.7f, 1.3f, 1.0f, 0.7f }; // 실제
    public float[] Arrow_Spawn = new float[5] { 0.9f, 0.8f, 0.7f, 0.6f, 0.5f }; // 실제

    // 슬라이더 이벤트를 처리하는 2가지 방법
    // 1. 슬라이더의 값이 바뀔때, 함수를 호출하는 방법
    // 2. 스트립트에서 슬라이더에 접근해서 처리하는 방식 -> 사용

    // 슬라이더 관련
    public Slider slider; // 0 ~ 100 정수값 slider.value

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
        Arrow = GameObject.FindGameObjectsWithTag("Arrow")[0];
        ArrowPos = Arrow.gameObject.transform.position; // 화살의 현재 위치를 받아온다.
        for(int i = 0; i < 10; i++)
        {
            monster.Add(null);
        }
        stage.text = "1"; //n Back 텍스트 설정
        //stage&life 변수 초기화
        life = 3;
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        var t = Instantiate(Arrow_Prefabs, ArrowPos, Quaternion.identity); // 새로운 화살 생성 Quaternion.identity : 회전값 지정 - 불필요   
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
                int x = Random.Range(2, 5);
                int y = Random.Range(1, 8);
                monster[i] = Instantiate(Monster_Prefabs, 
                    spawnPoints[x % 7].position + Vector3.up * (y % 7)
                    , Quaternion.identity); // 새로운 몬스터 생성 Quaternion.identity : 회전값 지정 - 불필요
                return;
            }
        }
    }
    //public void SpriteChange()
    //{
    //    SpriteChange2();
    //    Invoke("SpriteChange1", 0.1f);
    //    Invoke("SpriteChang01", 0.2f);
    //}
    //public void SpriteChange2() { spriteRenderer.sprite = sprites[2]; }
    //public void SpriteChange1() { spriteRenderer.sprite = sprites[1]; }
    //public void SpriteChange0() { spriteRenderer.sprite = sprites[0]; }
}
