// Decompiled with JetBrains decompiler
// Type: Robot_SingleArm
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Robot_SingleArm : RobotInterface3D
{
  public float pushbot_arm_speed = 75f;
  public HingeJoint mainArmJoint;
  public float arm_max_limit = 180f;
  public float arm_min_limit = -180f;
  private float turn_scale_original = -1f;
  private bool init_done;
  public ConfigurableJoint lift_arm;
  public ConfigurableJoint dummy_lift_arm;
  public float lift_down_pos;
  public float lift_up_pos = 10f;
  public float lift_speed = 10f;
  public HingeJoint collector;
  public float collector_speed = -1000f;
  private bool gamepad_a_previous;
  private bool gamepad_b_previous;
  private bool gamepad_y_previous;
  private Robot_SingleArm.Collector_States collector_state;

  public void Awake() => this.info = "<Missing Robot Specific Function: TBD>" + this.info;

  public override void Update_Robot()
  {
    HingeJoint mainArmJoint = this.mainArmJoint;
    JointSpring spring = mainArmJoint.spring;
    if (!this.init_done)
    {
      this.turn_scale_original = this.turn_scale;
      this.init_done = true;
    }
    if (this.gamepad1_dpad_right)
    {
      spring.targetPosition += this.pushbot_arm_speed * Time.deltaTime;
      if ((double) spring.targetPosition > (double) this.arm_max_limit)
        spring.targetPosition = this.arm_max_limit;
    }
    if (this.gamepad1_dpad_left)
    {
      spring.targetPosition -= this.pushbot_arm_speed * Time.deltaTime;
      if ((double) spring.targetPosition < (double) this.arm_min_limit)
        spring.targetPosition = this.arm_min_limit;
    }
    mainArmJoint.spring = spring;
    this.mainArmJoint = mainArmJoint;
    if ((double) spring.targetPosition < (double) this.arm_min_limit + 40.0)
    {
      float num = (float) (((double) this.arm_min_limit - (double) spring.targetPosition) / -40.0);
      if ((double) num < 0.300000011920929)
        num = 0.3f;
      this.turn_scale = this.turn_scale_original * num;
    }
    else
      this.turn_scale = this.turn_scale_original;
    if (this.gamepad1_dpad_down)
    {
      this.lift_arm.targetPosition = new Vector3(this.MoveTowards(this.lift_arm.targetPosition.x, this.lift_down_pos, this.lift_arm.targetPosition.x, Time.deltaTime * this.lift_speed), 0.0f, 0.0f);
      this.dummy_lift_arm.targetPosition = new Vector3(this.MoveTowards(this.dummy_lift_arm.targetPosition.x, this.lift_down_pos / 2f, this.dummy_lift_arm.targetPosition.x, (float) ((double) Time.deltaTime * (double) this.lift_speed / 2.0)), 0.0f, 0.0f);
    }
    if (this.gamepad1_dpad_up)
    {
      this.lift_arm.targetPosition = new Vector3(this.MoveTowards(this.lift_arm.targetPosition.x, this.lift_up_pos, this.lift_arm.targetPosition.x, Time.deltaTime * this.lift_speed), 0.0f, 0.0f);
      this.dummy_lift_arm.targetPosition = new Vector3(this.MoveTowards(this.dummy_lift_arm.targetPosition.x, this.lift_up_pos / 2f, this.dummy_lift_arm.targetPosition.x, (float) ((double) Time.deltaTime * (double) this.lift_speed / 2.0)), 0.0f, 0.0f);
    }
    if (this.gamepad1_b)
    {
      if (!this.gamepad_b_previous)
        this.collector_state = this.collector_state == Robot_SingleArm.Collector_States.off ? Robot_SingleArm.Collector_States.reverse : Robot_SingleArm.Collector_States.off;
      this.gamepad_b_previous = true;
    }
    else
      this.gamepad_b_previous = false;
    if (this.gamepad1_a)
    {
      if (!this.gamepad_a_previous)
        this.collector_state = this.collector_state == Robot_SingleArm.Collector_States.off ? Robot_SingleArm.Collector_States.on : Robot_SingleArm.Collector_States.off;
      this.gamepad_a_previous = true;
    }
    else
      this.gamepad_a_previous = false;
    float num1 = 0.0f;
    switch (this.collector_state)
    {
      case Robot_SingleArm.Collector_States.on:
        num1 = this.collector_speed;
        break;
      case Robot_SingleArm.Collector_States.reverse:
        num1 = -this.collector_speed;
        break;
    }
    this.collector.motor = this.collector.motor with
    {
      targetVelocity = num1
    };
    int num2 = this.gamepad1_x ? 1 : 0;
    int num3 = this.gamepad1_a ? 1 : 0;
  }

  private enum Collector_States
  {
    off,
    on,
    reverse,
  }
}
