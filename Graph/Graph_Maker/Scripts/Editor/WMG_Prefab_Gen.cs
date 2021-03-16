using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
#if TMP_PRESENT
using System.IO;
using UnityEngine.UI;
using TMPro;
#endif

public class PrefabGenerators : MonoBehaviour {
	
	static GameObject canvasGO;
	static GameObject baseAxis;
	static string axisGraphLocation = "Assets/Graph_Maker/Prefabs/Graphs/AxisGraphs/";

	static bool setup() {
		Canvas canvas = (Canvas)FindObjectOfType (typeof(Canvas));
		if (canvas == null) {
			Debug.LogError("Must generate UI prefabs in a scene that has a Canvas");
			return false;
		}
		canvasGO = canvas.gameObject;
		baseAxis = AssetDatabase.LoadAssetAtPath("Assets/Graph_Maker/Prefabs/Graphs/AxisGraphs/LineGraph.prefab", typeof(GameObject)) as GameObject;
		if (baseAxis == null) {
			Debug.LogError("The axis graph that is used as a base to generate the other axis graphs could not be found");
			return false;
		}
		return true;
	}

	[MenuItem ("Assets/Graph Maker/Generate Axis Graphs")]
	static void GenerateAxisGraphs () {
		if (!setup()) return;
		createEmptyGraph();
		createBarGraph();
		createScatterPlot();
		createAreaGraph();
		createStackedGraph();
		createRadarGraph();
	}

	static void createEmptyGraph() {
		GameObject graphGO = GameObject.Instantiate(baseAxis) as GameObject;
		WMG_Axis_Graph graph = graphGO.GetComponent<WMG_Axis_Graph>();
		graph.changeSpriteParent(graphGO, canvasGO);
		graphGO.name = "EmptyGraph";
		graph.changeSpritePositionTo(graphGO, Vector3.zero);

		for (int i = graph.lineSeries.Count-1; i >= 0; i--) {
			graph.deleteSeriesAt(i);
		}

		graph.InEditorUpdate ();
		createPrefab (graphGO, axisGraphLocation + "EmptyGraph.prefab");
		DestroyImmediate (graphGO);
	}

	static void createBarGraph() {
		GameObject graphGO = GameObject.Instantiate(baseAxis) as GameObject;
		WMG_Axis_Graph graph = graphGO.GetComponent<WMG_Axis_Graph>();
		graph.changeSpriteParent(graphGO, canvasGO);
		graphGO.name = "BarGraph";
		graph.graphType = WMG_Axis_Graph.graphTypes.bar_side;
		graph.changeSpriteSize(graphGO, 405, 280);
		graph.changeSpritePositionTo(graphGO, new Vector3(-250, 180, 0));
		graph.paddingTopBottom = new Vector2 (graph.paddingTopBottom.x, 60);
		graph.useGroups = true;
		graph.xAxis.LabelType = WMG_Axis.labelTypes.groups;
		graph.xAxis.AxisNumTicks = 13;
		graph.xAxis.AxisLabelRotation = 45;
		graph.legend.hideLegend = true;
		graph.legend.background.SetActive(false);

		graph.yAxis.hideGrid = true;
		graph.xAxis.hideGrid = true;

		graph.InEditorUpdate ();
		createPrefab (graphGO, axisGraphLocation + "BarGraph.prefab");
		DestroyImmediate (graphGO);
	}

