// Decompiled with JetBrains decompiler
// Type: RobotSkin_FRC_shooter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class RobotSkin_FRC_shooter : RobotSkin
{
  public List<RotateObject> shooter_gears = new List<RotateObject>();
  private bool gamepad1_x_old;

  public override void InitSkin()
  {
    base.InitSkin();
    foreach (RotateObject componentsInChild in this.GetComponentsInChildren<RotateObject>())
    {
      if (componentsInChild.tag == "shooter")
        this.shooter_gears.Add(componentsInChild);
    }
    foreach (RotateObject shooterGear in this.shooter_gears)
      shooterGear.run = false;
    this.gamepad1_x_old = false;
  }

  private void Update()
  {
    if (GLOBALS.CLIENT_MODE || this.gamepad1_x_old == this.ri3d.gamepad1_x)
      return;
    foreach (RotateObject shooterGear in this.shooter_gears)
      shooterGear.run = this.ri3d.gamepad1_x;
    this.gamepad1_x_old = this.ri3d.gamepad1_x;
  }

  public override string GetState() => !this.gamepad1_x_old ? "0" : "1";

  public override void SetState(string instate)
  {
    if (instate.Length < 1)
      return;
    bool flag = instate.StartsWith("1");
    if (flag == this.gamepad1_x_old)
      return;
    foreach (RotateObject shooterGear in this.shooter_gears)
      shooterGear.run = flag;
    this.gamepad1_x_old = flag;
  }
}
