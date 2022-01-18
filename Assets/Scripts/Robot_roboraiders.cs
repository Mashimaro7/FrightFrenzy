// Decompiled with JetBrains decompiler
// Type: Robot_roboraiders
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Robot_roboraiders : RobotInterface3D
{
  public ConfigurableJoint collector_l;
  public ConfigurableJoint collector_r;
  public HingeJoint limb_arm;
  public float limb_arm_angle_start = 5f;
  public float limb_arm_speed = 100f;
  public HingeJoint limb_forearm;
  public float limb_forearm_angle_start = 4f;
  public float limb_forearm_speed = 100f;
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
  private Robot_roboraiders.collectingStates collecting_state;
  private Robot_roboraiders.armStates arm_state;
  public int armstate;
  private float forearm_angle_saved;
  private float arm_angle_saved;
  private float wrist_angle_saved;

  public void Awake() => this.info = "<Missing Robot Specific Function: TBD>" + this.info;

  public override void Init_Robot() => this.ResetJoints();

  private void ResetJoints()
  {
    this.g1 = this.limb_arm.spring;
    this.g1.targetPosition = this.limb_arm_angle_start * -1f;
    this.limb_arm.spring = this.g1;
    this.g1 = this.limb_forearm.spring;
    this.g1.targetPosition = this.limb_forearm_angle_start;
    this.limb_forearm.spring = this.g1;
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
      this.collecting_state = Robot_roboraiders.collectingStates.reverse;
    if (this.gamepad1_a && !this.a_button_last)
      this.collecting_state = this.collecting_state != Robot_roboraiders.collectingStates.onNormal ? Robot_roboraiders.collectingStates.onNormal : Robot_roboraiders.collectingStates.off;
    this.a_button_last = this.gamepad1_a;
    if (this.gamepad1_b && !this.b_button_last)
    {
      this.forearm_angle_saved = this.limb_forearm.spring.targetPosition;
      this.arm_angle_saved = this.limb_arm.spring.targetPosition;
      this.wrist_angle_saved = this.limb_wrist.spring.targetPosition;
      this.arm_state = Robot_roboraiders.armStates.ungrip;
    }
    if (this.gamepad1_x && this.arm_state == Robot_roboraiders.armStates.start)
      this.arm_state = Robot_roboraiders.armStates.gripping;
    this.b_button_last = this.gamepad1_b;
    if (this.gamepad1_dpad_down && this.arm_state == Robot_roboraiders.armStates.ready)
      this.MoveHinge(this.limb_forearm, 0.0f, this.limb_forearm_speed / 2f);
    if (this.gamepad1_dpad_up && this.arm_state == Robot_roboraiders.armStates.ready)
      this.MoveHinge(this.limb_forearm, 180f, this.limb_forearm_speed / 2f);
    if (this.gamepad1_dpad_left && this.arm_state == Robot_roboraiders.armStates.ready)
      this.MoveHinge(this.limb_arm, this.limb_arm_angle_start * -1f, this.limb_arm_speed / 2f);
    if (this.gamepad1_dpad_right && this.arm_state == Robot_roboraiders.armStates.ready)
      this.MoveHinge(this.limb_arm, -180f, this.limb_arm_speed / 2f);
    if (this.gamepad1_left_bumper && this.arm_state == Robot_roboraiders.armStates.ready)
      this.MoveHinge(this.limb_wrist, -180f, this.limb_wrist_speed / 2f);
    if (this.gamepad1_right_bumper && this.arm_state == Robot_roboraiders.armStates.ready)
      this.MoveHinge(this.limb_wrist, 180f, this.limb_wrist_speed / 2f);
    if (this.collecting_state == Robot_roboraiders.collectingStates.onNormal)
    {
      this.collector_l.targetAngularVelocity = new Vector3(this.wheelspeed, 0.0f, 0.0f);
      this.collector_r.targetAngularVelocity = new Vector3(-1f * this.wheelspeed, 0.0f, 0.0f);
    }
    if (this.collecting_state == Robot_roboraiders.collectingStates.off)
    {
      this.collector_l.targetAngularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
      this.collector_r.targetAngularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
    }
    if (this.collecting_state == Robot_roboraiders.collectingStates.reverse)
    {
      this.collector_l.targetAngularVelocity = new Vector3(-1f * this.reversspeed, 0.0f, 0.0f);
      this.collector_r.targetAngularVelocity = new Vector3(this.reversspeed, 0.0f, 0.0f);
    }
    if (this.arm_state == Robot_roboraiders.armStates.start)
    {
      this.MoveHinge(this.limb_arm, this.limb_arm_angle_start * -1f, this.limb_arm_speed);
      this.MoveHinge(this.limb_forearm, this.limb_forearm_angle_start, this.limb_forearm_speed);
      this.MoveHinge(this.limb_wrist, this.limb_wrist_angle_start, this.limb_wrist_speed);
      this.MoveHinge(this.limb_hand, -1f * this.limb_hand_angle_start, this.limb_hand_speed);
      this.MoveHinge(this.limb_fingers, this.limb_fingers_angle_start, this.limb_fingers_speed);
    }
    if (this.arm_state == Robot_roboraiders.armStates.gripping && this.MoveHinge(this.limb_fingers, -60f, this.limb_fingers_speed))
      this.arm_state = Robot_roboraiders.armStates.starting_up;
    if (this.arm_state == Robot_roboraiders.armStates.starting_up)
    {
      bool flag = true;
      if (!this.MoveHinge(this.limb_arm, -15f, 0.5f * this.limb_arm_speed))
        flag = false;
      if (!this.MoveHinge(this.limb_forearm, 30f, this.limb_forearm_speed))
        flag = false;
      if (!this.MoveHinge(this.limb_wrist, 30f, this.limb_wrist_speed))
        flag = false;
      if (flag)
        this.arm_state = Robot_roboraiders.armStates.swiveling;
    }
    if (this.arm_state == Robot_roboraiders.armStates.swiveling)
    {
      bool flag = true;
      if (!this.MoveHinge(this.limb_arm, -40f, this.limb_arm_speed))
        flag = false;
      if (!this.MoveHinge(this.limb_forearm, 60f, this.limb_forearm_speed))
        flag = false;
      if (!this.MoveHinge(this.limb_wrist, -4f, this.limb_wrist_speed))
        flag = false;
      if (!this.MoveHinge(this.limb_hand, -180f, this.limb_hand_speed))
        flag = false;
      if (flag)
        this.arm_state = Robot_roboraiders.armStates.settling;
    }
    if (this.arm_state == Robot_roboraiders.armStates.settling)
    {
      bool flag = true;
      if (!this.MoveHinge(this.limb_arm, -20f, this.limb_arm_speed))
        flag = false;
      if (!this.MoveHinge(this.limb_forearm, 30f, this.limb_forearm_speed))
        flag = false;
      if (!this.MoveHinge(this.limb_wrist, -4f, this.limb_wrist_speed))
        flag = false;
      if (flag)
        this.arm_state = Robot_roboraiders.armStates.ready;
    }
    if (this.arm_state == Robot_roboraiders.armStates.ungrip)
    {
      bool flag = true;
      if (!this.MoveHinge(this.limb_fingers, this.limb_fingers_angle_start, this.limb_fingers_speed))
        flag = false;
      if (flag)
        this.arm_state = Robot_roboraiders.armStates.clear;
    }
    if (this.arm_state == Robot_roboraiders.armStates.clear)
    {
      bool flag = true;
      if (!this.MoveHinge(this.limb_arm, this.arm_angle_saved + 30f, this.limb_forearm_speed))
        flag = false;
      if (!this.MoveHinge(this.limb_wrist, this.limb_wrist_angle_start, this.limb_wrist_speed))
        flag = false;
      if (flag)
        this.arm_state = Robot_roboraiders.armStates.start;
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

  private enum collectingStates
  {
    off,
    onNormal,
    reverse,
  }

  private enum armStates
  {
    start,
    gripping,
    starting_up,
    swiveling,
    settling,
    ready,
    ungrip,
    clear,
  }
}
