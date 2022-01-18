// Decompiled with JetBrains decompiler
// Type: Robot_CheesyButtons
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Robot_CheesyButtons : RobotInterface3D
{
  public HingeJoint mainArmJoint;
  public float arm_speed = 75f;
  public float arm_max_limit = 180f;
  public float arm_min_limit = -180f;
  private float turn_scale_original = -1f;
  private bool init_done;
  public ConfigurableJoint hand;
  public ConfigurableJoint lift_arm;
  public ConfigurableJoint dummy_lift_arm;
  public float lift_down_pos;
  public float lift_up_pos = 10f;
  public float lift_speed = 10f;
  public HingeJoint fingers;
  public float finger_speed = 100f;
  private bool gamepad_a_previous;
  private bool gamepad_b_previous;
  private bool gamepad_y_previous;

  public void Awake() => this.info = "<Missing Robot Specific Function: TBD>" + this.info;

  public override void Update_Robot()
  {
    if (!this.init_done)
    {
      this.turn_scale_original = this.turn_scale;
      this.init_done = true;
    }
    float speed = this.arm_speed * (float) (1.0 - ((double) this.lift_arm.targetPosition.x - (double) this.lift_down_pos) / ((double) this.lift_up_pos - (double) this.lift_down_pos) * 0.800000011920929);
    if (this.gamepad1_dpad_right)
      this.MoveHinge(this.mainArmJoint, this.arm_max_limit, speed);
    if (this.gamepad1_dpad_left)
      this.MoveHinge(this.mainArmJoint, this.arm_min_limit, speed);
    if ((double) this.mainArmJoint.spring.targetPosition < (double) this.arm_min_limit + 40.0)
    {
      float num = (float) (((double) this.arm_min_limit - (double) this.mainArmJoint.spring.targetPosition) / -40.0);
      if ((double) num < 0.300000011920929)
        num = 0.3f;
      this.turn_scale = this.turn_scale_original * num;
    }
    else
      this.turn_scale = this.turn_scale_original;
    if (this.gamepad1_dpad_down)
    {
      this.hand.targetPosition = new Vector3(this.MoveTowards(this.hand.targetPosition.x, this.lift_down_pos + 0.68f, this.hand.targetPosition.x, Time.deltaTime * this.lift_speed), 0.0f, 0.0f);
      if ((double) this.hand.targetPosition.x >= (double) this.lift_arm.targetPosition.x + 0.670000016689301)
      {
        this.lift_arm.targetPosition = new Vector3(this.MoveTowards(this.lift_arm.targetPosition.x, this.lift_down_pos, this.lift_arm.targetPosition.x, Time.deltaTime * this.lift_speed), 0.0f, 0.0f);
        this.dummy_lift_arm.targetPosition = new Vector3(this.MoveTowards(this.dummy_lift_arm.targetPosition.x, this.lift_down_pos / 2f, this.dummy_lift_arm.targetPosition.x, (float) ((double) Time.deltaTime * (double) this.lift_speed / 2.0)), 0.0f, 0.0f);
      }
    }
    if (this.gamepad1_dpad_up)
    {
      this.hand.targetPosition = new Vector3(this.MoveTowards(this.hand.targetPosition.x, this.lift_up_pos, this.hand.targetPosition.x, Time.deltaTime * this.lift_speed), 0.0f, 0.0f);
      if ((double) this.hand.targetPosition.x <= (double) this.lift_arm.targetPosition.x)
      {
        this.lift_arm.targetPosition = new Vector3(this.MoveTowards(this.lift_arm.targetPosition.x, this.lift_up_pos, this.lift_arm.targetPosition.x, Time.deltaTime * this.lift_speed), 0.0f, 0.0f);
        this.dummy_lift_arm.targetPosition = new Vector3(this.MoveTowards(this.dummy_lift_arm.targetPosition.x, this.lift_up_pos / 2f, this.dummy_lift_arm.targetPosition.x, (float) ((double) Time.deltaTime * (double) this.lift_speed / 2.0)), 0.0f, 0.0f);
      }
    }
    if (this.gamepad1_x)
      this.fingers.motor = this.fingers.motor with
      {
        targetVelocity = -1f * this.finger_speed
      };
    if (!this.gamepad1_b)
      return;
    this.fingers.motor = this.fingers.motor with
    {
      targetVelocity = this.finger_speed
    };
  }

  private bool MoveHinge(HingeJoint hinge, float target, float speed)
  {
    JointLimits limits;
    if ((double) hinge.limits.max < (double) target)
    {
      limits = hinge.limits;
      target = limits.max;
    }
    limits = hinge.limits;
    if ((double) limits.min > (double) target)
    {
      limits = hinge.limits;
      target = limits.min;
    }
    if ((double) hinge.spring.targetPosition == (double) target)
      return true;
    float targetPosition = hinge.spring.targetPosition;
    float num = this.MoveTowards(targetPosition, target, targetPosition, Time.deltaTime * speed);
    JointSpring spring = hinge.spring with
    {
      targetPosition = num
    };
    hinge.spring = spring;
    return false;
  }
}
