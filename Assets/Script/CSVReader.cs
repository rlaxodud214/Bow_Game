using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class CSVReader
{
    public static List<Dictionary<string, object>> Read(string file) // 매개값 은 12행과 같 고 반환 타입은 Read(string형 file 변수)
    {
        var list = new List<Dictionary<string, object>>(); // 리스트 안에 string과 object 타입을 키, 벨류로 하는 딕셔너리를 생성

        FileInfo fi = new FileInfo(file); // FileInfo : C# 파일 이름, 확장자, 크기(용량), 수정 일자, 속성 등 알아내는 클래스

        StreamReader sr = new StreamReader(fi.FullName);    // 텍스트 파일 읽기

        string strData = "";                                // strData를 빈 문자열으로 초기화함

        var strKey = sr.ReadLine().Split(',');              // 첫 줄을 ','로 구분하여 Split한다.

        while ((strData = sr.ReadLine()) != null)           // 그 다음줄 부터 읽어서 strData에 저장한다. strData가 널이 아닐때 까지 실행
        {
            var strValue = strData.Split(',');              // 한 줄 읽은 걸 ','로 구분하여 Split해서 strValue에 저장

            Dictionary<string, object> obj = new Dictionary<string, object>(); // obj 라는 딕셔너리 생성 키는 string, value는 object 타입

            for (int i = 0; i < strKey.Length; i++)         // strKey의 길이만큼 반복
            {
                obj.Add(strKey[i], strValue[i]);            // 키 : strkey[i], 값 : strValue[i]를 obj라는 딕셔너리에 추가하는 코드
            }
            list.Add(obj);                                  // list는 리스트 안에 딕셔너리를 가지므로 list.Add(obj)로 추가 가능함
        }
        sr.Close();                                         // 텍스트 파일 닫기
        return list;                                        // list를 반환한다.
    }
}