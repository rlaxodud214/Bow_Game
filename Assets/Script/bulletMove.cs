using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletMove : MonoBehaviour
{
    #region Singleton  

    public float speed = 25f;  // 화살이 날아갈 속도
    public float Createspeed = 0.5f; // 화살 생성 속도 관련
    public Vector3 bulletPos; // 화살 초기 위치(화살 생성 좌표값)
    public Vector3 mousePos = Vector3.right; // 마우스 클릭시 좌표값
    public Vector3 target = Vector3.right; // 마우스 클릭시 좌표값
    public int Score;
    
    public GameObject Bullet; // 화살
    public Sprite BulletSprite; // 화살 이미지 - 아직 미사용
    public GameObject[] monster = new GameObject[3]; // 몬스터 오브젝트
    Camera Camera;

    private static bulletMove _sceneManager;          // 싱글톤 패턴을 사용하기 위한 인스턴스 변수, static 선언으로 어디서든 참조가 가능함
    public static bulletMove bullet                    // 객체에 접근하기 위한 속성으로 내부에 get set을 사용한다.
    {
        get { return _sceneManager; }                         // _sceneManager이 변수값을 리턴받을 수 있음.
    }

    void Awake()                                               // Start()보다 먼저 실행
    {
        _sceneManager = GetComponent<bulletMove>();    // _sceneManager변수에 자신의 SceneChangeManager 컴포넌트를 넣는다.
        Score = 0;
    }
    #endregion 

    void Start()
    {
        bulletPos = Bullet.gameObject.transform.position; // 화살의 현재 위치를 받아온다.
        // Debug.Log(bulletPos);
        Camera = GameObject.Find("MainCamera").GetComponent<Camera>(); // game 화면의 좌표값을 월드 좌표값으로 변경해주는 코드1
    }

    // Update is called once per frame
    void Update()
    {
//        if (gameObject.tag.Length <= 3)
//       {
//            Invoke("Create_Bullet", Createspeed);
//            Invoke("Create_Bullet", Createspeed+0.3f);
//        }


        if (Input.GetMouseButtonDown(0)) // 마우스 좌클릭시
        {
            mousePos = Input.mousePosition;
            mousePos = Camera.ScreenToWorldPoint(mousePos); // game 화면의 좌표값을 월드 좌표값으로 변경해주는 코드2
            Debug.Log(mousePos);
        }

        transform.Translate(Vector3.up * speed * Time.deltaTime);
        target = new Vector3(mousePos.x, Bullet.gameObject.transform.position.y, // 마우스클릭 위치의 y축 값만 대입시켜주면 된다.
                             Bullet.gameObject.transform.position.z);
        transform.position = Vector3.Lerp(gameObject.transform.position, target, 1f);
    }

    void OnTriggerEnter2D(Collider2D collision) // 화살 제거
    {
        if (collision.gameObject.tag == "TopBorder")
        {
            Create_Bullet();
            Destroy(gameObject);
            // Debug.Log("이전 화살 제거");
        }
        if (collision.gameObject.tag == "Border" || collision.gameObject.tag == "rightBorder" || collision.gameObject.tag == "TopBorder")
        {
            Create_Bullet();
            Destroy(gameObject);
            // Debug.Log("새로운 화살 생성 및 이전 화살 제거");
        }
            
        if (collision.gameObject.tag == "Monster")
        {
            Create_Bullet();
            GameManager.Game.countPlus();
            Debug.Log("Score : " + Score);
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }

    void Create_Bullet() // 새로운 화살 생성
    {
        // 초기 위치(bulletPos)에 화살을 생성시키는 코드
        Instantiate(Bullet, bulletPos, Quaternion.identity); // 새로운 화살 생성 Quaternion.identity : 회전값 지정 - 불필요   
    }
}

