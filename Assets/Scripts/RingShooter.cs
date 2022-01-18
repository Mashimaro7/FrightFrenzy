// Decompiled with JetBrains decompiler
// Type: RingShooter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class RingShooter : ballshooting
{
  private GenericFieldTracker nolaunchzone;

  public override void MyStart()
  {
    GameObject gameObject = GameObject.Find("NoLaunchZone");
    if (!(bool) (Object) gameObject)
      return;
    this.nolaunchzone = gameObject.GetComponent<GenericFieldTracker>();
  }

  public override void MarkGameElement(ball_data thisringdata)
  {
    if (!(bool) (Object) this.nolaunchzone)
    {
      this.MyStart();
      if (!(bool) (Object) this.nolaunchzone)
        return;
    }
    if (this.nolaunchzone.IsRobotInside(this.transform.root))
    {
      if (thisringdata.flags.ContainsKey("Bad"))
        return;
      thisringdata.flags.Add("Bad", "1");
    }
    else
      thisringdata.flags.Remove("Bad");
  }
}
