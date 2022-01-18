// Decompiled with JetBrains decompiler
// Type: Robot_SnailBot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Robot_SnailBot : RobotInterface3D
{
  public ConfigurableJoint collector_l;
  public ConfigurableJoint collector_r;
  public ConfigurableJoint collector_mid_l;
  public ConfigurableJoint collector_mid_r;
  public ConfigurableJoint collector_out_l;
  public ConfigurableJoint collector_out_r;
  public HingeJoint ball_feeder_hinge1;
  public HingeJoint ball_feeder_hinge2;
  public ballshooting ball_feeder;
  public ballshooting ball_feeder_pre;
  public float dumper1_wheelspeed = 40f;
  public float dumper1_reversspeed = 40f;
  public float ball_feeder_hinge_speed = -600f;
  public float ball_feeder_speed = 1.5f;
  private Robot_SnailBot.collectingStates collecting_state;
  private bool last_button_a;

  public void Awake() => this.info = "<Missing Robot Specific Function: TBD>" + this.info;

  public override void Update_Robot()
  {
    if (this.gamepad1_x)
    {
      this.ball_feeder.hard_stop = false;
      this.ball_feeder.speed = this.ball_feeder_speed;
      this.ball_feeder_pre.speed = this.ball_feeder_speed;
      this.ball_feeder_hinge1.motor = this.ball_feeder_hinge1.motor with
      {
        targetVelocity = this.ball_feeder_hinge_speed
      };
      this.ball_feeder_hinge2.motor = this.ball_feeder_hinge2.motor with
      {
        targetVelocity = -1f * this.ball_feeder_hinge_speed
      };
    }
    else
    {
      this.ball_feeder.speed = 1f;
      this.ball_feeder_pre.speed = 0.0f;
      this.ball_feeder.hard_stop = true;
      JointMotor motor = this.ball_feeder_hinge1.motor with
      {
        targetVelocity = 0.0f
      };
      this.ball_feeder_hinge1.motor = motor;
      motor = this.ball_feeder_hinge2.motor with
      {
        targetVelocity = 0.0f
      };
      this.ball_feeder_hinge2.motor = motor;
    }
    bool flag = false;
    if (this.gamepad1_y)
    {
      flag = true;
      this.ball_feeder.speed = -1f * this.ball_feeder_speed;
      this.ball_feeder_pre.speed = -1f * this.ball_feeder_speed;
      this.ball_feeder.hard_stop = false;
    }
    if (this.gamepad1_a && this.last_button_a != this.gamepad1_a)
      this.collecting_state = this.collecting_state != Robot_SnailBot.collectingStates.off ? Robot_SnailBot.collectingStates.off : Robot_SnailBot.collectingStates.onNormal;
    this.last_button_a = this.gamepad1_a;
    if (this.collecting_state == Robot_SnailBot.collectingStates.onNormal)
    {
      this.collector_l.targetAngularVelocity = new Vector3(this.dumper1_wheelspeed, 0.0f, 0.0f);
      this.collector_r.targetAngularVelocity = new Vector3(-1f * this.dumper1_wheelspeed, 0.0f, 0.0f);
      this.collector_mid_l.targetAngularVelocity = new Vector3(2f * this.dumper1_wheelspeed, 0.0f, 0.0f);
      this.collector_mid_r.targetAngularVelocity = new Vector3(-2f * this.dumper1_wheelspeed, 0.0f, 0.0f);
      this.collector_out_l.targetAngularVelocity = new Vector3(2f * this.dumper1_wheelspeed, 0.0f, 0.0f);
      this.collector_out_r.targetAngularVelocity = new Vector3(-2f * this.dumper1_wheelspeed, 0.0f, 0.0f);
    }
    if (this.collecting_state == Robot_SnailBot.collectingStates.off)
    {
      this.collector_l.targetAngularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
      this.collector_r.targetAngularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
      this.collector_mid_l.targetAngularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
      this.collector_mid_r.targetAngularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
      this.collector_out_l.targetAngularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
      this.collector_out_r.targetAngularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
    }
    if (!flag)
      return;
    this.collector_l.targetAngularVelocity = new Vector3(-1f * this.dumper1_reversspeed, 0.0f, 0.0f);
    this.collector_r.targetAngularVelocity = new Vector3(this.dumper1_reversspeed, 0.0f, 0.0f);
    this.collector_mid_l.targetAngularVelocity = new Vector3(-2f * this.dumper1_reversspeed, 0.0f, 0.0f);
    this.collector_mid_r.targetAngularVelocity = new Vector3(2f * this.dumper1_reversspeed, 0.0f, 0.0f);
    this.collector_out_l.targetAngularVelocity = new Vector3(-2f * this.dumper1_reversspeed, 0.0f, 0.0f);
    this.collector_out_r.targetAngularVelocity = new Vector3(2f * this.dumper1_reversspeed, 0.0f, 0.0f);
  }

  private enum collectingStates
  {
    off,
    onNormal,
    reverse,
  }
}