	static void createScatterPlot() {
		GameObject graphGO = GameObject.Instantiate(baseAxis) as GameObject;
		WMG_Axis_Graph graph = graphGO.GetComponent<WMG_Axis_Graph>();
		graph.changeSpriteParent(graphGO, canvasGO);
		graphGO.name = "ScatterPlot";
		graph.changeSpriteSize(graphGO, 405, 280);
		graph.changeSpritePositionTo(graphGO, new Vector3(250, 180, 0));
		graph.paddingTopBottom = new Vector2 (graph.paddingTopBottom.x, 60);
		graph.legend.hideLegend = true;
		graph.legend.background.SetActive(false);
		graph.xAxis.SetLabelsUsingMaxMin = true;
		graph.xAxis.LabelType = WMG_Axis.labelTypes.ticks;

		WMG_Series series1 = graph.lineSeries[0].GetComponent<WMG_Series>();
		if (series1 == null) return;
		series1.UseXDistBetweenToSpace = false;
		series1.hideLines = true;
		series1.pointWidthHeight = 5;
		List<Vector2> s1Data = new List<Vector2>();
		s1Data.Add(new Vector2(1, 19));
		s1Data.Add(new Vector2(3, 20));
		s1Data.Add(new Vector2(3, 16));
		s1Data.Add(new Vector2(5, 18));
		s1Data.Add(new Vector2(6, 13));
		s1Data.Add(new Vector2(7, 12));
		s1Data.Add(new Vector2(8, 14));
		s1Data.Add(new Vector2(13, 8));
		s1Data.Add(new Vector2(16, 7));
		s1Data.Add(new Vector2(18, 6));
		s1Data.Add(new Vector2(21, 5.6f));
		s1Data.Add(new Vector2(24, 5));
		s1Data.Add(new Vector2(27, 4.5f));
		s1Data.Add(new Vector2(38, 3.5f));
		s1Data.Add(new Vector2(45, 3));
		s1Data.Add(new Vector2(55, 2.5f));
		s1Data.Add(new Vector2(65, 2));
		s1Data.Add(new Vector2(75, 2.3f));
		s1Data.Add(new Vector2(80, 2));
		s1Data.Add(new Vector2(85, 1.6f));
		s1Data.Add(new Vector2(88, 1));
		s1Data.Add(new Vector2(91, 1.5f));
		s1Data.Add(new Vector2(93, 2));
		s1Data.Add(new Vector2(95, 1.3f));
		s1Data.Add(new Vector2(99, 1));
		series1.pointValues.SetList(s1Data);
		series1.pointValuesListChanged (false, true, false, -1);
		series1.pointColor = new Color32(65, 255, 0, 255);


		WMG_Series series2 = graph.lineSeries[1].GetComponent<WMG_Series>();
		if (series2 == null) return;
		series2.UseXDistBetweenToSpace = false;
		series2.hidePoints = true;
		series2.lineScale = 1;
		List<Vector2> s2Data = new List<Vector2>();
		s2Data.Add(new Vector2(2, 19));
		s2Data.Add(new Vector2(12, 7));
		s2Data.Add(new Vector2(45, 2.5f));
		s2Data.Add(new Vector2(95, 1.7f));
		series2.pointValues.SetList(s2Data);
		series2.pointValuesListChanged (false, true, false, -1);
		series2.pointPrefab = 0;
		series2.linkPrefab = 1;
		series2.lineColor = new Color32(0, 190, 255, 145);
		series2.pointColor = new Color32(0, 190, 255, 255);

		graph.yAxis.hideGrid = true;
		graph.xAxis.hideGrid = true;

		graph.InEditorUpdate ();
		createPrefab (graphGO, axisGraphLocation + "ScatterPlot.prefab");
		DestroyImmediate (graphGO);
	}

	static void createAreaGraph() {
		GameObject graphGO = GameObject.Instantiate(baseAxis) as GameObject;
		WMG_Axis_Graph graph = graphGO.GetComponent<WMG_Axis_Graph>();
		graph.changeSpriteParent(graphGO, canvasGO);
		graphGO.name = "AreaShadingGraph";
		graph.changeSpriteSize(graphGO, 525, 325);
		graph.changeSpritePositionTo(graphGO, new Vector3(-190.2f, 180.2f, 0));
		graph.paddingTopBottom = new Vector2 (graph.paddingTopBottom.x, 60);
		graph.legend.hideLegend = true;
		graph.legend.background.SetActive(false);
		DestroyImmediate(graph.lineSeries[1]);
		graph.lineSeries.RemoveAt(1);
		graph.yAxis.AxisMinValue = -5;
		graph.yAxis.AxisNumTicks = 6;
		graph.autoAnimationsEnabled = false;
		
		WMG_Series series = graph.lineSeries[0].GetComponent<WMG_Series>();
		series.areaShadingType = WMG_Series.areaShadingTypes.Gradient;
		series.areaShadingColor = new Color32(0, 20, 150, 255);
		series.areaShadingAxisValue = -2;
		series.areaShadingUsesComputeShader = true;
		
		graph.yAxis.hideGrid = true;
		graph.xAxis.hideGrid = true;

		graph.InEditorUpdate ();
		createPrefab (graphGO, axisGraphLocation + "AreaShadingGraph.prefab");
		DestroyImmediate (graphGO);
	}
	
