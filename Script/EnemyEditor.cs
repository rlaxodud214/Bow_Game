using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// Editor를 상속받으면 에디터에서만 작동함
// Enemy 클래스는 내가만든 에디터로 제어하겠다.
[CustomEditor(typeof(GameManager))]

public class EnemyEditor : Editor
{
    // EnemyEditor와 GameManager는 별개의 클래스이므로 실제 선택된 GameManager를 찾아올수 있어야함
    public GameManager selected;

    // Editor에서 OnEnable은 실제 에디터에서 오브젝트를 눌렀을때 활성화됨
    private void OnEnable()
    {
        // target은 Editor에 있는 변수로 선택한 오브젝트를 받아옴.
        if (AssetDatabase.Contains(target))
        {
            selected = null;
        }
        else
        {
            // target은 Object형이므로 GameManager로 형변환
            selected = (GameManager)target;
        }
    }

    // 유니티가 인스펙터를 GUI로 그려주는함수
    public override void OnInspectorGUI()
    {
        if (selected == null)
            return;

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("****** 몬스터, 화살 이동속도 및 생성주기 관리 ******");
        EditorGUILayout.LabelField("**** 스테이지 체크 및 슬라이더 조절에 따른 이동 ****");
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        // 씬 첫화면에 광고를 나오게한다. 뭘한다 등등 세팅을 쭉 띄워줄 수 있다.
        Color tempColor = Color.white;
        switch (selected.monsterType)
        {
            case GameManager.MonsterType.normal:
                tempColor = Color.yellow;
                break;
            default:
                break;
        }

        GUI.color = tempColor;
        selected.monsterType = (GameManager.MonsterType)EditorGUILayout.EnumPopup("몬스터 종류", selected.monsterType);
        GUI.color = Color.white;
        selected.count = (int)EditorGUILayout.FloatField("잡은 몬스터 수", selected.count);
        GUI.color = Color.cyan;
        selected.Timeplus_Arrow = EditorGUILayout.FloatField("화살 생성 주기", selected.Timeplus_Arrow);
        GUI.color = Color.cyan;
        selected.Timeplus_Monster = EditorGUILayout.FloatField("몬스터 생성 주기", selected.Timeplus_Monster);
        GUI.color = Color.white;
        // selected.tag = EditorGUILayout.TextField("설명", selected.tag);

        // Release 세팅하고 버튼누르면 모든변수가 다바뀌게. Test 세팅하면 그렇게 바뀌게 그런식으로 사용할 수 있음.
        if (GUILayout.Button("Resize"))
        {
            selected.transform.localScale = Vector3.one * Random.Range(0.5f, 1f);
        }
    }
}
