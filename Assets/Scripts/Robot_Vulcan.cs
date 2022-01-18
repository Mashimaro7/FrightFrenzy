// Decompiled with JetBrains decompiler
// Type: Robot_Vulcan
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Robot_Vulcan : RobotInterface3D
{
  public ConfigurableJoint limb_arm5;
  public ConfigurableJoint limb_arm4;
  public ConfigurableJoint limb_arm3;
  public ConfigurableJoint limb_arm2;
  public float limb_arm_start;
  public float limb_arm_speed = 100f;
  public HingeJoint limb_arm1;
  public float limb_arm1_angle_start;
  public float limb_arm1_speed = 100f;
  public ConfigurableJoint finger_l;
  public ConfigurableJoint finger_r;
  public float finger_speed = 100f;
  public float finger_start_pos;
  public float finger_end_pos = -0.2f;
  public Transform servoJoint1_R;
  public Transform servoJoint2_R;
  public float servoJoint1_startAngle_R;
  public float servoJoint1_endAngle_R = 90f;
  public float servoJoint2_startAngle_R;
  public float servoJoint2_endAngle_R = 90f;
  private float fingerMinPosVisual_R = 1.048f;
  private float fingerMaxPosVisual_R = 0.873f;
  public Transform servoJoint1_L;
  public Transform servoJoint2_L;
  public float servoJoint1_startAngle_L;
  public float servoJoint1_endAngle_L = 90f;
  public float servoJoint2_startAngle_L;
  public float servoJoint2_endAngle_L = 90f;
  private float fingerMinPosVisual_L = 0.576f;
  private float fingerMaxPosVisual_L = 0.771f;
  private bool gripped;
  private bool grip_button_last;
  private float lastResetTime;
  private bool a_button_last;
  private bool b_button_last;
  public Robot_Vulcan.armStates arm_state;
  public int armstate;
  private float arm_x_saved;
  private float wrist_angle_saved;
  private JointSpring g1;

  public void Awake() => this.info = "<Missing Robot Specific Function: TBD>" + this.info;

  public override void Start()
  {
    base.Start();
    this.ResetJoints();
  }

  private void ResetJoints()
  {
    this.g1 = this.limb_arm1.spring;
    this.g1.targetPosition = this.limb_arm1_angle_start;
    this.limb_arm1.spring = this.g1;
    this.MoveArm(this.limb_arm_start, 2f * this.limb_arm_speed);
  }

  public override void Update_Robot()
  {
    if (this.gamepad1_x && this.arm_state == Robot_Vulcan.armStates.ready2)
      this.arm_state = Robot_Vulcan.armStates.tilt_down;
    if (this.gamepad1_b)
      this.arm_state = Robot_Vulcan.armStates.start;
    if (this.gamepad1_dpad_down)
      this.MoveArm(0.0f, this.limb_arm_speed);
    if (this.gamepad1_dpad_up)
      this.MoveArm(1.5f, this.limb_arm_speed);
    if (this.gamepad1_dpad_right)
    {
      Vector3 targetPosition = this.finger_l.targetPosition;
      float num = this.MoveTowards(targetPosition.x, this.finger_end_pos, targetPosition.x, Time.deltaTime * this.finger_speed);
      targetPosition.x = num;
      this.finger_l.targetPosition = targetPosition;
      this.finger_r.targetPosition = targetPosition;
    }
    if (this.gamepad1_dpad_left)
    {
      Vector3 targetPosition = this.finger_l.targetPosition;
      float num = this.MoveTowards(targetPosition.x, this.finger_start_pos, targetPosition.x, Time.deltaTime * this.finger_speed);
      targetPosition.x = num;
      this.finger_l.targetPosition = targetPosition;
      this.finger_r.targetPosition = targetPosition;
    }
    double num1 = ((double) this.limb_arm5.transform.InverseTransformPoint(this.finger_l.transform.position).x - (double) this.fingerMinPosVisual_L) / ((double) this.fingerMaxPosVisual_L - (double) this.fingerMinPosVisual_L);
    double num2 = ((double) this.limb_arm5.transform.InverseTransformPoint(this.finger_r.transform.position).x - (double) this.fingerMinPosVisual_R) / ((double) this.fingerMaxPosVisual_R - (double) this.fingerMinPosVisual_R);
    if (this.arm_state == Robot_Vulcan.armStates.start)
    {
      bool flag = true;
      if (!this.MoveArm(this.limb_arm_start, 3f * this.limb_arm_speed))
        flag = false;
      if (!this.MoveHinge(this.limb_arm1, this.limb_arm1_angle_start, this.limb_arm1_speed))
        flag = false;
      if (flag)
        this.arm_state = Robot_Vulcan.armStates.ready2;
    }
    if (this.arm_state != Robot_Vulcan.armStates.tilt_down)
      return;
    bool flag1 = true;
    if (!this.MoveHinge(this.limb_arm1, -90f, this.limb_arm1_speed))
      flag1 = false;
    if (!flag1)
      return;
    this.arm_state = Robot_Vulcan.armStates.ready;
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
    this.limb_arm5.targetPosition = -1f * targetPosition * 4f / 3f;
    this.limb_arm4.targetPosition = targetPosition;
    targetPosition.x = (float) ((double) targetPosition.x * 2.0 / 3.0);
    this.limb_arm3.targetPosition = targetPosition;
    targetPosition.x /= 2f;
    this.limb_arm2.targetPosition = targetPosition;
    return false;
  }

  public enum armStates
  {
    start,
    ready,
    ready2,
    tilt_down,
    gripping,
    starting_up,
    swiveling,
    settling,
    ungrip,
    clear,
    clear2,
  }
}
