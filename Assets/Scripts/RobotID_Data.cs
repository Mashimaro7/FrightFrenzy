// Decompiled with JetBrains decompiler
// Type: RobotID_Data
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class RobotID_Data
{
  public Transform wheelTL;
  public Transform wheelTR;
  public Transform wheelBL;
  public Transform wheelBR;
  public Vector3 position;
  public string starting_pos;
  public int id;
  public bool is_red;
  public bool is_holding;
  public Dictionary<string, string> userData = new Dictionary<string, string>();

  public void Copy(RobotID refin)
  {
    this.starting_pos = refin.starting_pos;
    this.id = refin.id;
    this.is_red = refin.is_red;
    this.is_holding = refin.is_holding;
    foreach (string key in refin.userData.Keys)
      this.userData[key] = refin.userData[key];
  }
}
