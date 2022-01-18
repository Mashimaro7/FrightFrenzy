// Decompiled with JetBrains decompiler
// Type: Fireworks
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Experimental.VFX;

public class Fireworks : MonoBehaviour
{
  public VisualEffectAsset reduced_fireworks;

  private void Start()
  {
    if (QualitySettings.GetQualityLevel() >= 3)
      return;
    foreach (VisualEffect componentsInChild in this.transform.GetComponentsInChildren<VisualEffect>())
      componentsInChild.visualEffectAsset = this.reduced_fireworks;
  }

  public void Disable() => this.transform.gameObject.SetActive(false);
}
