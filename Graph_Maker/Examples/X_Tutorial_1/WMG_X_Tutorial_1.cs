using UnityEngine;
using System.Collections.Generic;

public class WMG_X_Tutorial_1 : MonoBehaviour
{
	// 싱글톤 패턴
	#region Singleton
	private static WMG_X_Tutorial_1 _Instance;    // 싱글톤 패턴을 사용하기 위한 인스턴스 변수, static으로 선언하여 어디서든 접근 가능

	public GameObject emptyGraphPrefab;
	public GameObject graphGO;
	public WMG_Axis_Graph graph;

	public WMG_Series series1;

	public List<Vector2> series1Data;
	public bool useData2;
	public List<string> series1Data2;

	public List<string> groups;
	public List<Vector2> data;

	public bool Show_Garph_Check;

	GameObject BackGround0, BackGround1;

	// 인스턴스에 접근하기 위한 프로퍼티
	public static WMG_X_Tutorial_1 Instance
	{
		get { return _Instance; }          // UIManager 인스턴스 변수를 리턴
	}

	// 인스턴스 변수 초기화
	void Awake()
	{
		_Instance = GetComponent<WMG_X_Tutorial_1>();  // _uiManager에 UIManager의 컴포넌트(자기 자신)에 대한 참조를 얻음
		Show_Garph_Check = false;
		groups = new List<string>();
		data = new List<Vector2>();
	}
	#endregion

	// Use this for initialization
	void Start()
	{
		series1Data2 = UIManager.Instance.series1Data2;
		//Debug.Log("series1Data2의 갯수 : " + series1Data2.Count);
		//for (int i = 0; i < series1Data2.Count; i++)
		//{
		//	// DB에서 일별 최대 각도 뽑아 온 배열 확인하는 코드
		//	// Debug.Log("series1Data2[i] : " + series1Data2[i]);
		//}

		graphGO = GameObject.Instantiate(emptyGraphPrefab);
		graphGO.transform.SetParent(this.transform, false);
		graphGO.transform.localScale = graphGO.transform.localScale * 2.4f;
		graphGO.transform.Translate(Vector3.up * 0.3f, Space.World);

		BackGround0 = GameObject.Find("Sprite_White"); // 흰색
		BackGround1 = GameObject.Find("Sprite_Black"); // 검은색
				
		BackGround0.transform.localScale = BackGround0.transform.localScale * 1.1f;
		BackGround1.transform.position += new Vector3(-0.3f, 0.7f, 0);
		move1();

		graph = graphGO.GetComponent<WMG_Axis_Graph>();
		series1 = graph.addSeries();
		graph.xAxis.AxisMaxValue = 5;

		if (useData2)
		{
			for (int i = 0; i < series1Data2.Count; i++)
			{
				string[] row = series1Data2[i].Split(',');
				groups.Add(row[0]);
				if (!string.IsNullOrEmpty(row[1]))
				{
					float y = float.Parse(row[1]);
					data.Add(new Vector2(i + 1, y));
				}
			}

			graph.groups.SetList(groups);
			graph.useGroups = true;

			graph.xAxis.LabelType = WMG_Axis.labelTypes.groups;
			graph.xAxis.AxisNumTicks = groups.Count;

			series1.seriesName = "Range Data";
			series1.UseXDistBetweenToSpace = true;
			series1.pointColor = GameManager.Instance.HexToColor("57CFEF");
			series1.lineColor = GameManager.Instance.HexToColor("FFFFFF");
			series1.pointValues.SetList(data);
		}
		else
		{
			series1.pointValues.SetList(series1Data);
		}
		graphGO.SetActive(false);
	}

	public void Show_Garph()
	{
		if (Show_Garph_Check)
		{
			graphGO.SetActive(false);
			Show_Garph_Check = false;
		}

		else
		{
			graphGO.SetActive(true);
			Show_Garph_Check = true;
		}
	}

	// 흰색 패널 움직이게 하는 버튼
	public void move1()
    {
		BackGround0.transform.position -= new Vector3(1.3f, 1.3f, 0);
	}
}