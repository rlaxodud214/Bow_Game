using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public string userID;
    public string gameID;
    public string sql;
    public Dictionary<string, float> Dic = new Dictionary<string, float>();
    public List<string> series1Data2;

    // 싱글톤 패턴
    #region Singleton
    private static Graph _Instance;    // 싱글톤 패턴을 사용하기 위한 인스턴스 변수, static으로 선언하여 어디서든 접근 가능

    // 인스턴스에 접근하기 위한 프로퍼티
    public static Graph Instance
    {
        get { return _Instance; }          // UIManager 인스턴스 변수를 리턴
    }

    // 인스턴스 변수 초기화
    void Awake()
    {
        _Instance = this;  // _uiManager에 UIManager의 컴포넌트(자기 자신)에 대한 참조를 얻음

        userID = SqlFormat("유저아이디2"); // 나중에 로그인 구현시 Column 클래스로 만들어서 그때 객체화하기
        gameID = SqlFormat("21arrow");     // 각자 게임 이름에 맞게 변경

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
                if (item.Key.Substring(10, 1) == "0") // 0~9일과 10일~30일 구분해서 처리하기 (한자리, 두자리 구분)
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
    #endregion

    public string SqlFormat(string sql)
    {
        return string.Format("\"{0}\"", sql);
    }
}
