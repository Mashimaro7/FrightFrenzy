﻿// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Utility.LerpControlledBob
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
  [Serializable]
  public class LerpControlledBob
  {
    public float BobDuration;
    public float BobAmount;
    private float m_Offset;

    public float Offset() => this.m_Offset;

    public IEnumerator DoBobCycle()
    {
      float t = 0.0f;
      while ((double) t < (double) this.BobDuration)
      {
        this.m_Offset = Mathf.Lerp(0.0f, this.BobAmount, t / this.BobDuration);
        t += Time.deltaTime;
        yield return (object) new WaitForFixedUpdate();
      }
      t = 0.0f;
      while ((double) t < (double) this.BobDuration)
      {
        this.m_Offset = Mathf.Lerp(this.BobAmount, 0.0f, t / this.BobDuration);
        t += Time.deltaTime;
        yield return (object) new WaitForFixedUpdate();
      }
      this.m_Offset = 0.0f;
    }
  }
}
