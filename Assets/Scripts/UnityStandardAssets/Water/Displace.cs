﻿// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Water.Displace
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace UnityStandardAssets.Water
{
  [ExecuteInEditMode]
  [RequireComponent(typeof (WaterBase))]
  public class Displace : MonoBehaviour
  {
    public void Awake()
    {
      if (this.enabled)
        this.OnEnable();
      else
        this.OnDisable();
    }

    public void OnEnable()
    {
      Shader.EnableKeyword("WATER_VERTEX_DISPLACEMENT_ON");
      Shader.DisableKeyword("WATER_VERTEX_DISPLACEMENT_OFF");
    }

    public void OnDisable()
    {
      Shader.EnableKeyword("WATER_VERTEX_DISPLACEMENT_OFF");
      Shader.DisableKeyword("WATER_VERTEX_DISPLACEMENT_ON");
    }
  }
}
