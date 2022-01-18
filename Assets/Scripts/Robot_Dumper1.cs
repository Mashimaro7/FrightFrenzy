// Decompiled with JetBrains decompiler
// Type: Robot_Dumper1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Robot_Dumper1 : RobotInterface3D
{
  public HingeJoint dumper;
  public HingeJoint gripper_1;
  public HingeJoint gripper_2;
  public ConfigurableJoint lift;
  public ConfigurableJoint collector_l;
  public ConfigurableJoint collector_r;
  public ConfigurableJoint collector_mid_l;
  public ConfigurableJoint collector_mid_r;
  public ConfigurableJoint collector_out_l;
  public ConfigurableJoint collector_out_r;
  public float dumper1_wheelspeed = 20f;
  public float dumper1_reversspeed = 40f;
  private bool gripped;
  private bool grip_button_last;
  private float lastResetTime;
  private bool a_button_last;
  private Robot_Dumper1.collectingStates collecting_state;

  public void Awake() => this.info = "<Missing Robot Specific Function: TBD>" + this.info;

  private void deactivateGrippers()
  {
    this.gripper_1.spring = this.gripper_1.spring with
    {
      targetPosition = 0.0f
    };
    this.gripper_2.spring = this.gripper_2.spring with
    {
      targetPosition = 0.0f
    };
  }

  private void activateGrippers()
  {
    this.gripper_1.spring = this.gripper_1.spring with
    {
      targetPosition = 40f
    };
    this.gripper_2.spring = this.gripper_2.spring with
    {
      targetPosition = -40f
    };
  }

  public override void Update_Robot()
  {
    if (this.gamepad1_b)
    {
      this.dumper.spring = this.dumper.spring with
      {
        targetPosition = 0.0f
      };
      ConfigurableJoint lift = this.lift;
      lift.targetPosition = new Vector3(0.0f, 0.0f, 0.0f);
      this.lift = lift;
      this.gripped = true;
      this.lastResetTime = Time.fixedTime;
    }
    if ((double) Time.fixedTime - (double) this.lastResetTime > 0.6 && (double) this.lastResetTime != -1.0)
    {
      this.gripped = false;
      this.lastResetTime = -1f;
    }
    if (this.gamepad1_x)
      this.dumper.spring = this.dumper.spring with
      {
        targetPosition = -117f
      };
    if (this.gamepad1_dpad_up)
    {
      ConfigurableJoint lift = this.lift;
      lift.targetPosition = new Vector3(0.0f, -0.6f, 0.0f);
      this.lift = lift;
    }
    bool flag = this.gamepad1_right_bumper || this.gamepad1_dpad_down || this.gamepad1_dpad_up;
    if (flag && !this.grip_button_last)
      this.gripped = !this.gripped;
    this.grip_button_last = flag;
    if (this.gripped)
      this.activateGrippers();
    else
      this.deactivateGrippers();
    if (this.gamepad1_y)
      this.collecting_state = Robot_Dumper1.collectingStates.reverse;
    if (this.gamepad1_a && !this.a_button_last)
      this.collecting_state = this.collecting_state != Robot_Dumper1.collectingStates.onNormal ? Robot_Dumper1.collectingStates.onNormal : Robot_Dumper1.collectingStates.off;
    this.a_button_last = this.gamepad1_a;
    if (this.collecting_state == Robot_Dumper1.collectingStates.onNormal)
    {
      this.collector_l.targetAngularVelocity = new Vector3(this.dumper1_wheelspeed, 0.0f, 0.0f);
      this.collector_r.targetAngularVelocity = new Vector3(-1f * this.dumper1_wheelspeed, 0.0f, 0.0f);
      this.collector_mid_l.targetAngularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
      this.collector_mid_r.targetAngularVelocity = new Vector3(-2f * this.dumper1_wheelspeed, 0.0f, 0.0f);
      this.collector_out_l.targetAngularVelocity = new Vector3(2f * this.dumper1_wheelspeed, 0.0f, 0.0f);
      this.collector_out_r.targetAngularVelocity = new Vector3(-2f * this.dumper1_wheelspeed, 0.0f, 0.0f);
    }
    if (this.collecting_state == Robot_Dumper1.collectingStates.off)
    {
      this.collector_l.targetAngularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
      this.collector_r.targetAngularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
      this.collector_mid_l.targetAngularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
      this.collector_mid_r.targetAngularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
      this.collector_out_l.targetAngularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
      this.collector_out_r.targetAngularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
    }
    if (this.collecting_state != Robot_Dumper1.collectingStates.reverse)
      return;
    this.collector_l.targetAngularVelocity = new Vector3(-1f * this.dumper1_reversspeed, 0.0f, 0.0f);
    this.collector_r.targetAngularVelocity = new Vector3(this.dumper1_reversspeed, 0.0f, 0.0f);
    this.collector_mid_l.targetAngularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
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
