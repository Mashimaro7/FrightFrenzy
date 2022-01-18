// Decompiled with JetBrains decompiler
// Type: OVRMonoscopic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class OVRMonoscopic : MonoBehaviour
{
  public OVRInput.RawButton toggleButton = OVRInput.RawButton.B;
  private bool monoscopic;

  private void Update()
  {
    if (!OVRInput.GetDown(this.toggleButton))
      return;
    this.monoscopic = !this.monoscopic;
    OVRManager.instance.monoscopic = this.monoscopic;
  }
}
