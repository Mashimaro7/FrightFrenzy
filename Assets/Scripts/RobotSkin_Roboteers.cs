// Decompiled with JetBrains decompiler
// Type: RobotSkin_Roboteers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class RobotSkin_Roboteers : RobotSkin
{
  public List<RotateObject> shooter_gears = new List<RotateObject>();
  private bool gamepad1_x_old;
  public List<RotateObject> intake_gears = new List<RotateObject>();
  public List<float> intake_gears_speed = new List<float>();
  private Robot_FRC_shooter myrobot;
  public int intake_state;

  public override void InitSkin()
  {
    base.InitSkin();
    foreach (RotateObject componentsInChild in this.GetComponentsInChildren<RotateObject>())
    {
      if (componentsInChild.tag == "shooter")
        this.shooter_gears.Add(componentsInChild);
      if (componentsInChild.tag == "intake")
      {
        this.intake_gears.Add(componentsInChild);
        this.intake_gears_speed.Add(componentsInChild.speed);
        componentsInChild.run = true;
        componentsInChild.speed = 0.0f;
      }
    }
    foreach (RotateObject shooterGear in this.shooter_gears)
    {
      shooterGear.run = true;
      shooterGear.speed = 600f;
    }
    this.myrobot = this.GetComponent<Robot_FRC_shooter>();
    this.gamepad1_x_old = false;
  }

  private void Update()
  {
    if (GLOBALS.CLIENT_MODE)
      return;
    if (this.gamepad1_x_old != this.ri3d.gamepad1_x)
    {
      foreach (RotateObject shooterGear in this.shooter_gears)
        shooterGear.speed = this.ri3d.gamepad1_x ? -1200f : 600f;
      this.gamepad1_x_old = this.ri3d.gamepad1_x;
    }
    if (!(bool) (Object) this.myrobot)
      return;
    this.intake_state = (int) this.myrobot.intake_statemachine;
    this.DoIntakeWheels();
  }

  private void DoIntakeWheels()
  {
    for (int index = this.intake_gears.Count - 1; index >= 0; --index)
    {
      switch (this.intake_state)
      {
        case 0:
          this.intake_gears[index].speed = 0.0f;
          break;
        case 1:
          this.intake_gears[index].speed = this.intake_gears_speed[index];
          break;
        case 2:
          this.intake_gears[index].speed = -1f * this.intake_gears_speed[index];
          break;
      }
    }
  }

  public override string GetState() => !this.gamepad1_x_old ? "0|" + (object) this.intake_state : "1";

  public override void SetState(string instate)
  {
    string[] strArray = instate.Split('|');
    if (strArray.Length < 2 || strArray[0].Length < 1)
      return;
    bool flag = strArray[0].StartsWith("1");
    if (this.gamepad1_x_old != flag)
    {
      foreach (RotateObject shooterGear in this.shooter_gears)
        shooterGear.speed = flag ? -1200f : 600f;
      this.gamepad1_x_old = flag;
    }
    int.TryParse(strArray[1], out this.intake_state);
    this.DoIntakeWheels();
  }
}
