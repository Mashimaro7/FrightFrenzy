// Decompiled with JetBrains decompiler
// Type: Graph
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using CodeMonkey.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Graph : MonoBehaviour
{
  private static Graph instance;
  [SerializeField]
  private Sprite dotSprite;
  private RectTransform graphContainer;
  private RectTransform labelTemplateX;
  private RectTransform labelTemplateY;
  private RectTransform dashContainer;
  private RectTransform dashTemplateX;
  private RectTransform dashTemplateY;
  private List<GameObject> gameObjectList;
  private List<Graph.IGraphVisualObject> graphVisualObjectList;
  private GameObject tooltipGameObject;
  private List<float> xvalueList;
  private List<float> yvalueList;
  private Graph.IGraphVisual graphVisual;
  private float xmax;
  private float ymax;

  private void Awake()
  {
    Graph.instance = this;
    this.graphContainer = this.transform.Find("graphContainer").GetComponent<RectTransform>();
    this.labelTemplateX = this.graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
    this.labelTemplateY = this.graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
    this.dashContainer = this.graphContainer.Find("dashContainer").GetComponent<RectTransform>();
    this.dashTemplateX = this.dashContainer.Find("dashTemplateX").GetComponent<RectTransform>();
    this.dashTemplateY = this.dashContainer.Find("dashTemplateY").GetComponent<RectTransform>();
    this.tooltipGameObject = this.graphContainer.Find("tooltip").gameObject;
    this.gameObjectList = new List<GameObject>();
    this.graphVisualObjectList = new List<Graph.IGraphVisualObject>();
    this.graphVisual = (Graph.IGraphVisual) new Graph.LineGraphVisual(this.graphContainer, this.dotSprite, Color.green, new Color(1f, 1f, 1f, 0.5f));
    this.yvalueList = new List<float>()
    {
      0.0f,
      0.0f,
      0.02f,
      0.08f,
      0.25f,
      0.57f,
      1.08f,
      1.83f,
      2.84f,
      4.14f,
      5.73f,
      7.64f,
      9.85f,
      12.4f,
      15.2f,
      18.4f
    };
    this.xvalueList = new List<float>()
    {
      0.0f,
      0.2f,
      0.4f,
      0.6f,
      0.8f,
      1f,
      1.2f,
      1.4f,
      1.6f,
      1.8f,
      2f,
      2.2f,
      2.4f,
      2.6f,
      2.8f,
      3f
    };
    this.xmax = 3f;
    this.ymax = 12f;
    this.HideTooltip();
  }

  public static void ShowTooltip_Static(string tooltipText, Vector2 anchoredPosition) => Graph.instance.ShowTooltip(tooltipText, anchoredPosition);

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

  public static void HideTooltip_Static() => Graph.instance.HideTooltip();

  private void HideTooltip() => this.tooltipGameObject.SetActive(false);

  public void UpdateGraph(List<float> xvalueList, List<float> yvalueList, float xmax, float ymax)
  {
    this.xvalueList = xvalueList;
    this.yvalueList = yvalueList;
    this.xmax = xmax;
    this.ymax = ymax;
    this.ShowGraph();
  }

  private void ShowGraph()
  {
    if (this.gameObjectList == null)
      return;
    foreach (UnityEngine.Object gameObject in this.gameObjectList)
      UnityEngine.Object.Destroy(gameObject);
    this.gameObjectList.Clear();
    foreach (Graph.IGraphVisualObject graphVisualObject in this.graphVisualObjectList)
      graphVisualObject.CleanUp();
    this.graphVisualObjectList.Clear();
    this.graphVisual.CleanUp();
    float width = this.graphContainer.rect.width;
    float height = this.graphContainer.rect.height;
    float ymax = this.ymax;
    float num1 = 0.0f;
    double num2 = (double) ymax - (double) num1;
    float xmax = this.xmax;
    float num3 = 0.0f;
    double num4 = (double) ymax - (double) num1;
    float graphPositionWidth = width / (this.xmax + 1f);
    int num5 = 0;
    bool flag = false;
    for (int index = 0; index < this.xvalueList.Count; ++index)
    {
      float x1 = this.xvalueList[index];
      float y1 = this.yvalueList[index];
      if (!flag)
      {
        if ((double) x1 >= 0.0 && (double) y1 >= 0.0)
        {
          if ((double) y1 > (double) this.ymax)
          {
            if (index != 0)
            {
              flag = true;
              Vector2 intersection = new Vector2(0.0f, 0.0f);
              if (Graph.LineIntersection(new Vector2(num3 - 1000f, this.ymax), new Vector2(xmax + 1000f, this.ymax), new Vector2(this.xvalueList[index - 1], this.yvalueList[index - 1]), new Vector2(x1, y1), ref intersection))
              {
                x1 = intersection.x;
                y1 = intersection.y;
              }
              else
                break;
            }
            else
              continue;
          }
          if ((double) x1 > (double) this.xmax)
          {
            if (index != 0)
            {
              flag = true;
              Vector2 intersection = new Vector2(0.0f, 0.0f);
              if (Graph.LineIntersection(new Vector2(this.xmax, num1 - 1000f), new Vector2(this.xmax, ymax + 1000f), new Vector2(this.xvalueList[index - 1], this.yvalueList[index - 1]), new Vector2(x1, y1), ref intersection))
              {
                x1 = intersection.x;
                y1 = intersection.y;
              }
              else
                break;
            }
            else
              continue;
          }
          float x2 = (float) (((double) x1 - (double) num3) / ((double) xmax - (double) num3)) * width;
          float y2 = (float) (((double) y1 - (double) num1) / ((double) ymax - (double) num1)) * height;
          string tooltipText = "(" + x1.ToString("0.#") + "," + y1.ToString("0.#") + ")";
          this.graphVisualObjectList.Add(this.graphVisual.CreateGraphVisualObject(new Vector2(x2, y2), graphPositionWidth, tooltipText));
          ++num5;
        }
      }
      else
        break;
    }
    float num6 = 6.001f;
    float num7;
    for (int index = 0; (double) index <= (double) num6; ++index)
    {
      RectTransform rectTransform1 = UnityEngine.Object.Instantiate<RectTransform>(this.labelTemplateX);
      rectTransform1.SetParent((Transform) this.graphContainer, false);
      rectTransform1.gameObject.SetActive(true);
      float num8 = (float) index * 1f / num6;
      rectTransform1.anchoredPosition = new Vector2(num8 * width, -7f);
      Text component = rectTransform1.GetComponent<Text>();
      num7 = (float) index * this.xmax / num6;
      string str = num7.ToString("0.#");
      component.text = str;
      this.gameObjectList.Add(rectTransform1.gameObject);
      RectTransform rectTransform2 = UnityEngine.Object.Instantiate<RectTransform>(this.dashTemplateX);
      rectTransform2.SetParent((Transform) this.dashContainer, false);
      rectTransform2.gameObject.SetActive(true);
      rectTransform2.anchoredPosition = new Vector2(num8 * width, -3f);
      this.gameObjectList.Add(rectTransform2.gameObject);
    }
    float num9 = 12.001f;
    for (int index = 0; (double) index <= (double) num9; ++index)
    {
      RectTransform rectTransform3 = UnityEngine.Object.Instantiate<RectTransform>(this.labelTemplateY);
      rectTransform3.SetParent((Transform) this.graphContainer, false);
      rectTransform3.gameObject.SetActive(true);
      float num10 = (float) index * 1f / num9;
      rectTransform3.anchoredPosition = new Vector2(-7f, num10 * height);
      Text component = rectTransform3.GetComponent<Text>();
      num7 = (float) index * this.ymax / num9;
      string str = num7.ToString("0.#");
      component.text = str;
      this.gameObjectList.Add(rectTransform3.gameObject);
      RectTransform rectTransform4 = UnityEngine.Object.Instantiate<RectTransform>(this.dashTemplateY);
      rectTransform4.SetParent((Transform) this.dashContainer, false);
      rectTransform4.gameObject.SetActive(true);
      rectTransform4.anchoredPosition = new Vector2(-4f, num10 * height);
      this.gameObjectList.Add(rectTransform4.gameObject);
    }
  }

  public static bool LineIntersection(
    Vector2 p1,
    Vector2 p2,
    Vector2 p3,
    Vector2 p4,
    ref Vector2 intersection)
  {
    float num1 = p2.x - p1.x;
    float num2 = p3.x - p4.x;
    float x1;
    float x2;
    if ((double) num1 < 0.0)
    {
      x1 = p2.x;
      x2 = p1.x;
    }
    else
    {
      x2 = p2.x;
      x1 = p1.x;
    }
    if ((double) num2 > 0.0)
    {
      if ((double) x2 < (double) p4.x || (double) p3.x < (double) x1)
        return false;
    }
    else if ((double) x2 < (double) p3.x || (double) p4.x < (double) x1)
      return false;
    float num3 = p2.y - p1.y;
    float num4 = p3.y - p4.y;
    float y1;
    float y2;
    if ((double) num3 < 0.0)
    {
      y1 = p2.y;
      y2 = p1.y;
    }
    else
    {
      y2 = p2.y;
      y1 = p1.y;
    }
    if ((double) num4 > 0.0)
    {
      if ((double) y2 < (double) p4.y || (double) p3.y < (double) y1)
        return false;
    }
    else if ((double) y2 < (double) p3.y || (double) p4.y < (double) y1)
      return false;
    float num5 = p1.x - p3.x;
    float num6 = p1.y - p3.y;
    float num7 = (float) ((double) num4 * (double) num5 - (double) num2 * (double) num6);
    float num8 = (float) ((double) num3 * (double) num2 - (double) num1 * (double) num4);
    if ((double) num8 > 0.0)
    {
      if ((double) num7 < 0.0 || (double) num7 > (double) num8)
        return false;
    }
    else if ((double) num7 > 0.0 || (double) num7 < (double) num8)
      return false;
    float num9 = (float) ((double) num1 * (double) num6 - (double) num3 * (double) num5);
    if ((double) num8 > 0.0)
    {
      if ((double) num9 < 0.0 || (double) num9 > (double) num8)
        return false;
    }
    else if ((double) num9 > 0.0 || (double) num9 < (double) num8)
      return false;
    if ((double) num8 == 0.0)
      return false;
    float num10 = num7 * num1;
    intersection.x = p1.x + num10 / num8;
    float num11 = num7 * num3;
    intersection.y = p1.y + num11 / num8;
    return true;
  }

  private interface IGraphVisual
  {
    Graph.IGraphVisualObject CreateGraphVisualObject(
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

  private class LineGraphVisual : Graph.IGraphVisual
  {
    private RectTransform graphContainer;
    private Sprite dotSprite;
    private Graph.LineGraphVisual.LineGraphVisualObject lastLineGraphVisualObject;
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
      this.lastLineGraphVisualObject = (Graph.LineGraphVisual.LineGraphVisualObject) null;
    }

    public void CleanUp() => this.lastLineGraphVisualObject = (Graph.LineGraphVisual.LineGraphVisualObject) null;

    public Graph.IGraphVisualObject CreateGraphVisualObject(
      Vector2 graphPosition,
      float graphPositionWidth,
      string tooltipText)
    {
      GameObject dot = this.CreateDot(graphPosition);
      GameObject dotConnectionGameObject = (GameObject) null;
      if (this.lastLineGraphVisualObject != null)
        dotConnectionGameObject = this.CreateDotConnection(this.lastLineGraphVisualObject.GetGraphPosition(), dot.GetComponent<RectTransform>().anchoredPosition);
      Graph.LineGraphVisual.LineGraphVisualObject graphVisualObject = new Graph.LineGraphVisual.LineGraphVisualObject(dot, dotConnectionGameObject, this.lastLineGraphVisualObject);
      graphVisualObject.SetGraphVisualObjectInfo(graphPosition, graphPositionWidth, tooltipText);
      this.lastLineGraphVisualObject = graphVisualObject;
      return (Graph.IGraphVisualObject) graphVisualObject;
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
      component.sizeDelta = new Vector2(8f, 8f);
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
      component.sizeDelta = new Vector2(x, 1f);
      component.anchoredPosition = dotPositionA + normalized * x * 0.5f;
      component.localEulerAngles = new Vector3(0.0f, 0.0f, UtilsClass.GetAngleFromVectorFloat((Vector3) normalized));
      return dotConnection;
    }

    public class LineGraphVisualObject : Graph.IGraphVisualObject
    {
      private GameObject dotGameObject;
      private GameObject dotConnectionGameObject;
      private Graph.LineGraphVisual.LineGraphVisualObject lastVisualObject;

      public event EventHandler OnChangedGraphVisualObjectInfo;

      public LineGraphVisualObject(
        GameObject dotGameObject,
        GameObject dotConnectionGameObject,
        Graph.LineGraphVisual.LineGraphVisualObject lastVisualObject)
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
        component.MouseOverOnceFunc = (Action) (() => Graph.ShowTooltip_Static(tooltipText, graphPosition));
        component.MouseOutOnceFunc = (Action) (() => Graph.HideTooltip_Static());
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