	static void createStackedGraph() {
		GameObject graphGO = GameObject.Instantiate(baseAxis) as GameObject;
		WMG_Axis_Graph graph = graphGO.GetComponent<WMG_Axis_Graph>();
		graph.changeSpriteParent(graphGO, canvasGO);
		graphGO.name = "StackedLineGraph";
		graph.changeSpriteSize(graphGO, 525, 325);
		graph.changeSpritePositionTo(graphGO, new Vector3(210.2f, -155.2f, 0));
		graph.paddingTopBottom = new Vector2 (graph.paddingTopBottom.x, 60);
		graph.legend.hideLegend = true;
		graph.legend.background.SetActive(false);
		graph.yAxis.AxisMinValue = -5;
		graph.yAxis.AxisNumTicks = 6;
		graph.autoAnimationsEnabled = false;
		
		WMG_Series series = graph.lineSeries[0].GetComponent<WMG_Series>();
		series.areaShadingType = WMG_Series.areaShadingTypes.Solid;
		series.areaShadingColor = new Color32(0, 20, 150, 255);
		series.areaShadingAxisValue = -4.75f;
		series.areaShadingUsesComputeShader = true;
		List<Vector2> s1Data = new List<Vector2>();
		s1Data.Add(new Vector2(0, 0.5f));
		s1Data.Add(new Vector2(0, 1));
		s1Data.Add(new Vector2(0, 1.5f));
		s1Data.Add(new Vector2(0, 3));
		s1Data.Add(new Vector2(0, 4));
		s1Data.Add(new Vector2(0, 6));
		s1Data.Add(new Vector2(0, 9));
		s1Data.Add(new Vector2(0, 14));
		s1Data.Add(new Vector2(0, 15));
		s1Data.Add(new Vector2(0, 17));
		s1Data.Add(new Vector2(0, 19));
		s1Data.Add(new Vector2(0, 20));
		series.pointValues.SetList(s1Data);
		series.pointValuesListChanged (false, true, false, -1);
		series.extraXSpace = 2;
		
		WMG_Series series2 = graph.lineSeries[1].GetComponent<WMG_Series>();
		series2.areaShadingType = WMG_Series.areaShadingTypes.Solid;
		series2.areaShadingColor = new Color32(0, 125, 15, 255);
		series2.areaShadingAxisValue = -4.75f;
		List<Vector2> s2Data = new List<Vector2>();
		s2Data.Add(new Vector2(0, -3));
		s2Data.Add(new Vector2(0, -2));
		s2Data.Add(new Vector2(0, -3));
		s2Data.Add(new Vector2(0, -2));
		s2Data.Add(new Vector2(0, 0));
		s2Data.Add(new Vector2(0, 1));
		s2Data.Add(new Vector2(0, 2));
		s2Data.Add(new Vector2(0, 4));
		s2Data.Add(new Vector2(0, 8));
		s2Data.Add(new Vector2(0, 6));
		s2Data.Add(new Vector2(0, 7));
		s2Data.Add(new Vector2(0, 4));
		series2.pointValues.SetList(s2Data);
		series2.pointValuesListChanged (false, true, false, -1);
		series2.extraXSpace = 2;
		series2.pointColor = new Color32(255, 120, 0, 255);
		series2.areaShadingUsesComputeShader = true;
		
		graph.yAxis.hideGrid = true;
		graph.xAxis.hideGrid = true;

		graph.InEditorUpdate ();
		createPrefab (graphGO, axisGraphLocation + "StackedLineGraph.prefab");
		DestroyImmediate (graphGO);
	}

