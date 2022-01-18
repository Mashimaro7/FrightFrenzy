// Decompiled with JetBrains decompiler
// Type: Robot_Ri30H_Curiosity
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Robot_Ri30H_Curiosity : RobotInterface3D
{
  public ConfigurableJoint lift_arm;
  public ConfigurableJoint lift_arm2;
  public ConfigurableJoint Bucket;
  public ConfigurableJoint BucketHinge;
  public HingeJoint collector;
  public Quaternion bucket_load_pos = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
  public Quaternion bucket_up_pos = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
  public Quaternion bucket_down_pos = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
  public float lift_speed = 3f;
  public float bucket_speed = 3f;
  public float collector_speed = -1000f;

  public void Awake() => this.info = "<Missing Robot Specific Function: TBD>" + this.info;

  public override void Update_Robot()
  {
    if (this.gamepad1_dpad_down)
    {
      if ((double) this.lift_arm.targetPosition.x < 0.66)
        this.lift_arm.targetPosition += new Vector3(Time.deltaTime * 2f * this.lift_speed, 0.0f, 0.0f);
      if ((double) this.lift_arm2.targetPosition.x < 0.33)
        this.lift_arm2.targetPosition += new Vector3(Time.deltaTime * this.lift_speed, 0.0f, 0.0f);
      if ((double) this.BucketHinge.targetPosition.x < 0.9)
        this.BucketHinge.targetPosition += new Vector3((float) ((double) Time.deltaTime * 3.0 * 0.400000005960464 / 0.300000011920929) * this.lift_speed, 0.0f, 0.0f);
      this.Bucket.targetRotation = (double) this.lift_arm.targetPosition.x <= 0.25 || (double) this.lift_arm2.targetPosition.x <= 0.25 || (double) this.BucketHinge.targetPosition.x <= 0.85 ? this.bucket_up_pos : this.bucket_load_pos;
    }
    if (this.gamepad1_dpad_up)
    {
      if ((double) this.lift_arm.targetPosition.x > -0.3)
        this.lift_arm.targetPosition -= new Vector3(Time.deltaTime * 2f * this.lift_speed, 0.0f, 0.0f);
      if ((double) this.lift_arm2.targetPosition.x > -0.15)
        this.lift_arm2.targetPosition -= new Vector3(Time.deltaTime * this.lift_speed, 0.0f, 0.0f);
      if ((double) this.BucketHinge.targetPosition.x > -0.9)
        this.BucketHinge.targetPosition -= new Vector3(Time.deltaTime * 3f * this.lift_speed, 0.0f, 0.0f);
      if (!this.gamepad1_x)
        this.Bucket.targetRotation = this.bucket_up_pos;
    }
    if (this.gamepad1_x)
      this.Bucket.targetRotation = this.bucket_down_pos;
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
    if (!this.gamepad1_y)
      return;
    this.collector.motor = this.collector.motor with
    {
      targetVelocity = 0.0f
    };
  }
}
