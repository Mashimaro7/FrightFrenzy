﻿// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.CrossPlatformInput.ButtonHandler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace UnityStandardAssets.CrossPlatformInput
{
  public class ButtonHandler : MonoBehaviour
  {
    public string Name;

    private void OnEnable()
    {
    }

    public void SetDownState() => CrossPlatformInputManager.SetButtonDown(this.Name);

    public void SetUpState() => CrossPlatformInputManager.SetButtonUp(this.Name);

    public void SetAxisPositiveState() => CrossPlatformInputManager.SetAxisPositive(this.Name);

    public void SetAxisNeutralState() => CrossPlatformInputManager.SetAxisZero(this.Name);

    public void SetAxisNegativeState() => CrossPlatformInputManager.SetAxisNegative(this.Name);

    public void Update()
    {
    }
  }
}
