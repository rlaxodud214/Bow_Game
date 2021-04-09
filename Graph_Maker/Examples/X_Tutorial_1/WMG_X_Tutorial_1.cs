using UnityEngine;
using System.Collections.Generic;

public class WMG_X_Tutorial_1 : MonoBehaviour
{
	// 싱글톤 패턴
	#region Singleton
	private static WMG_X_Tutorial_1 _Instance;    // 싱글톤 패턴을 사용하기 위한 인스턴스 변수, static으로 선언하여 어디서든 접근 가능

	public GameObject emptyGraphPrefab;

	public GameObject Graphs; // graphGO 그래프 리스트를 담는 빈 오브젝트, 복제시 부모로 지정하기 위해 넣음
	public List<GameObject> graphGO;
	

	public List<GameObject> Position_Objects;
	public List<Vector3> Position_Value;

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

		//for(int i=0; i< GameCount; i++)
  //      {
		//	Position_Value.Add(Position_Objects[i].transform.position);
		//}
	}
	#endregion

	// Use this for initialization
	void Start()
	{
		series1Data2 = Graph.Instance.series1Data2;
		Debug.Log("series1Data2의 갯수 : " + series1Data2.Count);

		// DB에서 일별 최대 각도 뽑아 온 배열 확인하는 코드
		//for (int i = 0; i < series1Data2.Count; i++)
		//{
		//	Debug.Log("series1Data2[i] : " + series1Data2[i]);
		//}

		for(int i=0; i<GameCount; i++) {
			graphGO.Add(GameObject.Instantiate(emptyGraphPrefab));
			set(graphGO[i], Position_Value[i]);
		}
	}

	public void set(GameObject graphGO, Vector3 position)
    {
		groups = new List<string>();
		data = new List<Vector2>();
		graphGO.transform.SetParent(Graphs.transform, false);
		graphGO.transform.localScale = graphGO.transform.localScale * 1.2f;
		graphGO.transform.Translate(Vector3.up * 0.3f, Space.World);
		
		BackGround1 = GameObject.Find("Sprite_Black"); // 검은색
		BackGround1.transform.position += new Vector3(-0.3f, 0.7f, 0);

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
			series1.pointColor = HexToColor("57CFEF");
			series1.lineColor = HexToColor("FFFFFF");
			series1.pointValues.SetList(data);
		}
		else
		{
			series1.pointValues.SetList(series1Data);
		}
		// graphGO.SetActive(false);
		graphGO.transform.position = position;
	}
	public void Show_Garph()
	{
		if (Show_Garph_Check)
		{
			Graphs.SetActive(false);
			Show_Garph_Check = false;
		}

		else
		{
			Graphs.SetActive(true);
			Show_Garph_Check = true;
		}
	}

	// 흰색 패널 움직이게 하는 버튼
	public void move1()
    {
		BackGround0.transform.position -= new Vector3(1.3f, 1.3f, 0);
	}

	// 함수명 그대로
	public Color HexToColor(string hex)
	{
		hex = hex.Replace("0x", "");
		hex = hex.Replace("#", "");
		byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
		byte a = 255;
		if (hex.Length == 8)
			a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
		return new Color32(r, g, b, a);
	}
}