// Decompiled with JetBrains decompiler
// Type: Robot_DarkKnight
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

public class Robot_DarkKnight : RobotInterface3D
{
  public ConfigurableJoint collector_l;
  public ConfigurableJoint collector_r;
  public ConfigurableJoint collector_mid_l;
  public ConfigurableJoint collector_mid_r;
  public ConfigurableJoint collector_out_l;
  public ConfigurableJoint collector_out_r;
  public HingeJoint top_intake;
  public ballshooting top_intake_hitbox;
  public ConfigurableJoint ball_lift;
  public float intake_wheelspeed = 20f;
  public float intake_reversespeed = 20f;
  public float ball_feeder_hinge_speed = -200f;
  public float ball_feeder_speed = 0.5f;
  public float lift_speed = 0.1f;
  public float lift_top_pos = -0.9f;
  private Robot_DarkKnight.collectingStates top_states;
  private Robot_DarkKnight.collectingStates intake_states;
  private bool last_button_x;
  private bool last_button_a;

  public void Awake() => this.info = "<Missing Robot Specific Function: TBD>" + this.info;

  public override void Update_Robot()
  {
    bool flag1 = false;
    if (this.gamepad1_x != this.last_button_x && this.gamepad1_x)
      this.top_states = this.top_states != Robot_DarkKnight.collectingStates.off ? Robot_DarkKnight.collectingStates.off : Robot_DarkKnight.collectingStates.onNormal;
    this.last_button_x = this.gamepad1_x;
    if (this.gamepad1_y)
      flag1 = true;
    if (this.top_states == Robot_DarkKnight.collectingStates.off)
      this.top_intake.motor = this.top_intake.motor with
      {
        targetVelocity = 0.0f
      };
    if (this.top_states == Robot_DarkKnight.collectingStates.onNormal)
      this.top_intake.motor = this.top_intake.motor with
      {
        targetVelocity = this.ball_feeder_hinge_speed
      };
    if (flag1)
    {
      this.top_intake_hitbox.speed = this.ball_feeder_speed;
      this.top_intake.motor = this.top_intake.motor with
      {
        targetVelocity = -1f * this.ball_feeder_hinge_speed
      };
    }
    else
      this.top_intake_hitbox.speed = 0.0f;
    bool flag2 = false;
    if (this.gamepad1_b)
      flag2 = true;
    if (this.gamepad1_a != this.last_button_a && this.gamepad1_a)
      this.intake_states = this.intake_states != Robot_DarkKnight.collectingStates.off ? Robot_DarkKnight.collectingStates.off : Robot_DarkKnight.collectingStates.onNormal;
    this.last_button_a = this.gamepad1_a;
    if (this.intake_states == Robot_DarkKnight.collectingStates.onNormal)
    {
      this.collector_l.targetAngularVelocity = new Vector3(this.intake_wheelspeed, 0.0f, 0.0f);
      this.collector_r.targetAngularVelocity = new Vector3(-1f * this.intake_wheelspeed, 0.0f, 0.0f);
      this.collector_mid_l.targetAngularVelocity = new Vector3(2f * this.intake_wheelspeed, 0.0f, 0.0f);
      this.collector_mid_r.targetAngularVelocity = new Vector3(-2f * this.intake_wheelspeed, 0.0f, 0.0f);
      this.collector_out_l.targetAngularVelocity = new Vector3(2f * this.intake_wheelspeed, 0.0f, 0.0f);
      this.collector_out_r.targetAngularVelocity = new Vector3(-2f * this.intake_wheelspeed, 0.0f, 0.0f);
    }
    if (this.intake_states == Robot_DarkKnight.collectingStates.off)
    {
      this.collector_l.targetAngularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
      this.collector_r.targetAngularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
      this.collector_mid_l.targetAngularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
      this.collector_mid_r.targetAngularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
      this.collector_out_l.targetAngularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
      this.collector_out_r.targetAngularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
    }
    if (flag2)
    {
      this.collector_l.targetAngularVelocity = new Vector3(-1f * this.intake_reversespeed, 0.0f, 0.0f);
      this.collector_r.targetAngularVelocity = new Vector3(this.intake_reversespeed, 0.0f, 0.0f);
      this.collector_mid_l.targetAngularVelocity = new Vector3(-2f * this.intake_reversespeed, 0.0f, 0.0f);
      this.collector_mid_r.targetAngularVelocity = new Vector3(2f * this.intake_reversespeed, 0.0f, 0.0f);
      this.collector_out_l.targetAngularVelocity = new Vector3(-2f * this.intake_reversespeed, 0.0f, 0.0f);
      this.collector_out_r.targetAngularVelocity = new Vector3(2f * this.intake_reversespeed, 0.0f, 0.0f);
    }
    if (this.gamepad1_dpad_up && (double) this.ball_lift.targetPosition.y > (double) this.lift_top_pos)
    {
      Vector3 targetPosition = this.ball_lift.targetPosition;
      targetPosition.y = this.MoveTowards(targetPosition.y, this.lift_top_pos, targetPosition.y, Time.deltaTime * this.lift_speed);
      this.ball_lift.targetPosition = targetPosition;
    }
    if (!this.gamepad1_dpad_down || (double) this.ball_lift.targetPosition.y >= 0.0)
      return;
    Vector3 targetPosition1 = this.ball_lift.targetPosition;
    targetPosition1.y = this.MoveTowards(targetPosition1.y, 0.0f, targetPosition1.y, Time.deltaTime * this.lift_speed);
    this.ball_lift.targetPosition = targetPosition1;
  }

  public override void SetName(string name)
  {
    base.SetName(name);
    bool flag = false;
    Transform transform1 = this.transform.Find("Body/NametagB1");
    if ((bool) (Object) transform1)
    {
      transform1.GetComponent<TextMeshPro>().text = name;
      flag = true;
    }
    Transform transform2 = this.transform.Find("Body/NametagB2");
    if ((bool) (Object) transform2)
    {
      transform2.GetComponent<TextMeshPro>().text = name;
      flag = true;
    }
    if (!flag)
      return;
    this.transform.Find("Body/Nametag").gameObject.SetActive(false);
  }

  private enum collectingStates
  {
    off,
    onNormal,
    reverse,
  }
}
