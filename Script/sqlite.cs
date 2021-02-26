using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Data;
using Mono.Data.Sqlite; //db 사용
using UnityEngine.Networking; //네트워크 사용
using Mono.Data.SqliteClient;

using System;
using UnityEngine.UI;
using SqliteConnection = Mono.Data.Sqlite.SqliteConnection;

public class sqlite : MonoBehaviour
{
    public Text MaxScore_Text; // 최고점수
    public Text test;          // 디버깅용 - 게임씬 - 화면 상단에 text오브젝트 두고 상태나 오류확인
    public Text test_Result;   // 빌드시 디버깅용 - 게임씬 - 결과패널에 GAMEOVER 텍스트 오브젝트 유니티에서 넣기 - 상태나 오류 확인
    public int maxScore;
    public string date;
    public string userID;
    public string gameID; 
    public string sql;

    private static sqlite _Instance;             // 싱글톤 객체 선언, 어디에서든지 접근할 수 있도록 하기위해 

    public static sqlite Instance                // 객체에 접근하기 위한 속성으로 내부에 get set을 사용한다.
    {
        get { return _Instance; }                     // GameManager 객체 리턴
    }

    private void Awake()
    {
        _Instance = this;
        
        // 오늘날짜 + 현재시간
        date = DateTime.Now.ToString("yyyy년 MM월 dd일 HH시 mm분 ss초");
        date = string.Format("\"{0}\"", date);

        Debug.Log ("date : " + date);
        userID = "\"유저아이디4\""; // 나중에 로그인 구현시 Column 클래스로 만들어서 그때 객체화하기
        gameID = "\"21arrow\"";     // 각자 게임 이름에 맞게 변경
        maxScore = 0;
        //StartCoroutine(DBCreate());
    }

    IEnumerator DBCreate() //DB 생성하는 코드인데 우리는 굳이 사용할 필요 없음, 알아만 두세연
                           // IEnumerator 쓴 이유는 가장 빠르게 생성되기 때문
                           // 실행하면 에셋 파일 안에 생성됨, db만 생성되기 때문에 테이블은 따로 만들어줘야 함
    {
        string filepath = string.Empty;
        if(Application.platform==RuntimePlatform.Android)
        {
            filepath = Application.persistentDataPath + "/test1.db"; //생성될 파일 경로와 db 이름
            if(!File.Exists(filepath))
            {
                UnityWebRequest unityWebRequest = UnityWebRequest.Get("jar:file://" + Application.dataPath + "!/assets/test1.db"); //생성하고 싶은 경로와 db 이름
                unityWebRequest.downloadedBytes.ToString();
                yield return unityWebRequest.SendWebRequest().isDone;
                File.WriteAllBytes(filepath, unityWebRequest.downloadHandler.data);
            }
        }
        else
        {
            filepath = Application.dataPath + "/test1.db";
            if(!File.Exists(filepath))
            {
                File.Copy(Application.streamingAssetsPath + "/test1.db", filepath);
            }
        }
        Debug.Log("db생성 완료");
        test.text = "db생성 완료";
    }

    void Start()
    {
        DbConnectionCHek();
        sql = string.Format("Insert into Game(date, userID, gameID, gamePlayTime, gameScore) " +
              "VALUES( {0}, {1}, {2}, {3}, {4})", sqlite.Instance.date, sqlite.Instance.userID, sqlite.Instance.gameID, 0, 0);
        sqlite.Instance.DatabaseSQLAdd(sql); // 디비 비어있을 때 기본 레코드 추가하는 코드
        DatabaseSQLAdd(sql);
        sql = string.Format("SELECT * FROM Game Where userID = {0}", userID);
        DataBaseRead(sql);
        //Select * From test Where testID = "hi" : 사용 예시
    }

    public void DbConnectionCHek() //연결상태 확인
    {
        try
        {
            IDbConnection dbConnection = new SqliteConnection(GetDBFilePath());
            dbConnection.Open(); // DB열기

            if (dbConnection.State == ConnectionState.Open) // DB상태가 잘 열렸다면
            {
                Debug.Log("DB 연결 성공");
                test.text = "DB 연결 성공";
            }
            else
            {
                Debug.Log("DB 연결 실패");
                test.text = "DB 연결 실패";
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
            test.text = e.ToString();
        }
    }

    public string GetDBFilePath() //파일 경로를 가져오는 코드 
    {
        string str = string.Empty;
        if (Application.platform == RuntimePlatform.Android)
        {
            str = "URI=file:" + Application.persistentDataPath + "/StreamingAssets/reupex.db";
        }
        else
        {
            str = "URI=file:" + Application.dataPath + "/StreamingAssets/reupex.db";
        }
        return str;
    }

    public void DatabaseSQLAdd(string query) //삽입이라고 썼지만 삭제도 가능
    {
        IDbConnection dbConnection = new SqliteConnection(GetDBFilePath());
        dbConnection.Open();            // DB 열기
        IDbCommand dbCommand = dbConnection.CreateCommand();

        dbCommand.CommandText = query;  // 쿼리 입력
        try
        {
            dbCommand.ExecuteNonQuery();    // 쿼리 실행
        }

        catch (Exception e)
        {
            Debug.LogError(e);
            test.text = e.ToString();

            // 빌드시 에러확인을 위해 넣음 결과패널에 Game Over대신 에러 나오게 하는 코드
            test_Result.GetComponent<Text>().fontSize = 30; // 폰트 사이즈 작게 변경 - 이유 : 오류가 길어서
            test_Result.text = e.ToString(); 
        }

        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;
        Debug.Log("데이터 삽입 완료");
        test.text = "결과 데이터 삽입 완료";
    }

    public void DataBaseRead(String query) //DB 읽어오기 - 인자로 쿼리문을 받는다.
    {
        IDbConnection dbConnection = new SqliteConnection(GetDBFilePath());
        dbConnection.Open();           // DB 열기
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText= query;  // 쿼리 입력
        IDataReader dataReader = dbCommand.ExecuteReader(); // 쿼리 실행
        
        while (dataReader.Read())                            // 쿼리로 돌아온 레코드 읽기
        {
            // Debug.Log(dataReader.GetInt32(5));               // 5번 점수 필드 읽기
            int score = dataReader.GetInt32(5);
            
            // score를 기준으로 내림차순 정렬후 제일 첫 레코드의 값을 가져오면 데이터가 많을 때 좋은 효율을 보일 듯?
            if (score > maxScore)
                maxScore = score;
            
            MaxScore_Text.text = "최고점수 : " + maxScore;
        }

        
        dataReader.Dispose();  // 생성순서와 반대로 닫아줍니다.
        dataReader = null;
        dbCommand.Dispose();
        dbCommand = null;
        // DB에는 1개의 쓰레드만이 접근할 수 있고 동시에 접근시 에러가 발생한다. 그래서 Open과 Close는 같이 써야한다.
        dbConnection.Close();  
        dbConnection = null;
    }
}

