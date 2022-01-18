// Decompiled with JetBrains decompiler
// Type: OVRModeParms
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class OVRModeParms : MonoBehaviour
{
  public OVRInput.RawButton resetButton = OVRInput.RawButton.X;

  private void Start()
  {
    if (!OVRManager.isHmdPresent)
      this.enabled = false;
    else
      this.InvokeRepeating("TestPowerStateMode", 10f, 10f);
  }

  private void Update()
  {
    if (!OVRInput.GetDown(this.resetButton))
      return;
    OVRPlugin.cpuLevel = 0;
    OVRPlugin.gpuLevel = 1;
  }

  private void TestPowerStateMode()
  {
    if (!OVRPlugin.powerSaving)
      return;
    Debug.Log((object) "POWER SAVE MODE ACTIVATED");
  }
}
