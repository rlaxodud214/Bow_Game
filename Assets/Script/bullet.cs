using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    #region Singleton  

    public float speed = 5f;  // 화살이 날아갈 속도->??
    public GameObject Bullet; // 화살

    // 슬라이더 값에 따라 회전하게 하기
    float time = 0f;
    float slider_value;   // 처리전 슬라이더 값
    float Rotation_angle; // 처리후 회전각
       

    private static bullet _Instance;          // 싱글톤 패턴을 사용하기 위한 인스턴스 변수, static 선언으로 어디서든 참조가 가능함
    public static bullet Instance                    // 객체에 접근하기 위한 속성으로 내부에 get set을 사용한다.
    {
        get { return _Instance; }                         // _sceneManager이 변수값을 리턴받을 수 있음.
    }

    void Awake()                                               // Start()보다 먼저 실행
    {
        _Instance = GetComponent<bullet>();    // _sceneManager변수에 자신의 SceneChangeManager 컴포넌트를 넣는다.
    }
    #endregion 

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        slider_value = GameManager.Instance.slider.value; // 처리전 슬라이더 값
        Rotation_angle = 180 - slider_value; //40 ~ 140 -> 20 ~ 160 0 - 140 x - ? = 90
        // transform.localEulerAngles = new Vector3(0, 0, Rotation_angle);
        if (time < 0.2f)
            //transform.localEulerAngles = new Vector3(0, 0, Rotation_angle);
            transform.rotation = Quaternion.Euler(0, 0, Rotation_angle);
        transform.Translate(Vector3.left * -1f * speed * Time.deltaTime, Space.Self);  //(0,1,0)
        // Debug.Log("회전값 : " + Rotation_angle);
        // Debug.Log("회전값 : " + transform.localEulerAngles);
    }

    void OnTriggerEnter2D(Collider2D collision) // 화살 제거
    {
        // Arrow_Destory : 위에 생성된 몬스터가 화면에 나타나기전에 미리 명중되는 것을 막기위해
        if (collision.gameObject.tag == "Border" || collision.gameObject.tag == "Arrow_Destory")
        {
            Destroy(gameObject);
        }
            
        if (collision.gameObject.tag == "Monster")
        {
            GameManager.Instance.countPlus();
            // Debug.Log("Score : " + Score);
            SoundManager.Instance.Monster_die();
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}