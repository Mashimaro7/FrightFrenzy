// Decompiled with JetBrains decompiler
// Type: OVRRaycaster
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof (Canvas))]
public class OVRRaycaster : GraphicRaycaster, IPointerEnterHandler, IEventSystemHandler
{
  [Tooltip("A world space pointer for this canvas")]
  public GameObject pointer;
  public int sortOrder;
  [NonSerialized]
  private Canvas m_Canvas;
  [NonSerialized]
  private List<OVRRaycaster.RaycastHit> m_RaycastResults = new List<OVRRaycaster.RaycastHit>();
  [NonSerialized]
  private static readonly List<OVRRaycaster.RaycastHit> s_SortedGraphics = new List<OVRRaycaster.RaycastHit>();

  protected OVRRaycaster()
  {
  }

  private Canvas canvas
  {
    get
    {
      if ((UnityEngine.Object) this.m_Canvas != (UnityEngine.Object) null)
        return this.m_Canvas;
      this.m_Canvas = this.GetComponent<Canvas>();
      return this.m_Canvas;
    }
  }

  public override Camera eventCamera => this.canvas.worldCamera;

  public override int sortOrderPriority => this.sortOrder;

  private void Raycast(
    PointerEventData eventData,
    List<RaycastResult> resultAppendList,
    Ray ray,
    bool checkForBlocking)
  {
    if ((UnityEngine.Object) this.canvas == (UnityEngine.Object) null)
      return;
    float num1 = float.MaxValue;
    if (checkForBlocking && this.blockingObjects != GraphicRaycaster.BlockingObjects.None)
    {
      float farClipPlane = this.eventCamera.farClipPlane;
      if (this.blockingObjects == GraphicRaycaster.BlockingObjects.ThreeD || this.blockingObjects == GraphicRaycaster.BlockingObjects.All)
      {
        UnityEngine.RaycastHit[] raycastHitArray = Physics.RaycastAll(ray, farClipPlane, (int) this.m_BlockingMask);
        if (raycastHitArray.Length != 0 && (double) raycastHitArray[0].distance < (double) num1)
          num1 = raycastHitArray[0].distance;
      }
      if (this.blockingObjects == GraphicRaycaster.BlockingObjects.TwoD || this.blockingObjects == GraphicRaycaster.BlockingObjects.All)
      {
        RaycastHit2D[] rayIntersectionAll = Physics2D.GetRayIntersectionAll(ray, farClipPlane, (int) this.m_BlockingMask);
        if (rayIntersectionAll.Length != 0 && (double) rayIntersectionAll[0].fraction * (double) farClipPlane < (double) num1)
          num1 = rayIntersectionAll[0].fraction * farClipPlane;
      }
    }
    this.m_RaycastResults.Clear();
    this.GraphicRaycast(this.canvas, ray, this.m_RaycastResults);
    for (int index = 0; index < this.m_RaycastResults.Count; ++index)
    {
      GameObject gameObject = this.m_RaycastResults[index].graphic.gameObject;
      bool flag = true;
      if (this.ignoreReversedGraphics)
        flag = (double) Vector3.Dot(ray.direction, gameObject.transform.rotation * Vector3.forward) > 0.0;
      if ((double) this.eventCamera.transform.InverseTransformPoint(this.m_RaycastResults[index].worldPos).z <= 0.0)
        flag = false;
      if (flag)
      {
        float num2 = Vector3.Distance(ray.origin, this.m_RaycastResults[index].worldPos);
        if ((double) num2 < (double) num1)
        {
          RaycastResult raycastResult = new RaycastResult()
          {
            gameObject = gameObject,
            module = (BaseRaycaster) this,
            distance = num2,
            index = (float) resultAppendList.Count,
            depth = this.m_RaycastResults[index].graphic.depth,
            worldPosition = this.m_RaycastResults[index].worldPos
          };
          resultAppendList.Add(raycastResult);
        }
      }
    }
  }

  public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
  {
    if (!eventData.IsVRPointer())
      return;
    this.Raycast(eventData, resultAppendList, eventData.GetRay(), true);
  }

