using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource myAudio;
    public AudioClip button;        // 버튼클릭 효과음
    public AudioClip monster_die;        // 몬스터 사망
    public AudioClip player_damage;      // life 감소시

    // 싱글톤 패턴
    #region Singleton
    public static SoundManager Instance;    // 싱글톤 패턴을 사용하기 위한 인스턴스 변수, static으로 선언하여 어디서든 접근 가능

    // 인스턴스 변수 초기화
    void Awake()
    {
        // 인스턴스가 생성되지 않았을 때 (인스턴스 중복 생성을 막기 위함)
        if (SoundManager.Instance == null)
            SoundManager.Instance = this;   // 자기 자신을 참조하는 인스턴스 생성
    }
    #endregion

    // Use this for initialization
    void Start()
    {
        myAudio = GetComponent<AudioSource>();  // 오디오 소스(SoundManager 오브젝트)를 가져와 myAudio에 저장
        DontDestroyOnLoad(gameObject);  // 씬 넘어가도 SoundManager 오브젝트 살리기 (Main씬을 넘어가도 SoundManager 오브젝트는 살아있음)
    }
    public void Btn_Click() //클릭 버튼음
    {
        myAudio.PlayOneShot(button); // 오디오 소스로 소리를 한 번 재생시킴
    }
    public void Monster_die() //성공 효과음
    {
        myAudio.PlayOneShot(monster_die); // 오디오 소스로 소리를 한 번 재생시킴

    }
    public void Player_damage() //실패 효과음
    {
        myAudio.PlayOneShot(player_damage); // 오디오 소스로 소리를 한 번 재생시킴
    }
}
