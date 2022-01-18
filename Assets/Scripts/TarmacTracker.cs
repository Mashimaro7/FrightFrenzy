// Decompiled with JetBrains decompiler
// Type: TarmacTracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class TarmacTracker : GenericFieldTracker
{
  public List<RobotID> starting_robots = new List<RobotID>();

  public void MarkStartingRobots()
  {
    this.starting_robots.Clear();
    foreach (RobotID robot in this.robots)
      this.starting_robots.Add(robot);
  }

  public override void RobotExited(RobotID robotid)
  {
    if ((Object) robotid == (Object) null || !this.starting_robots.Contains(robotid))
      return;
    robotid.SetUserBool("Taxi");
  }
}
