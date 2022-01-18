// Decompiled with JetBrains decompiler
// Type: ResizePanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

public class ResizePanel : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IDragHandler
{
  public Vector2 minSize = new Vector2(100f, 100f);
  public Vector2 maxSize = new Vector2(400f, 400f);
  private RectTransform panelRectTransform;
  private Vector2 originalLocalPointerPosition;
  private Vector2 originalSizeDelta;

  private void Awake() => this.panelRectTransform = this.transform.parent.GetComponent<RectTransform>();

  public void OnPointerDown(PointerEventData data)
  {
    this.originalSizeDelta = this.panelRectTransform.sizeDelta;
    RectTransformUtility.ScreenPointToLocalPointInRectangle(this.panelRectTransform, data.position, data.pressEventCamera, out this.originalLocalPointerPosition);
  }

  public void OnDrag(PointerEventData data)
  {
    if ((Object) this.panelRectTransform == (Object) null)
      return;
    Vector2 localPoint;
    RectTransformUtility.ScreenPointToLocalPointInRectangle(this.panelRectTransform, data.position, data.pressEventCamera, out localPoint);
    Vector3 vector3 = (Vector3) (localPoint - this.originalLocalPointerPosition);
    Vector2 vector2 = this.originalSizeDelta + new Vector2(vector3.x, -vector3.y);
    vector2 = new Vector2(Mathf.Clamp(vector2.x, this.minSize.x, this.maxSize.x), Mathf.Clamp(vector2.y, this.minSize.y, this.maxSize.y));
    this.panelRectTransform.sizeDelta = vector2;
  }
}
