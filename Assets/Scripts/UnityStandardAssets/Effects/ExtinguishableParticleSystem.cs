// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Effects.ExtinguishableParticleSystem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace UnityStandardAssets.Effects
{
  public class ExtinguishableParticleSystem : MonoBehaviour
  {
    public float multiplier = 1f;
    private ParticleSystem[] m_Systems;

    private void Start() => this.m_Systems = this.GetComponentsInChildren<ParticleSystem>();

    public void Extinguish()
    {
      foreach (ParticleSystem system in this.m_Systems)
        system.emission.enabled = false;
    }
  }
}
