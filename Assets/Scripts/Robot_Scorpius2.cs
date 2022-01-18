// Decompiled with JetBrains decompiler
// Type: Robot_Scorpius2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class Robot_Scorpius2 : RobotInterface3D
{
  public ConfigurableJoint lift_arm;
  public ConfigurableJoint lift_arm2;
  public ConfigurableJoint Bucket;
  public ConfigurableJoint BucketHinge;
  public ConfigurableJoint mainCollectorArm;
  public ConfigurableJoint secondCollectorArm;
  public HingeJoint CollectorBody;
  public HingeJoint collector;
  public float lift_speed = 3f;
  private float bucket_speed = 3f;
  public float bucket_rot_speed = 3f;
  public float collector_lift_speed = 3f;
  public float collector_reach_speed = 3f;
  public float collector_speed = -1000f;
  public float collector_collect_pos;
  public float collector_deposit_pos;
  public float collector_max_reach = -1.2f;
  public float collector_spring = 500f;
  public float bucket_deposit_pos;
  public float bucket_arm_reach = -0.48f;
  public float bucket_arm_reach_scaler = 3f;
  private bool state_col_down;
  private bool init_done;
  private float turn_scale_original = -1f;
  public Transform Robot_position;
  public bool enable_orbit = true;
  public float target_x_pos = 96f;
  public float target_z_pos = -165.3f;
  public float target_angle;
  private bool gamepad_a_previous;
  private bool gamepad_b_previous;
  private bool gamepad_y_previous;
  private Robot_Scorpius2.Collector_States collector_state;
  private Robot_Scorpius2.Delivery_States auto_mode;
  private long time_wait_start;
  public long auto_dumping_wait = 1000;

  public void Awake() => this.info = "<Missing Robot Specific Function: TBD>" + this.info;

  public override void Update_Robot()
  {
    if (!this.init_done)
    {
      this.init_done = true;
      this.turn_scale_original = this.turn_scale;
    }
    switch (this.auto_mode)
    {
      case Robot_Scorpius2.Delivery_States.raising_arm:
        if ((double) Math.Abs(this.lift_arm2.targetPosition.x) > (double) Math.Abs(0.999f * this.bucket_arm_reach) && (double) Math.Abs(this.BucketHinge.targetPosition.x) > (double) Math.Abs(0.999f * this.bucket_arm_reach * this.bucket_arm_reach_scaler))
        {
          this.auto_mode = Robot_Scorpius2.Delivery_States.rotating_bucket;
          break;
        }
        break;
      case Robot_Scorpius2.Delivery_States.rotating_bucket:
        if ((double) this.Bucket.targetRotation.x == (double) this.bucket_deposit_pos)
        {
          this.auto_mode = Robot_Scorpius2.Delivery_States.waiting;
          this.time_wait_start = MyUtils.GetTimeMillis();
          break;
        }
        break;
      case Robot_Scorpius2.Delivery_States.waiting:
        if (MyUtils.GetTimeMillis() - this.time_wait_start > this.auto_dumping_wait)
        {
          this.auto_mode = Robot_Scorpius2.Delivery_States.lowering_arm;
          break;
        }
        break;
      case Robot_Scorpius2.Delivery_States.lowering_arm:
        if ((double) Math.Abs(this.lift_arm2.targetPosition.x) < 0.00999999977648258)
        {
          this.auto_mode = Robot_Scorpius2.Delivery_States.off;
          break;
        }
        break;
    }
    if (this.gamepad1_dpad_left)
    {
      this.state_col_down = true;
      if ((double) this.CollectorBody.spring.targetPosition < (double) this.collector_collect_pos)
      {
        Vector3 targetPosition = this.mainCollectorArm.targetPosition;
        targetPosition.x = this.MoveTowards(targetPosition.x, this.collector_max_reach, targetPosition.x, Time.deltaTime * this.collector_reach_speed);
        this.mainCollectorArm.targetPosition = targetPosition;
        this.secondCollectorArm.targetPosition = targetPosition / 2f;
      }
    }
    if (this.state_col_down)
    {
      JointSpring spring = this.CollectorBody.spring;
      if ((double) spring.targetPosition < (double) this.collector_collect_pos)
      {
        spring.spring = 0.0f;
      }
      else
      {
        spring.spring = this.collector_spring;
        spring.targetPosition -= Time.deltaTime * this.collector_lift_speed;
      }
      this.CollectorBody.spring = spring;
    }
    if (this.gamepad1_dpad_right)
    {
      if (this.state_col_down && (double) this.CollectorBody.spring.targetPosition < 0.0)
      {
        Vector3 targetPosition = this.mainCollectorArm.targetPosition;
        if ((double) targetPosition.x < 0.0)
        {
          targetPosition.x = this.MoveTowards(targetPosition.x, 0.0f, targetPosition.x, Time.deltaTime * this.collector_reach_speed);
          this.mainCollectorArm.targetPosition = targetPosition;
          this.secondCollectorArm.targetPosition = targetPosition / 2f;
        }
        else
          this.state_col_down = false;
      }
      else
        this.state_col_down = false;
    }
    if (!this.state_col_down)
    {
      JointSpring spring = this.CollectorBody.spring with
      {
        spring = this.collector_spring
      };
      spring.targetPosition = this.MoveTowards(spring.targetPosition, this.collector_deposit_pos, spring.targetPosition, Time.deltaTime * this.collector_lift_speed);
      this.CollectorBody.spring = spring;
    }
    if (this.gamepad1_dpad_down || this.auto_mode == Robot_Scorpius2.Delivery_States.lowering_arm)
    {
      this.lift_arm.targetPosition = new Vector3(this.MoveTowards(this.lift_arm.targetPosition.x, 0.0f, this.lift_arm.targetPosition.x, Time.deltaTime * 2f * this.lift_speed), 0.0f, 0.0f);
      this.lift_arm2.targetPosition = this.lift_arm.targetPosition / 2f;
      this.BucketHinge.targetPosition = new Vector3(this.MoveTowards(this.BucketHinge.targetPosition.x, 0.0f, this.BucketHinge.targetPosition.x, Time.deltaTime * this.bucket_arm_reach_scaler * this.lift_speed), 0.0f, 0.0f);
      this.Bucket.targetRotation = this.Bucket.targetRotation with
      {
        x = 0.0f
      };
    }
    if (this.gamepad1_dpad_up || this.auto_mode == Robot_Scorpius2.Delivery_States.raising_arm)
    {
      this.lift_arm.targetPosition = new Vector3(this.MoveTowards(this.lift_arm.targetPosition.x, 2f * this.bucket_arm_reach, this.lift_arm.targetPosition.x, Time.deltaTime * 2f * this.lift_speed), 0.0f, 0.0f);
      this.lift_arm2.targetPosition = this.lift_arm.targetPosition / 2f;
      float delta = Time.deltaTime * this.bucket_arm_reach_scaler * this.lift_speed;
      if ((double) Math.Abs(this.BucketHinge.targetPosition.x) > (double) Math.Abs((float) ((double) this.bucket_arm_reach * (double) this.bucket_arm_reach_scaler * 0.75)))
        delta *= 0.33f;
      this.BucketHinge.targetPosition = new Vector3(this.MoveTowards(this.BucketHinge.targetPosition.x, this.bucket_arm_reach * this.bucket_arm_reach_scaler, this.BucketHinge.targetPosition.x, delta), 0.0f, 0.0f);
      if (!this.gamepad1_x)
        this.Bucket.targetRotation = this.Bucket.targetRotation with
        {
          x = 0.0f
        };
    }
    if (this.gamepad1_x || this.auto_mode == Robot_Scorpius2.Delivery_States.rotating_bucket)
    {
      Quaternion targetRotation = this.Bucket.targetRotation;
      targetRotation.x += Time.deltaTime * this.bucket_rot_speed * this.bucket_deposit_pos;
      if ((double) this.bucket_deposit_pos > 0.0 && (double) targetRotation.x >= (double) this.bucket_deposit_pos || (double) this.bucket_deposit_pos < 0.0 && (double) targetRotation.x <= (double) this.bucket_deposit_pos)
        targetRotation.x = this.bucket_deposit_pos;
      this.Bucket.targetRotation = targetRotation;
    }
    if (this.gamepad1_b)
    {
      if (!this.gamepad_b_previous)
        this.collector_state = this.collector_state == Robot_Scorpius2.Collector_States.off ? Robot_Scorpius2.Collector_States.reverse : Robot_Scorpius2.Collector_States.off;
      this.gamepad_b_previous = true;
    }
    else
      this.gamepad_b_previous = false;
    if (this.gamepad1_a)
    {
      if (!this.gamepad_a_previous)
        this.collector_state = this.collector_state == Robot_Scorpius2.Collector_States.off ? Robot_Scorpius2.Collector_States.on : Robot_Scorpius2.Collector_States.off;
      this.gamepad_a_previous = true;
    }
    else
      this.gamepad_a_previous = false;
    float num1 = 0.0f;
    switch (this.collector_state)
    {
      case Robot_Scorpius2.Collector_States.on:
        num1 = this.collector_speed;
        break;
      case Robot_Scorpius2.Collector_States.reverse:
        num1 = -this.collector_speed;
        break;
    }
    this.collector.motor = this.collector.motor with
    {
      targetVelocity = num1
    };
    this.turn_scale = this.turn_scale_original * (float) (((double) this.collector_max_reach - (double) this.mainCollectorArm.targetPosition.x) / (double) this.collector_max_reach * 0.5 + 0.5);
    Vector3 position = this.Robot_position.position;
    float x = this.target_x_pos - position.x;
    float num2 = (float) (-(Math.Atan2((double) this.target_z_pos - (double) position.z, (double) x) * 180.0 / Math.PI) - 90.0) - this.Robot_position.eulerAngles.y;
    while ((double) num2 > 180.0)
      num2 -= 360f;
    while ((double) num2 < -180.0)
      num2 += 360f;
    this.target_angle = num2;
    if (this.gamepad1_y)
    {
      if (!this.gamepad_y_previous)
        this.auto_mode = this.auto_mode == Robot_Scorpius2.Delivery_States.off ? Robot_Scorpius2.Delivery_States.raising_arm : Robot_Scorpius2.Delivery_States.off;
      this.gamepad_y_previous = true;
    }
    else
      this.gamepad_y_previous = false;
    if (this.enable_orbit && (this.auto_mode == Robot_Scorpius2.Delivery_States.raising_arm || this.auto_mode == Robot_Scorpius2.Delivery_States.rotating_bucket || this.auto_mode == Robot_Scorpius2.Delivery_States.waiting))
    {
      this.turning_overide = num2 / 10f;
      if ((double) this.turning_overide > 1.0)
        this.turning_overide = 1f;
      if ((double) this.turning_overide >= -1.0)
        return;
      this.turning_overide = -1f;
    }
    else
      this.turning_overide = 0.0f;
  }

  private enum Collector_States
  {
    off,
    on,
    reverse,
  }

  private enum Delivery_States
  {
    off,
    raising_arm,
    rotating_bucket,
    waiting,
    lowering_arm,
  }
}
