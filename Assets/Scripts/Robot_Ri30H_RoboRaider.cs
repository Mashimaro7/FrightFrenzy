// Decompiled with JetBrains decompiler
// Type: Robot_Ri30H_RoboRaider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class Robot_Ri30H_RoboRaider : RobotInterface3D
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
  public float bucket_speed = 3f;
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

  public void Awake() => this.info = "<Missing Robot Specific Function: TBD>" + this.info;

  public override void Update_Robot()
  {
    if (!this.init_done)
    {
      this.init_done = true;
      this.turn_scale_original = this.turn_scale;
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
    if (this.gamepad1_dpad_down)
    {
      this.lift_arm.targetPosition = new Vector3(this.MoveTowards(this.lift_arm.targetPosition.x, 0.0f, this.lift_arm.targetPosition.x, Time.deltaTime * 2f * this.lift_speed), 0.0f, 0.0f);
      this.lift_arm2.targetPosition = this.lift_arm.targetPosition / 2f;
      this.BucketHinge.targetPosition = new Vector3(this.MoveTowards(this.BucketHinge.targetPosition.x, 0.0f, this.BucketHinge.targetPosition.x, Time.deltaTime * this.bucket_arm_reach_scaler * this.lift_speed), 0.0f, 0.0f);
      this.Bucket.targetRotation = this.Bucket.targetRotation with
      {
        x = 0.0f
      };
    }
    if (this.gamepad1_dpad_up)
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
    if (this.gamepad1_x)
      this.Bucket.targetRotation = this.Bucket.targetRotation with
      {
        x = this.bucket_deposit_pos
      };
    if (this.gamepad1_a)
      this.collector.motor = this.collector.motor with
      {
        targetVelocity = this.collector_speed
      };
    if (this.gamepad1_b)
      this.collector.motor = this.collector.motor with
      {
        targetVelocity = -this.collector_speed
      };
    if (this.gamepad1_y)
      this.collector.motor = this.collector.motor with
      {
        targetVelocity = 0.0f
      };
    this.turn_scale = this.turn_scale_original * (float) (((double) this.collector_max_reach - (double) this.mainCollectorArm.targetPosition.x) / (double) this.collector_max_reach * 0.660000026226044 + 0.330000013113022);
  }
}
