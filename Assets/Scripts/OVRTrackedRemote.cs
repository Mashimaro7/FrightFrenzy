// Decompiled with JetBrains decompiler
// Type: OVRTrackedRemote
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class OVRTrackedRemote : MonoBehaviour
{
  public GameObject m_modelGearVrController;
  public GameObject m_modelOculusGoController;
  public OVRInput.Controller m_controller;
  private bool m_isOculusGo;
  private bool m_prevControllerConnected;
  private bool m_prevControllerConnectedCached;

  private void Start() => this.m_isOculusGo = OVRPlugin.productName == "Oculus Go";

  private void Update()
  {
    bool flag = OVRInput.IsControllerConnected(this.m_controller);
    if (flag != this.m_prevControllerConnected || !this.m_prevControllerConnectedCached)
    {
      this.m_modelOculusGoController.SetActive(flag && this.m_isOculusGo);
      this.m_modelGearVrController.SetActive(flag && !this.m_isOculusGo);
      this.m_prevControllerConnected = flag;
      this.m_prevControllerConnectedCached = true;
    }
    int num = flag ? 1 : 0;
  }
}
