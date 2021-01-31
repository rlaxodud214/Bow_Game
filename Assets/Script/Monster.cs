using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    #region Singleton                                         // 싱글톤 패턴은 하나의 인스턴스에 전역적인 접근을 시키며 보통 호출될 때 인스턴스화 되므로 사용하지 않는다면 생성되지도 않습니다.

    public int life = 3;
    public int count = 7;
    public int maxMonsterCount = 10;
    public float MonsterSpeed = 0.03f; // 몬스터 이동 속도

    // dynamic m, s, l, c;

    private static Monster _Instance;          // 싱글톤 패턴을 사용하기 위한 인스턴스 변수, static 선언으로 어디서든 참조가 가능함
    public static Monster Instance                    // 객체에 접근하기 위한 속성으로 내부에 get set을 사용한다.
    {
        get { return _Instance; }                         // _sceneManager이 변수값을 리턴받을 수 있음.
    }

    void Awake()                                               // Start()보다 먼저 실행
    {
        _Instance = GetComponent<Monster>();    // _sceneManager변수에 자신의 SceneChangeManager 컴포넌트를 넣는다.
        for (int i = 0; i < count; i++)
        {
            // GameManager.Game.monster_location[i] = transform.position; // 몬스터들의 현재 위치를 받아온다.
            // Debug.Log("monster_location[i] : " + GameManager.Game.monster_location[i]);
        }
    }
    #endregion 

    void Start()
    {

    }

    // 1 : 몬스터 2초마다 스폰, 2 : 몬스터 1.5초마다 스폰, 3 : 몬스터 1초마다 스폰, 4 : 몬스터 0.7초마다 스폰
    void Update()
    {
        int count = GameManager.Instance.count; // 잡은 몬스터 수

        Vector3 curPos = transform.position;
        Vector3 nextPos = Vector3.down * MonsterSpeed;
        transform.position = curPos + nextPos;
        // Debug.Log("몬스터 이동중");
    }

    void OnTriggerEnter2D(Collider2D collision) // 제거
    {
        if (collision.gameObject.tag == "Border")
            Destroy(gameObject);
       
        if (collision.gameObject.tag == "Life")
        {
            UIManager.Instance.ui();
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Cylinder") // 몬스터와 실린더가 충돌한 경우
        {
            Destroy(gameObject);
        }
    }
}