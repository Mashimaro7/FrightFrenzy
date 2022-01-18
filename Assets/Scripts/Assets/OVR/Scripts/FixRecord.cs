// Decompiled with JetBrains decompiler
// Type: Assets.OVR.Scripts.FixRecord
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace Assets.OVR.Scripts
{
  public class FixRecord : Record
  {
    public FixMethodDelegate fixMethod;
    public Object targetObject;
    public string[] buttonNames;
    public bool complete;

    public FixRecord(
      string cat,
      string msg,
      FixMethodDelegate fix,
      Object target,
      string[] buttons)
      : base(cat, msg)
    {
      this.buttonNames = buttons;
      this.fixMethod = fix;
      this.targetObject = target;
      this.complete = false;
    }
  }
}
