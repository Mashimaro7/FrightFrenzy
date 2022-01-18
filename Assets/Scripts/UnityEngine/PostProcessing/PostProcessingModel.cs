// Decompiled with JetBrains decompiler
// Type: UnityEngine.PostProcessing.PostProcessingModel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;

namespace UnityEngine.PostProcessing
{
  [Serializable]
  public abstract class PostProcessingModel
  {
    [SerializeField]
    [GetSet("enabled")]
    private bool m_Enabled;

    public bool enabled
    {
      get => this.m_Enabled;
      set
      {
        this.m_Enabled = value;
        if (!value)
          return;
        this.OnValidate();
      }
    }

    public abstract void Reset();

    public virtual void OnValidate()
    {
    }
  }
}
