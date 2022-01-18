// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Utility.AutoMobileShaderSwitch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
  public class AutoMobileShaderSwitch : MonoBehaviour
  {
    [SerializeField]
    private AutoMobileShaderSwitch.ReplacementList m_ReplacementList;

    private void OnEnable()
    {
    }

    [Serializable]
    public class ReplacementDefinition
    {
      public Shader original;
      public Shader replacement;
    }

    [Serializable]
    public class ReplacementList
    {
      public AutoMobileShaderSwitch.ReplacementDefinition[] items = new AutoMobileShaderSwitch.ReplacementDefinition[0];
    }
  }
}
