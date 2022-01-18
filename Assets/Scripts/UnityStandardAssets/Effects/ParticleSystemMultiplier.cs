// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Effects.ParticleSystemMultiplier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace UnityStandardAssets.Effects
{
  public class ParticleSystemMultiplier : MonoBehaviour
  {
    public float multiplier = 1f;

    private void Start()
    {
      foreach (ParticleSystem componentsInChild in this.GetComponentsInChildren<ParticleSystem>())
      {
        ParticleSystem.MainModule main = componentsInChild.main;
        main.startSizeMultiplier *= this.multiplier;
        main.startSpeedMultiplier *= this.multiplier;
        main.startLifetimeMultiplier *= Mathf.Lerp(this.multiplier, 1f, 0.5f);
        componentsInChild.Clear();
        componentsInChild.Play();
      }
    }
  }
}
