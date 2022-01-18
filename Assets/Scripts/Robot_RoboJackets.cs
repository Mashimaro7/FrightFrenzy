// Decompiled with JetBrains decompiler
// Type: Robot_RoboJackets
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Robot_RoboJackets : RobotInterface3D
{
  public ConfigurableJoint collector_l;
  public ConfigurableJoint collector_r;
  public ConfigurableJoint limb_arm4;
  public ConfigurableJoint limb_arm3;
  public ConfigurableJoint limb_arm2;
  public float limb_arm_start;
  public float limb_arm_speed = 100f;
  public HingeJoint limb_wrist;
  public float limb_wrist_angle_start;
  public float limb_wrist_speed = 100f;
  public HingeJoint limb_hand;
  public float limb_hand_angle_start;
  public float limb_hand_speed = 100f;
  public HingeJoint limb_fingers;
  public float limb_fingers_angle_start = 100f;
  public float limb_fingers_speed = 100f;
  public float wheelspeed = 20f;
  public float reversspeed = 40f;
  private bool gripped;
  private bool grip_button_last;
  private float lastResetTime;
  private bool a_button_last;
  private bool b_button_last;
  private JointSpring g1;
  public Robot_RoboJackets.collectingStates collecting_state;
  private Robot_RoboJackets.armStates arm_state;
  public int armstate;
  private float arm_x_saved;
  private float wrist_angle_saved;

  public void Awake() => this.info = "<Missing Robot Specific Function: TBD>" + this.info;

  public override void Init_Robot() => this.ResetJoints();

  private void ResetJoints()
  {
    this.limb_arm4.targetPosition = this.limb_arm4.targetPosition with
    {
      x = this.limb_arm_start
    };
    this.g1 = this.limb_wrist.spring;
    this.g1.targetPosition = this.limb_wrist_angle_start;
    this.limb_wrist.spring = this.g1;
    this.g1 = this.limb_hand.spring;
    this.g1.targetPosition = this.limb_hand_angle_start * -1f;
    this.limb_hand.spring = this.g1;
    this.g1 = this.limb_fingers.spring;
    this.g1.targetPosition = this.limb_fingers_angle_start;
    this.limb_fingers.spring = this.g1;
  }

  public override void Update_Robot()
  {
    if (this.gamepad1_y)
      this.collecting_state = Robot_RoboJackets.collectingStates.reverse;
    if (this.gamepad1_a && !this.a_button_last)
      this.collecting_state = this.collecting_state != Robot_RoboJackets.collectingStates.onNormal ? Robot_RoboJackets.collectingStates.onNormal : Robot_RoboJackets.collectingStates.off;
    this.a_button_last = this.gamepad1_a;
    if (this.gamepad1_b && !this.b_button_last)
    {
      this.arm_x_saved = this.limb_arm4.targetPosition.x * -1f;
      this.wrist_angle_saved = this.limb_wrist.spring.targetPosition;
      this.arm_state = Robot_RoboJackets.armStates.ungrip;
    }
    if (this.gamepad1_x && this.arm_state == Robot_RoboJackets.armStates.start)
      this.arm_state = Robot_RoboJackets.armStates.gripping;
    this.b_button_last = this.gamepad1_b;
    if (this.gamepad1_dpad_down && this.arm_state == Robot_RoboJackets.armStates.ready)
      this.MoveArm(0.0f, this.limb_arm_speed);
    if (this.gamepad1_dpad_up && this.arm_state == Robot_RoboJackets.armStates.ready)
      this.MoveArm(1.2f, this.limb_arm_speed);
    if (this.collecting_state == Robot_RoboJackets.collectingStates.onNormal)
    {
      this.collector_l.targetAngularVelocity = new Vector3(this.wheelspeed, 0.0f, 0.0f);
      this.collector_r.targetAngularVelocity = new Vector3(-1f * this.wheelspeed, 0.0f, 0.0f);
    }
    if (this.collecting_state == Robot_RoboJackets.collectingStates.off)
    {
      this.collector_l.targetAngularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
      this.collector_r.targetAngularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
    }
    if (this.collecting_state == Robot_RoboJackets.collectingStates.reverse)
    {
      this.collector_l.targetAngularVelocity = new Vector3(-1f * this.reversspeed, 0.0f, 0.0f);
      this.collector_r.targetAngularVelocity = new Vector3(this.reversspeed, 0.0f, 0.0f);
    }
    if (this.arm_state == Robot_RoboJackets.armStates.start)
    {
      this.MoveArm(this.limb_arm_start, 3f * this.limb_arm_speed);
      this.MoveHinge(this.limb_wrist, this.limb_wrist_angle_start, 2f * this.limb_wrist_speed);
      this.MoveHinge(this.limb_hand, -1f * this.limb_hand_angle_start, 2f * this.limb_hand_speed);
      this.MoveHinge(this.limb_fingers, this.limb_fingers_angle_start, this.limb_fingers_speed);
    }
    if (this.arm_state == Robot_RoboJackets.armStates.gripping && this.MoveHinge(this.limb_fingers, -60f, this.limb_fingers_speed))
      this.arm_state = Robot_RoboJackets.armStates.starting_up;
    if (this.arm_state == Robot_RoboJackets.armStates.starting_up)
    {
      bool flag = true;
      if (!this.MoveArm(0.2f, this.limb_arm_speed))
        flag = false;
      if (!this.MoveHinge(this.limb_wrist, 180f, this.limb_wrist_speed))
        flag = false;
      if (!this.MoveHinge(this.limb_hand, -180f, this.limb_hand_speed))
        flag = false;
      if (flag)
        this.arm_state = Robot_RoboJackets.armStates.swiveling;
    }
    if (this.arm_state == Robot_RoboJackets.armStates.swiveling)
      this.arm_state = Robot_RoboJackets.armStates.settling;
    if (this.arm_state == Robot_RoboJackets.armStates.settling)
    {
      bool flag = true;
      if (!this.MoveArm(0.1f, this.limb_arm_speed))
        flag = false;
      if (flag)
        this.arm_state = Robot_RoboJackets.armStates.ready;
    }
    if (this.arm_state == Robot_RoboJackets.armStates.ungrip)
    {
      bool flag = true;
      if (!this.MoveHinge(this.limb_fingers, this.limb_fingers_angle_start, this.limb_fingers_speed))
        flag = false;
      if (flag)
        this.arm_state = Robot_RoboJackets.armStates.clear;
    }
    if (this.arm_state == Robot_RoboJackets.armStates.clear)
    {
      bool flag = true;
      if (!this.MoveArm(this.arm_x_saved + 0.2f, 3f * this.limb_arm_speed))
        flag = false;
      if (flag)
        this.arm_state = Robot_RoboJackets.armStates.clear2;
    }
    if (this.arm_state == Robot_RoboJackets.armStates.clear2)
    {
      bool flag = true;
      if (!this.MoveHinge(this.limb_wrist, 90f, 2f * this.limb_wrist_speed))
        flag = false;
      if (!this.MoveHinge(this.limb_hand, -90f, 2f * this.limb_hand_speed))
        flag = false;
      if (flag)
        this.arm_state = Robot_RoboJackets.armStates.start;
    }
    this.armstate = (int) this.arm_state;
  }

  private bool MoveHinge(HingeJoint myhinge, float target, float speed)
  {
    JointLimits limits;
    if ((double) myhinge.limits.max < (double) target)
    {
      limits = myhinge.limits;
      target = limits.max;
    }
    limits = myhinge.limits;
    if ((double) limits.min > (double) target)
    {
      limits = myhinge.limits;
      target = limits.min;
    }
    if ((double) myhinge.spring.targetPosition == (double) target)
      return true;
    this.g1 = myhinge.spring;
    this.g1.targetPosition = this.MoveTowards(this.g1.targetPosition, target, this.g1.targetPosition, Time.deltaTime * speed);
    myhinge.spring = this.g1;
    return false;
  }

  private bool MoveArm(float target, float speed)
  {
    if ((double) this.limb_arm4.linearLimit.limit < (double) target)
      target = this.limb_arm4.linearLimit.limit;
    if (0.0 > (double) target)
      target = 0.0f;
    if (-1.0 * (double) this.limb_arm4.targetPosition.x == (double) target)
      return true;
    Vector3 targetPosition = this.limb_arm4.targetPosition;
    float num = this.MoveTowards(targetPosition.x, -1f * target, targetPosition.x, Time.deltaTime * speed);
    targetPosition.x = num;
    this.limb_arm4.targetPosition = targetPosition;
    targetPosition.x = (float) ((double) targetPosition.x * 2.0 / 3.0);
    this.limb_arm3.targetPosition = targetPosition;
    targetPosition.x /= 2f;
    this.limb_arm2.targetPosition = targetPosition;
    return false;
  }

  public enum collectingStates
  {
    off,
    onNormal,
    reverse,
  }

  public enum armStates
  {
    start,
    gripping,
    starting_up,
    swiveling,
    settling,
    ready,
    ungrip,
    clear,
    clear2,
  }
}
