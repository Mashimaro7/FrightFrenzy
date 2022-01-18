// Decompiled with JetBrains decompiler
// Type: Robot_Cupbot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Robot_Cupbot : RobotInterface3D
{
  public float pushbot_arm_speed = 75f;
  public HingeJoint mainArmJoint;
  public float arm_max_limit = 180f;
  public float arm_min_limit = -180f;
  private float turn_scale_original = -1f;
  private bool init_done;

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
    if (this.gamepad1_dpad_down)
    {
      spring.targetPosition += this.pushbot_arm_speed * Time.deltaTime;
      if ((double) spring.targetPosition > (double) this.arm_max_limit)
        spring.targetPosition = this.arm_max_limit;
    }
    if (this.gamepad1_dpad_up)
    {
      spring.targetPosition -= this.pushbot_arm_speed * Time.deltaTime;
      if ((double) spring.targetPosition < (double) this.arm_min_limit)
        spring.targetPosition = this.arm_min_limit;
    }
    mainArmJoint.spring = spring;
    this.mainArmJoint = mainArmJoint;
    if ((double) spring.targetPosition > (double) this.arm_max_limit - 60.0)
    {
      float num = (float) (((double) this.arm_max_limit - (double) spring.targetPosition) / 60.0);
      if ((double) num < 0.300000011920929)
        num = 0.3f;
      this.turn_scale = this.turn_scale_original * num;
    }
    else
      this.turn_scale = this.turn_scale_original;
    int num1 = this.gamepad1_x ? 1 : 0;
    int num2 = this.gamepad1_a ? 1 : 0;
  }
}
