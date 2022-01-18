// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Utility.AutoMoveAndRotate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
  public class AutoMoveAndRotate : MonoBehaviour
  {
    public AutoMoveAndRotate.Vector3andSpace moveUnitsPerSecond;
    public AutoMoveAndRotate.Vector3andSpace rotateDegreesPerSecond;
    public bool ignoreTimescale;
    private float m_LastRealTime;

    private void Start() => this.m_LastRealTime = Time.realtimeSinceStartup;

    private void Update()
    {
      float num = Time.deltaTime;
      if (this.ignoreTimescale)
      {
        num = Time.realtimeSinceStartup - this.m_LastRealTime;
        this.m_LastRealTime = Time.realtimeSinceStartup;
      }
      this.transform.Translate(this.moveUnitsPerSecond.value * num, this.moveUnitsPerSecond.space);
      this.transform.Rotate(this.rotateDegreesPerSecond.value * num, this.moveUnitsPerSecond.space);
    }

    [Serializable]
    public class Vector3andSpace
    {
      public Vector3 value;
      public Space space = Space.Self;
    }
  }
}
