// Decompiled with JetBrains decompiler
// Type: UnityEngine.EventSystems.OVRPhysicsRaycaster
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

namespace UnityEngine.EventSystems
{
  [RequireComponent(typeof (OVRCameraRig))]
  public class OVRPhysicsRaycaster : BaseRaycaster
  {
    protected const int kNoEventMaskSet = -1;
    [SerializeField]
    protected LayerMask m_EventMask = (LayerMask) -1;
    public int sortOrder = 20;

    protected OVRPhysicsRaycaster()
    {
    }

    public override Camera eventCamera => this.GetComponent<OVRCameraRig>().leftEyeCamera;

    public virtual int depth => !((UnityEngine.Object) this.eventCamera != (UnityEngine.Object) null) ? 16777215 : (int) this.eventCamera.depth;

    public override int sortOrderPriority => this.sortOrder;

    public int finalEventMask => !((UnityEngine.Object) this.eventCamera != (UnityEngine.Object) null) ? -1 : this.eventCamera.cullingMask & (int) this.m_EventMask;

    public LayerMask eventMask
    {
      get => this.m_EventMask;
      set => this.m_EventMask = value;
    }

    public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
    {
      if ((UnityEngine.Object) this.eventCamera == (UnityEngine.Object) null || !eventData.IsVRPointer())
        return;
      UnityEngine.RaycastHit[] array = Physics.RaycastAll(eventData.GetRay(), this.eventCamera.farClipPlane - this.eventCamera.nearClipPlane, this.finalEventMask);
      if (array.Length > 1)
        Array.Sort<UnityEngine.RaycastHit>(array, (Comparison<UnityEngine.RaycastHit>) ((r1, r2) => r1.distance.CompareTo(r2.distance)));
      if (array.Length == 0)
        return;
      int index = 0;
      for (int length = array.Length; index < length; ++index)
      {
        RaycastResult raycastResult = new RaycastResult()
        {
          gameObject = array[index].collider.gameObject,
          module = (BaseRaycaster) this,
          distance = array[index].distance,
          index = (float) resultAppendList.Count,
          worldPosition = array[0].point,
          worldNormal = array[0].normal
        };
        resultAppendList.Add(raycastResult);
      }
    }

    public void Spherecast(
      PointerEventData eventData,
      List<RaycastResult> resultAppendList,
      float radius)
    {
      if ((UnityEngine.Object) this.eventCamera == (UnityEngine.Object) null || !eventData.IsVRPointer())
        return;
      Ray ray = eventData.GetRay();
      float num = this.eventCamera.farClipPlane - this.eventCamera.nearClipPlane;
      double radius1 = (double) radius;
      double maxDistance = (double) num;
      int finalEventMask = this.finalEventMask;
      UnityEngine.RaycastHit[] array = Physics.SphereCastAll(ray, (float) radius1, (float) maxDistance, finalEventMask);
      if (array.Length > 1)
        Array.Sort<UnityEngine.RaycastHit>(array, (Comparison<UnityEngine.RaycastHit>) ((r1, r2) => r1.distance.CompareTo(r2.distance)));
      if (array.Length == 0)
        return;
      int index = 0;
      for (int length = array.Length; index < length; ++index)
      {
        RaycastResult raycastResult = new RaycastResult()
        {
          gameObject = array[index].collider.gameObject,
          module = (BaseRaycaster) this,
          distance = array[index].distance,
          index = (float) resultAppendList.Count,
          worldPosition = array[0].point,
          worldNormal = array[0].normal
        };
        resultAppendList.Add(raycastResult);
      }
    }

    public Vector2 GetScreenPos(Vector3 worldPosition) => (Vector2) this.eventCamera.WorldToScreenPoint(worldPosition);
  }
}
