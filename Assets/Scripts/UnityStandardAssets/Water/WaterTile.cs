﻿// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Water.WaterTile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace UnityStandardAssets.Water
{
  [ExecuteInEditMode]
  public class WaterTile : MonoBehaviour
  {
    public PlanarReflection reflection;
    public WaterBase waterBase;

    public void Start() => this.AcquireComponents();

    private void AcquireComponents()
    {
      if (!(bool) (Object) this.reflection)
        this.reflection = !(bool) (Object) this.transform.parent ? this.transform.GetComponent<PlanarReflection>() : this.transform.parent.GetComponent<PlanarReflection>();
      if ((bool) (Object) this.waterBase)
        return;
      if ((bool) (Object) this.transform.parent)
        this.waterBase = this.transform.parent.GetComponent<WaterBase>();
      else
        this.waterBase = this.transform.GetComponent<WaterBase>();
    }

    public void OnWillRenderObject()
    {
      if ((bool) (Object) this.reflection)
        this.reflection.WaterTileBeingRendered(this.transform, Camera.current);
      if (!(bool) (Object) this.waterBase)
        return;
      this.waterBase.WaterTileBeingRendered(this.transform, Camera.current);
    }
  }
}
