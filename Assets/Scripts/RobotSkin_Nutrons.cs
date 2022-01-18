// Decompiled with JetBrains decompiler
// Type: RobotSkin_Nutrons
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class RobotSkin_Nutrons : RobotSkin
{
  public List<RotateObject> turret_gears = new List<RotateObject>();
  public List<float> turret_gears_speed = new List<float>();
  public List<RotateObject> intake_gears = new List<RotateObject>();
  public List<float> intake_gears_speed = new List<float>();
  public bool gamepad1_x_old;
  public int intake_state;
  private Robot_FRC_shooter myrobot;
  private float time_previous;
  public float Koff = 1f;
  public float Kexp = 1f;
  public string skin_state;

  public override void InitSkin()
  {
    base.InitSkin();
    foreach (RotateObject componentsInChild in this.GetComponentsInChildren<RotateObject>())
    {
      if (componentsInChild.tag == "turret")
      {
        this.turret_gears.Add(componentsInChild);
        this.turret_gears_speed.Add(componentsInChild.speed);
        componentsInChild.run = true;
        componentsInChild.speed = 0.0f;
      }
      if (componentsInChild.tag == "intake")
      {
        this.intake_gears.Add(componentsInChild);
        this.intake_gears_speed.Add(componentsInChild.speed);
        componentsInChild.run = true;
        componentsInChild.speed = 0.0f;
      }
    }
    this.myrobot = this.GetComponent<Robot_FRC_shooter>();
    this.gamepad1_x_old = false;
  }

  private void Update()
  {
    if (GLOBALS.CLIENT_MODE)
      return;
    if (this.gamepad1_x_old != this.ri3d.gamepad1_x && this.ri3d.gamepad1_x)
      this.Inertia_SpinUpFlywheel();
    if (!this.ri3d.gamepad1_x)
      this.Inertia_StopFlywheel();
    if ((bool) (Object) this.myrobot)
    {
      this.intake_state = (int) this.myrobot.intake_statemachine;
      this.DoIntakeWheels();
    }
    this.gamepad1_x_old = this.ri3d.gamepad1_x;
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

  private void Inertia_SpinUpFlywheel()
  {
    for (int index = this.turret_gears.Count - 1; index >= 0; --index)
      this.turret_gears[index].speed = this.turret_gears_speed[index];
  }

  private void Inertia_StopFlywheel()
  {
    for (int index = this.turret_gears.Count - 1; index >= 0; --index)
    {
      this.turret_gears[index].speed *= Mathf.Pow((float) (1.0 / (1.0 + (double) Time.deltaTime * (double) this.Kexp)), 2f);
      this.turret_gears[index].speed += (float) ((double) Time.deltaTime * (double) this.Koff * ((double) this.turret_gears_speed[index] > 0.0 ? -1.0 : 1.0));
      if ((double) this.turret_gears[index].speed * (double) this.turret_gears_speed[index] < 0.0)
        this.turret_gears[index].speed = 0.0f;
    }
  }

  public override string GetState() => !this.gamepad1_x_old ? "0|" + (object) this.intake_state : "1";

  public override void SetState(string instate)
  {
    string[] strArray = instate.Split('|');
    if (strArray.Length < 2)
      return;
    this.skin_state = strArray[0];
    if (this.skin_state.Length < 1)
      return;
    bool flag = this.skin_state.StartsWith("1");
    if (this.gamepad1_x_old != flag & flag)
      this.Inertia_SpinUpFlywheel();
    if (!flag)
      this.Inertia_StopFlywheel();
    this.gamepad1_x_old = flag;
    int.TryParse(strArray[1], out this.intake_state);
    this.DoIntakeWheels();
  }
}