	static void createRadarGraph() {
		GameObject graphGO = GameObject.Instantiate(baseAxis) as GameObject;
		WMG_Axis_Graph axisGraph = graphGO.GetComponent<WMG_Axis_Graph>();
		WMG_Radar_Graph radar = graphGO.AddComponent<WMG_Radar_Graph>();
		
		graphGO.name = "RadarGraph";
		
		copyFromAxisToRadar(ref radar, axisGraph);
		
		DestroyImmediate(axisGraph);
		
		radar.changeSpriteParent(graphGO, canvasGO);
		radar.changeSpriteSize(graphGO, 405, 280);
		radar.changeSpritePositionTo(graphGO, new Vector3(0, 0, 0));
		radar.autoPaddingEnabled = false;
		radar.paddingLeftRight = new Vector2 (60, 63);
		radar.paddingTopBottom = new Vector2 (25, 45);
		radar.legend.hideLegend = true;
		radar.legend.background.SetActive(false);
		radar.legend.theGraph = radar;
		radar.yAxis.graph = radar;
		radar.xAxis.graph = radar;
		radar.SetActive(radar.xAxis.AxisObj, false);
		radar.SetActive(radar.yAxis.AxisObj, false);
		radar.axesType = WMG_Axis_Graph.axesTypes.CENTER;
		DestroyImmediate(radar.lineSeries[1]);
		radar.lineSeries.RemoveAt(1);
		DestroyImmediate(radar.lineSeries[0]);
		radar.lineSeries.RemoveAt(0);
		
		radar.yAxis.AxisMinValue = -100;
		radar.yAxis.AxisMaxValue = 100;
		radar.xAxis.AxisMinValue = -100;
		radar.xAxis.AxisMaxValue = 100;
		radar.yAxis.AxisNumTicks = 5;
		radar.autoAnimationsEnabled = false;
		radar.xAxis.hideLabels = true;
		radar.yAxis.hideLabels = true;
		radar.xAxis.hideTicks = true;
		radar.yAxis.hideTicks = true;
		
		radar.randomData = true;
		radar.numPoints = 5;
		radar.offset = new Vector2(0,-20);
		radar.degreeOffset = 90;
		radar.radarMaxVal = 100;
		radar.numGrids = 7;
		radar.gridLineWidth = 0.5f;
		radar.gridColor = new Color32(125, 125, 125, 255);
		radar.numDataSeries = 1;
		radar.dataSeriesLineWidth = 1;
		List<Color> radarColors = new List<Color>();
		radarColors.Add(new Color32(0,255,180,255));
		radarColors.Add(new Color32(210,0,255,255));
		radarColors.Add(new Color32(160,210,65,255));
		radar.dataSeriesColors.SetList(radarColors);
		radar.dataSeriesColorsChanged (false, true, false, -1);
		radar.labelsColor = Color.white;
		radar.labelsOffset = 26;
		radar.fontSize = 14;
		List<string> labelStrings = new List<string>();
		labelStrings.Add("Strength");
		labelStrings.Add("Speed");
		labelStrings.Add("Agility");
		labelStrings.Add("Magic");
		labelStrings.Add("Defense");
		radar.labelStrings.SetList(labelStrings);
		radar.labelStringsChanged (false, true, false, -1);
		
		radar.pointPrefabs.Add(AssetDatabase.LoadAssetAtPath("Assets/Graph_Maker/Prefabs/Nodes/TextNode.prefab", typeof(GameObject)));
		//		UnityEditorInternal.ComponentUtility.MoveComponentUp(radar);
		
		radar.xAxis.hideGrid = true;
		radar.yAxis.hideGrid = true;

		radar.InEditorUpdate ();
		createPrefab (graphGO, axisGraphLocation + "RadarGraph.prefab");
		DestroyImmediate (graphGO);
	}

	static void copyFromAxisToRadar(ref WMG_Radar_Graph radar, WMG_Axis_Graph axis) {
		// lists
//		radar.groups.SetList(axis.groups.list);
//		radar.yAxisLabels.SetList(axis.yAxisLabels.list);
//		radar.xAxisLabels.SetList(axis.xAxisLabels.list);

		radar.xAxis = axis.xAxis;
		radar.yAxis = axis.yAxis;

		// Public variables without change tracking
		radar.tooltipOffset = axis.tooltipOffset;
		radar.tooltipNumberDecimals = axis.tooltipNumberDecimals;
		radar.tooltipDisplaySeriesName = axis.tooltipDisplaySeriesName;
		radar.tooltipAnimationsEnabled = axis.tooltipAnimationsEnabled;
		radar.tooltipAnimationsEasetype = axis.tooltipAnimationsEasetype;
		radar.tooltipAnimationsDuration = axis.tooltipAnimationsDuration;
		radar.autoAnimationsEasetype = axis.autoAnimationsEasetype;
		radar.autoAnimationsDuration = axis.autoAnimationsDuration;
		radar.lineSeries = axis.lineSeries;
		radar.pointPrefabs = axis.pointPrefabs;
		radar.linkPrefabs = axis.linkPrefabs;
		radar.barPrefab = axis.barPrefab;
		radar.seriesPrefab = axis.seriesPrefab;
		radar.legend = axis.legend;
		radar.graphTitle = axis.graphTitle;
		radar.graphBackground = axis.graphBackground;
		radar.graphAreaBoundsParent = axis.graphAreaBoundsParent;
		radar.anchoredParent = axis.anchoredParent;
		radar.yAxis = axis.yAxis;
		radar.xAxis = axis.xAxis;
		radar.seriesParent = axis.seriesParent;
		radar.toolTipPanel = axis.toolTipPanel;
		radar.toolTipLabel = axis.toolTipLabel;

		// Private backing variables
		radar.graphType = axis.graphType;
		radar.orientationType = axis.orientationType;
		radar.axesType = axis.axesType;
		radar.resizeEnabled = axis.resizeEnabled;
		radar.resizeProperties = axis.resizeProperties;
		radar.useGroups = axis.useGroups;
		radar.paddingLeftRight = axis.paddingLeftRight;
		radar.paddingTopBottom = axis.paddingTopBottom;
		radar.theOrigin = axis.theOrigin;
		radar.barWidth = axis.barWidth;
		radar.barAxisValue = axis.barAxisValue;
		radar.autoUpdateOrigin = axis.autoUpdateOrigin;
		radar.autoUpdateBarWidth = axis.autoUpdateBarWidth;
		radar.autoUpdateBarWidthSpacing = axis.autoUpdateBarWidthSpacing;
		radar.autoUpdateSeriesAxisSpacing = axis.autoUpdateSeriesAxisSpacing;
		radar.autoUpdateBarAxisValue = axis.autoUpdateBarAxisValue;
		radar.axisWidth = axis.axisWidth;
		radar.autoShrinkAtPercent = axis.autoShrinkAtPercent;
		radar.autoGrowAndShrinkByPercent = axis.autoGrowAndShrinkByPercent;
		radar.tooltipEnabled = axis.tooltipEnabled;
		radar.autoAnimationsEnabled = axis.autoAnimationsEnabled;
		radar.autoPaddingEnabled = axis.autoPaddingEnabled;
		radar.autoPaddingProperties = axis.autoPaddingProperties;
		radar.autoPaddingAmount = axis.autoPaddingAmount;
		radar.tickSize = axis.tickSize;
		radar.graphTitleString = axis.graphTitleString;
		radar.graphTitleOffset = axis.graphTitleOffset;
	}

