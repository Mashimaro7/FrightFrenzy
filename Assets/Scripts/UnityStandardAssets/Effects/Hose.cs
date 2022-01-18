// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Effects.Hose
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace UnityStandardAssets.Effects
{
  public class Hose : MonoBehaviour
  {
    public float maxPower = 20f;
    public float minPower = 5f;
    public float changeSpeed = 5f;
    public ParticleSystem[] hoseWaterSystems;
    public Renderer systemRenderer;
    private float m_Power;

    private void Update()
    {
      this.m_Power = Mathf.Lerp(this.m_Power, Input.GetMouseButton(0) ? this.maxPower : this.minPower, Time.deltaTime * this.changeSpeed);
      if (Input.GetKeyDown(KeyCode.Alpha1))
        this.systemRenderer.enabled = !this.systemRenderer.enabled;
      foreach (ParticleSystem hoseWaterSystem in this.hoseWaterSystems)
      {
        hoseWaterSystem.main.startSpeed = (ParticleSystem.MinMaxCurve) this.m_Power;
        hoseWaterSystem.emission.enabled = (double) this.m_Power > (double) this.minPower * 1.10000002384186;
      }
    }
  }
}
