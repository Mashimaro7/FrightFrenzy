// Decompiled with JetBrains decompiler
// Type: ChampsInit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ChampsInit : MonoBehaviour
{
  private GameObject lighting;
  private Color old_color;

  private void OnEnable()
  {
    this.lighting = GameObject.Find("Lighting");
    if ((bool) (Object) this.lighting)
      this.lighting.SetActive(false);
    this.old_color = RenderSettings.ambientLight;
    RenderSettings.ambientLight = new Color(0.2f, 0.2f, 0.2f);
  }

  private void OnDisable()
  {
    if ((bool) (Object) this.lighting)
      this.lighting.SetActive(true);
    RenderSettings.ambientLight = this.old_color;
  }
}
