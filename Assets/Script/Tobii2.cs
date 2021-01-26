using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tobii2 : MonoBehaviour
{
    public bool setTimer = true;
    public float timer = 0f;

    public int count;
    // Start is called before the first frame update
    void Start()
    {
        setTimer = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(setTimer)
        {
            timer += Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D collision) // 제거
    {
        Debug.Log("OnTriggerEnter2D");
        //setTimer = true;
        if (collision.tag == "Monster") // 몬스터와 실린더가 충돌한 경우
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D collision) // 제거
    {
        Debug.Log("OnTriggerStay2D");
        
    }

    void OnTriggerExit2D(Collider2D collision) // 제거
    {
        Debug.Log("OnTriggerExit2D");
        setTimer = false;
        //timer = 0f;
    }
}
