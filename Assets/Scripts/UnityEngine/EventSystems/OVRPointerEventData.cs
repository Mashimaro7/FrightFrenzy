// Decompiled with JetBrains decompiler
// Type: UnityEngine.EventSystems.OVRPointerEventData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Text;

namespace UnityEngine.EventSystems
{
  public class OVRPointerEventData : PointerEventData
  {
    public Ray worldSpaceRay;
    public Vector2 swipeStart;

    public OVRPointerEventData(EventSystem eventSystem)
      : base(eventSystem)
    {
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("<b>Position</b>: " + (object) this.position);
      stringBuilder.AppendLine("<b>delta</b>: " + (object) this.delta);
      stringBuilder.AppendLine("<b>eligibleForClick</b>: " + this.eligibleForClick.ToString());
      stringBuilder.AppendLine("<b>pointerEnter</b>: " + (object) this.pointerEnter);
      stringBuilder.AppendLine("<b>pointerPress</b>: " + (object) this.pointerPress);
      stringBuilder.AppendLine("<b>lastPointerPress</b>: " + (object) this.lastPress);
      stringBuilder.AppendLine("<b>pointerDrag</b>: " + (object) this.pointerDrag);
      stringBuilder.AppendLine("<b>worldSpaceRay</b>: " + (object) this.worldSpaceRay);
      stringBuilder.AppendLine("<b>swipeStart</b>: " + (object) this.swipeStart);
      stringBuilder.AppendLine("<b>Use Drag Threshold</b>: " + this.useDragThreshold.ToString());
      return stringBuilder.ToString();
    }
  }
}
