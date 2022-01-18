// Decompiled with JetBrains decompiler
// Type: UnityEngine.EventSystems.PointerEventDataExtension
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

namespace UnityEngine.EventSystems
{
  public static class PointerEventDataExtension
  {
    public static bool IsVRPointer(this PointerEventData pointerEventData) => pointerEventData is OVRPointerEventData;

    public static Ray GetRay(this PointerEventData pointerEventData) => (pointerEventData as OVRPointerEventData).worldSpaceRay;

    public static Vector2 GetSwipeStart(this PointerEventData pointerEventData) => (pointerEventData as OVRPointerEventData).swipeStart;

    public static void SetSwipeStart(this PointerEventData pointerEventData, Vector2 start) => (pointerEventData as OVRPointerEventData).swipeStart = start;
  }
}
