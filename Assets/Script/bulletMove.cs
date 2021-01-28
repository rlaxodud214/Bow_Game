using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletMove : MonoBehaviour
{
    #region Singleton  

    public float speed = 10f;  // 화살이 날아갈 속도
    public GameObject Bullet; // 화살

    // 슬라이더 값에 따라 회전하게 하기
    float slider_value;   // 처리전 슬라이더 값
    float Rotation_angle; // 처리후 회전각
    float map = 1f;            // 슬라이더 벨류범위 0~100을 각도로 mapping 시키기 위한 변수

    

    private static bulletMove _sceneManager;          // 싱글톤 패턴을 사용하기 위한 인스턴스 변수, static 선언으로 어디서든 참조가 가능함
    public static bulletMove bullet                    // 객체에 접근하기 위한 속성으로 내부에 get set을 사용한다.
    {
        get { return _sceneManager; }                         // _sceneManager이 변수값을 리턴받을 수 있음.
    }

    void Awake()                                               // Start()보다 먼저 실행
    {
        _sceneManager = GetComponent<bulletMove>();    // _sceneManager변수에 자신의 SceneChangeManager 컴포넌트를 넣는다.
    }
    #endregion 

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        slider_value = GameManager.Game.slider.value; // 처리전 슬라이더 값
        Rotation_angle = 140 - slider_value;
        // transform.localEulerAngles = new Vector3(0, 0, Rotation_angle);
        transform.eulerAngles = new Vector3(0, 0, Rotation_angle);
        // transform.position = Vector3.up * speed * Time.deltaTime;
        // transform.Translate(transform.right * speed * Time.deltaTime);
        transform.Translate(transform.up *-1f * speed * Time.deltaTime);  
        Debug.Log("회전값 : " + Rotation_angle);
    }

    void OnTriggerEnter2D(Collider2D collision) // 화살 제거
    {
        // Arrow_Destory : 위에 생성된 몬스터가 화면에 나타나기전에 미리 명중되는 것을 막기위해
        if (collision.gameObject.tag == "Border" || collision.gameObject.tag == "Arrow_Destory")
        {
            Destroy(gameObject);
            // Debug.Log("새로운 화살 생성 및 이전 화살 제거");
        }
            
        if (collision.gameObject.tag == "Monster")
        {
            GameManager.Game.countPlus();
            // Debug.Log("Score : " + Score);
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}