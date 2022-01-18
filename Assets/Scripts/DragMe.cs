// Decompiled with JetBrains decompiler
// Type: DragMe
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof (Image))]
public class DragMe : 
  MonoBehaviour,
  IBeginDragHandler,
  IEventSystemHandler,
  IDragHandler,
  IEndDragHandler
{
  public bool dragOnSurfaces = true;
  private Dictionary<int, GameObject> m_DraggingIcons = new Dictionary<int, GameObject>();
  private Dictionary<int, RectTransform> m_DraggingPlanes = new Dictionary<int, RectTransform>();

  public void OnBeginDrag(PointerEventData eventData)
  {
    Canvas inParents = DragMe.FindInParents<Canvas>(this.gameObject);
    if ((Object) inParents == (Object) null)
      return;
    this.m_DraggingIcons[eventData.pointerId] = new GameObject("icon");
    this.m_DraggingIcons[eventData.pointerId].transform.SetParent(inParents.transform, false);
    this.m_DraggingIcons[eventData.pointerId].transform.SetAsLastSibling();
    Image image = this.m_DraggingIcons[eventData.pointerId].AddComponent<Image>();
    this.m_DraggingIcons[eventData.pointerId].AddComponent<CanvasGroup>().blocksRaycasts = false;
    image.sprite = this.GetComponent<Image>().sprite;
    image.SetNativeSize();
    this.m_DraggingPlanes[eventData.pointerId] = !this.dragOnSurfaces ? inParents.transform as RectTransform : this.transform as RectTransform;
    this.SetDraggedPosition(eventData);
  }

  public void OnDrag(PointerEventData eventData)
  {
    if (!((Object) this.m_DraggingIcons[eventData.pointerId] != (Object) null))
      return;
    this.SetDraggedPosition(eventData);
  }

  private void SetDraggedPosition(PointerEventData eventData)
  {
    if (this.dragOnSurfaces && (Object) eventData.pointerEnter != (Object) null && (Object) (eventData.pointerEnter.transform as RectTransform) != (Object) null)
      this.m_DraggingPlanes[eventData.pointerId] = eventData.pointerEnter.transform as RectTransform;
    RectTransform component = this.m_DraggingIcons[eventData.pointerId].GetComponent<RectTransform>();
    Vector3 worldPoint;
    if (!RectTransformUtility.ScreenPointToWorldPointInRectangle(this.m_DraggingPlanes[eventData.pointerId], eventData.position, eventData.pressEventCamera, out worldPoint))
      return;
    component.position = worldPoint;
    component.rotation = this.m_DraggingPlanes[eventData.pointerId].rotation;
  }

  public void OnEndDrag(PointerEventData eventData)
  {
    if ((Object) this.m_DraggingIcons[eventData.pointerId] != (Object) null)
      Object.Destroy((Object) this.m_DraggingIcons[eventData.pointerId]);
    this.m_DraggingIcons[eventData.pointerId] = (GameObject) null;
  }

  public static T FindInParents<T>(GameObject go) where T : Component
  {
    if ((Object) go == (Object) null)
      return default (T);
    T component = go.GetComponent<T>();
    if ((Object) component != (Object) null)
      return component;
    for (Transform parent = go.transform.parent; (Object) parent != (Object) null && (Object) component == (Object) null; parent = parent.parent)
      component = parent.gameObject.GetComponent<T>();
    return component;
  }
}
