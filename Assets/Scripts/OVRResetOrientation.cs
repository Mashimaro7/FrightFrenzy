// Decompiled with JetBrains decompiler
// Type: OVRResetOrientation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class OVRResetOrientation : MonoBehaviour
{
  public OVRInput.RawButton resetButton = OVRInput.RawButton.Y;

  private void Update()
  {
    if (!OVRInput.GetDown(this.resetButton))
      return;
    OVRManager.display.RecenterPose();
  }
}
