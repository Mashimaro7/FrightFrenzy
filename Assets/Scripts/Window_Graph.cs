// Decompiled with JetBrains decompiler
// Type: Window_Graph
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using CodeMonkey.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window_Graph : MonoBehaviour
{
  private static Window_Graph instance;
  [SerializeField]
  private Sprite dotSprite;
  private RectTransform graphContainer;
  private RectTransform labelTemplateX;
  private RectTransform labelTemplateY;
  private RectTransform dashContainer;
  private RectTransform dashTemplateX;
  private RectTransform dashTemplateY;
  private List<GameObject> gameObjectList;
  private List<Window_Graph.IGraphVisualObject> graphVisualObjectList;
  private GameObject tooltipGameObject;
  private List<int> valueList;
  private Window_Graph.IGraphVisual graphVisual;
  private int maxVisibleValueAmount;
  private Func<int, string> getAxisLabelX;
  private Func<float, string> getAxisLabelY;

  private void Awake()
  {
    Window_Graph.instance = this;
    this.graphContainer = this.transform.Find("graphContainer").GetComponent<RectTransform>();
    this.labelTemplateX = this.graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
    this.labelTemplateY = this.graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
    this.dashContainer = this.graphContainer.Find("dashContainer").GetComponent<RectTransform>();
    this.dashTemplateX = this.dashContainer.Find("dashTemplateX").GetComponent<RectTransform>();
    this.dashTemplateY = this.dashContainer.Find("dashTemplateY").GetComponent<RectTransform>();
    this.tooltipGameObject = this.graphContainer.Find("tooltip").gameObject;
    this.gameObjectList = new List<GameObject>();
    this.graphVisualObjectList = new List<Window_Graph.IGraphVisualObject>();
    Window_Graph.IGraphVisual lineGraphVisual = (Window_Graph.IGraphVisual) new Window_Graph.LineGraphVisual(this.graphContainer, this.dotSprite, Color.green, new Color(1f, 1f, 1f, 0.5f));
    Window_Graph.IGraphVisual barChartVisual = (Window_Graph.IGraphVisual) new Window_Graph.BarChartVisual(this.graphContainer, Color.white, 0.8f);
    this.transform.Find("barChartBtn").GetComponent<Button_UI>().ClickFunc = (Action) (() => this.SetGraphVisual(barChartVisual));
    this.transform.Find("lineGraphBtn").GetComponent<Button_UI>().ClickFunc = (Action) (() => this.SetGraphVisual(lineGraphVisual));
    this.transform.Find("decreaseVisibleAmountBtn").GetComponent<Button_UI>().ClickFunc = (Action) (() => this.DecreaseVisibleAmount());
    this.transform.Find("increaseVisibleAmountBtn").GetComponent<Button_UI>().ClickFunc = (Action) (() => this.IncreaseVisibleAmount());
    this.transform.Find("dollarBtn").GetComponent<Button_UI>().ClickFunc = (Action) (() => this.SetGetAxisLabelY((Func<float, string>) (_f => "$" + (object) Mathf.RoundToInt(_f))));
    this.transform.Find("euroBtn").GetComponent<Button_UI>().ClickFunc = (Action) (() => this.SetGetAxisLabelY((Func<float, string>) (_f => "€" + (object) Mathf.RoundToInt(_f / 1.18f))));
    this.HideTooltip();
    this.ShowGraph(new List<int>()
    {
      5,
      98,
      56,
      45,
      30,
      22,
      17,
      15,
      13,
      17,
      25,
      37,
      40,
      36,
      33
    }, barChartVisual, getAxisLabelX: ((Func<int, string>) (_i => "Day " + (object) (_i + 1))), getAxisLabelY: ((Func<float, string>) (_f => "$" + (object) Mathf.RoundToInt(_f))));
  }

  public static void ShowTooltip_Static(string tooltipText, Vector2 anchoredPosition) => Window_Graph.instance.ShowTooltip(tooltipText, anchoredPosition);

  private void ShowTooltip(string tooltipText, Vector2 anchoredPosition)
  {
    this.tooltipGameObject.SetActive(true);
    this.tooltipGameObject.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
    Text component = this.tooltipGameObject.transform.Find("text").GetComponent<Text>();
    component.text = tooltipText;
    float num = 4f;
    Vector2 vector2 = new Vector2(component.preferredWidth + num * 2f, component.preferredHeight + num * 2f);
    this.tooltipGameObject.transform.Find("background").GetComponent<RectTransform>().sizeDelta = vector2;
    this.tooltipGameObject.transform.SetAsLastSibling();
  }

  public static void HideTooltip_Static() => Window_Graph.instance.HideTooltip();

  private void HideTooltip() => this.tooltipGameObject.SetActive(false);

  private void SetGetAxisLabelX(Func<int, string> getAxisLabelX) => this.ShowGraph(this.valueList, this.graphVisual, this.maxVisibleValueAmount, getAxisLabelX, this.getAxisLabelY);

  private void SetGetAxisLabelY(Func<float, string> getAxisLabelY) => this.ShowGraph(this.valueList, this.graphVisual, this.maxVisibleValueAmount, this.getAxisLabelX, getAxisLabelY);

  private void IncreaseVisibleAmount() => this.ShowGraph(this.valueList, this.graphVisual, this.maxVisibleValueAmount + 1, this.getAxisLabelX, this.getAxisLabelY);

  private void DecreaseVisibleAmount() => this.ShowGraph(this.valueList, this.graphVisual, this.maxVisibleValueAmount - 1, this.getAxisLabelX, this.getAxisLabelY);

  private void SetGraphVisual(Window_Graph.IGraphVisual graphVisual) => this.ShowGraph(this.valueList, graphVisual, this.maxVisibleValueAmount, this.getAxisLabelX, this.getAxisLabelY);

  private void ShowGraph(
    List<int> valueList,
    Window_Graph.IGraphVisual graphVisual,
    int maxVisibleValueAmount = -1,
    Func<int, string> getAxisLabelX = null,
    Func<float, string> getAxisLabelY = null)
  {
    this.valueList = valueList;
    this.graphVisual = graphVisual;
    this.getAxisLabelX = getAxisLabelX;
    this.getAxisLabelY = getAxisLabelY;
    if (maxVisibleValueAmount <= 0)
      maxVisibleValueAmount = valueList.Count;
    if (maxVisibleValueAmount > valueList.Count)
      maxVisibleValueAmount = valueList.Count;
    this.maxVisibleValueAmount = maxVisibleValueAmount;
    if (getAxisLabelX == null)
      getAxisLabelX = (Func<int, string>) (_i => _i.ToString());
    if (getAxisLabelY == null)
      getAxisLabelY = (Func<float, string>) (_f => Mathf.RoundToInt(_f).ToString());
    foreach (UnityEngine.Object gameObject in this.gameObjectList)
      UnityEngine.Object.Destroy(gameObject);
    this.gameObjectList.Clear();
    foreach (Window_Graph.IGraphVisualObject graphVisualObject in this.graphVisualObjectList)
      graphVisualObject.CleanUp();
    this.graphVisualObjectList.Clear();
    graphVisual.CleanUp();
    float x1 = this.graphContainer.sizeDelta.x;
    float y1 = this.graphContainer.sizeDelta.y;
    float num1 = (float) valueList[0];
    float num2 = (float) valueList[0];
    for (int index = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); index < valueList.Count; ++index)
    {
      int num3 = valueList[index];
      if ((double) num3 > (double) num1)
        num1 = (float) num3;
      if ((double) num3 < (double) num2)
        num2 = (float) num3;
    }
    float num4 = num1 - num2;
    if ((double) num4 <= 0.0)
      num4 = 5f;
    float num5 = num1 + num4 * 0.2f;
    float num6 = num2 - num4 * 0.2f;
    float num7 = 0.0f;
    float graphPositionWidth = x1 / (float) (maxVisibleValueAmount + 1);
    int num8 = 0;
    for (int index = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); index < valueList.Count; ++index)
    {
      float x2 = graphPositionWidth + (float) num8 * graphPositionWidth;
      float y2 = (float) (((double) valueList[index] - (double) num7) / ((double) num5 - (double) num7)) * y1;
      string tooltipText = getAxisLabelY((float) valueList[index]);
      this.graphVisualObjectList.Add(graphVisual.CreateGraphVisualObject(new Vector2(x2, y2), graphPositionWidth, tooltipText));
      RectTransform rectTransform1 = UnityEngine.Object.Instantiate<RectTransform>(this.labelTemplateX);
      rectTransform1.SetParent((Transform) this.graphContainer, false);
      rectTransform1.gameObject.SetActive(true);
      rectTransform1.anchoredPosition = new Vector2(x2, -7f);
      rectTransform1.GetComponent<Text>().text = getAxisLabelX(index);
      this.gameObjectList.Add(rectTransform1.gameObject);
      RectTransform rectTransform2 = UnityEngine.Object.Instantiate<RectTransform>(this.dashTemplateX);
      rectTransform2.SetParent((Transform) this.dashContainer, false);
      rectTransform2.gameObject.SetActive(true);
      rectTransform2.anchoredPosition = new Vector2(x2, -3f);
      this.gameObjectList.Add(rectTransform2.gameObject);
      ++num8;
    }
    int num9 = 10;
    for (int index = 0; index <= num9; ++index)
    {
      RectTransform rectTransform3 = UnityEngine.Object.Instantiate<RectTransform>(this.labelTemplateY);
      rectTransform3.SetParent((Transform) this.graphContainer, false);
      rectTransform3.gameObject.SetActive(true);
      float num10 = (float) index * 1f / (float) num9;
      rectTransform3.anchoredPosition = new Vector2(-7f, num10 * y1);
      rectTransform3.GetComponent<Text>().text = getAxisLabelY(num7 + num10 * (num5 - num7));
      this.gameObjectList.Add(rectTransform3.gameObject);
      RectTransform rectTransform4 = UnityEngine.Object.Instantiate<RectTransform>(this.dashTemplateY);
      rectTransform4.SetParent((Transform) this.dashContainer, false);
      rectTransform4.gameObject.SetActive(true);
      rectTransform4.anchoredPosition = new Vector2(-4f, num10 * y1);
      this.gameObjectList.Add(rectTransform4.gameObject);
    }
  }

  private interface IGraphVisual
  {
    Window_Graph.IGraphVisualObject CreateGraphVisualObject(
      Vector2 graphPosition,
      float graphPositionWidth,
      string tooltipText);

    void CleanUp();
  }

  private interface IGraphVisualObject
  {
    void SetGraphVisualObjectInfo(
      Vector2 graphPosition,
      float graphPositionWidth,
      string tooltipText);

    void CleanUp();
  }

  private class BarChartVisual : Window_Graph.IGraphVisual
  {
    private RectTransform graphContainer;
    private Color barColor;
    private float barWidthMultiplier;

    public BarChartVisual(RectTransform graphContainer, Color barColor, float barWidthMultiplier)
    {
      this.graphContainer = graphContainer;
      this.barColor = barColor;
      this.barWidthMultiplier = barWidthMultiplier;
    }

    public void CleanUp()
    {
    }

    public Window_Graph.IGraphVisualObject CreateGraphVisualObject(
      Vector2 graphPosition,
      float graphPositionWidth,
      string tooltipText)
    {
      Window_Graph.BarChartVisual.BarChartVisualObject graphVisualObject = new Window_Graph.BarChartVisual.BarChartVisualObject(this.CreateBar(graphPosition, graphPositionWidth), this.barWidthMultiplier);
      graphVisualObject.SetGraphVisualObjectInfo(graphPosition, graphPositionWidth, tooltipText);
      return (Window_Graph.IGraphVisualObject) graphVisualObject;
    }

    private GameObject CreateBar(Vector2 graphPosition, float barWidth)
    {
      GameObject bar = new GameObject("bar", new System.Type[1]
      {
        typeof (Image)
      });
      bar.transform.SetParent((Transform) this.graphContainer, false);
      bar.GetComponent<Image>().color = this.barColor;
      RectTransform component = bar.GetComponent<RectTransform>();
      component.anchoredPosition = new Vector2(graphPosition.x, 0.0f);
      component.sizeDelta = new Vector2(barWidth * this.barWidthMultiplier, graphPosition.y);
      component.anchorMin = new Vector2(0.0f, 0.0f);
      component.anchorMax = new Vector2(0.0f, 0.0f);
      component.pivot = new Vector2(0.5f, 0.0f);
      bar.AddComponent<Button_UI>();
      return bar;
    }

    public class BarChartVisualObject : Window_Graph.IGraphVisualObject
    {
      private GameObject barGameObject;
      private float barWidthMultiplier;

      public BarChartVisualObject(GameObject barGameObject, float barWidthMultiplier)
      {
        this.barGameObject = barGameObject;
        this.barWidthMultiplier = barWidthMultiplier;
      }

      public void SetGraphVisualObjectInfo(
        Vector2 graphPosition,
        float graphPositionWidth,
        string tooltipText)
      {
        RectTransform component1 = this.barGameObject.GetComponent<RectTransform>();
        component1.anchoredPosition = new Vector2(graphPosition.x, 0.0f);
        component1.sizeDelta = new Vector2(graphPositionWidth * this.barWidthMultiplier, graphPosition.y);
        Button_UI component2 = this.barGameObject.GetComponent<Button_UI>();
        component2.MouseOverOnceFunc = (Action) (() => Window_Graph.ShowTooltip_Static(tooltipText, graphPosition));
        component2.MouseOutOnceFunc = (Action) (() => Window_Graph.HideTooltip_Static());
      }

      public void CleanUp() => UnityEngine.Object.Destroy((UnityEngine.Object) this.barGameObject);
    }
  }

  private class LineGraphVisual : Window_Graph.IGraphVisual
  {
    private RectTransform graphContainer;
    private Sprite dotSprite;
    private Window_Graph.LineGraphVisual.LineGraphVisualObject lastLineGraphVisualObject;
    private Color dotColor;
    private Color dotConnectionColor;

    public LineGraphVisual(
      RectTransform graphContainer,
      Sprite dotSprite,
      Color dotColor,
      Color dotConnectionColor)
    {
      this.graphContainer = graphContainer;
      this.dotSprite = dotSprite;
      this.dotColor = dotColor;
      this.dotConnectionColor = dotConnectionColor;
      this.lastLineGraphVisualObject = (Window_Graph.LineGraphVisual.LineGraphVisualObject) null;
    }

    public void CleanUp() => this.lastLineGraphVisualObject = (Window_Graph.LineGraphVisual.LineGraphVisualObject) null;

    public Window_Graph.IGraphVisualObject CreateGraphVisualObject(
      Vector2 graphPosition,
      float graphPositionWidth,
      string tooltipText)
    {
      GameObject dot = this.CreateDot(graphPosition);
      GameObject dotConnectionGameObject = (GameObject) null;
      if (this.lastLineGraphVisualObject != null)
        dotConnectionGameObject = this.CreateDotConnection(this.lastLineGraphVisualObject.GetGraphPosition(), dot.GetComponent<RectTransform>().anchoredPosition);
      Window_Graph.LineGraphVisual.LineGraphVisualObject graphVisualObject = new Window_Graph.LineGraphVisual.LineGraphVisualObject(dot, dotConnectionGameObject, this.lastLineGraphVisualObject);
      graphVisualObject.SetGraphVisualObjectInfo(graphPosition, graphPositionWidth, tooltipText);
      this.lastLineGraphVisualObject = graphVisualObject;
      return (Window_Graph.IGraphVisualObject) graphVisualObject;
    }

    private GameObject CreateDot(Vector2 anchoredPosition)
    {
      GameObject dot = new GameObject("dot", new System.Type[1]
      {
        typeof (Image)
      });
      dot.transform.SetParent((Transform) this.graphContainer, false);
      dot.GetComponent<Image>().sprite = this.dotSprite;
      dot.GetComponent<Image>().color = this.dotColor;
      RectTransform component = dot.GetComponent<RectTransform>();
      component.anchoredPosition = anchoredPosition;
      component.sizeDelta = new Vector2(11f, 11f);
      component.anchorMin = new Vector2(0.0f, 0.0f);
      component.anchorMax = new Vector2(0.0f, 0.0f);
      dot.AddComponent<Button_UI>();
      return dot;
    }

    private GameObject CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
      GameObject dotConnection = new GameObject("dotConnection", new System.Type[1]
      {
        typeof (Image)
      });
      dotConnection.transform.SetParent((Transform) this.graphContainer, false);
      dotConnection.GetComponent<Image>().color = this.dotConnectionColor;
      dotConnection.GetComponent<Image>().raycastTarget = false;
      RectTransform component = dotConnection.GetComponent<RectTransform>();
      Vector2 normalized = (dotPositionB - dotPositionA).normalized;
      float x = Vector2.Distance(dotPositionA, dotPositionB);
      component.anchorMin = new Vector2(0.0f, 0.0f);
      component.anchorMax = new Vector2(0.0f, 0.0f);
      component.sizeDelta = new Vector2(x, 3f);
      component.anchoredPosition = dotPositionA + normalized * x * 0.5f;
      component.localEulerAngles = new Vector3(0.0f, 0.0f, UtilsClass.GetAngleFromVectorFloat((Vector3) normalized));
      return dotConnection;
    }

    public class LineGraphVisualObject : Window_Graph.IGraphVisualObject
    {
      private GameObject dotGameObject;
      private GameObject dotConnectionGameObject;
      private Window_Graph.LineGraphVisual.LineGraphVisualObject lastVisualObject;

      public event EventHandler OnChangedGraphVisualObjectInfo;

      public LineGraphVisualObject(
        GameObject dotGameObject,
        GameObject dotConnectionGameObject,
        Window_Graph.LineGraphVisual.LineGraphVisualObject lastVisualObject)
      {
        this.dotGameObject = dotGameObject;
        this.dotConnectionGameObject = dotConnectionGameObject;
        this.lastVisualObject = lastVisualObject;
        if (lastVisualObject == null)
          return;
        lastVisualObject.OnChangedGraphVisualObjectInfo += new EventHandler(this.LastVisualObject_OnChangedGraphVisualObjectInfo);
      }

      private void LastVisualObject_OnChangedGraphVisualObjectInfo(object sender, EventArgs e) => this.UpdateDotConnection();

      public void SetGraphVisualObjectInfo(
        Vector2 graphPosition,
        float graphPositionWidth,
        string tooltipText)
      {
        this.dotGameObject.GetComponent<RectTransform>().anchoredPosition = graphPosition;
        this.UpdateDotConnection();
        Button_UI component = this.dotGameObject.GetComponent<Button_UI>();
        component.MouseOverOnceFunc = (Action) (() => Window_Graph.ShowTooltip_Static(tooltipText, graphPosition));
        component.MouseOutOnceFunc = (Action) (() => Window_Graph.HideTooltip_Static());
        if (this.OnChangedGraphVisualObjectInfo == null)
          return;
        this.OnChangedGraphVisualObjectInfo((object) this, EventArgs.Empty);
      }

      public void CleanUp()
      {
        UnityEngine.Object.Destroy((UnityEngine.Object) this.dotGameObject);
        UnityEngine.Object.Destroy((UnityEngine.Object) this.dotConnectionGameObject);
      }

      public Vector2 GetGraphPosition() => this.dotGameObject.GetComponent<RectTransform>().anchoredPosition;

      private void UpdateDotConnection()
      {
        if (!((UnityEngine.Object) this.dotConnectionGameObject != (UnityEngine.Object) null))
          return;
        RectTransform component = this.dotConnectionGameObject.GetComponent<RectTransform>();
        Vector2 normalized = (this.lastVisualObject.GetGraphPosition() - this.GetGraphPosition()).normalized;
        float x = Vector2.Distance(this.GetGraphPosition(), this.lastVisualObject.GetGraphPosition());
        component.sizeDelta = new Vector2(x, 3f);
        component.anchoredPosition = this.GetGraphPosition() + normalized * x * 0.5f;
        component.localEulerAngles = new Vector3(0.0f, 0.0f, UtilsClass.GetAngleFromVectorFloat((Vector3) normalized));
      }
    }
  }
}
