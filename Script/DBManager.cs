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

public class DBManager
{
    static IDbConnection dbConnection;
    static IDbCommand dbCommand;
    public static IDataReader dataReader;

    static public Text MaxScore_Text; // 최고점수
    static public Text test;          // 디버깅용 - 게임씬 - 화면 상단에 text오브젝트 두고 상태나 오류확인
    static public Text test_Result;   // 빌드시 디버깅용 - 게임씬 - 결과패널에 GAMEOVER 텍스트 오브젝트 유니티에서 넣기 - 상태나 오류 확인
    static public int maxScore;
    static public string date;
    static public string userID;
    static public string gameID;
    static public string sql;

    internal static void DbConnectionCHek()
    {
        try
        {
            IDbConnection dbConnection = new SqliteConnection(GetDBFilePath());
            dbConnection.Open(); // DB열기

            if (dbConnection.State == ConnectionState.Open) // DB상태가 잘 열렸다면
            {
                Debug.Log("DB 연결 성공");
            }
            else
            {
                Debug.Log("DB 연결 실패");
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
    internal static void DataBaseRead(string query) //DB 읽어오기 - 인자로 쿼리문을 받는다.
    {
        //Debug.Log("query : " + query);
        dbConnection = new SqliteConnection(GetDBFilePath());
        dbConnection.Open();           // DB 열기
        dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = query;  // 쿼리 입력
        dataReader = dbCommand.ExecuteReader(); // 쿼리 실행
    }

    internal static void DBClose()
    {
        dataReader.Dispose();  // 생성순서와 반대로 닫아줍니다.
        dataReader = null;
        dbCommand.Dispose();
        dbCommand = null;
        // DB에는 1개의 쓰레드만이 접근할 수 있고 동시에 접근시 에러가 발생한다. 그래서 Open과 Close는 같이 써야한다.
        dbConnection.Close();
        dbConnection = null;
    }

    //DB 생성하는 코드인데 우리는 굳이 사용할 필요 없음, 알아만 두세연
    // IEnumerator 쓴 이유는 가장 빠르게 생성되기 때문
    // 실행하면 에셋 파일 안에 생성됨, db만 생성되기 때문에 테이블은 따로 만들어줘야 함
    static IEnumerator DBCreate()
    {
        string filepath = string.Empty;
        if (Application.platform == RuntimePlatform.Android)
        {
            filepath = Application.persistentDataPath + "/test1.db"; //생성될 파일 경로와 db 이름
            if (!File.Exists(filepath))
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
            if (!File.Exists(filepath))
            {
                File.Copy(Application.streamingAssetsPath + "/test1.db", filepath);
            }
        }
        Debug.Log("db생성 완료");
    }

    internal static string GetDBFilePath() //파일 경로를 가져오는 코드 
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

    internal static void DatabaseSQLAdd(string query) //삽입이라고 썼지만 삭제도 가능
    {
        IDbConnection dbConnection = new SqliteConnection(GetDBFilePath());
        dbConnection.Open();            // DB 열기
        dbCommand = dbConnection.CreateCommand();

        dbCommand.CommandText = query;  // 쿼리 입력
        try
        {
            dbCommand.ExecuteNonQuery();    // 쿼리 실행
        }

        catch (Exception e)
        {
            Debug.LogError(e);
            //test.text = e.ToString();
        }

        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
        dbConnection = null;
        Debug.Log("데이터 삽입 완료");
        //test.text = "결과 데이터 삽입 완료";
    }


}