  public void RaycastPointer(PointerEventData eventData, List<RaycastResult> resultAppendList)
  {
    if (!((UnityEngine.Object) this.pointer != (UnityEngine.Object) null) || !this.pointer.activeInHierarchy)
      return;
    this.Raycast(eventData, resultAppendList, new Ray(this.eventCamera.transform.position, (this.pointer.transform.position - this.eventCamera.transform.position).normalized), false);
  }

  private void GraphicRaycast(Canvas canvas, Ray ray, List<OVRRaycaster.RaycastHit> results)
  {
    IList<Graphic> graphicsForCanvas = GraphicRegistry.GetGraphicsForCanvas(canvas);
    OVRRaycaster.s_SortedGraphics.Clear();
    for (int index = 0; index < graphicsForCanvas.Count; ++index)
    {
      Graphic graphic = graphicsForCanvas[index];
      Vector3 worldPos;
      if (graphic.depth != -1 && !((UnityEngine.Object) this.pointer == (UnityEngine.Object) graphic.gameObject) && OVRRaycaster.RayIntersectsRectTransform(graphic.rectTransform, ray, out worldPos))
      {
        Vector2 screenPoint = (Vector2) this.eventCamera.WorldToScreenPoint(worldPos);
        if (graphic.Raycast(screenPoint, this.eventCamera))
        {
          OVRRaycaster.RaycastHit raycastHit;
          raycastHit.graphic = graphic;
          raycastHit.worldPos = worldPos;
          raycastHit.fromMouse = false;
          OVRRaycaster.s_SortedGraphics.Add(raycastHit);
        }
      }
    }
    OVRRaycaster.s_SortedGraphics.Sort((Comparison<OVRRaycaster.RaycastHit>) ((g1, g2) => g2.graphic.depth.CompareTo(g1.graphic.depth)));
    for (int index = 0; index < OVRRaycaster.s_SortedGraphics.Count; ++index)
      results.Add(OVRRaycaster.s_SortedGraphics[index]);
  }

  public Vector2 GetScreenPosition(RaycastResult raycastResult) => (Vector2) this.eventCamera.WorldToScreenPoint(raycastResult.worldPosition);

  private static bool RayIntersectsRectTransform(
    RectTransform rectTransform,
    Ray ray,
    out Vector3 worldPos)
  {
    Vector3[] fourCornersArray = new Vector3[4];
    rectTransform.GetWorldCorners(fourCornersArray);
    float enter;
    if (!new Plane(fourCornersArray[0], fourCornersArray[1], fourCornersArray[2]).Raycast(ray, out enter))
    {
      worldPos = Vector3.zero;
      return false;
    }
    Vector3 point = ray.GetPoint(enter);
    Vector3 rhs1 = fourCornersArray[3] - fourCornersArray[0];
    Vector3 rhs2 = fourCornersArray[1] - fourCornersArray[0];
    float num1 = Vector3.Dot(point - fourCornersArray[0], rhs1);
    float num2 = Vector3.Dot(point - fourCornersArray[0], rhs2);
    if ((double) num1 < (double) rhs1.sqrMagnitude && (double) num2 < (double) rhs2.sqrMagnitude && (double) num1 >= 0.0 && (double) num2 >= 0.0)
    {
      worldPos = fourCornersArray[0] + num2 * rhs2 / rhs2.sqrMagnitude + num1 * rhs1 / rhs1.sqrMagnitude;
      return true;
    }
    worldPos = Vector3.zero;
    return false;
  }

  public bool IsFocussed()
  {
    OVRInputModule currentInputModule = EventSystem.current.currentInputModule as OVRInputModule;
    return (bool) (UnityEngine.Object) currentInputModule && (UnityEngine.Object) currentInputModule.activeGraphicRaycaster == (UnityEngine.Object) this;
  }

  public void OnPointerEnter(PointerEventData e)
  {
    if (!e.IsVRPointer())
      return;
    (EventSystem.current.currentInputModule as OVRInputModule).activeGraphicRaycaster = this;
  }

  private struct RaycastHit
  {
    public Graphic graphic;
    public Vector3 worldPos;
    public bool fromMouse;
  }
}