	#if TMP_PRESENT
	[MenuItem ("Assets/Graph Maker/UGUI -> TMP Prefabs")]
	static void uGUItoTMPPrefabs () {
		if (!setup()) return;
		UGUItoTMPPrefabs();
	}
	
	[MenuItem ("Assets/Graph Maker/TMP -> UGUI Prefabs")]
	static void tMPtoUGUIPrefabs () {
		if (!setup()) return;
		TMPtoUGUIPrefabs();
	}

	static void UGUItoTMPPrefabs() {
		// Values to copy over to TMP
		string defaultText = "";
		int fontSize = 0;
		Color fontColor = Color.white;
		TextAlignmentOptions textAlignment;
		
		string[] allPrefabPaths = Directory.GetFiles(Application.dataPath + "/Graph_Maker/Prefabs", "*.prefab", SearchOption.AllDirectories);
		for (int i = 0; i < allPrefabPaths.Length; i++) {
			string prefabPath =  allPrefabPaths[i].Remove(0, allPrefabPaths[i].IndexOf("Assets/"));
			prefabPath = prefabPath.Replace('\\', '/');
			GameObject prefab = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject)) as GameObject;
			GameObject go = GameObject.Instantiate(prefab) as GameObject;
			go.transform.SetParent(canvasGO.transform, false);
			go.name = prefab.name;
			Text[] uguiTexts = go.GetComponentsInChildren<Text>(true);
			if (uguiTexts.Length > 0) {
				foreach (Text uText in uguiTexts) {
					defaultText = uText.text;
					fontSize = uText.fontSize;
					fontColor = uText.color;
					textAlignment = getTMPalignment(uText.alignment);
					GameObject textGO = uText.gameObject;
					DestroyImmediate(uText);
					TextMeshProUGUI tmpText = textGO.AddComponent<TextMeshProUGUI>();
					tmpText.text = defaultText;
					tmpText.fontSize = fontSize;
					tmpText.color = fontColor;
					tmpText.alignment = textAlignment;
				}
				createPrefab(go, prefabPath);
				DestroyImmediate(go);
			}
			else {
				DestroyImmediate(go);
			}
		}
	}
	
	static void TMPtoUGUIPrefabs() {
		// Values to copy over to TMP
		string defaultText = "";
		int fontSize = 0;
		Color fontColor = Color.white;
		TextAnchor textAlignment;
		
		string[] allPrefabPaths = Directory.GetFiles(Application.dataPath + "/Graph_Maker/Prefabs", "*.prefab", SearchOption.AllDirectories);
		for (int i = 0; i < allPrefabPaths.Length; i++) {
			string prefabPath =  allPrefabPaths[i].Remove(0, allPrefabPaths[i].IndexOf("Assets/"));
			prefabPath = prefabPath.Replace('\\', '/');
			GameObject prefab = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject)) as GameObject;
			GameObject go = GameObject.Instantiate(prefab) as GameObject;
			go.transform.SetParent(canvasGO.transform, false);
			go.name = prefab.name;
			TextMeshProUGUI[] tmpTexts = go.GetComponentsInChildren<TextMeshProUGUI>(true);
			if (tmpTexts.Length > 0) {
				foreach (TextMeshProUGUI tmpText in tmpTexts) {
					defaultText = tmpText.text;
					fontSize = Mathf.RoundToInt(tmpText.fontSize);
					fontColor = tmpText.color;
					textAlignment = getUGUIalignment(tmpText.alignment);
					GameObject textGO = tmpText.gameObject;
					DestroyImmediate(tmpText);
					Text uText = textGO.AddComponent<Text>();
					uText.text = defaultText;
					uText.fontSize = fontSize;
					uText.color = fontColor;
					uText.alignment = textAlignment;
					uText.horizontalOverflow = HorizontalWrapMode.Overflow;
					uText.verticalOverflow = VerticalWrapMode.Overflow;
				}
				createPrefab(go, prefabPath);
				DestroyImmediate(go);
			}
			else {
				DestroyImmediate(go);
			}
		}
	}
	
	static TextAlignmentOptions getTMPalignment(TextAnchor uguiAnchor) {
		if (uguiAnchor == TextAnchor.LowerCenter) {
			return TextAlignmentOptions.Bottom;
		}
		else if (uguiAnchor == TextAnchor.LowerLeft) {
			return TextAlignmentOptions.BottomLeft;
		}
		else if (uguiAnchor == TextAnchor.LowerRight) {
			return TextAlignmentOptions.BottomRight;
		}
		else if (uguiAnchor == TextAnchor.MiddleCenter) {
			return TextAlignmentOptions.Center;
		}
		else if (uguiAnchor == TextAnchor.MiddleLeft) {
			return TextAlignmentOptions.Left;
		}
		else if (uguiAnchor == TextAnchor.MiddleRight) {
			return TextAlignmentOptions.Right;
		}
		else if (uguiAnchor == TextAnchor.UpperCenter) {
			return TextAlignmentOptions.Top;
		}
		else if (uguiAnchor == TextAnchor.UpperLeft) {
			return TextAlignmentOptions.TopLeft;
		}
		else if (uguiAnchor == TextAnchor.UpperRight) {
			return TextAlignmentOptions.TopRight;
		}
		return TextAlignmentOptions.Baseline;
	}
	
	static TextAnchor getUGUIalignment(TextAlignmentOptions tmpAnchor) {
		if (tmpAnchor == TextAlignmentOptions.Bottom) {
			return TextAnchor.LowerCenter;
		}
		else if (tmpAnchor == TextAlignmentOptions.BottomLeft) {
			return TextAnchor.LowerLeft;
		}
		else if (tmpAnchor == TextAlignmentOptions.BottomRight) {
			return TextAnchor.LowerRight;
		}
		else if (tmpAnchor == TextAlignmentOptions.Center) {
			return TextAnchor.MiddleCenter;
		}
		else if (tmpAnchor == TextAlignmentOptions.Left) {
			return TextAnchor.MiddleLeft;
		}
		else if (tmpAnchor == TextAlignmentOptions.Right) {
			return TextAnchor.MiddleRight;
		}
		else if (tmpAnchor == TextAlignmentOptions.Top) {
			return TextAnchor.UpperCenter;
		}
		else if (tmpAnchor == TextAlignmentOptions.TopLeft) {
			return TextAnchor.UpperLeft;
		}
		else if (tmpAnchor == TextAlignmentOptions.TopRight) {
			return TextAnchor.UpperRight;
		}
		return TextAnchor.MiddleCenter;
	}
	#endif

	static void createPrefab(GameObject obj, string prefabPath, bool replaceIfExists = true) {
		// Create / overwrite prefab
		Object prefab = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject));
		
		if (prefab != null) {
			if (replaceIfExists) {
				PrefabUtility.ReplacePrefab(obj, prefab, ReplacePrefabOptions.ReplaceNameBased);
			}
		}
		else {
			prefab = PrefabUtility.CreateEmptyPrefab(prefabPath);
			PrefabUtility.ReplacePrefab(obj, prefab, ReplacePrefabOptions.ReplaceNameBased);
		}
	}

}

