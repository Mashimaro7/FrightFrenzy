﻿// Decompiled with JetBrains decompiler
// Type: UnityEngine.PostProcessing.UserLutModel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;

namespace UnityEngine.PostProcessing
{
  [Serializable]
  public class UserLutModel : PostProcessingModel
  {
    [SerializeField]
    private UserLutModel.Settings m_Settings = UserLutModel.Settings.defaultSettings;

    public UserLutModel.Settings settings
    {
      get => this.m_Settings;
      set => this.m_Settings = value;
    }

    public override void Reset() => this.m_Settings = UserLutModel.Settings.defaultSettings;

    [Serializable]
    public struct Settings
    {
      [Tooltip("Custom lookup texture (strip format, e.g. 256x16).")]
      public Texture2D lut;
      [Range(0.0f, 1f)]
      [Tooltip("Blending factor.")]
      public float contribution;

      public static UserLutModel.Settings defaultSettings => new UserLutModel.Settings()
      {
        lut = (Texture2D) null,
        contribution = 1f
      };
    }
  }
}
