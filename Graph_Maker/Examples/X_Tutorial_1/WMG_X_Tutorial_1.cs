using UnityEngine;
using System.Collections.Generic;

public class WMG_X_Tutorial_1 : MonoBehaviour {
	
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

	public bool Show_Garph_Check;

	// 인스턴스에 접근하기 위한 프로퍼티
	public static WMG_X_Tutorial_1 Instance
	{
		get { return _Instance; }          // UIManager 인스턴스 변수를 리턴
	}

	// 인스턴스 변수 초기화
	void Awake()
	{
		_Instance = GetComponent<WMG_X_Tutorial_1>();  // _uiManager에 UIManager의 컴포넌트(자기 자신)에 대한 참조를 얻음
		Debug.Log("graphGO.gameObject.name : " + graphGO.gameObject.name);
		if(graphGO.gameObject.name != "EmptyGraph 1(Clone)")
			graphGO = GameObject.Instantiate(emptyGraphPrefab);
		graphGO.transform.SetParent(this.transform, false);
		Show_Garph_Check = false;
	}
	#endregion

	// Use this for initialization
	void Start () {
		if (!Show_Garph_Check)
			return;
		series1Data2 = sqlite.Instance.series1Data2;
		Debug.Log("sqlite.Instance.series1Data2.Count : " + sqlite.Instance.series1Data2.Count);
		for(int i=0; i< sqlite.Instance.series1Data2.Count; i++)
        {
			Debug.Log("sqlite.Instance.series1Data2[i] : " + sqlite.Instance.series1Data2[i]);
		}
		
		graphGO.transform.localScale = graphGO.transform.localScale * 2.5f;
		GameObject BackGround0 = graphGO.gameObject.transform.GetChild(0).transform.GetChild(0).gameObject;
		GameObject BackGround1 = graphGO.gameObject.transform.GetChild(0).transform.GetChild(1).gameObject;
        BackGround0.transform.localScale = BackGround0.transform.localScale * 1.3f;

        Debug.Log("1. BackGround1.GetComponent<Transform>().position : " + BackGround1.GetComponent<Transform>().position.x);
		BackGround1.GetComponent<Transform>().position = BackGround1.GetComponent<Transform>().position + new Vector3(16.5f, -4.5f , 0);
		Debug.Log("2. BackGround1.GetComponent<Transform>().position : " + BackGround1.GetComponent<Transform>().position.x);

		graph = graphGO.GetComponent<WMG_Axis_Graph>();

		series1 = graph.addSeries();
		graph.xAxis.AxisMaxValue = 5;

		if (useData2) {
			List<string> groups = new List<string>();
			List<Vector2> data = new List<Vector2>();
			for (int i = 0; i < series1Data2.Count; i++) {
				string[] row = series1Data2[i].Split(',');
				groups.Add(row[0]);
				if (!string.IsNullOrEmpty(row[1])) {
					float y = float.Parse(row[1]);
					data.Add(new Vector2(i+1, y));
				}
			}

			graph.groups.SetList(groups);
			graph.useGroups = true;

			graph.xAxis.LabelType = WMG_Axis.labelTypes.groups;
			graph.xAxis.AxisNumTicks = groups.Count;

			series1.seriesName = "Fruit Data";

			series1.UseXDistBetweenToSpace = true;
			series1.pointColor = GameManager.Instance.HexToColor("57CFEF");
			series1.lineColor = GameManager.Instance.HexToColor("FFFFFF");
			series1.pointValues.SetList(data);
		}
		else {
			series1.pointValues.SetList(series1Data);
		}
		graphGO.SetActive(false);
	}

	public void Show_Garph()
	{
		if (Show_Garph_Check)
        {
			graphGO.gameObject.SetActive(false);
			Show_Garph_Check = false;
		}

		else
        {
			Start();
			graphGO.gameObject.SetActive(true);
			Show_Garph_Check = true;
		}
	}
}