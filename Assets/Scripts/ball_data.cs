// Decompiled with JetBrains decompiler
// Type: ball_data
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class ball_data : MonoBehaviour
{
  public bool scored_red;
  public bool scored_blue;
  public int thrown_by_id = -1;
  public RobotID thrown_robotid;
  public Dictionary<string, string> flags = new Dictionary<string, string>();

  public void Clear()
  {
    this.thrown_by_id = -1;
    this.thrown_robotid = (RobotID) null;
    this.scored_red = false;
    this.scored_blue = false;
    this.flags.Clear();
  }

  public bool IsFlagSet(string myflag) => this.flags.ContainsKey(myflag) && this.flags[myflag][0] == '1';

  public void SetFlag(string myflag, bool value = true) => this.flags[myflag] = value ? "1" : "0";

  public void ClearFlag(string myflag)
  {
    if (!this.flags.ContainsKey(myflag))
      return;
    this.flags.Remove(myflag);
  }
}
