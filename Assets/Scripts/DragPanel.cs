// Decompiled with JetBrains decompiler
// Type: DragPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

public class DragPanel : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IDragHandler
{
  private Vector2 originalLocalPointerPosition;
  private Vector3 originalPanelLocalPosition;
  private RectTransform panelRectTransform;
  private RectTransform parentRectTransform;

  private void Awake()
  {
    this.panelRectTransform = this.transform.parent as RectTransform;
    this.parentRectTransform = this.panelRectTransform.parent as RectTransform;
  }

  public void OnPointerDown(PointerEventData data)
  {
    this.originalPanelLocalPosition = this.panelRectTransform.localPosition;
    RectTransformUtility.ScreenPointToLocalPointInRectangle(this.parentRectTransform, data.position, data.pressEventCamera, out this.originalLocalPointerPosition);
  }

  public void OnDrag(PointerEventData data)
  {
    if ((Object) this.panelRectTransform == (Object) null || (Object) this.parentRectTransform == (Object) null)
      return;
    Vector2 localPoint;
    if (RectTransformUtility.ScreenPointToLocalPointInRectangle(this.parentRectTransform, data.position, data.pressEventCamera, out localPoint))
      this.panelRectTransform.localPosition = this.originalPanelLocalPosition + (Vector3) (localPoint - this.originalLocalPointerPosition);
    this.ClampToWindow();
  }

  private void ClampToWindow()
  {
    Vector3 localPosition = this.panelRectTransform.localPosition;
    Rect rect1 = this.parentRectTransform.rect;
    Vector2 min1 = rect1.min;
    rect1 = this.panelRectTransform.rect;
    Vector2 min2 = rect1.min;
    Vector3 vector3_1 = (Vector3) (min1 - min2);
    Rect rect2 = this.parentRectTransform.rect;
    Vector2 max1 = rect2.max;
    rect2 = this.panelRectTransform.rect;
    Vector2 max2 = rect2.max;
    Vector3 vector3_2 = (Vector3) (max1 - max2);
    localPosition.x = Mathf.Clamp(this.panelRectTransform.localPosition.x, vector3_1.x, vector3_2.x);
    localPosition.y = Mathf.Clamp(this.panelRectTransform.localPosition.y, vector3_1.y, vector3_2.y);
    this.panelRectTransform.localPosition = localPosition;
  }
}
